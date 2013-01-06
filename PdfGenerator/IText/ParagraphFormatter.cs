using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IT = iTextSharp.text;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    class ParagraphFormatter
    {
        public static IT.Paragraph GetFormattedParagraph(Paragraph paragraph) {


            IT.Paragraph formattedPara = new IT.Paragraph();

            foreach (var chunk in paragraph.SubElements)
            {
                IT.Chunk formattedChunk = TextFormatter.GetFormattedText( (Text) chunk);
                formattedPara.Add(formattedChunk);
            }

            // TODO: Paragraph formatting
            formattedPara.MultipliedLeading = paragraph.Leading;
            formattedPara.SpacingAfter = paragraph.SpacingAfter;
            formattedPara.SpacingBefore = paragraph.SpacingBefore;

            return formattedPara;
        }
    }
}
