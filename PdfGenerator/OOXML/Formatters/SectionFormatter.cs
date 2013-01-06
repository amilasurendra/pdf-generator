using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocGen.ObjectModel;
using DocumentFormat.OpenXml;


namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Section object to formatted OpenXML objects.
    /// </summary>
    class SectionFormatter
    {
        public static DocumentFormat.OpenXml.OpenXmlElement[] GetFormattedSection(Section section)
        {

            List<DocumentFormat.OpenXml.OpenXmlElement> formattedSection = new List<DocumentFormat.OpenXml.OpenXmlElement>();
            Element[] elements = section.SubElements;

            foreach (var element in elements)
            {
                if (section.CanContain(element.GetElementType())) formattedSection.AddRange(ElementFactory.GetElement(element));
                else throw new InvalidSubFeatureException( section.GetElementType().ToString(), element.GetElementType().ToString() );
            }

            Word.Columns cols = new Word.Columns()
            {
                ColumnCount = (Int16Value)1
            };

            Word.SectionProperties secProps = new Word.SectionProperties(cols);
            Word.ParagraphProperties paraProp = new Word.ParagraphProperties(secProps);
            Word.Paragraph para = new Word.Paragraph(paraProp);

            formattedSection.Add(para);

            return formattedSection.ToArray();
        }
    }
}
