using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    /// <summary>
    /// Provides a unified interface for accessing details of styles and features.
    /// Combines and simplifies interfaces provided in RunConfig, DocFeatures and Defaults classes.
    /// </summary>
    public class ConfigFacade
    {
        private RunConfig runConfig;
        private DocFeatures docFeatures;
        private Defaults defaultConfig;

        public ConfigFacade(RunConfig runConfig, DocFeatures docFeatures, Defaults defaults)
        {
            this.runConfig = runConfig;
            this.docFeatures = docFeatures;
            this.defaultConfig = defaults;
        }


        public IEnumerable<StyleInfo> GetStyles(String featurePath) {

            IEnumerable<StyleInfo> tmp = runConfig.GetStyles(featurePath); ;

            return FillTypes(tmp);
        }


        public IEnumerable<StyleInfo> GetStylesPartial(String featurePath)
        {

            IEnumerable<StyleInfo> tmp = runConfig.GetStylesPartial(featurePath); ;

            return FillTypes(tmp);
        }


        private IEnumerable<StyleInfo> GetStyleList() {

            IEnumerable<StyleInfo> tmp = runConfig.GetStyleList();

            return FillTypes(tmp);
        }


        public IEnumerable<StyleInfo> GetStyleList(String parentFeature)
        {

            var allStyles = GetStyleList();

            List<StyleInfo> tmp = new List<StyleInfo>();

            foreach (var style in allStyles)
            {
                if (docFeatures.IsStyleSupported(parentFeature, style.Name))
                {
                    tmp.Add(style);
                }
            }

            return tmp.AsEnumerable();
        }


        public int GetMaxNestingDepth(String feature) {
            return runConfig.GetMaxNestedDepth(feature);
        }


        public Range GetSubFeatureCount(String parent, String feature) {
            Range featureCount = runConfig.GetElementCount(parent, feature);

            if (featureCount == null)
            {
                featureCount = defaultConfig.GetDefualtElementCount(parent, feature);
            }

            return featureCount;
        }


        public IEnumerable<String> GetAllChildFeatures(String parent) {

            var allFeatures = runConfig.GetFeatureList();

            List<String> tmp = new List<String>();

            foreach (var feature in allFeatures)
            {
                if (docFeatures.IsChildfeature(parent, feature))
                {
                    tmp.Add(feature);
                }
            }

            return tmp.AsEnumerable();
        }


        

        /// <summary>
        /// Combines information on styles from docFeatures and runConfig
        /// </summary>
        /// <param name="styleInfoList"></param>
        /// <returns></returns>
        private IEnumerable<StyleInfo> FillTypes(IEnumerable<StyleInfo> styleInfoList)
        {

            List<StyleInfo> resultList = new List<StyleInfo>();

            foreach (var styleInfo in styleInfoList)
            {
                IEnumerable<String> features = docFeatures.GetParentFeatures(styleInfo.Name);

                foreach (var feature in features)
                {
                    StyleInfo tmp = (StyleInfo)styleInfo.Clone();
                    tmp.Feature = feature;
                    tmp.Type = docFeatures.GetStyleType(styleInfo.Name, feature);
                    tmp.PossibleValues = docFeatures.GetPossibleValues(styleInfo.Name, feature);

                    resultList.Add(tmp);

                }

            }

            return resultList.AsEnumerable();
        }
    }
}
