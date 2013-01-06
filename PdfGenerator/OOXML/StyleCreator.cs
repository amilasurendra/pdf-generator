using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Model = DocGen.ObjectModel;


namespace DocGen.OOXML
{
    /// <summary>
    /// Responsible for adding relevent style parts to the docx package.
    /// </summary>
    public class StyleCreator
    {
        private static int numberingId = 0;

        /// <summary>
        /// Add styles needed for chapter headings
        /// </summary>
        public static void AddStylePart() {
            StyleDefinitionsPart part = DocumentPackager.GetInstance().GetStylePart();
            AddNewStyle(part, "Heading1" ,"Heading1");
        }

        /// <summary>
        /// Adds the style for provided heading type if it's not already added.
        /// </summary>
        /// <param name="headingType"></param>
        public static void AddHeadingStyle(Model.HeadingType headingType) {

            if (IsStylePresent(StyleIds.GetStyleId(headingType))) {
                return;
            }

            int outlineLevel;

            switch (headingType)
            {
                case DocGen.ObjectModel.HeadingType.H1:
                    outlineLevel = 0;
                    break;
                case DocGen.ObjectModel.HeadingType.H2:
                    outlineLevel = 1;
                    break;
                case DocGen.ObjectModel.HeadingType.H3:
                    outlineLevel = 2;
                    break;
                case DocGen.ObjectModel.HeadingType.H4:
                    outlineLevel = 3;
                    break;
                case DocGen.ObjectModel.HeadingType.H5:
                    outlineLevel = 4;
                    break;
                case DocGen.ObjectModel.HeadingType.H6:
                    outlineLevel = 5;
                    break;
                default:
                    throw new NotImplementedException(headingType + ": Enum value not added");
            }


            Word.Styles styles =  DocumentPackager.GetInstance().GetStylePart().Styles;

            Word.Style style1 = new Word.Style() { Type = Word.StyleValues.Paragraph, StyleId = StyleIds.GetStyleId(headingType) };
            Word.BasedOn basedOn1 = new Word.BasedOn() { Val = "Normal" };

            Word.StyleParagraphProperties styleParagraphProperties1 = new Word.StyleParagraphProperties();
            Word.KeepNext keepNext1 = new Word.KeepNext();
            Word.KeepLines keepLines1 = new Word.KeepLines();
            Word.SpacingBetweenLines spacingBetweenLines1 = new Word.SpacingBetweenLines() { Before = "480", After = "0" };
            Word.OutlineLevel outlineLevel1 = new Word.OutlineLevel() { Val = outlineLevel };

            styleParagraphProperties1.Append(keepNext1);
            styleParagraphProperties1.Append(keepLines1);
            styleParagraphProperties1.Append(spacingBetweenLines1);
            styleParagraphProperties1.Append(outlineLevel1);

            style1.Append(basedOn1);
            style1.Append(styleParagraphProperties1);


            // Add the style to the styles part.
            styles.Append(style1);

        }


        public static int AddNumberingStyle(bool ordered, String symbol) {

            numberingId++;

            //TODO:Check numbering style existance before adding new one
            Word.Numbering numbering = DocumentPackager.GetInstance().GetNumberingPart().Numbering;
            numbering.Append(GenerateAbstractNum(numberingId, ordered, symbol));
            numbering.Append(GenerateNumberingInstance(numberingId));

            return numberingId;
        }


        private static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart, string styleid, string stylename)
        {
            // Get access to the root element of the styles part.
            Word.Styles styles = styleDefinitionsPart.Styles;

            Word.Style style1 = new Word.Style() { Type = Word.StyleValues.Paragraph, StyleId = "Heading1" };
            Word.StyleName styleName1 = new Word.StyleName() { Val = "heading 1" };
            Word.BasedOn basedOn1 = new Word.BasedOn() { Val = "Normal" };

            Word.StyleParagraphProperties styleParagraphProperties1 = new Word.StyleParagraphProperties();
            Word.KeepNext keepNext1 = new Word.KeepNext();
            Word.KeepLines keepLines1 = new Word.KeepLines();
            Word.SpacingBetweenLines spacingBetweenLines1 = new Word.SpacingBetweenLines() { Before = "480", After = "0" };
            Word.OutlineLevel outlineLevel1 = new Word.OutlineLevel() { Val = 0 };

            styleParagraphProperties1.Append(keepNext1);
            styleParagraphProperties1.Append(keepLines1);
            styleParagraphProperties1.Append(spacingBetweenLines1);
            styleParagraphProperties1.Append(outlineLevel1);

            style1.Append(styleName1);
            style1.Append(basedOn1);
            style1.Append(styleParagraphProperties1);


            // Add the style to the styles part.
            styles.Append(style1);
        }

        private static bool IsStylePresent(String styleId) {
            // Get access to the Styles element for this document.
            Styles s =  DocumentPackager.GetInstance().GetStylePart().Styles;

            // Check that there are styles and how many.
            int n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleid.
            Style style = s.Elements<Style>()
                .Where(st => (st.StyleId == styleId) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
                return false;

            return true;
        }

        private static string GetStyleIDFromName(string styleName) {
            StyleDefinitionsPart stylePart = DocumentPackager.GetInstance().GetStylePart();
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                    (((Style)s.Parent).Type == StyleValues.Paragraph))
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }

        private static AbstractNum GenerateAbstractNum(int id, bool ordered, String symbol)
        {
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = id };
            Nsid nsid1 = new Nsid() { Val = "1FAB1E90" };
            MultiLevelType multiLevelType1 = new MultiLevelType() { Val = MultiLevelValues.SingleLevel };
            TemplateCode templateCode1 = new TemplateCode() { Val = "5BB0F638" };

            Level level1 = new Level() { LevelIndex = 0, TemplateCode = "04090001" };
            StartNumberingValue startNumberingValue1 = new StartNumberingValue() { Val = 1 };

            NumberingFormat numberingFormat1 = new NumberingFormat();
            LevelText levelText1 = new LevelText();


            if (ordered) { 
                numberingFormat1.Val = NumberFormatValues.Decimal;
                if (symbol == null) symbol = ".";
                levelText1.Val = "%1" + symbol;
            }else { 
                numberingFormat1.Val = NumberFormatValues.Bullet;
                if (symbol == null) symbol = "-";
                levelText1.Val = symbol;
            }


            LevelJustification levelJustification1 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties1 = new PreviousParagraphProperties();
            Indentation indentation1 = new Indentation() { Left = "720", Hanging = "360" };

            previousParagraphProperties1.Append(indentation1);

            NumberingSymbolRunProperties numberingSymbolRunProperties1 = new NumberingSymbolRunProperties();
            //RunFonts runFonts1 = new RunFonts() { Hint = FontTypeHintValues.Default, Ascii = "Symbol", HighAnsi = "Symbol" };

            //numberingSymbolRunProperties1.Append(runFonts1);

            level1.Append(startNumberingValue1);
            level1.Append(numberingFormat1);
            level1.Append(levelText1);
            level1.Append(levelJustification1);
            level1.Append(previousParagraphProperties1);
            level1.Append(numberingSymbolRunProperties1);

            abstractNum1.Append(nsid1);
            abstractNum1.Append(multiLevelType1);
            abstractNum1.Append(templateCode1);
            abstractNum1.Append(level1);
            return abstractNum1;
        }

        private static NumberingInstance GenerateNumberingInstance(int id)
        {
            NumberingInstance numberingInstance1 = new NumberingInstance() { NumberID = id };
            AbstractNumId abstractNumId1 = new AbstractNumId() { Val = id };

            numberingInstance1.Append(abstractNumId1);
            return numberingInstance1;
        }



    }
}
