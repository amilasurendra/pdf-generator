using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Model = DocGen.ObjectModel;

namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for converting a given Image object to formatted OpenXML objects.
    /// </summary>
    class ImageFormatter
    {
        private static int imageId = 0;

        public static OpenXmlElement GetFormattedElement(Model.Image image)
        {
            MainDocumentPart mainPart = DocumentPackager.GetInstance().GetMainPart();

            //Adding image part to main document part
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            if (image.ImageURL == null) throw new InvalidInputException("Input XML error : image location not provided for Image element.");

            using (FileStream stream = new FileStream(image.ImageURL, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }

            var imageContent = GetImage(image, mainPart.GetIdOfPart(imagePart));

            var drawing = GetImage(image, mainPart.GetIdOfPart(imagePart));


            Model.Paragraph tmpImagePara = new Model.Paragraph();

            switch (image.HorizontalAlignment)
            {
                case DocGen.ObjectModel.HorizontalAlignment.Left:
                    tmpImagePara.Justification = Model.Justification.Left;
                    break;
                case DocGen.ObjectModel.HorizontalAlignment.Center:
                    tmpImagePara.Justification = Model.Justification.Center;
                    break;
                case DocGen.ObjectModel.HorizontalAlignment.Right:
                    tmpImagePara.Justification = Model.Justification.Right;
                    break;
            }

            Run imageRun = new Run();
            imageRun.Append(drawing);
            var imagePara = ParagraphFormatter.GetFormattedElement(tmpImagePara);
            imagePara.Append(imageRun);

            return imagePara;
        }





        private static OpenXmlElement GetImage(Model.Image image, string id)
        {

            //TODO: Calculating From EMU - Should see
            Int64Value width = (Int64Value)image.Width * 1530350 / 72;
            Int64Value height = (Int64Value)image.Width * 1530350 / 72;
            FileInfo file = new FileInfo(image.ImageURL);
            

            var element = new Drawing(
              new DW.Inline(
              new DW.Extent() { Cx = width, Cy = height },
              new DW.EffectExtent()
              {
                  LeftEdge = 0L,
                  TopEdge = 0L,
                  RightEdge = 0L,
                  BottomEdge = 0L
              },
              new DW.DocProperties()
              {
                  Id = (UInt32Value)1U,
                  Name = file.Name
              },
              new DW.NonVisualGraphicFrameDrawingProperties(
                  new A.GraphicFrameLocks() { NoChangeAspect = true }),
              new A.Graphic(
                  new A.GraphicData(
                      new PIC.Picture(
                          new PIC.NonVisualPictureProperties(
                              new PIC.NonVisualDrawingProperties()
                              {
                                  Id = (UInt32Value)0U,
                                  Name = file.Name
                              },
                              new PIC.NonVisualPictureDrawingProperties()),
                          new PIC.BlipFill(
                              new A.Blip()
                              {
                                  Embed = id,
                                  CompressionState = A.BlipCompressionValues.Print
                              },
                              new A.Stretch(
                                  new A.FillRectangle())),
                          new PIC.ShapeProperties(
                              new A.Transform2D(
                                  new A.Offset() { X = 0L, Y = 0L },
                                  new A.Extents() { Cx = width, Cy = height }),
                              new A.PresetGeometry(
                                  new A.AdjustValueList()
                              ) { Preset = A.ShapeTypeValues.Rectangle }))
                  ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
          )
          {
              DistanceFromTop = (UInt32Value)0U,
              DistanceFromBottom = (UInt32Value)0U,
              DistanceFromLeft = (UInt32Value)0U,
              DistanceFromRight = (UInt32Value)0U,
          });

            return element;
        }
    }
}
