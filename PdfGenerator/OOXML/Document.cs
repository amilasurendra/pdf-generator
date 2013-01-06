using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocGen.OOXML;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using DocGen.ObjectModel;

namespace DocGen.OOXML
{
    /// <summary>
    /// IDocument implementation for OOXML
    /// </summary>
    public class Document : IDocument
    {
        
        private Word.Body body;
        private string docFileName;
        private DocumentPackager packager;
        private WordprocessingDocument docxFile;


        public Document(String fileName)
        {
            this.FileName = fileName;
            docFileName = fileName + ".docx";

            packager = DocumentPackager.GetInstance();
            docxFile = packager.CreateDocument(docFileName);
            body = packager.GetBody();
        }


        public String FileName { get; private set; }


        public void AddElement(Element element) {

            //Special processing for metadata, needs access to document package
            if (element.GetElementType() == ElementType.Metadata) {
                AddMetaData(element as MetaData);
            }

            body.Append(ElementFactory.GetElement(element));
        }



        public void Save()
        {
            packager.SavePackage();
            FileInfo pdfFileInfo = new FileInfo(FileName);
            FileInfo wordFileInfo = new FileInfo(docFileName);

            if (AutoGenerateTOC)
            {
                using (PostProcessor processor = new PostProcessor(wordFileInfo.FullName))
                {
                    processor.AddToc();
                }
            }

            PdfConverter.Convert(wordFileInfo.FullName, pdfFileInfo.FullName);
        }



        public void AddMetaData(MetaData data)
        {
            docxFile.PackageProperties.Creator = data.Author;
            docxFile.PackageProperties.Title = data.Title;
            docxFile.PackageProperties.Subject = data.Subject;
            

            if(data.Tags != null){

                StringBuilder tags = new StringBuilder(data.Tags[0]);

                for (int i = 1; i < data.Tags.Length; i++)
			    {
                    tags.Append(", " + data.Tags[i]);
			    }

                docxFile.PackageProperties.Keywords = tags.ToString();
            }
        }


        public bool AutoGenerateTOC
        {
            get;
            set;
        }

        public void AddSection(Section section)
        {
            AddElement(section);
        }

        public void AddChapter(Chapter chapter)
        {
            AddElement(chapter);
        }
        public void AddSection(MultiColumnSection section)
        {
            AddElement(section);
        }
    }
}
