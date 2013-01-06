using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    internal class FeatureCreator
    {
        private Randomizer randomGen;
        ConfigFacade configuration;
        DocumentTree docTree;
        IDataProvider DataProvider;
        Random randomNumberGen;

        public FeatureCreator(ConfigFacade configuration, IDataProvider dataProvider, Randomizer randomGen)
        {
            docTree = new DocumentTree();
            randomNumberGen = new Random();
            this.configuration = configuration;
            this.DataProvider = dataProvider;
            this.randomGen = randomGen;
        }


        public DocumentTree CreateFeatures()
        {
            while (!IsFeaturesCompleted())
            {
                ProcessLeaves();
            }

            return docTree;
        }


        private void ProcessLeaves()
        {
            IEnumerable<Feature> leafNodes = docTree.GetLeafNodes();

            foreach (var node in leafNodes)
            {
                ProcessLeaf(node);
            }

        }


        private void ProcessLeaf(Feature node)
        {
            if (node.SubFeaturesCompleted) return;

            IEnumerable<String> randomFeatures = randomGen.GetRandomFeatureList(node.Name);


            foreach (var randomFeature in randomFeatures)
            {
                //Limiting depth of recursive features
                if (docTree.GetNestedDepth(node, randomFeature) >= configuration.GetMaxNestingDepth(randomFeature))
                {
                    continue;
                }

                int childCount = GetChildCount(node.Name, randomFeature);

                for (int i = 0; i < childCount; i++)
                {
                    if (node.GetSubFeatureCount(randomFeature) < childCount)
                    {
                        node.AddSubElement(FeatureFactory.CreateFeature(randomFeature, DataProvider));
                    }
                }
            }

            node.ShuffleChilds();

            node.SubFeaturesCompleted = true;
        }


        private int GetChildCount(String node, String randomFeature)
        {
            Range featureCount = configuration.GetSubFeatureCount(node, randomFeature);
            return randomNumberGen.Next(featureCount.minValue, featureCount.maxValue + 1);
        }


        /// <summary>
        /// Returns true if:
        /// * All leaves are complete or
        /// * Max depth occured
        /// </summary>
        private bool IsFeaturesCompleted()
        {
            Feature[] leafNodes = docTree.GetLeafNodes().ToArray();

            foreach (var node in leafNodes)
            {
                if (!node.SubFeaturesCompleted) return false;
            }

            return true;
        }

    }
}
