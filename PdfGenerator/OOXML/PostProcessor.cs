using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for creating table of contents given a path to a docx file.
    /// </summary>
    public class PostProcessor : IDisposable
    {
        private string filePath;
        private Microsoft.Office.Interop.Word.Document wordDocument = null;
        private Application wordApp = null;
        private object paramMissing = Type.Missing;


        public PostProcessor(string filePath)
        {
            this.filePath = filePath;

            
        }


        public void AddToc() {

            wordApp = new Application();
            object file = filePath;

            try
            {

                wordDocument = wordApp.Documents.Open(ref file, ref paramMissing, ref paramMissing,
                                    ref paramMissing, ref paramMissing, ref paramMissing,
                                    ref paramMissing, ref paramMissing, ref paramMissing,
                                    ref paramMissing, ref paramMissing, ref paramMissing,
                                    ref paramMissing, ref paramMissing, ref paramMissing,
                                    ref paramMissing
                );


                Range tocRange = wordDocument.Range(0, 0);
                TableOfContents toc = wordDocument.TablesOfContents.Add(tocRange, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing);
                toc.Update();


                int end = toc.Range.End;
                Range tocEnd = wordDocument.Range(end, end);

                tocEnd.InsertParagraphBefore();
                tocEnd.InsertBreak(WdBreakType.wdPageBreak);

            }
            catch (COMException ex)
            {
                throw new InvalidOperationException("Invalid Document", ex);
            }
            finally {
                Dispose();
            }
        }


        public void Dispose()
        {
            if (wordDocument != null)
            {
                wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                wordDocument = null;
            }

            // Quit Word and release the ApplicationClass object.
            if (wordApp != null)
            {
                wordApp.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                wordApp = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
