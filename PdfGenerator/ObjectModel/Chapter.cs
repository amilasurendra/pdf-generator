using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocGen.ObjectModel
{
    /// <summary>
    /// Used to create automatic bookmark and TOC creation.
    /// </summary>
    [XmlRoot("chapter")]
    public class Chapter : Element
    {
        private List<Element> sections;

        [XmlAttribute("title")]
        public String Title { get; set; }

        [XmlAttribute("chapterNumber")]
        public int ChapterNumber { get; set; }

        [XmlIgnore]
        public override Element[] SubElements
        {
            get { 
                return sections.ToArray(); 
            }
            set
            {
                if (value != null) sections = new List<Element>(value);
                else sections = new List<Element>();
            }

        }

        public Chapter()
            : base()
        {
            sections = new List<Element>();
        }

        public Chapter(int chapterNumber)
        {
            this.ChapterNumber = chapterNumber;
        }

        public Chapter(String title, int chapterNumber)
            : this()
        {
            this.Title = title;
            this.ChapterNumber = chapterNumber;
        }

        public void AddSection(Section section)
        {
            sections.Add(section);
        }

        public void AddSection(MultiColumnSection section)
        {
            sections.Add(section);
        }

        public override ElementType GetElementType()
        {
            return ElementType.Chapter;
        }

        public override bool CanContain(ElementType subType)
        {
            if (subType == ElementType.Section || subType == ElementType.MultiColumnSection) return true;
            else return false;
        }
    }
}
