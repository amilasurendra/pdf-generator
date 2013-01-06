using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class Randomizer
    {
        Random random;
        ConfigFacade configFacade;

        public Randomizer(ConfigFacade config)
        {
            random = new Random();
            configFacade = config;
        }


        /// <summary>
        /// Returns all possible sub-features for a given feature in random order.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<String> GetRandomFeatureList(String parent) {

            List<String> tmp = new List<string>(configFacade.GetAllChildFeatures(parent));
            return Shuffle(tmp);
        }


        public IEnumerable<StyleInfo> GetRandomStyles(String feature) {


            List<StyleInfo> tmp = new List<StyleInfo>(configFacade.GetStyleList(feature));
            return Shuffle(tmp);
        }


        public IEnumerable<StyleInfo> GetRandomStyleSubset(String feature)
        {
            //TODO : Complete
            IEnumerable<StyleInfo> tmp = GetRandomStyles(feature);

            int styleCount = random.Next(tmp.Count() + 1);

            for (int i = 0; i < styleCount; i++)
            {
                yield return tmp.ElementAt(i);
            }
        }



        private List<T> Shuffle<T> (List<T> source) {
            for (int i = 0; i < source.Count; i++)
                Swap(source, i, random.Next(source.Count));
            return source;
        }


        private void Swap<T>(List<T> list, int i, int j) {
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

    }
}
