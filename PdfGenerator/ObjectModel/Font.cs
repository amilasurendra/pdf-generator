using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("font")]
    public class Font : Element
    {
        [XmlAttribute("colour")]
        public String Colour { get; set; }

        [XmlAttribute("size")]
        public float Size { get; set; }

        public Font() : base()
        {
            Formats = new List<FontFormats>();
        }

        [XmlAttribute("backgroundColour")]
        public String BackgroundColour
        {
            get;
            set;
        }

        [XmlElement("fontFormat")]
        public List<FontFormats> Formats
        {
            get;
            set;
        }

        public static Font GetFont(FontFormats format)
        {
            Font font = new Font();
            font.Formats.Add(format);
            return font;
        }

        public Font GetFont(FontFormats[] formats)
        {
            Font font = new Font();
            font.Formats = new List<FontFormats>(formats);
            return font;
        }

        public override ElementType GetElementType()
        {
            throw new NotImplementedException();
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
