using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IT = iTextSharp.text;
using PDF = iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Imaging;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    public class Document : IDocument
    {
        IT.Document document;
        PDF.PdfWriter pdfWriter;

        public Document(String fileName)
        {
            document = new IT.Document(IT.PageSize.A4);
            FileStream file = new FileStream(fileName, FileMode.Create);
            pdfWriter = PDF.PdfWriter.GetInstance(document, file);

            pdfWriter.CompressionLevel = 0;

            document.Open();
        }


        public void AddChapter(Chapter chapter) {
            IT.Chapter tmp = new IT.Chapter(chapter.ChapterNumber);
            tmp.Title =  new IT.Paragraph( chapter.Title ); //TODO : Add formatting

            foreach (var section in chapter.SubElements)
            {
                if (section is MultiColumnSection) { 

                }else SectionFormatter.AddFormattedSection( (Section) section, tmp);
            }

            document.Add(tmp);
        }



        public void AddSection(Section section)
        {
            AddElements(section, null);

        }

        public void AddSection(MultiColumnSection section)
        {
            PDF.MultiColumnText multiCol = new PDF.MultiColumnText(450);
            multiCol.AddRegularColumns(36, document.PageSize.Width - 36, 18, section.NumberOfColumns);
            AddElements(section, multiCol);
        }

        public void Save()
        {
            document.Close();
        }



        private void AddElements(Section section, PDF.MultiColumnText multiColPart)
        {
            Element[] elements = section.SubElements;

            foreach (var element in elements)
            {
                switch (element.GetElementType())
                {
                    case ElementType.Text:
                        AddElement(element as Text, multiColPart);
                        break;
                    case ElementType.Paragraph:
                        AddElement(element as Paragraph, multiColPart);
                        break;
                    case ElementType.Table:
                        AddElement(element as Table, multiColPart);
                        break;
                    case ElementType.Image:
                        AddElement(element as Image, multiColPart);
                        break;
                }
            }
        }


        private void AddElement(Text text, PDF.MultiColumnText columns) {
            Paragraph para = new Paragraph(text);
            IT.Paragraph formatted = ParagraphFormatter.GetFormattedParagraph(para);
            AddToDocument(formatted, columns);
        }

      

        private void AddElement(Image image, PDF.MultiColumnText columns)
        {
            IT.Image formattedImage = ImageFormatter.GetFormattedImage(image);
            AddToDocument(formattedImage, columns);
        }


        private void AddElement(Paragraph paragraph, PDF.MultiColumnText columns)
        {
            IT.Paragraph para = ParagraphFormatter.GetFormattedParagraph(paragraph);
            AddToDocument(para, columns);            
        }


        private void AddElement(Table table, PDF.MultiColumnText columns)
        {
            IT.Paragraph tablePara = new IT.Paragraph();
            tablePara.Add(TableFormatter.GetFormattedTable(table));
            AddToDocument(tablePara, columns);  
        }


        //This method is used to add any Element part to the pdf file
        private void AddToDocument(IT.IElement pdfElement,  PDF.MultiColumnText columns)
        {
            if (columns != null)
            {
                columns.AddElement(pdfElement);
                if (columns.IsOverflow()) columns.NextColumn();
                document.Add(columns);
            }
            else
            {
                document.Add(pdfElement);
            }

        }


        public void AddMetaData(MetaData data)
        {
            throw new NotImplementedException();
        }


        public bool AutoGenerateTOC
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void AddElement(Element element)
        {
            throw new NotImplementedException();
        }
    }
}
