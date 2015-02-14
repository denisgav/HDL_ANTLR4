using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Schematix.ProjectExplorer
{
    public class TreeViewExtensions : DependencyObject
    {
        public static bool GetEnableMultiSelect(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableMultiSelectProperty);
        }

        public static void SetEnableMultiSelect(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableMultiSelectProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableMultiSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableMultiSelectProperty =
            DependencyProperty.RegisterAttached("EnableMultiSelect", typeof(bool), typeof(TreeViewExtensions), new FrameworkPropertyMetadata(false)
            {
                PropertyChangedCallback = EnableMultiSelectChanged,
                BindsTwoWayByDefault = true
            });

        public static ObservableCollection<TreeViewItem> GetSelectedItems(DependencyObject obj)
        {
            return (ObservableCollection<TreeViewItem>)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, ObservableCollection<TreeViewItem> value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(ObservableCollection<TreeViewItem>), typeof(TreeViewExtensions), new PropertyMetadata(null));



        static TreeViewItem GetAnchorItem(DependencyObject obj)
        {
            return (TreeViewItem)obj.GetValue(AnchorItemProperty);
        }

        static void SetAnchorItem(DependencyObject obj, TreeViewItem value)
        {
            obj.SetValue(AnchorItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnchorItem.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty AnchorItemProperty =
            DependencyProperty.RegisterAttached("AnchorItem", typeof(TreeViewItem), typeof(TreeViewExtensions), new PropertyMetadata(null));



        static void EnableMultiSelectChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
        {
            TreeView tree = (TreeView)s;
            var wasEnable = (bool)args.OldValue;
            var isEnabled = (bool)args.NewValue;
            if (wasEnable)
            {
                tree.RemoveHandler(TreeViewItem.MouseUpEvent, new MouseButtonEventHandler(ItemClicked));
                tree.RemoveHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
            }
            if (isEnabled)
            {
                tree.AddHandler(TreeViewItem.MouseUpEvent, new MouseButtonEventHandler(ItemClicked), true);
                tree.AddHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
            }
        }

        static TreeView GetTree(TreeViewItem item)
        {
            Func<DependencyObject, DependencyObject> getParent = (o) => VisualTreeHelper.GetParent(o);
            FrameworkElement currentItem = item;
            while (!(getParent(currentItem) is TreeView))
                currentItem = (FrameworkElement)getParent(currentItem);
            return (TreeView)getParent(currentItem);
        }



        static void RealSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TreeViewItem item = (TreeViewItem)sender;
            var selectedItems = GetSelectedItems(GetTree(item));
            if (selectedItems != null)
            {
                var isSelected = GetIsSelected(item);
                if (isSelected)
                    try
                    {
                        selectedItems.Add(item);
                    }
                    catch (ArgumentException)
                    {
                    }
                else
                    selectedItems.Remove(item);
            }
        }

        static void KeyDown(object sender, KeyEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            if (e.Key == Key.A && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                foreach (var item in GetExpandedTreeViewItems(tree))
                {
                    SetIsSelected(item, true);
                }
                e.Handled = true;
            }
        }

        static void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = FindTreeViewItem(e.OriginalSource);
            if (item == null)
                return;
            TreeView tree = (TreeView)sender;

            var mouseButton = e.ChangedButton;
            if (mouseButton != MouseButton.Left)
            {
                if ((mouseButton == MouseButton.Right) && ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == ModifierKeys.None))
                {
                    if (GetIsSelected(item))
                    {
                        UpdateAnchorAndActionItem(tree, item);
                        return;
                    }
                    MakeSingleSelection(tree, item);
                }
                return;
            }
            if (mouseButton != MouseButton.Left)
            {
                if ((mouseButton == MouseButton.Right) && ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == ModifierKeys.None))
                {
                    if (GetIsSelected(item))
                    {
                        UpdateAnchorAndActionItem(tree, item);
                        return;
                    }
                    MakeSingleSelection(tree, item);
                }
                return;
            }
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != (ModifierKeys.Shift | ModifierKeys.Control))
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    MakeToggleSelection(tree, item);
                    return;
                }
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    MakeAnchorSelection(tree, item, true);
                    return;
                }
                var selectedItems = GetSelectedItems(tree);
                selectedItems.Clear();
                MakeSingleSelection(tree, item);
                return;
            }
            //MakeAnchorSelection(item, false);


            //SetIsSelected(tree.SelectedItem
        }

        private static TreeViewItem FindTreeViewItem(object obj)
        {
            DependencyObject dpObj = obj as DependencyObject;
            if (dpObj == null)
                return null;
            if (dpObj is TreeViewItem)
                return (TreeViewItem)dpObj;
            return FindTreeViewItem(VisualTreeHelper.GetParent(dpObj));
        }



        private static IEnumerable<TreeViewItem> GetExpandedTreeViewItems(ItemsControl tree)
        {
            for (int i = 0; i < tree.Items.Count; i++)
            {
                var item = (TreeViewItem)tree.ItemContainerGenerator.ContainerFromIndex(i);
                if (item == null)
                    continue;
                yield return item;
                if (item.IsExpanded)
                    foreach (var subItem in GetExpandedTreeViewItems(item))
                        yield return subItem;
            }
        }

        private static void MakeAnchorSelection(TreeView tree, TreeViewItem actionItem, bool clearCurrent)
        {
            if (GetAnchorItem(tree) == null)
            {
                var selectedItems = GetSelectedTreeViewItems(tree);
                if (selectedItems.Count > 0)
                {
                    SetAnchorItem(tree, selectedItems[selectedItems.Count - 1]);
                }
                else
                {
                    SetAnchorItem(tree, GetExpandedTreeViewItems(tree).Skip(3).FirstOrDefault());
                }
                if (GetAnchorItem(tree) == null)
                {
                    return;
                }
            }

            var anchor = GetAnchorItem(tree);

            var items = GetExpandedTreeViewItems(tree);
            bool betweenBoundary = false;
            bool end = false;
            foreach (var item in items)
            {
                bool isBoundary = item == anchor || item == actionItem;
                if (isBoundary)
                {
                    betweenBoundary = !betweenBoundary;
                }
                if (betweenBoundary || isBoundary)
                    SetIsSelected(item, true);
                else
                    if (clearCurrent)
                        SetIsSelected(item, false);
                    else
                        break;

            }
        }

        private static List<TreeViewItem> GetSelectedTreeViewItems(TreeView tree)
        {
            return GetExpandedTreeViewItems(tree).Where(i => GetIsSelected(i)).ToList();
        }

        private static void MakeSingleSelection(TreeView tree, TreeViewItem item)
        {
            foreach (TreeViewItem selectedItem in GetExpandedTreeViewItems(tree))
            {
                if (selectedItem == null)
                    continue;
                if (selectedItem != item)
                    SetIsSelected(selectedItem, false);
                else
                {
                    SetIsSelected(selectedItem, true);
                }
            }
            UpdateAnchorAndActionItem(tree, item);
        }

        private static void MakeToggleSelection(TreeView tree, TreeViewItem item)
        {
            SetIsSelected(item, !GetIsSelected(item));
            UpdateAnchorAndActionItem(tree, item);
        }

        private static void UpdateAnchorAndActionItem(TreeView tree, TreeViewItem item)
        {
            SetAnchorItem(tree, item);
        }




        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(TreeViewExtensions), new PropertyMetadata(false)
            {
                PropertyChangedCallback = RealSelectedChanged
            });

        // Helper to search up the VisualTree
        public static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }

        public static TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        public static bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            {
                _isEqual = true;
            }
            return _isEqual;

        }


    }
}
