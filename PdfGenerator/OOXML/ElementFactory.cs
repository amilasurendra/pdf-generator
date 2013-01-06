using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocGen.ObjectModel;
using DocumentFormat.OpenXml;

namespace DocGen.OOXML
{
    class ElementFactory
    {
        /// <summary>
        /// Returns formatted OOXML elements according to the given Element object.
        /// </summary>
        /// <param name="element">Subclass of Element representing the document feature</param>
        /// <returns></returns>
        public static IEnumerable<OpenXmlElement> GetElement(Element element)
        {
            switch (element.GetElementType())
            {
                case ElementType.Text:
                    return GetElement(element as Text);

                case ElementType.Paragraph:
                    return GetElement(element as Paragraph);

                case ElementType.Table:
                    return TableFormatter.GetFormattedElement(element as Table);

                case ElementType.Image:
                    return GetElement(element as Image);
                
                case ElementType.Heading:
                    return GetElement(element as Heading);

                case ElementType.Section:
                    return SectionFormatter.GetFormattedSection(element as Section);

                case ElementType.MultiColumnSection:
                    return MultiColumnSectionFormatter.GetFormattedSection(element as MultiColumnSection);
                    
                case ElementType.Chapter:
                    return ChapterFormatter.GetFormattedChapter(element as Chapter);
                
                case ElementType.List:
                    return GetElement(element as List);

                default: throw new InvalidInputException(element.GetElementType() + " ");
            }
        }



        private static IEnumerable<OpenXmlElement> GetElement(Text text)
        {
            Paragraph para = new Paragraph(text);
            var formattedPara = ParagraphFormatter.GetFormattedElement(para);
            OpenXmlElement[] result = new OpenXmlElement[1];
            result[0] = formattedPara;
            return result;
        }

        private static IEnumerable<OpenXmlElement> GetElement(Paragraph paragraph)
        {
            var paragraphContent = ParagraphFormatter.GetFormattedElement(paragraph);
            OpenXmlElement[] result = new OpenXmlElement[1];
            result[0] = paragraphContent;
            return result;
        }

        private static IEnumerable<OpenXmlElement> GetElement(Image image)
        {
            var imageContent = ImageFormatter.GetFormattedElement(image);
            OpenXmlElement[] result = new OpenXmlElement[1];
            result[0] = imageContent;
            return result;
        }

        private static IEnumerable<OpenXmlElement> GetElement(List list)
        {
            List<OpenXmlElement> FormattedList = new List<OpenXmlElement>();
            foreach (var listItem in ListFormatter.GetFormattedElement((List)list))
            {
                FormattedList.Add(listItem);
            }
            return FormattedList.ToArray();
        }

        private static IEnumerable<OpenXmlElement> GetElement(Heading heading)
        {
            var formattedHeading = HeadingFormatter.GetFormattedElement(heading);
            OpenXmlElement[] result = new OpenXmlElement[1];
            result[0] = formattedHeading;
            return result;
        }
    }
}
