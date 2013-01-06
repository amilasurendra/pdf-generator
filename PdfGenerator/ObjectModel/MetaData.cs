using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [System.Xml.Serialization.XmlRoot("metadata")]
    public class MetaData : Element
    {
        public MetaData() : base() { }

        [System.Xml.Serialization.XmlAttribute("title")]
        public String Title { get; set; }

        [System.Xml.Serialization.XmlAttribute("subject")]
        public String Subject { get; set; }

        [System.Xml.Serialization.XmlAttribute("author")]
        public String Author { get; set; }

        [System.Xml.Serialization.XmlAttribute("creator")]
        public String Creator { get; set; }

        [System.Xml.Serialization.XmlArrayItem("tag")]
        public String[] Tags { get; set; }


        public override ElementType GetElementType()
        {
            return ElementType.Metadata;
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
