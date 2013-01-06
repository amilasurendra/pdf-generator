using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    /// <summary>
    /// Main interface for using the library
    /// </summary>
    public class DocumentCreator
    {
        private DocumentTree docTree;
        private Randomizer randomGen;
        private Defaults defualts;
        private Random randomNumberGen;
        private XmlWriter xmlWriter;
        private StyleCreator styleCreator;
        private FeatureCreator featureCreator;
        private RunConfig runConfig;
        private ConfigFacade config;

        DocFeatures docFeatures;


        public DocumentCreator(IDataProvider dataProvider, String outputFilePath, String runConfigPath, String docFeaturesPath)
        {
            docTree = new DocumentTree();

            randomNumberGen = new Random();
            defualts = new Defaults();
            xmlWriter = new XmlWriter(outputFilePath);
            runConfig = new RunConfig(runConfigPath, defualts);


            docFeatures = new DocFeatures(docFeaturesPath);
            config = new ConfigFacade(runConfig, docFeatures, defualts);
            randomGen = new Randomizer(config);
            styleCreator = new StyleCreator(config, randomGen);
            featureCreator = new FeatureCreator(config, dataProvider, randomGen);

        }


        public void CreateDocument()
        {
            docTree = featureCreator.CreateFeatures();

            styleCreator.AddStyles(docTree);

            xmlWriter.WriteXml(docTree);
        }

    }
}
