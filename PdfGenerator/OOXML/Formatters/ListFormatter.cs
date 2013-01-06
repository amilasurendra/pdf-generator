using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using Model = DocGen.ObjectModel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocGen.OOXML
{
    //Feature extension
    /// <summary>
    /// Responsible for converting a given List object to formatted OpenXML objects.
    /// </summary>
    class ListFormatter
    {
        /// <summary>
        /// Returns formatted OpenXmlElement[] representing given List instance
        /// </summary>
        /// <param name="list">List object</param>
        /// <returns></returns>
        public static IEnumerable<OpenXmlElement> GetFormattedElement(Model.List list) {

            List<OpenXmlElement> result = new List<OpenXmlElement>();

            int numStyleId = StyleCreator.AddNumberingStyle(list.Ordered, list.Label);

            foreach (Model.Element element in list.SubElements)
            {
                if (element.GetElementType() == Model.ElementType.ListItem)
                {
                    result.Add( GetListItem((Model.ListItem)element, numStyleId) );
                }
                else {
                    throw new InvalidSubFeatureException( list.GetElementType().ToString(), element.GetElementType().ToString());
                }
            }

            result.Add(new Paragraph());
            return result.AsEnumerable();
        }


        /// <summary>
        /// Returns a OpenXMl paragraph representing formatted listItem.
        /// </summary>
        /// <param name="item">listItem object</param>
        /// <param name="numStyleId">style id to use</param>
        /// <returns></returns>
        private static Paragraph GetListItem(Model.ListItem item, int numStyleId)
        {

            Paragraph listItemPara = new Paragraph();

            ParagraphProperties paraProps = new ParagraphProperties();

            NumberingProperties numberingProps = new NumberingProperties();
            NumberingLevelReference numberingLevelReference = new NumberingLevelReference() { Val = 0 };


            NumberingId numberingId = new NumberingId() { Val = numStyleId };

            numberingProps.Append(numberingLevelReference);
            numberingProps.Append(numberingId);
            paraProps.Append(numberingProps);

            Run listRun = new Run();
            Text listItemText = new Text()
            {
                Text = item.Body
            };
            listRun.Append(listItemText);

            listItemPara.Append(paraProps);
            listItemPara.Append(listRun);

            return listItemPara;
        }
    }
}
