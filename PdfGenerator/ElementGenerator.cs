using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocGen.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace DocGen
{
    public class ElementGenerator
    {
        private Dictionary<String, Type> elementDictionary;

        public ElementGenerator()
        {
            elementDictionary = new Dictionary<string, Type>();

            //Add new Types here
            AddElement(typeof(MetaData));
            AddElement(typeof(Section));
            AddElement(typeof(MultiColumnSection));
            AddElement(typeof(Chapter));
            AddElement(typeof(Paragraph));
            AddElement(typeof(Image));
            AddElement(typeof(Heading));
            AddElement(typeof(Table));
            AddElement(typeof(Text));
            AddElement(typeof(List));//Feature extension

        }


        /// <summary>
        /// Returns a Element object given the xml representation of that object.
        /// </summary>
        /// <param name="element">XElement object representing the element</param>
        /// <returns>Subclass of Element</returns>
        public Element GetElement(XElement element)
        {
            String tagName = element.Name.ToString();

            try
            {
                Type elementType = elementDictionary[tagName];
                return CreateElement(element, elementType);
            }

            catch (KeyNotFoundException e)
            {
                throw new InvalidInputException("Invalid input. " + element.Name.ToString() + " is not supported",e);
            }
        }


        /// <summary>
        /// Adds the given Element type to element dictonary, which is used to recreate Element objects from XML.
        /// </summary>
        /// <param name="elementType">Type object representing target element. element Must be a sub class of "DocGen.ObjectModel.Element" </param>
        private void AddElement(Type elementType) { 

            String attribName = null;
            object[] customAttribs = elementType.GetCustomAttributes( typeof(XmlRootAttribute) , false);

            if (customAttribs != null && customAttribs.Count() > 0)
            {
                attribName = ((XmlRootAttribute)customAttribs[0]).ElementName;
            }
            
            Element element = (Element) Activator.CreateInstance(elementType);

            if(attribName==null) attribName = elementType.Name;

            elementDictionary.Add(attribName, elementType);
        }


        private Element CreateElement(System.Xml.Linq.XElement element, Type type)
        {
            Element createdElement;

            XmlSerializer deserializer = new XmlSerializer(type);
            MemoryStream stream = new MemoryStream();
            element.Save(stream);

            stream.Position = 0;
            createdElement = (Element)deserializer.Deserialize(stream);

            return createdElement;
        }
    }
}
