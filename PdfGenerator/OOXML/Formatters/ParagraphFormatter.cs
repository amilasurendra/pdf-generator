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
    /// Responsible for converting a given Paragraph object to formatted OpenXML objects.
    /// </summary>
    public class ParagraphFormatter
    {
        public static OpenXmlElement GetFormattedElement(Model.Paragraph paragraph) {

            Paragraph para = new Paragraph();
            para.Append(GetParagraphProperties(paragraph));

            foreach (Model.Element element in paragraph.SubElements)
            {
                if (element is Model.Text)
                {
                    Model.Text text = (Model.Text)element;
                    if (paragraph.Font != null && text.Font == null) text.Font = paragraph.Font;
                    para.Append(TextFormatter.GetFormattedElement(text));
                }
                else {
                    throw new InvalidSubFeatureException(paragraph.GetElementType().ToString(), element.GetElementType().ToString());
                }
            }

            return para;
        }

        private static ParagraphProperties GetParagraphProperties(Model.Paragraph paragraph) {

            
            ParagraphProperties paraProps = new ParagraphProperties();

            SpacingBetweenLines paraSpacing = new SpacingBetweenLines()
            {
                Before = Utilities.GetDxaFromPoints(paragraph.SpacingBefore),
                After = Utilities.GetDxaFromPoints(paragraph.SpacingAfter),
                LineRule = LineSpacingRuleValues.Auto,
                //If the value of the lineRule attribute is auto, then the value of the line attribute shall be interpreted as 240ths of a line
                Line = (paragraph.Leading * 240).ToString()
            };

            Justification justification = new Justification();
            switch (paragraph.Justification)
            {
                case DocGen.ObjectModel.Justification.Left:
                    justification.Val = JustificationValues.Left;
                    break;
                case DocGen.ObjectModel.Justification.Center:
                    justification.Val = JustificationValues.Center;
                    break;
                case DocGen.ObjectModel.Justification.Right:
                    justification.Val = JustificationValues.Right;
                    break;
                case DocGen.ObjectModel.Justification.Justified:
                    justification.Val = JustificationValues.Both;
                    break;
            }

            paraProps.Append(justification);
            paraProps.Append(paraSpacing);

            return paraProps;
        }
    }
}
