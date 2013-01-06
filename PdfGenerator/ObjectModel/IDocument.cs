using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DocGen.ObjectModel
{
    /// <summary>
    /// Represents main interface for document creation
    /// </summary>
    public interface IDocument
    {
        bool AutoGenerateTOC { get; set; }
        
        void AddSection(Section section);
        void AddSection(MultiColumnSection section);
        void AddChapter(Chapter chapter);
        void AddMetaData(MetaData data);
        void AddElement(Element element);
        void Save();
    }
}
