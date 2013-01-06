using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    /// <summary>
    /// Represents a pdf Table structure
    /// </summary>
    [XmlRoot("table")]
    public class Table : Element
    {
        private DataTable data;

        [XmlAttribute("borderType")]
        public TableBorders BorderType { get; set; }

        [XmlAttribute("widthUnits")]
        public ElementWidth WidthUnits { get; set; }

        [XmlAttribute("width")]
        public float Width { get; set; }

        [XmlAttribute("height")]
        public float Height { get; set; }

        [XmlAttribute("columnCount")]
        public int ColumnCount { get; set; }

        [XmlElement("headings")]
        public TableRow Headings { get; set; }

        [XmlAttribute("caption")]
        public String Caption
        {
            get;
            set;
        }

        
        public Table():base(){

            data = new DataTable();

        }


        public Table(int columnCount) : this()
        {
            this.ColumnCount = columnCount;
            CreateColumns();
        }

        private void CreateColumns() {
            //TODO: Check this
            for (int i = 0; i < ColumnCount; i++)
            {
                data.Columns.Add(i.ToString(), typeof(Element));
            }
        }


        /// <summary>
        /// Creates a PDF table with default formatting using given data.
        /// </summary>
        /// <param name="data">a Datatable object representing data</param>
        public Table(DataTable data)  : this()
        {
            //TODO: should evaluate the feasibility of this for multiple type of objects
            throw new NotImplementedException();
        }


        public void AddRow(TableRow row) {
           data.Rows.Add( (TableCell[]) row.SubElements);
         }


        private TableRow[] GetData() {

            List<TableRow> rows = new List<TableRow>();
            TableRow tmpRow;

            foreach (DataRow row in data.Rows)
            {
                tmpRow = new TableRow(GetCells(row));
                rows.Add(tmpRow);
            }

            return rows.ToArray();
        }

        private TableCell[] GetCells(DataRow row) {
            List<TableCell> cells = new List<TableCell>();
            for (int i = 0; i < data.Columns.Count; i++)
            {
                cells.Add( row[i] as TableCell);
            }
            return cells.ToArray();
        }

        public override ElementType GetElementType()
        {
            return ElementType.Table;
        }


        [XmlIgnore]
        public override Element[] SubElements
        {
            get
            {
                return GetData();
            }
            set
            {
                if (value == null)
                {
                    data.Rows.Clear();
                    return;
                }

                //Set max cellCount of all rows as column Count
                foreach (Element row in value)
                {
                    int tmpColCnt = row.SubElements.Count();
                    if (ColumnCount < tmpColCnt) ColumnCount = tmpColCnt;
                }

                //TODO:change later
                data.Columns.Clear();
                CreateColumns();

                data.Rows.Clear();

                foreach (TableRow row in value)
                {
                    data.Rows.Add(row.SubElements);
                }
            }
        }

        public override bool CanContain(ElementType subType)
        {
            if (subType == ElementType.TableRow) return true;
            return false;
        }
    }


    [XmlRoot("tableCell")]
    public class TableCell : Element
    {
        private List<Element> elements;

        [XmlIgnore]
        public override Element[] SubElements { 
            get {
                return elements.ToArray();
            }
            set {
                if (value != null) elements = new List<Element>(value);
                else elements = new List<Element>();
            }
        }

        [XmlAttribute("fillColour")]
        public String FillColour { get; set; }


        [System.Xml.Serialization.XmlAttribute("verticalAlignment")]
        public VerticalAlignment VerticalAlignment { get; set; }

        public TableCell()
        {
            elements = new List<Element>();
        }

        public TableCell(Paragraph paragraph) : this()
        {
            elements.Add(paragraph);
        }

        public TableCell(Text text) : this()
        {
            elements.Add(text);
        }

        public TableCell(Table table) : this()
        {
            elements.Add(table);
        }

        public TableCell(Image image) : this()
        {
            elements.Add(image);
        }

        public void AddElement(Paragraph paragraph) {
            elements.Add(paragraph);
        }

        public void AddElement(Image image)
        {
            elements.Add(image);
        }

        public void AddElement(Table Table)
        {
            elements.Add(Table);
        }

        public void AddElement(Text text)
        {
            elements.Add(text);
        }

        public override ElementType GetElementType()
        {
            return ElementType.TableCell;
        }



        public override bool CanContain(ElementType subType)
        {
            ElementType[] tableCellElements =
            { 
                  ElementType.Image,
                  ElementType.List,//Feature extension
                  ElementType.Paragraph,
                  ElementType.Table,
                  ElementType.Text
            };

            if (tableCellElements.Contains(subType)) return true;
            else return false;
        }
    }

    [XmlRoot("tableRow")]
    public class TableRow : Element {

        private List<Element> cells;

        [XmlIgnore]
        public override Element[] SubElements
        {
            get
            {
                return cells.ToArray();
            }
            set
            {
                if (value != null) cells = new List<Element>(value);
                else cells = new List<Element>();
            }
        }

        //For XML serialization
        public TableRow(){}

        public TableRow(TableCell[] cells)
        {
            this.cells = new List<Element>(cells);
        }

        public void AddCell(TableCell cell) {
            cells.Add(cell);      
        }

        
        public override ElementType GetElementType()
        {
            return ElementType.TableRow;
        }


        public override bool CanContain(ElementType subType)
        {
            if (subType == ElementType.TableCell) return true;
            return false;
        }
    }
}
