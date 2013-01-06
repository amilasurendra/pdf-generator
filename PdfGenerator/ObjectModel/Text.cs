using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("text")]
    public class Text : Element
    {
        //For serialization
        public Text()
            : base()
        {
            Font = new Font();
        }

        public Text(string text) : this()
        {
            this.TextContent = text;
        }

        public Text(string text, Font font) : this(text)
        {
            this.Font = font;
        }


        #region XML attributes for simplicity
        [XmlAttribute("underlined")]
        public bool Underlined {
            set {
                if(value) Font.Formats.Add(FontFormats.Underlined);
            }
            get
            {
                return Font.Formats.Contains(FontFormats.Underlined);
            }
        }

        [XmlAttribute("italic")]
        public bool Italic
        {
            set
            {
                if (value) Font.Formats.Add(FontFormats.Italic);
            }
            get {
                return Font.Formats.Contains(FontFormats.Italic);
            }
        }

        [XmlAttribute("bold")]
        public bool Bold
        {
            set
            {
                if (value) Font.Formats.Add(FontFormats.Bold);
            }
            get
            {
                return Font.Formats.Contains(FontFormats.Bold);
            }
        }

        [XmlAttribute("size")]
        public float Size
        {
            set
            {
                Font.Size = value;
            }
            get {
                return Font.Size;
            }
        }
        #endregion


        [XmlText]
        public string TextContent
        {
            get;
            set;
        }

        [XmlElement("font")]
        public Font Font { get; set; }


        public override ElementType GetElementType()
        {
            return ElementType.Text;
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
