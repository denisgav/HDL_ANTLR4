using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Schematix.Waveform.Value_Dump;

namespace Schematix.Waveform.UserControls
{
    delegate Point GetPositionDelegate(IInputElement element);
    public delegate void UpdateView();

    public class ListViewDragDropManager :IDisposable
    {
        /// <summary>
        /// элемент управления, над которым выполяется контроль
        /// </summary>
        private TreeListView list;

        /// <summary>
        /// предыдущий индекс
        /// </summary>
        int oldIndex = -1;

        public TreeListView List
        {
            get { return list; }
            set { list = value; }
        }

        public event UpdateView UpdateViewEvent;

        private WaveformCore core;

        DragEventHandler dragEventHandler;

        public ListViewDragDropManager(WaveformCore core, TreeListView list)
        {
            this.core = core;
            this.list = list;

            list.AllowDrop = true;

            dragEventHandler = new DragEventHandler(ListView_Drop);
            list.Drop += dragEventHandler;
        }

        // function called during drop operation
        void ListView_Drop(object sender, DragEventArgs e)
        {
            if (oldIndex < 0)
                return;

            int index = this.GetCurrentIndex(e.GetPosition);

            if (index < 0)
                return;

            if (index == oldIndex)
                return;

            object variable = list.Items[oldIndex];
            list.Items.RemoveAt(oldIndex);
            list.Items.Insert(index, variable);

            My_Variable mv = core.CurrentDump[oldIndex];
            core.CurrentDump.RemoveAt(oldIndex);
            core.CurrentDump.Insert(index, mv);

            UpdateViewEvent();
        }

        public void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Выполняем перетягивание
            oldIndex = this.GetCurrentIndex(e.GetPosition);

            if (oldIndex < 0)
                return;

            //(list.Items[oldIndex] as TreeViewItem).IsSelected = true;
            TreeViewItem variable = this.list.Items[oldIndex] as TreeViewItem;

            if (variable == null)
                return;

            // this will create the drag "rectangle"
            DragDropEffects allowedEffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(this.list, variable, allowedEffects) != DragDropEffects.None)
            {
                // The item was dropped into a new location,
                // so make it the new selected item.
                //this.list.SelectedItem = variable;
            }
        }

        TreeViewItem GetListViewItem(int index)
        {
            if (list.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return list.ItemContainerGenerator.ContainerFromIndex(index) as TreeViewItem;
        }

        // returns the index of the item in the ListView
        int GetCurrentIndex(GetPositionDelegate getPosition)
        {
            int index = -1;
            for (int i = 0; i < this.list.Items.Count; ++i)
            {
                TreeViewItem item = GetListViewItem(i);
                if (this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        bool IsMouseOverTarget(TreeViewItem item, GetPositionDelegate getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(item);
            Point mousePos = getPosition(item);
            return bounds.Contains(mousePos);
        }

        #region IDisposable Members

        public void Dispose()
        {
            list.Drop -= dragEventHandler;
        }

        #endregion
    }
}
