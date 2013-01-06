using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IT = iTextSharp.text;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    class TextFormatter
    {
        public static IT.Chunk GetFormattedText(Text text){


            IT.Chunk formattedChuck = new IT.Chunk(text.TextContent);
            formattedChuck.Font = GetFormattedFont(text.Font);

            return formattedChuck;
        
        }


        private static IT.Font GetFormattedFont(Font font)
        {

            IT.Font iTextFont = new IT.Font();

            //Return default font if font style is not available.
            if (font == null)
            {
                return new IT.Font();
            }

            foreach (var format in font.Formats)
            {
                switch (format)
                {
                    case FontFormats.Bold:
                        iTextFont.SetStyle(IT.Font.BOLD);
                        break;
                    case FontFormats.Italic:
                        iTextFont.SetStyle(IT.Font.ITALIC);
                        break;
                    case FontFormats.Underlined:
                        iTextFont.SetStyle(IT.Font.UNDERLINE);
                        break;
                    default:
                        break;
                }
            }
            return iTextFont;
        }
    }
}
