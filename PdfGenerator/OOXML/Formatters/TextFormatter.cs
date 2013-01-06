using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Model = DocGen.ObjectModel;

namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Text object to formatted OpenXML objects.
    /// </summary>
    class TextFormatter
    {

        public static OpenXmlElement GetFormattedElement(Model.Text text)
        {

            Text docText = new Text(text.TextContent);
            Run run = new Run();
            RunProperties runProp = CreateRun(text);
            
            if (runProp != null) run.Append(runProp);
            run.Append(docText);
            
            return run;
        }


        private static RunProperties CreateRun(Model.Text text)
        {

            RunProperties runProperties = new RunProperties();
            if (text.Font == null) return null;


            FontSize size = new FontSize()
            {
                Val = Utilities.GetHPSValue(text.Font.Size)
            };

            runProperties.Append(size);

            foreach (var format in text.Font.Formats)
            {
                switch (format)
                {
                    case Model.FontFormats.Bold: runProperties.Append(new Bold()); break;
                    case Model.FontFormats.Italic: runProperties.Append(new Italic()); break;
                    case Model.FontFormats.Underlined: runProperties.Append(new Underline() { Val = UnderlineValues.Single}); break;
                }
            }

            return runProperties;
        }

    }
}
