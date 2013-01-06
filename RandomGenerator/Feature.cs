using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    /// <summary>
    /// Defines a feature of a document
    /// </summary>
    public class Feature
    {
        private List<Feature> subElements;
        private Dictionary<String, Style> styles;


        public Feature()
        {
            subElements = new List<Feature>();
            styles = new Dictionary<String,Style>();
        }

        public String Name { get; set; }

        public String Value { get; set; }

        public Feature Parent { get; private set; }

        public bool SubFeaturesCompleted { get; set; }

        
        public void AddStyle(Style style) {

            if (styles.ContainsKey(style.Name)) return;

            styles.Add(style.Name,style);
        }


        public void AddStyle(Style style, bool replace)
        {
            if (replace)
            {
                styles.Remove(style.Name);
                styles.Add(style.Name, style);
            }

            else AddStyle(style);
            
        }


        public IEnumerable<Style> GetStyles(){
            foreach (var style in styles.Values)
            {
                yield return style;
            }
        }

        public void AddSubElement(Feature subElement) {
            subElements.Add(subElement);
            subElement.Parent = this;
        }

        public IEnumerable<Feature> GetChildFeatures() {
            return subElements.AsEnumerable();
        }


        public int GetSubFeatureCount()
        {
            return subElements.Count;
        }

        public int GetSubFeatureCount(String featureName) {

            int count = 0;

            foreach (var feature in subElements)
            {
                if (feature.Name.Equals(featureName)) {
                    count++;
                }
            }
            return count;
        }

        //Testing
        public void ShuffleChilds() { 
            Random rnd = new Random();

            subElements = subElements.OrderBy<Feature, int>((item) => rnd.Next()).ToList();
        }
    }
}
