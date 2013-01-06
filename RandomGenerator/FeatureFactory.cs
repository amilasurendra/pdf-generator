using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public class FeatureFactory
    {
        public static Feature CreateFeature(String featureName, IDataProvider dataProvider)
        {

            Feature tmp = new Feature();
            tmp.Name = featureName;


            tmp.Value = dataProvider.GetSampleData(featureName);
            return tmp;

        }
    }
}
