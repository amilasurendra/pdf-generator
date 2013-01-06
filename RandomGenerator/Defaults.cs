using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class Defaults
    {
        public Range GetDefualtElementCount(String parent, String Child) {
            //TODO: Get data from a deafults.xml and implement
            Range tmp = new Range();
            tmp.minValue = 0;
            tmp.maxValue = 1;
            return tmp;
        }

        public int GetDefaultNestingLevel(String parent) {
            return 1;
        }


        public String GetDefaultStyleValue(StyleInfo style) {

            switch (style.Type)
            {
                case StyleType.INT: return "100";

                case StyleType.BOOL: return "false";

                case StyleType.FLOAT : return "20.5";

                default:
                    return "";
            }
                        
        }
    }
}
