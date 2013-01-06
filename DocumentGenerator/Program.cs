using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RandomGenerator;
using DocGen;
using DocumentGenerator;
using System.IO;
using System.Threading;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace DocumentGen
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                int input = 1;

                if (args != null && args.Length > 0)
                {

                    try
                    {
                        input = int.Parse(args[0]);
                    }
                    catch (FormatException e) { }
                }

                new Program().CreateDocuments(input);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                log.Error("File not found", e);
            }
            catch (DirectoryNotFoundException e) {
                Console.WriteLine(e.Message);
                log.Error("Directory not found", e);
            }

            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
        }


        private void CreateDocuments(int count) {

            CreateDirectory("Xml");
            CreateDirectory("Pdf");

            for (int i = 1; i <= count; i++)
            {
                CreateDocument(i + ".pdf");
            }
            
        }


        private void CreateDirectory(String name) {

            DirectoryInfo dirInfo = new DirectoryInfo(name);
            if (!dirInfo.Exists) dirInfo.Create();

            foreach (FileInfo file in dirInfo.EnumerateFiles())
            {
                file.Delete();
            }
        }

        
        private void MakePdf(String input, String output) {
            Generator test = new Generator(input, output, "./Config/Defaults.xml");
            test.Generate();
        }


        private void CreateDocument(String fileName) {

            DataProvider dataProvider = new DataProvider();
            DocumentCreator test = new DocumentCreator(dataProvider, "./Xml/"+fileName+".xml", "./Config/RunConfig.xml", "./Config/DocFeatures.xml");
            test.CreateDocument();


            Console.WriteLine(fileName + ".xml created");

            try
            {
                MakePdf("./Xml/" + fileName + ".xml", "./Pdf/" + fileName);
                Console.WriteLine(fileName + " created");
            }
            catch (InvalidSubFeatureException e)
            {
                Console.WriteLine("Error generating file. Invalid subfeature specified. See log for details.");
                log.Info("Invalid subfeature specified", e);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine("PDF generation error, Invalid input. See log for details.");
                log.Info(e);
            }
        }


    }
}
