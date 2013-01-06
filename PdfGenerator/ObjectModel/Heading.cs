using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("heading")]
    public class Heading : Paragraph
    {
        [XmlAttribute("headingType")]
        public HeadingType HeadingType { get; set; }

        public override ElementType GetElementType()
        {
            return ElementType.Heading;
        }
    }
}
