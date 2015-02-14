using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    public class Graph_History
    {
        private List<HistoryElem> history;
        public List<HistoryElem> History
        {
            get { return history; }
        }

        private HistoryElem elem;
        public HistoryElem CurrentElement
        {
            get { return elem; }
            set { elem = value; }
        }

        Schematix.FSM.Constructor_Core core;

        public Graph_History(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            history = new List<HistoryElem>();
            this.elem = new HistoryElem(core.Graph.Clone(), "Start value");
            history.Add(elem);
        }

        /// <summary>
        /// Можно ли отменить действие
        /// </summary>
        /// <returns></returns>
        public bool СanUndo()
        {
            int index = history.IndexOf(elem);
            return (index>0);
        }

        /// <summary>
        /// Можно ли повторить действие
        /// </summary>
        /// <returns></returns>
        public bool СanRedo()
        {
            int index = history.IndexOf(elem);
            return (index<history.Count-1);
        }

        /// <summary>
        /// Отменить действие
        /// </summary>
        /// <returns></returns>
        public My_Graph UnDo()
        {
            Schematix.FSM.My_Graph graph = null;
            int index = history.IndexOf(elem);
            if (index <= 0)
                graph = elem.Graph;
            if (index > 0)
            {
                elem = history.ElementAt<HistoryElem>(index - 1);
                graph = elem.Graph;
            }
            return graph.Clone();
        }

        /// <summary>
        /// Повторить действие
        /// </summary>
        /// <returns></returns>
        public My_Graph ReDo()
        {
            Schematix.FSM.My_Graph graph = null;
            int index = history.IndexOf(elem);
            if (index == history.Count - 1)
                elem = history.Last<HistoryElem>();
            else
            {
                if (index == -1)
                    elem = history.First<HistoryElem>();
                else
                    elem = history.ElementAt<HistoryElem>(index + 1);
            }
            graph = elem.Graph;
            return graph.Clone();
        }

        public void add(HistoryElem item)
        {
            int index = history.IndexOf(elem);

            if (index == history.Count - 1)
            {
                history.Add(item);
                elem = item;
                return;
            }
            history.RemoveRange(index + 1, (history.Count - index - 1));
            history.Add(item);
            elem = item;
        }

        public void SetPosition(string name)
        {
            foreach (Schematix.FSM.HistoryElem history_elem in history)
            {
                if (history_elem.Name == name)
                {
                    int index = history.IndexOf(history_elem);
                    elem = history.ElementAt<HistoryElem>(index);
                    core.Graph = history_elem.Graph;
                    core.Bitmap.UpdateBitmap();
                    core.form.Invalidate();
                    return;
                }
            }
        }

        public void SetAsSaved()
        {
            foreach (Schematix.FSM.HistoryElem history_elem in history)
            {
                history_elem.IsSaved = (history_elem == elem);
            }
        }       
    }
}