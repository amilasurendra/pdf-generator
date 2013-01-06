using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DocGen.ObjectModel;
using System.Xml.Schema;
using DocGen.OOXML; //Select from IText or OOXML, have to add external interface once IText is complete

namespace DocGen
{
    /// <summary>
    /// Main class for generating documents given the XML input.
    /// </summary>
    public class Generator
    {
        private XDocument document;
        private DocGen.ObjectModel.IDocument resultDoc;
        private ElementGenerator generator;

        private String InputFileURI { get; set; }

        public Generator(String xmlFilePath, String outputFileName, String defaultsXmlPath)
        {
            Element.DefaultsXml = defaultsXmlPath;

            generator = new ElementGenerator();

            try
            {
                this.InputFileURI = xmlFilePath;
                document = XDocument.Load(InputFileURI);
                resultDoc = new Document(outputFileName);
            }
            catch (XmlException e)
            {
                throw new InvalidInputException("Malformed XML", e);
            }
        }


        private void ValidateXml() {

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null,"PdfGen.xsd");
            document.Validate(schemaSet, ValidationErrorHandler);
        
        }

        private void ValidationErrorHandler(object sender, ValidationEventArgs e) {
            throw new InvalidInputException("Input XML is Invalid: " + e.Message);
        }


        public void Generate()
        {
            //TODO: Add all to xsd and enable
            //ValidateXml();

            try
            {
                FileInfo outputFile = new FileInfo(InputFileURI);

                ProcessAttribues();

                foreach (var element in GetMainTags())
                {
                    AddToDocument(element);
                }

                resultDoc.Save();
            }

            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException("Input XML is Invalid", ex);
            }

            catch (FileNotFoundException e)
            {
                throw new InvalidInputException("Input XML is Invalid, " + e.Message, e);
            }


        }

        private void ProcessAttribues()
        {
            XElement root = document.Root;

            XAttribute autoGenerateTOC = root.Attribute("autoGenerateToc");
            if (autoGenerateTOC != null)
            {
                if ("true".Equals(autoGenerateTOC.Value))
                {
                    resultDoc.AutoGenerateTOC = true;
                }
            }
        }

        private IEnumerable<XElement> GetMainTags()
        {
            XElement root = document.Root;
            IEnumerable<XElement> childElements = root.Elements();
            return childElements;
        }


        public void AddToDocument(XElement element)
        {
                Element generatedElement = generator.GetElement(element);

                if (generatedElement == null) return; //TODO:Remove and add exception

                resultDoc.AddElement(generatedElement);
        }
    }
}
