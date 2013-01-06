using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocGen.ObjectModel;

namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Table object to formatted OpenXML objects.
    /// </summary>
    class TableFormatter
    {
        public static IEnumerable<OpenXmlElement> GetFormattedElement(Table table)
        {
            List<OpenXmlElement> result = new List<OpenXmlElement>();

            Word.Table wordTable = new Word.Table();

            wordTable.Append(GetTableProperties(table));

            //Add Headings
            if(table.Headings != null) wordTable.Append(GetFormattedTableRow(table.Headings));

            //Add Data Rows
            foreach (Element element in table.SubElements)
            {
                if (element.GetElementType() == ElementType.TableRow) wordTable.Append(GetFormattedTableRow((TableRow)element));
                else throw new InvalidSubFeatureException(table.GetElementType().ToString(), element.GetElementType().ToString());
            }

            result.Add(wordTable);
            result.Add(new Word.Paragraph());
            return result;
        }


        private static Word.TableProperties GetTableProperties(Table table) {

            Word.TableWidth widthProps = new Word.TableWidth();

            switch (table.WidthUnits)
            {
                case ElementWidth.Absolute:
                    widthProps.Width = (table.Width * 20).ToString();
                    widthProps.Type = Word.TableWidthUnitValues.Dxa;
                    break;
                case ElementWidth.Percentage:
                    widthProps.Width = (table.Width * 50).ToString();
                    widthProps.Type = Word.TableWidthUnitValues.Pct;
                    break;
                default:
                    break;
            }


            Word.BorderValues border;

            switch (table.BorderType)
            {
                case TableBorders.None:
                    border = Word.BorderValues.None;
                    break;
                case TableBorders.Single:
                    border = Word.BorderValues.Single;
                    break;
                default:
                    border = Word.BorderValues.None;
                    break;
            }


            Word.TableProperties tableProps = new Word.TableProperties()
            {
                TableWidth = widthProps
                ,
                TableBorders = new Word.TableBorders()
                {

                    LeftBorder = new Word.LeftBorder()
                    {
                        Val = border
                    }
                    ,
                    RightBorder = new Word.RightBorder()
                    {
                        Val = border
                    }
                    ,
                    TopBorder = new Word.TopBorder()
                    {
                        Val = border
                    }
                    ,
                    BottomBorder = new Word.BottomBorder()
                    {
                        Val = border
                    }
                    ,
                    InsideHorizontalBorder = new Word.InsideHorizontalBorder()
                    {
                        Val = border
                    }
                    ,
                    InsideVerticalBorder = new Word.InsideVerticalBorder()
                    {
                        Val = border
                    }

                }
            };
            return tableProps;
        }


        private static OpenXmlElement GetFormattedTableRow(TableRow row)
        {
            Word.TableRow formattedRow = new Word.TableRow();

            foreach (var cell in row.SubElements)
            {
                if (cell == null) { 
                    formattedRow.Append(null);
                    continue;
                }
                if (cell.GetElementType() == ElementType.TableCell) formattedRow.Append(GetFormattedCell((TableCell)cell));
                else throw new InvalidSubFeatureException(row.GetElementType().ToString(), cell.GetElementType().ToString());
            }

            return formattedRow;
        }


        private static OpenXmlElement GetFormattedCell(TableCell cell)
        {
            //The only way to handle empty cells in OOXML - add an empty paragaph or prevent creating the document
            if (cell == null || cell.SubElements.Count() == 0)
            {
                var emptyCell = new Word.TableCell();
                emptyCell.Append(new Word.Paragraph());
                return emptyCell;
            }

            Word.TableCell formattedCell = new Word.TableCell();
            formattedCell.Append(GetCellProperties(cell));

            Element[] cellElements = cell.SubElements;

            foreach (Element cellElement in cellElements)
            {
                switch (cellElement.GetElementType())
                {
                    case ElementType.Table:
                        Word.Paragraph nestedTablePara = new Word.Paragraph();
                        formattedCell.Append(GetFormattedElement(cellElement as Table));
                        formattedCell.Append(new Word.Paragraph());
                        break;

                    default:
                        if (cell.CanContain(cellElement.GetElementType()))
                        {
                            formattedCell.Append(ElementFactory.GetElement(cellElement));
                            break;
                        }
                        else throw new InvalidSubFeatureException(cell.GetElementType().ToString(), cellElement.GetElementType().ToString());
                }
            }

            return formattedCell;
        }


        private static Word.StyleTableCellProperties GetCellProperties(TableCell cell) {

            Word.StyleTableCellProperties cellProps = new Word.StyleTableCellProperties();

            Word.TableCellVerticalAlignment vAlign = new Word.TableCellVerticalAlignment();

            switch (cell.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    vAlign.Val = Word.TableVerticalAlignmentValues.Top;
                    break;
                case VerticalAlignment.Center:
                    vAlign.Val = Word.TableVerticalAlignmentValues.Center;
                    break;
                case VerticalAlignment.Bottom:
                    vAlign.Val = Word.TableVerticalAlignmentValues.Bottom;
                    break;
                default:
                    break;
            }

            cellProps.Append(vAlign);

            return cellProps;
        }
    }
}
