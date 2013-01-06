using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    class StyleFactory
    {
        static Random random = new Random();

        public static Style CreateStyle(StyleInfo style, Defaults defaults)
        {
            Style tmp = new Style();
            tmp.Name = style.Name;
            tmp.Type = style.Type;
            tmp.Value = style.Value;


            if (style.Value.Equals("")) {

                String generated = GenerateValue(style);

                if (generated == null || generated.Equals("")) generated = defaults.GetDefaultStyleValue(style);

                tmp.Value = generated;
            }


            return tmp;
        }

        private static String GenerateValue(StyleInfo styleInfo) {

            switch (styleInfo.Type)
            {
                case StyleType.INT:
                    return CreateInt(styleInfo);

                case StyleType.FLOAT:
                    return CreateFloat(styleInfo);

                case StyleType.ENUM:
                    return CreateEnum(styleInfo);

                case StyleType.BOOL:
                    return CreateBool();

                default:
                    return "";
            }
        
        }



        private static String CreateBool() {
            bool result =  random.NextDouble() > 0.5;
            return result.ToString();
        }


        private static String CreateInt(StyleInfo styleInfo)
        {
            int min = int.Parse(styleInfo.MinValue);
            int max = int.Parse(styleInfo.MaxValue);
            return random.Next(min, max + 1).ToString();
        }


        private static String CreateFloat(StyleInfo styleInfo)
        {
            float min = float.Parse(styleInfo.MinValue);
            float max = float.Parse(styleInfo.MaxValue);
            float randomDouble = (float)random.NextDouble();
            float result =  randomDouble * (max - min) + min;
            return result.ToString();
        }

        private static String CreateEnum(StyleInfo styleInfo)
        {
            int index = random.Next(styleInfo.PossibleValues.Count());
            return styleInfo.PossibleValues.ElementAt(index);
        }
    }

    

}
