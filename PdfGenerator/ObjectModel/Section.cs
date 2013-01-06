using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("section")]
    public class Section : Element
    {

        private List<Element> elementList;

        [XmlIgnore]
        public override Element[] SubElements
        {
            get
            {
                return elementList.ToArray();
            }
            set
            {
                if (value != null) elementList = new List<Element>(value);
                else elementList = new List<Element>();
            }
        }

        public Section()
            : base()
        {
            elementList = new List<Element>();
        }

        public void AddElement(Paragraph paragraph)
        {
            elementList.Add(paragraph);
        }

        public void AddElement(Table table)
        {
            elementList.Add(table);
        }

        public void AddElement(Text text)
        {
            elementList.Add(text);
        }

        public void AddElement(Image image)
        {
            elementList.Add(image);
        }

        public void AddElement(Heading heading)
        {
            elementList.Add(heading);
        }


        public override ElementType GetElementType()
        {
            return ElementType.Section;
        }

        public override bool CanContain(ElementType subType)
        {
            ElementType[] sectionElements = 
            { 
                  ElementType.Heading,
                  ElementType.Image,
                  ElementType.List, //Feature extension
                  ElementType.Paragraph,
                  ElementType.Table,
                  ElementType.Text,
            };

            if (sectionElements.Contains(subType)) return true;
            else return false;
        }
    }
}
