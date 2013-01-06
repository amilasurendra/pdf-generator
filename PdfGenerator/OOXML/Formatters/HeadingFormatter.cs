using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using Model = DocGen.ObjectModel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Heading object to formatted OpenXML objects.
    /// </summary>
    public class HeadingFormatter
    {
        public static OpenXmlElement GetFormattedElement(Model.Heading heading) {

            OpenXmlElement formattedPara = ParagraphFormatter.GetFormattedElement(heading);

            IEnumerable<ParagraphProperties> list = formattedPara.Elements<ParagraphProperties>();

            ParagraphProperties tmp = list.FirstOrDefault();

            StyleCreator.AddHeadingStyle(heading.HeadingType);

            ParagraphStyleId style = new ParagraphStyleId()
            {
                Val = StyleIds.GetStyleId(heading.HeadingType)
            };

            tmp.InsertAt(style,1);

            return formattedPara;
        }
    }
}
