using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("multiColumnSection")]
    public class MultiColumnSection : Section
    {
        [XmlAttribute("numberOfColumns")]
        public int NumberOfColumns
        {
            get;
            set;
        }
        [XmlAttribute("gutterSize")]
        public int GutterSize
        {
            get;
            set;
        }

        //For XML serializer
        public MultiColumnSection() : base() {}

        public MultiColumnSection(int columnCount)
        {
            this.NumberOfColumns = columnCount;
        }
    }
}
