using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RandomGenerator
{
    /// <summary>
    /// Writes the document tree to the XML File
    /// </summary>
    class XmlWriter
    {
        private String outputFile;
        private XDocument outDocument;

        public XmlWriter(String outputFile)
        {
            this.outputFile = outputFile;
        }

        public void WriteXml(DocumentTree document) {

            outDocument = new XDocument();


            XElement root = new XElement(document.Root.Name);

            outDocument.Add(root);

            AddToXml(document.Root, outDocument.Root);



            outDocument.Save(outputFile);
            
        }

        private void AddToXml(Feature feature, XElement element) {

            if (feature.Value != null) {
                element.Value = feature.Value;
            }

            foreach (var style in feature.GetStyles())
            {
                XAttribute tmp = new XAttribute(style.Name, style.Value);
                element.Add(tmp);
            }

            IEnumerable<Feature> subElements = feature.GetChildFeatures();
            
            foreach (var subElement in subElements)
            {
                XElement tmp = new XElement(subElement.Name);
                element.Add(tmp);
                AddToXml(subElement, tmp);
            }
        
        }
    }
}
