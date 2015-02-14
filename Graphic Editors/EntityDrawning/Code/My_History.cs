using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public class My_History
    {
        private List<My_Picture> history;
        private My_Picture current;
        private My_Picture savedItem;
        private EntityDrawningCore core;

        public void ClearHistory()
        {
            history.Clear();
        }

        public My_History(EntityDrawningCore core)
        {
            this.core = core;
            history = new List<My_Picture>();
            current = core.Picture.Clone();
            savedItem = current;
            history.Add(current);
        }

        public bool IsSaved()
        {
            return current == savedItem;
        }

        public void SetAsSaved()
        {
            savedItem = current;
        }

        public bool canUndo()
        {
            int index = history.IndexOf(current);
            return (index > 0);
        }

        public bool canRedo()
        {
            int index = history.IndexOf(current);
            return (index < history.Count - 1);
        }

        public My_Picture Undo()
        {
            int index = history.IndexOf(current);
            if (index > 0)
            {
                current = history.ElementAt<My_Picture>(index - 1);
            }
            return current.Clone();
        }

        public My_Picture ReDo()
        {
            int index = history.IndexOf(current);
            if (index == history.Count - 1)
                current = history.Last<My_Picture>();
            if (index == -1)
                current = history.First<My_Picture>();
            current = history.ElementAt<My_Picture>(index + 1);
            return current.Clone();
        }

        public void add(My_Picture item)
        {
            int index = history.IndexOf(current);
            current = item.Clone();

            if (index == history.Count - 1)
            {
                history.Add(current);
                return;
            }
            history.RemoveRange(index + 1, (history.Count - index - 1));
            history.Add(current);
        }
    }
}