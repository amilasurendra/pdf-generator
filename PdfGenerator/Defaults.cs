using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace DocGen
{
    /// <summary>
    /// Provides default values for the Object Model classes
    /// </summary>
    public class Defaults
    {
        XDocument xDoc;

        public Defaults(String fileName)
        {
            xDoc = XDocument.Load(fileName);
        }


        /// <summary>
        /// Returns the default value for a given property of a Element class
        /// </summary>
        /// <param name="classType">Tupe object representing class type</param>
        /// <param name="property">PropertyInfo object representing the property to get defaults</param>
        /// <returns>Default value on success. Returns null if default value not found.</returns>
        public object GetDefaultValue(Type classType, PropertyInfo property)
        {
            String className = GetClassName(classType);

            XElement node = xDoc.Element("defaults").Element( XName.Get(className));

            if (node == null) return null;

            String attribName = GetAttributeName(property);
            String defaultObjStr = null;

            if (attribName != null) {
                XAttribute attribute = node.Attribute(attribName);
                defaultObjStr = attribute == null ? null : attribute.Value;
            }

            object result = null;

            if (defaultObjStr != null)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(property.PropertyType);
                result = tc.ConvertFromString(null, CultureInfo.InvariantCulture, defaultObjStr);
            }

            return result;
        }


        /// <summary>
        /// Returns attribute name for the given property
        /// </summary>
        /// <param name="property">property</param>
        /// <returns>attribute name</returns>
        private static String GetAttributeName(PropertyInfo property)
        {
            String attribName = null;

            object[] customAttribs = property.GetCustomAttributes(typeof(XmlAttributeAttribute), false);

            if (customAttribs != null && customAttribs.Count() > 0)
            {
                attribName = ((XmlAttributeAttribute)customAttribs[0]).AttributeName;
            }

            return attribName;
        }



        /// <summary>
        /// Returns the element name used in xml input for given class using serialization attributes
        /// </summary>
        /// <param name="classType">class type</param>
        /// <returns>String representing class name</returns>
        private String GetClassName(Type classType) {

            String elementName = null;

            object[] test = classType.GetCustomAttributes(typeof(XmlRootAttribute), false);

            if (test != null && test.Count() > 0)
            {
                XmlRootAttribute tmp = (XmlRootAttribute)test[0];
                elementName = tmp.ElementName;
            }

            if (elementName == null)
            {
                elementName = classType.Name;
            }

            return elementName;
        }


    }
}
