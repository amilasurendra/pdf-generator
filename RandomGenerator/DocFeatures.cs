using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RandomGenerator
{
    /// <summary>
    /// Provides the structure of the document
    /// Relations between elements
    /// styles supported for each feature
    /// </summary>
    public class DocFeatures
    {
        private XmlDocument featureDoc;
        private XmlNode root;

        private Dictionary<String, StyleType> styleTypesMap;

        public DocFeatures(String xmlPath)
        {
            featureDoc = new XmlDocument();
            featureDoc.Load(xmlPath);
            root = featureDoc.DocumentElement;

            styleTypesMap = new Dictionary<string, StyleType> { 
                {"string", StyleType.STRING},
                {"int" , StyleType.INT},
                {"enum" , StyleType.ENUM},
                {"float" , StyleType.FLOAT},
                {"boolean", StyleType.BOOL}
            };
        }

        /// <summary>
        /// Returns list of all child features possible for a given parent.
        /// </summary>
        /// <param name="parent">name of the parent features</param>
        /// <returns>list of child features</returns>
        public IEnumerable<String> GetChildFeatures(String parent) {

            XmlNodeList childFeatures = root.SelectNodes("/docFeatures/relations/relation[@parent='"+parent+"']");

            foreach (var node in childFeatures)
            {
                if (!(node is XmlElement)) { throw new Exception("Xml config error"); } //TODO : Change

                XmlElement tmp = (XmlElement)node;

                yield return tmp.GetAttribute("child");
            }

        }


        /// <summary>
        /// Returns a list of styles possible for a given feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public IEnumerable<Style> GetSupportedStyles(String feature) {

            XmlNodeList styles = root.SelectNodes("/docFeatures/styles/feature[@name='"+feature+"']/*");

            foreach (var node in styles)
            {
                if (!(node is XmlElement)) { throw new Exception("Xml config error"); } //TODO : Change

                XmlElement tmp = (XmlElement)node;
                Style tmpStyle = new Style();
                tmpStyle.Name = tmp.Name;
                tmpStyle.Type = styleTypesMap[ tmp.GetAttribute("type") ];


                yield return tmpStyle;
            }
        }

        /// <summary>
        /// Returns whether a given style is supported under the given feature
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public bool IsStyleSupported(String feature, String style) {

            XmlNode result = root.SelectSingleNode("/docFeatures/styles/feature[@name='"+feature+"']/"+ style);
            return result != null;
        }


        /// <summary>
        /// Returns whether a given child feature is supported under the given parent.
        /// </summary>
        /// <param name="parentFeature"></param>
        /// <param name="childFeature"></param>
        /// <returns></returns>
        public bool IsChildfeature(String parentFeature, String childFeature) {

            XmlNode relation = root.SelectSingleNode("/docFeatures/relations/relation[@parent='"+parentFeature+"' and @child='"+childFeature+"']");
            return relation != null;
        }


        /// <summary>
        /// Returns the type of the style under a given feature.(sevaral features can have styles with same name)
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public StyleType GetStyleType(String styleName, String featureName)
        {

            XmlNode style = root.SelectSingleNode("/docFeatures/styles/feature[@name='"+featureName+"']/" + styleName);

            if (style == null) throw new InvalidOperationException("Style "+styleName+" not defined on " + featureName);

            String result =  ((XmlElement)style).GetAttribute("type");

            return styleTypesMap[result];
        }


        /// <summary>
        /// returns possible values for a given style, feature pair
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="featureName"></param>
        /// <returns></returns>
        public IEnumerable<String> GetPossibleValues(String styleName, String featureName)
        {
            XmlNode style = root.SelectSingleNode("/docFeatures/styles/feature[@name='" + featureName + "']/" + styleName);

            if (style == null) throw new InvalidOperationException("Style " + styleName + " not defined on " + featureName);

            String values = ((XmlElement)style).GetAttribute("values");
            return values.Split(',');
        }



        /// <summary>
        /// Returns list of all features a given style can come under
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public IEnumerable<String> GetParentFeatures(String styleName) {

            XmlNodeList styles = root.SelectNodes("/docFeatures/styles/feature/" + styleName);

            foreach (XmlNode style in styles)
            {
                XmlElement tmp = (XmlElement)style.ParentNode;
                yield return tmp.GetAttribute("name");
            }
        }
    }

}
