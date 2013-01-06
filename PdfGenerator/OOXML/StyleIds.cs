using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.OOXML
{
    /// <summary>
    /// Maps style Ids with styles
    /// </summary>
    public class StyleIds
    {
        /// <summary>
        /// Returns ID of styles that should be used in document packaging for a given heading type
        /// </summary>
        /// <param name="headingType"></param>
        /// <returns></returns>
        public static String GetStyleId(DocGen.ObjectModel.HeadingType headingType) {
            return headingType.ToString();
        }
    }
}
