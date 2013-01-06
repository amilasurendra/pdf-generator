using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDF = iTextSharp.text.pdf;
using IT = iTextSharp.text;
using System.IO;
using DocGen.ObjectModel;

namespace DocGen.IText
{
    class ImageFormatter
    {
        public static IT.Image GetFormattedImage(Image image) {

            IT.Image formattedImage = IT.Image.GetInstance(image.ImageURL);

            if (image.AbsolutePositioning) {

                formattedImage.SetAbsolutePosition(image.AbsolutePositionX, image.AbsolutePositionY);
            }


            formattedImage.Alignment = GetHorozontalAlignment(image.HorizontalAlignment);

            return formattedImage;           

        }

        private static int GetHorozontalAlignment(HorizontalAlignment alignment) {

            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return IT.Element.ALIGN_LEFT;
                case HorizontalAlignment.Center:
                    return IT.Element.ALIGN_CENTER;
                case HorizontalAlignment.Right:
                    return IT.Element.ALIGN_RIGHT;
            }
            return IT.Element.ALIGN_UNDEFINED;
        }
    }
}
