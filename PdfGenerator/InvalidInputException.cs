using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen
{
    /// <summary>
    /// Throws when an error is found in input XML
    /// </summary>
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base("Error in input XML.") { }

        public InvalidInputException(String message) : base(message) { }

        public InvalidInputException(string message, Exception innerException) : base(message, innerException) { }
    }
}
