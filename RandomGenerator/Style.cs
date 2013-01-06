using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{

    public class Style
    {
        public String Name { get; set; }
        public StyleType Type { get; set; }
        public Object Value { get; set; }

        public Style(){}

        public Style(String name, StyleType type)
        {
            this.Name = name;
            this.Type = type;
        }

    }

    public enum StyleType { INT, FLOAT, STRING, ENUM , BOOL}

}
