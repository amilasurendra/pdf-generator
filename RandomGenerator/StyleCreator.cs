using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class StyleCreator
    {
        private DocumentTree document;
        private Randomizer randomizer;
        private ConfigFacade configFacade;
        private Random randomNumber;
        private Defaults defaults;


        public StyleCreator(ConfigFacade config, Randomizer randomizer)
        {
            randomNumber = new Random();
            configFacade = config;
            this.randomizer = randomizer;
            defaults = new Defaults();
        }


        public void AddStyles(DocumentTree document) {
            this.document = document;
            AddRandomStyles();
            AddSpecificStyles();
        }


        private void AddRandomStyles() {

            AddStyle(document.Root);
        }


        private void AddStyle(Feature feature){

            IEnumerable<StyleInfo> stylesConstraints = randomizer.GetRandomStyleSubset(feature.Name);

            float styleAddingProb = Config.Default.StyleAddProb;

            float randomVal = (float) randomNumber.NextDouble();


            //Only add styles to fraction of features
            if (randomVal < styleAddingProb)
            {
                List<Style> possibleStyles = new List<Style>();

                foreach (var item in stylesConstraints)
                {
                    possibleStyles.Add(StyleFactory.CreateStyle(item, defaults));
                }

                foreach (var style in possibleStyles)
                {
                    feature.AddStyle(style);
                }
            }

            
            foreach (var subFeature in feature.GetChildFeatures())
            {
                AddStyle(subFeature);
            }
        }


        private void AddSpecificStyles() {

            AddSpecificStyle(document.Root);
        
        }


        private void AddSpecificStyle(Feature feature) {

            String path = document.GetNodePath(feature);
            IEnumerable<StyleInfo> styleConstraints = configFacade.GetStylesPartial(path);

            List<Style> specificStyles = new List<Style>();

            foreach (var item in styleConstraints)
            {
                specificStyles.Add(StyleFactory.CreateStyle(item, defaults));
            }


            foreach (var style in specificStyles)
            {
                feature.AddStyle(style, true); //Should we check style compatibility here
            }

            foreach (var subFeature in feature.GetChildFeatures())
            {
                AddSpecificStyle(subFeature);
            }
        }

        

    }
}
