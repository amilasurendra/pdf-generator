using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Diagnostics;
using System.Xml;

namespace RandomGenerator
{
    /// <summary>
    /// Provides features and styles that should be included for a given run.
    /// </summary>
    public class RunConfig{

        /// TODO: Limit to single instance of document (caching) 
        XmlNode root;
        Defaults defaults;


        public RunConfig(String fileName, Defaults defaults)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            root = doc.DocumentElement;
            this.defaults = defaults;
        }


        /// <summary>
        /// Returns list of features to be used for a run
        /// </summary>
        /// <returns>List of strings representing feature names</returns>
        public IEnumerable<String> GetFeatureList(){

            XmlNodeList features = root.SelectNodes("/config/features/*");

            foreach (var feature in features)
            {
                XmlElement tmp = (XmlElement)feature;
                yield return tmp.Name;
            }
        }


        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public int GetMaxNestedDepth(String feature) {

            XmlNode featureNode = root.SelectSingleNode("/config/features/" + feature);

            String nestingAttrib = ((XmlElement)featureNode).GetAttribute("nesting");

            int result;

            bool success = Int32.TryParse(nestingAttrib, out result);

            if (success) return result;

            else {
                return defaults.GetDefaultNestingLevel(feature);
            }
        }


        public IEnumerable<StyleInfo> GetStyleList(){

            XmlNodeList styles = root.SelectNodes("/config/styles/*");

            foreach (var style in styles)
            {
                XmlElement tmp = (XmlElement)style;
                yield return CreateStyleInfo(tmp);
            }

        }


        private StyleInfo CreateStyleInfo(XmlElement styleElement) {

            StyleInfo tmpStyle = new StyleInfo();
            tmpStyle.Name = styleElement.Name;
            tmpStyle.Value = styleElement.GetAttribute("value");
            tmpStyle.MinValue = styleElement.GetAttribute("minValue");
            tmpStyle.MaxValue = styleElement.GetAttribute("maxValue");

            return tmpStyle;
        }


        public Range GetElementCount(String parent, String Child) {

            XmlElement element = (XmlElement) root.SelectSingleNode("/config/constraints/"+parent+"/"+Child);
            if (element == null) return null;

            Range tmp = new Range();
            tmp.minValue = int.Parse( element.GetAttribute("minCount"));
            tmp.maxValue = int.Parse(element.GetAttribute("maxCount"));

            return tmp;
        }


        /// <summary>
        /// Returns all contraints for given path and all sub paths
        /// </summary>
        /// <param name="featurePath"></param>
        /// <returns></returns>
        public IEnumerable<StyleInfo> GetStyles(String featurePath) {

            XmlNodeList styles = root.SelectNodes("/config/specific" + featurePath +"/styles/*");

            foreach (var style in styles)
            {
                XmlElement tmp = (XmlElement)style;
                yield return CreateStyleInfo(tmp);
            }
        }


        public IEnumerable<StyleInfo> GetStylesPartial(String featurePath) {

            String[] features = featurePath.Split('/');
            List<StyleInfo> styleList = new List<StyleInfo>();
            StringBuilder pathBuilder = new StringBuilder();

            for (int i = features.Length-1 ; i >= 0; i--)
            {
                pathBuilder.Insert(0, features[i]);
                pathBuilder.Insert(0, '/');

                styleList.AddRange(GetStyles(pathBuilder.ToString()));
            }
            
            return styleList.AsEnumerable();
        }

    }


    public class Range {
        public int minValue;
        public int maxValue;
    }
    
}
