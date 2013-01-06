using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDF = iTextSharp.text.pdf;
using IT = iTextSharp.text;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    class TableFormatter
    {
        public static PDF.PdfPTable GetFormattedTable(Table table)
        {

            PDF.PdfPTable pdfTable = new PDF.PdfPTable(table.ColumnCount);


            for (int i = 0; i < table.SubElements.Length; i++)
            {
                PDF.PdfPCell[] cells = new PDF.PdfPCell[pdfTable.NumberOfColumns];

                TableRow row = (TableRow) table.SubElements[i];

                for (int j = 0; j < cells.Length; j++)
                {
                    pdfTable.AddCell(GetFormattedCell( (TableCell) row.SubElements[j]));
                }

            }

            return pdfTable;
        }


        private static PDF.PdfPCell GetFormattedCell(TableCell cell)
        {
            PDF.PdfPCell formattedCell = new PDF.PdfPCell();

            Element[] cellElements = cell.SubElements;

            foreach (Element temp in cellElements)
            {
                switch (temp.GetElementType())
                {
                    //TODO: Add other enum values
                    case ElementType.Text:
                        IT.Phrase phrase = new IT.Phrase(TextFormatter.GetFormattedText((Text)temp));
                        formattedCell.AddElement(phrase);
                        break;

                    case ElementType.Paragraph:
                        formattedCell.AddElement(ParagraphFormatter.GetFormattedParagraph((Paragraph)temp));
                        break;

                    case ElementType.Table:
                        formattedCell.AddElement(TableFormatter.GetFormattedTable((Table)temp));
                        break;

                    case ElementType.Image:
                        formattedCell.AddElement(ImageFormatter.GetFormattedImage((Image)temp));
                        break;
                }

            }


            return formattedCell;
        }
    }
}
