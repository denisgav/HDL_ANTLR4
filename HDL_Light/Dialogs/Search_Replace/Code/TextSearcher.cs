using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Dialogs.Search_Replace.Code
{    
    public class TextSearcher
    {
        private SchematixCore core;
        private SimpleSearcher searcher;
        public SimpleSearcher Searcher
        {
            get { return searcher; }
        }

        public List<SearchSource> curProjFiles;
        private List<SearchSource> CurProjFiles
        {
            get { return curProjFiles; }
        }

        public List<SearchSource> solutionFiles;
        private List<SearchSource> SolutionFiles
        {
            get { return solutionFiles; }
        }

        private SearchSource currentDocument;
        public SearchSource CurrentDocument
        {
            get { return currentDocument; }
        }

        private List<SearchSource> allOpenedDocuments;
        public List<SearchSource> AllOpenedDocuments
        {
            get { return allOpenedDocuments; }
        }
        
        

        public TextSearcher()
            : this(SchematixCore.Core, null)
        { }

        public TextSearcher(Schematix.Windows.Code.Code code)
            : this(SchematixCore.Core, code)
        { }

        public TextSearcher(SchematixCore core, Schematix.Windows.Code.Code code)
        {
            this.core = core;

            searcher = new SimpleSearcher(string.Empty, string.Empty, 0);
            solutionFiles = new List<SearchSource>();
            curProjFiles = new List<SearchSource>();
            allOpenedDocuments = new List<SearchSource>();
        }

        private void LoadTextForSearch()
        {
            
            if ((core.Solution != null))
            {
                solutionFiles.Clear();
                foreach (Schematix.ProjectExplorer.Project proj in core.Solution.ProjectList)
                {
                    foreach (Schematix.ProjectExplorer.VHDL_Code_File vhdlFile in proj.GetProjectElements<Schematix.ProjectExplorer.VHDL_Code_File>())
                    {
                        string text = GetTextFromProjElement(vhdlFile);
                        solutionFiles.Add(new SearchSource(text, vhdlFile.Path));
                    }
                }
                
            }
            if((core.Solution != null) && (core.Solution.CurrentSelectedProject != null))
            {
                curProjFiles.Clear();
                foreach (Schematix.ProjectExplorer.VHDL_Code_File vhdlFile in core.Solution.CurrentSelectedProject.GetProjectElements<Schematix.ProjectExplorer.VHDL_Code_File>())
                {
                    string text = GetTextFromProjElement(vhdlFile);
                    curProjFiles.Add(new SearchSource(text, vhdlFile.Path));
                }
            }
            if ((core.MainWindow.CurrentActiveWindow != null) && (core.MainWindow.CurrentActiveWindow is Schematix.Windows.Code.Code))
            {
                string path = (core.MainWindow.CurrentActiveWindow as Schematix.Windows.Code.Code).ProjectElement.Path;
                string text = (core.MainWindow.CurrentActiveWindow as Schematix.Windows.Code.Code).EditorText;
                currentDocument = new SearchSource(text, path);                
            }
            if (core.MainWindow.CurrentActiveWindow != null)
            {
                allOpenedDocuments.Clear();
                foreach (Schematix.Windows.SchematixBaseWindow window in core.OpenedWindows().ToList())
                    if ((window != null) && (window is Schematix.Windows.Code.Code))
                    {
                        string path = (window as Schematix.Windows.Code.Code).ProjectElement.Path;
                        string text = (window as Schematix.Windows.Code.Code).EditorText;
                        SearchSource res = new SearchSource(text, path);
                        allOpenedDocuments.Add(res);
                    }
            }
        }

        private string GetTextFromProjElement(Schematix.ProjectExplorer.VHDL_Code_File code)
        {
            foreach (Schematix.Windows.SchematixBaseWindow w in core.OpenedWindows().ToList())
            {
                if (w.ProjectElement == code)
                {
                    return (w as Schematix.Windows.Code.Code).EditorText;
                }
            }
            return null;
        }

        #region IEnumerableSearch
        private IEnumerable<SearchSource> FileEnumerator(SearchType searchType)
        {
            switch (searchType)
            {
                case SearchType.CurrentDocument:
                    {
                        yield return currentDocument;                            
                    }
                    break;
                case SearchType.AllOpenedDocuments:
                    {
                        foreach (SearchSource file in allOpenedDocuments)
                            yield return file;
                    }
                    break;
                case SearchType.CurrentProject:
                    {
                        foreach (SearchSource file in curProjFiles)
                            yield return file;
                    }
                    break;
                case SearchType.EntireSolution:
                    {
                        foreach (SearchSource file in solutionFiles)
                            yield return file;
                    }
                    break;
            }
        }

        private IEnumerable<SearchResult> FoundedSequence(SearchType searchType)
        {
            My_Editor.Document.ISegment segment;
            List<SearchSource> files = FileEnumerator(searchType).ToList();
            foreach (SearchSource file in files)
            {
                searcher.Text = file.Text;

                segment = searcher.FindNext();
                while (segment.Length != 0)
                {
                    yield return new SearchResult(file, segment);
                    segment = searcher.FindNext();
                }
            }
        }
        #endregion

        #region step by step search
        private SearchType currentSearchType;

        private int currentFileIndex;
        private SearchSource currentFile;
        private List<SearchSource> currentFileList;

        public void Reset(SearchType currentSearchType)
        {
            this.currentSearchType = currentSearchType;
            LoadTextForSearch();
            currentFileList = FileEnumerator(currentSearchType).ToList();
            currentFileIndex = 0;
            if (currentFileList.Count != 0)
            {
                currentFile = currentFileList[0];
                if (currentFile != null)
                {
                    searcher.Text = currentFile.Text;
                }
                searcher.StartOffset = 0;
            }
        }

        public void NextFile()
        {
            if ((currentFileList.Count > currentFileIndex) && (currentFileList[currentFileIndex] == currentFile))
            {
                currentFileIndex++;
                if (currentFileIndex < currentFileList.Count)
                {
                    currentFile = currentFileList[currentFileIndex];
                }
            }
            else
                Reset(currentSearchType);
        }

        public bool isEndOfFileSequence()
        {
            return currentFileIndex >= currentFileList.Count;
        }

        public SearchResult GetNextSearchResult()
        {
            if (isEndOfFileSequence() == true)
            {
                return null;
            }
            else
            {
                My_Editor.Document.ISegment segment = searcher.FindNext();
                searcher.StartOffset += segment.Length;
                while (segment.Length == 0)
                {
                    NextFile();
                    if (isEndOfFileSequence() == false)
                    {
                        string text = System.IO.File.ReadAllText(currentFile.Path);
                        searcher.Text = text;
                        searcher.StartOffset = 0;
                        segment = searcher.FindNext();                        
                        if (segment.Length != 0)
                            return new SearchResult(currentFile, segment);
                    }
                    else
                        return null;
                }
                return new SearchResult(currentFile, segment);
            }
        }

        #endregion

    }

    public enum SearchType
    {
        CurrentDocument,
        AllOpenedDocuments,
        CurrentProject,
        EntireSolution
    }

    public class SearchResult
    {
        private SearchSource code;
        public SearchSource Code
        {
            get { return code;}
        }

        private My_Editor.Document.ISegment segment;
        public My_Editor.Document.ISegment Segment
        {
            get {return segment;}
        }

        public SearchResult(SearchSource code, My_Editor.Document.ISegment segment)
        {
            this.code = code;
            this.segment = segment;
        }
    }

    public class SearchSource
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public SearchSource(string text, string path)
        {
            this.text = text;
            if (text == null)
                LoadText(path);
            this.path = path;
        }

        public SearchSource(string path)
        {
            this.path = path;
            this.text = LoadText(path);
        }

        private string LoadText(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.TextReader reader = new System.IO.StreamReader(stream);
                text = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            return string.Empty;
        }
    }
}
