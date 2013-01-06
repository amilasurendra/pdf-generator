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
    /// Responsible for converting a given MultiColumnSection object to formatted OpenXML objects.
    /// </summary>
    public class MultiColumnSectionFormatter
    {
        public static DocumentFormat.OpenXml.OpenXmlElement[] GetFormattedSection(MultiColumnSection section)
        {

            List<DocumentFormat.OpenXml.OpenXmlElement> formattedSection = new List<DocumentFormat.OpenXml.OpenXmlElement>();
            Element[] elements = section.SubElements;

            foreach (var element in elements)
            {
                formattedSection.AddRange(ElementFactory.GetElement(element));
            }

            Word.Columns cols = new Word.Columns()
            {
                ColumnCount = (Int16Value)section.NumberOfColumns
            };

            Word.SectionProperties secProps = new Word.SectionProperties(cols);
            Word.ParagraphProperties paraProp = new Word.ParagraphProperties(secProps);
            Word.Paragraph para = new Word.Paragraph(paraProp);

            formattedSection.Add(para);

            return formattedSection.ToArray();
        }
    }
}
