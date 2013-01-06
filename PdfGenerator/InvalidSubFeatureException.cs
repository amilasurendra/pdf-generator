using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen
{
    /// <summary>
    /// Throws when a invalid subfeature is specified for a parent feature.
    /// </summary>
    public class InvalidSubFeatureException : InvalidInputException
    {
        public String Parent { get; private set; }
        public String Child { get; private set; }

        public InvalidSubFeatureException() : base("Invalid child feature specified") { }

        public InvalidSubFeatureException(String parent, String child):base(parent + " can't contain " + child + " elements."){
            this.Parent = parent;
            this.Child = child;
        }

        public InvalidSubFeatureException(String message) : base(message) { }

        public InvalidSubFeatureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
