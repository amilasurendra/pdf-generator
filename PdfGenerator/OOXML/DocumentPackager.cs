using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocGen.OOXML
{
    /// <summary>
    /// Manages current document packaging
    /// (No support for concurrent documents)
    /// </summary>
    internal class DocumentPackager
    {
        private static DocumentPackager _instance = null;


        private WordprocessingDocument docxFile;
        private MainDocumentPart mainPart;
        private Body body;
        private StyleDefinitionsPart stylePart;
        private NumberingDefinitionsPart numberingPart;

        private DocumentPackager() { }

        internal static DocumentPackager GetInstance() {
            if (_instance == null) _instance = new DocumentPackager();
            return _instance;
        }

        internal WordprocessingDocument CreateDocument(String fileName) {
            docxFile = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document);
            mainPart = docxFile.AddMainDocumentPart();
            docxFile.MainDocumentPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new Body());
            body = docxFile.MainDocumentPart.Document.Body;
            return docxFile;
        }

        internal MainDocumentPart GetMainPart() {
            return mainPart;
        }

        internal Body GetBody() {
            return body;
        }

        internal StyleDefinitionsPart GetStylePart(){
            if (stylePart == null) {
                stylePart = docxFile.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
                Styles root = new Styles();
                root.Save(stylePart);
            }
            return stylePart;
        }


        internal NumberingDefinitionsPart GetNumberingPart()
        {
            if (numberingPart == null)
            {
                numberingPart = docxFile.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>();
                Numbering numberingRoot = new Numbering();
                numberingRoot.Save(numberingPart);
            }
            return numberingPart;
        }

        internal void SavePackage() {
            mainPart.Document.Save();
            docxFile.Close();
        }
    }
}
