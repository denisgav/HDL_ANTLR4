using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Schematix_all;

namespace Schematix.Windows.MDIChild
{
    public class History
    {
        private List<HistoryElement> history;
        private HistoryElement currentElement;

        public void ClearHistory()
        {
            history.Clear();
        }

        private SchemaUserControl parent;

        public bool canUndo
        {
            get
            {
                int index = history.IndexOf(currentElement);
                return (index > 0);
            }
        }
        public bool canRedo
        {
            get
            {
                int index = history.IndexOf(currentElement);
                return (index < history.Count - 1);
            }
        }

        public History(SchemaUserControl parent)
        {
            this.parent = parent;
            Init();
        }

        private void Init()
        {
            this.history = new List<HistoryElement>();
            this.MakeSnapshot();
        }

        public void RegisterSave()
        {
            SetAsUnsavedToAll();
            if (currentElement != null)
                currentElement.IsSaved = true;
        }

        private MemoryStream GetUndoStream()
        {
            if (this.canUndo)
            {
                MemoryStream resStream = null;
                int index = history.IndexOf(currentElement);
                if (index <= 0)
                    resStream = currentElement.Stream;
                if (index > 0)
                {
                    currentElement = history.ElementAt<HistoryElement>(index - 1);
                    resStream = currentElement.Stream;
                }
                return resStream;
            }
            return null;
        }

        public void Undo()
        {
            MemoryStream data = GetUndoStream();
            if (data != null)
                parent.OpenUndoFromHistory(data);
        }

        private MemoryStream GetRedoStream()
        {
            if (this.canRedo)
            {
                MemoryStream resStream = null;
                int index = history.IndexOf(currentElement);
                if (index == history.Count - 1)
                    currentElement = history.Last<HistoryElement>();
                else
                {
                    if (index == -1)
                        currentElement = history.First<HistoryElement>();
                    else
                        currentElement = history.ElementAt<HistoryElement>(index + 1);
                }
                resStream = currentElement.Stream;
                return resStream;
            }
            return null;
        }

        public void Redo()
        {
            MemoryStream data = GetRedoStream();
            if (data != null)
            {
                parent.OpenRedoFromHistory(data);
            }
        }

        private bool MakeSnapshot()
        {
            MemoryStream currState = new MemoryStream();
            currState.Seek(0, SeekOrigin.Begin);
            bool result = this.parent.SaveToHistory(currState);
            currState.Seek(0, SeekOrigin.Begin);
            //currState.Close();
            if (!result)
            {
                return false;
            }
            HistoryElement item = new HistoryElement(currState, false);

            int index = -1;
            index = history.IndexOf(currentElement);
            if (index == history.Count - 1)
            {
                history.Add(item);
                currentElement = item;
                return true;
            }
            history.RemoveRange(index + 1, (history.Count - index - 1));
            history.Add(item);
            currentElement = item;
            return true;
        }

        public bool Changed()
        {            
            return MakeSnapshot();
        }

        private void SetAsUnsavedToAll()
        {
            foreach (HistoryElement el in history)
            {
                el.IsSaved = false;
            }
        }

        public bool isModified
        {
            get { return ((currentElement != null) && (currentElement.IsSaved == false)); }
        }
    }

    public class HistoryElement
    {
        private bool isSaved;
        public bool IsSaved
        {
            get { return isSaved; }
            set { isSaved = value; }
        }

        public MemoryStream Stream
        {
            get 
            {
                MemoryStream res = new MemoryStream(data);
                return res; 
            }
            set 
            {
                value.Seek(0, SeekOrigin.Begin);
                data = value.ToArray();
            }
        }

        private byte[] data;
        public byte[] Data
        {
            get { return data; }
        }
        

        public HistoryElement(MemoryStream stream, bool isSaved = false)
        {
            this.Stream = stream;
            this.isSaved = isSaved;
        }        
    }
}
