using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace DocGen.OOXML
{
    class PdfConverter
    {
        /// <summary>
        /// Converts a given word document to PDF format.
        /// </summary>
        /// <param name="docFilePath">Path to word document</param>
        /// <param name="targetPdfPath">name of the output pdf file.</param>
        public static void Convert(string docFilePath, string targetPdfPath)
        {

            object paramMissing = Type.Missing;
            object filePath = docFilePath;


            WdExportFormat paramExportFormat = WdExportFormat.wdExportFormatPDF;
            bool paramOpenAfterExport = false;
            WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;
            WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
            int paramStartPage = 0;
            int paramEndPage = 0;
            WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;
            bool paramIncludeDocProps = true;
            bool paramKeepIRM = true;
            WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateHeadingBookmarks;  //Do not change
            bool paramDocStructureTags = true;  //Pdf Tagging
            bool paramBitmapMissingFonts = true;
            bool paramUseISO19005_1 = false;


            Microsoft.Office.Interop.Word.Document wordDocument = null;
            Application wordApp = new Application();


            try
            {
                wordDocument = wordApp.Documents.Open(ref filePath, ref paramMissing, ref paramMissing,
                                ref paramMissing, ref paramMissing, ref paramMissing,
                                ref paramMissing, ref paramMissing, ref paramMissing,
                                ref paramMissing, ref paramMissing, ref paramMissing,
                                ref paramMissing, ref paramMissing, ref paramMissing,
                                ref paramMissing
                );


                if (wordDocument != null)
                {
                    wordDocument.ExportAsFixedFormat(
                        targetPdfPath, paramExportFormat, paramOpenAfterExport,
                        paramExportOptimizeFor, paramExportRange, paramStartPage,
                        paramEndPage, paramExportItem, paramIncludeDocProps,
                        paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                        paramBitmapMissingFonts, paramUseISO19005_1,
                        ref paramMissing
                    );
                }
            }

            catch (COMException ex) {
                throw new InvalidOperationException("Invalid Document", ex);
            }

            finally
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

            }


        }

    }
}
