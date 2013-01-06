using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IT = iTextSharp.text;
using PDF = iTextSharp.text.pdf;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    public class SectionFormatter
    {
        public static IT.Section AddFormattedSection(Section section, IT.Chapter chapter)
        {

            IT.Section Formattedsection = chapter.AddSection("Title");
            Formattedsection.Title = null;


            foreach (var item in section.SubElements)
            {
                Formattedsection.Add(GetElement(item));
            }


            return Formattedsection;
        }





        private static IT.IElement GetElement(Element element)
        {
            switch (element.GetElementType())
            {
                case ElementType.Text:
                    return GetElement(element as Text);
                case ElementType.Paragraph:
                    return GetElement(element as Paragraph);
                case ElementType.Table:
                    return GetElement(element as Table);
                case ElementType.Image:
                    return GetElement(element as Image);
                default :
                    return null;
            }
        }


        private static IT.IElement GetElement(Text text)
        {
            Paragraph para = new Paragraph(text);
            IT.Paragraph formatted = ParagraphFormatter.GetFormattedParagraph(para);
            return formatted;
        }



        private static IT.IElement GetElement(Image image)
        {
            IT.Image formattedImage = ImageFormatter.GetFormattedImage(image);
            return formattedImage;
        }


        private static IT.IElement GetElement(Paragraph paragraph)
        {
            IT.Paragraph para = ParagraphFormatter.GetFormattedParagraph(paragraph);
            return para;
        }


        private static IT.IElement GetElement(Table table)
        {
            IT.Paragraph tablePara = new IT.Paragraph();
            tablePara.Add(TableFormatter.GetFormattedTable(table));
            return tablePara;
        }


    }
}
