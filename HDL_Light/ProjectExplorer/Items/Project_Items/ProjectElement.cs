using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Schematix.Dialogs.NewFileDialogWizard;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, хранящий информацию об элементе проекта
    /// </summary>
    [Serializable]
    public abstract class ProjectElement : ProjectElementBase
    {
        /// <summary>
        /// Коструктор для элемента, чье имя не совадает с именем файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caption"></param>
        /// <param name="parent"></param>
        public ProjectElement(string path, string caption, ProjectElementBase parent)
            :base (path, caption, parent)
        {
            removed = false;
        }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public ProjectElement(string path, ProjectElementBase parent)
            : base(path, parent)
        {
            removed = false;
        }

        /// <summary>
        /// Идентификатор того, что элемент был удален из проекта
        /// </summary>
        private bool removed;
        public bool Removed
        {
            get { return removed; }
            set { removed = value; }
        }
        

        #region Context Menu
        /// <summary>
        /// Функция для создания контекстного меню
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        protected static ContextMenu CreateContextMenu(ProjectElement elem, ProjectExplorerControl control)
        {
            ContextMenu res = new ContextMenu();            

            MenuItem itemRename = new MenuItem();
            itemRename.Header = "Rename";
            itemRename.Click += new System.Windows.RoutedEventHandler(elem.itemRename_Click);
            res.Items.Add(itemRename);

            MenuItem itemRemove = new MenuItem();
            itemRemove.Header = "Remove";
            itemRemove.Command = control.CommandRemove;
            itemRemove.CommandBindings.Add(control.BindingRemoveCommand);
            res.Items.Add(itemRemove);

            

            MenuItem itemPaste = new MenuItem();
            itemPaste.Header = "Paste";
            itemPaste.Icon = CreateImageByPath("/HDL_Light;component/Images/Paste.png");
            itemPaste.Command = control.CommandPaste;
            itemPaste.CommandBindings.Add(control.BindingPasteCommand);
            res.Items.Add(itemPaste);

            
            MenuItem itemСut = new MenuItem();
            itemСut.Header = "Cut";
            itemСut.Icon = CreateImageByPath("/HDL_Light;component/Images/Cut.png");
            itemСut.Command = control.CommandCut;
            itemСut.CommandBindings.Add(control.BindingCutCommand);
            res.Items.Add(itemСut);

            
            MenuItem itemCopy = new MenuItem();
            itemCopy.Header = "Copy";
            itemCopy.Icon = CreateImageByPath("/HDL_Light;component/Images/Copy.png"); ;
            itemCopy.Command = control.CommandCopy;
            itemCopy.CommandBindings.Add(control.BindingCopyCommand);
            res.Items.Add(itemCopy);

            MenuItem itemOpenFolderInExplorer = new MenuItem();
            itemOpenFolderInExplorer.Header = "Open Folder In Explorer";
            itemOpenFolderInExplorer.Click += new System.Windows.RoutedEventHandler(elem.itemOpenFolderInExplorer_Click);
            res.Items.Add(itemOpenFolderInExplorer);

            return res;
        }

        private void itemRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Remove();
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e) { }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e) { }                    

        public override void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        public override void Copy_Executed(object sender, ExecutedRoutedEventArgs e) {}

        protected void itemOpenFolderInExplorer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomOpenInWindowsExplorer();
        }

        protected void itemRename_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomRenameHandler(sender, e);
        }

        #endregion

        /// <summary>
        /// Перегрузка функции для файла
        /// Для папки будет использоваться другая функция (рекурсивная)
        /// </summary>
        public override void Remove()
        {
            var res = System.Windows.MessageBox.Show(string.Format("Rempve file {0} from file system permanently?", path), "Remove file", System.Windows.MessageBoxButton.YesNoCancel);
            if (res == System.Windows.MessageBoxResult.Yes)
            {
                //уладить с проекта,
                //удалить папку
                (parent as ProjectFolder).RemoveElement(this);
                SchematixCore.Core.SaveSolution();
                SchematixCore.Core.UpdateExplorerPanel();
                if (System.IO.File.Exists(path) == true)
                    System.IO.File.Delete(path);
                Project.Compiler.UpdateFileList();
                removed = true;
            }
            if (res == System.Windows.MessageBoxResult.No)
            {
                //только удалить с проекта
                (parent as ProjectFolder).RemoveElement(this);
                SchematixCore.Core.SaveSolution();
                SchematixCore.Core.UpdateExplorerPanel();
                Project.Compiler.UpdateFileList();
                removed = true;
            }
            if (res == System.Windows.MessageBoxResult.Cancel)
            {
                //Ничего не делать, пользователь передумал
            }
        }


        /// <summary>
        /// Получение ссылки на проект, которому принадлежит элемент
        /// </summary>
        public Project Project
        {
            get 
            {
                ProjectElement elem = this;
                for (int i = 0; i < 100; i++)
                {
                    if (elem is Project)
                        return elem as Project;
                    if (elem == null)
                        return null;
                    elem = elem.parent as ProjectElement;
                }
                return null;
            }
        }


        protected override void OnCaptionChange(string oldPath)
        {
            try
            {
                System.IO.File.Move(oldPath, path);
            }
            catch (System.IO.IOException ex)
            {
                Schematix.Core.Logger.Log.Error("OnCaptionChange error.", ex);
                System.Windows.MessageBox.Show(string.Format("Could not rename file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);
            }

            SchematixCore.Core.Solution.Save();
            base.OnCaptionChange(oldPath);
        }
    }
}
