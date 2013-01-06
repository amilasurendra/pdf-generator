using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;

namespace DocGen.OOXML
{
    /// <summary>
    /// Provides common format conversions used in OOXML
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Convert points to half-point measurement values
        /// </summary>
        /// <param name="points">size in points</param>
        /// <returns>size in half-point value</returns>
        public static StringValue GetHPSValue(float points) {
            return new StringValue((points * 2).ToString());
        }

        /// <summary>
        /// Convert points to dxa(twentieths of a point) measurement values
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static StringValue GetDxaFromPoints(float points) {
            return new StringValue((points * 20).ToString());
        }

        
    }
}
