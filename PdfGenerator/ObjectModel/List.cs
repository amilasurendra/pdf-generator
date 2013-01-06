using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    //Newly Added
    [XmlRoot("list")]
    public class List : Element
    {
        private List<Element> listItems;
        
        [XmlAttribute("ordered")]
        public bool Ordered { get; set; }

        [XmlAttribute("label")]
        public String Label { get; set; }

        [XmlIgnore]
        public override Element[] SubElements
        {
            get
            {
                return listItems.ToArray();
            }
            set
            {
                if (value != null) listItems = new List<Element>(value);
                else listItems = new List<Element>();
            }
        }

        public List()
            : base()
        {
            listItems = new List<Element>();
        }

        public override ElementType GetElementType()
        {
            return ElementType.List;
        }


        public override bool CanContain(ElementType subType)
        {
            if (subType == ElementType.ListItem) return true;
            else return false;
        }
    }

    [XmlRoot("listItem")]
    public class ListItem : Element
    {
        /// <summary>
        /// Symbol of the list item
        /// </summary>
        [XmlAttribute("label")]
        public String Label
        {
            get;
            set;
        }

        [XmlText]
        public String Body { get; set; }

        public override ElementType GetElementType()
        {
            return ElementType.ListItem;
        }

        [XmlIgnore]
        public override Element[] SubElements
        {
            get;
            set;
        }

        public override bool CanContain(ElementType subType)
        {
            return false;
        }
    }
}
