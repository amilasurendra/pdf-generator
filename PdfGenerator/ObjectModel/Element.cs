using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;


namespace DocGen.ObjectModel
{
    /// <summary>
    /// Abstract class for representing all features. All feature classes should inherit
    /// from this and provide necessary features.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Setting default values. All inheriting classes should call this.
        /// </summary>
        public Element()
        {
            if (DefaultsXml == null) throw new InvalidOperationException("DefaultsXml property cannot be null.");
            SetDefaults(DefaultsXml);
        }

        public static String DefaultsXml { get; set; }

        public abstract ElementType GetElementType();

        public abstract bool CanContain(ElementType subType);
        

        [XmlElement("chapter", typeof(Chapter))]
        [XmlElement("section", typeof(Section))]
        [XmlElement("metadata", typeof(MetaData))]
        [XmlElement("multiColumnSection", typeof(MultiColumnSection))]
        [XmlElement("tableRow", typeof(TableRow))]
        [XmlElement("tableCell", typeof(TableCell))]
        [XmlElement("paragraph", typeof(Paragraph))]
        [XmlElement("table", typeof(Table))]
        [XmlElement("image", typeof(Image))]
        [XmlElement("text", typeof(Text))]
        [XmlElement("heading", typeof(Heading))]
        //Feature extension
        [XmlElement("list", typeof(List))]
        [XmlElement("listItem", typeof(ListItem))]
        //Feature extension
        public abstract Element[] SubElements { get; set; }

        
        /// <summary>
        /// Get defaults from defaults.xml and add default properties using reflection
        /// </summary>
        public void SetDefaults(String DefaultsXml)
        {
            Defaults defaults = new Defaults(DefaultsXml);

            //Only get default values for public properties
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (var item in properties)
            {
                object defaultVal = defaults.GetDefaultValue(this.GetType(), item);
                if(defaultVal != null) item.SetValue(this, defaultVal, null);
            }
        }



    }

    /// <summary>
    /// Represents all the insertable elements.
    /// </summary>
    public enum ElementType { 
        Metadata,
        Paragraph,
        Section,
        MultiColumnSection,
        Chapter,
        Image,
        Table,
        TableRow,
        TableCell,
        Text,
        Heading,
        //Feature extension
        List,
        ListItem,
        //Feature extension
    }
}
