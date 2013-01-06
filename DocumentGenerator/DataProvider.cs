using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RandomGenerator;
using System.IO;

namespace DocumentGenerator
{
    public class DataProvider : IDataProvider
    {
        static int wordCount = 0;
        String imagePath = "Images";

        public DataProvider()
        {
            CreateImages();
        }


        private void CreateImages() {

            files = new List<String>();

            FileInfo[] filesList;

            DirectoryInfo directory = new DirectoryInfo(imagePath);

            filesList = directory.GetFiles();

            foreach (var item in filesList)
            {
                files.Add(item.FullName);
            }
        
        }

        public string GetSampleData(String featureName)
        {
            switch (featureName)
            {
                case "text": return GetWord();
                case "image": return GetImageUrl();
                case "listItem": return GetListItem();
                default: return null;
            }
        }


        private String GetWord()
        {
            wordCount++;
            return "word " + wordCount;
        }


        List<String> files;
        int fileIndex = 0;

        private String GetImageUrl()
        {
            if (files.Count == 0) return null;

            String filePath =  files[fileIndex];
            fileIndex = (fileIndex + 1) % files.Count;
            return filePath;

        }


        int listIndex = 0;

        private String GetListItem() {

            listIndex++;
            return "List Item " + listIndex;
        }
    }
}
