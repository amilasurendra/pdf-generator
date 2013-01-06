using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocGen.ObjectModel;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;


namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Chapter object to formatted OpenXML objects.
    /// </summary>
    public class ChapterFormatter
    {
        private static bool stylesAdded = false;

        public static DocumentFormat.OpenXml.OpenXmlElement[] GetFormattedChapter(Chapter chapter) {

            if(!stylesAdded) StyleCreator.AddStylePart();

            List<OpenXmlElement> result = new List<OpenXmlElement>();

            //Setting up Heading style
            Word.ParagraphProperties paraProps = new Word.ParagraphProperties();
            Word.ParagraphStyleId style = new Word.ParagraphStyleId()
            {
                Val = "Heading1"
            };
            paraProps.Append(style);

            //Adding chapter title
            Word.Paragraph para = new Word.Paragraph();
            Word.Run run = new Word.Run();
            Word.Text text = new Word.Text()
            {
                Text = chapter.Title
            };

            run.Append(text);
            para.Append(paraProps);
            para.Append(run);

            result.Add(para);


            //Add all child elements
            foreach (Element element in chapter.SubElements)
            {
                if (element.GetElementType() == ElementType.MultiColumnSection)
                {
                    result.AddRange(MultiColumnSectionFormatter.GetFormattedSection((MultiColumnSection)element));
                }
                else if (element.GetElementType() == ElementType.Section) result.AddRange(SectionFormatter.GetFormattedSection((Section)element));

                else throw new InvalidSubFeatureException( chapter.GetElementType().ToString() , element.GetElementType().ToString());
            }

            return result.ToArray();
        }
    }
}
