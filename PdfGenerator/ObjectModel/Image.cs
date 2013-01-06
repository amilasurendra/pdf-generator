using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("image")]
    public class Image : Element
    {
        [XmlAttribute("absolutePositioning")]
        public bool AbsolutePositioning { get; set; }

        [XmlAttribute("absoluteSize")]
        public bool AbsoluteSize { get; set; }

        [XmlAttribute("width")]
        public float Width { get; set; }

        [XmlAttribute("height")]
        public float Height { get; set; }

        [XmlAttribute("absolutePositionX")]
        public float AbsolutePositionX { get; set; }

        [XmlAttribute("absolutePositionY")]
        public float AbsolutePositionY { get; set; }

        [XmlAttribute("horizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [XmlAttribute("caption")]
        public String Caption
        {
            get;
            set;
        }

        public Image() : base() { } //For Serialization

        public Image(Image image)
        {
            throw new NotImplementedException();
        }

        public Image(string imagePath)
        {
            ImageURL = imagePath;
            System.Drawing.Image imageContent = System.Drawing.Image.FromFile(imagePath);
            Width = imageContent.Width;
            Height = imageContent.Height;
            imageContent.Dispose();
        }

        public override ElementType GetElementType()
        {
            return ElementType.Image;
        }

        
        [XmlText()]
        public String ImageURL
        {
            get;
            set;
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
