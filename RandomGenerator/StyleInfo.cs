using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class StyleInfo : ICloneable
    {
        public String Name { get; set; }
        public String Feature { get; set; }
        public StyleType Type { get; set; }
        public String Value { get; set; }

        public String MinValue { get; set; }
        public String MaxValue { get; set; }
        public IEnumerable<String> PossibleValues { get; set; }

        public object Clone()
        {
            StyleInfo clonedObj = new StyleInfo();

            clonedObj.Name = this.Name;
            clonedObj.Feature = this.Feature;
            clonedObj.Value = this.Value;
            clonedObj.MinValue = this.MinValue;
            clonedObj.MaxValue = this.MaxValue;
            clonedObj.Type = this.Type;
            clonedObj.PossibleValues = this.PossibleValues;

            return clonedObj;
        }
    }
}
