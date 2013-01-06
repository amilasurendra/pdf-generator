using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    [XmlRoot("paragraph")]
    public class Paragraph : Element
    {
        private List<Element> contents;

        [XmlIgnore]
        public override Element[] SubElements
        {
            get
            {
                return contents.ToArray();
            }
            set
            {
                if (value != null) { 
                    contents = new List<Element>(value);
                }else contents = new List<Element>();
            }
        }

        
        [XmlAttribute("leading")]
        public float Leading { get; set; }

        [XmlAttribute("spacingBefore")]
        public float SpacingBefore { get; set; }

        [XmlAttribute("spacingAfter")]
        public float SpacingAfter { get; set; }

        [XmlAttribute("justification")]
        public Justification Justification { get; set; }

        [XmlElement("font")]
        public Font Font { get; set; }


        public Paragraph() : base()
        {
            contents = new List<Element>();
            SetDefaultValues();
        }

        private void SetDefaultValues() {
            Leading = 1.5f;
            SpacingAfter = 0;
            SpacingBefore = 10;
            Justification = Justification.Left;
        }

        public Paragraph(string text) : this()
        {
            contents.Add(new Text(text));
        }

        public Paragraph(string text, Font font):this()
        {
            throw new System.NotImplementedException();
        }

        public Paragraph(Text text) : this()
        {
            contents.Add(text);
        }

        public Paragraph(Text[] parts)
        {
            contents = new List<Element>(parts);
        }

        
        public void AddPart(Text part)
        {
            contents.Add(part);
        }

        public override ElementType GetElementType()
        {
            return ElementType.Paragraph;
        }

        public override bool CanContain(ElementType subType)
        {
            if (subType == ElementType.Text) return true;
            return false;
        }
    }
}
