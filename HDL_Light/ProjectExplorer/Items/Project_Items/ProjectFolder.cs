using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Schematix.Dialogs.NewFileDialogWizard;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Каталог внутри проекта
    /// </summary>
    [Serializable]
    public class ProjectFolder : ProjectElement, IElementProvider, IDropable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public ProjectFolder(string path, ProjectElement parent)
            : base(path, parent)
        {
            parentProjectElement = parent;
            childrens = new List<ProjectElement>();
        }

        /// <summary>
        /// Раскрыто/свернуто
        /// </summary>
        private bool expanded;
        public bool Expanded
        {
            get { return expanded; }
            set { expanded = value; }
        }

        /// <summary>
        /// Родительский элемент
        /// </summary>
        private ProjectElement parentProjectElement;

        /// <summary>
        /// Контекстное меню
        /// </summary>
        public override ContextMenu CreateElementContextMenu(ProjectExplorerControl control)
        {
            return CreateContextMenu(this, control);
        }

        /// <summary>
        /// Иконка
        /// </summary>
        public override string Icon
        {
            get { return "/HDL_Light;component/Images/Folder.png"; }
        }

        /// <summary>
        /// Дочерние элементы
        /// </summary>
        private List<ProjectElement> childrens;
        public List<ProjectElement> SubElements
        {
            get 
            {
                List<ProjectElement> res = new List<ProjectElement>(childrens);
                return res; 
            }
        }
        public override List<ProjectElementBase> Childrens
        {
            get
            {
                List<ProjectElementBase> res = new List<ProjectElementBase>();
                res.AddRange(Folders);
                res.AddRange(Files);
                return res;
            }
        }

        /// <summary>
        /// Папки
        /// </summary>
        public List<ProjectFolder> Folders
        {
            get
            {
                List<ProjectFolder> folders = GetProjectElements<ProjectFolder>();
                folders.Sort((x, y) => x.Caption.CompareTo(y.Caption));
                return folders;
            }
        }

        /// <summary>
        /// Файлы
        /// </summary>
        public List<ProjectElement> Files
        {
            get
            {
                List<ProjectElement> files = new List<ProjectElement>();

                foreach (ProjectElement el in childrens)
                    if ((el is ProjectFolder) == false)
                        files.Add(el);

                files.Sort((x, y) => x.Caption.CompareTo(y.Caption));
                files = files.OrderBy(x => x.GetType().Name).ToList();

                return files;
            }
        }

        /// <summary>
        /// Отфильтровать элементы по типу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetProjectElements<T>() where T : ProjectElement
        {
            List<T> res = new List<T>();

            foreach (ProjectElement el in childrens)
                if (el is T)
                    res.Add(el as T);

            return res;
        }

        /// <summary>
        /// Сохранение папки на диск
        /// </summary>
        public override void Save()
        {
            if (System.IO.Directory.Exists(path) == false)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            foreach (ProjectElement el in childrens)
                el.Save();
        }

        /// <summary>
        /// Удаляем папку
        /// </summary>
        public override void Remove()
        {
            var res = System.Windows.MessageBox.Show(string.Format("Rempve folder {0} from file system permanently?", path), "Remove folder", System.Windows.MessageBoxButton.YesNoCancel);
            if (res == System.Windows.MessageBoxResult.Yes)
            {
                //уладить с проекта,
                //удалить папку
                (parent as ProjectFolder).RemoveElement(this);
                SchematixCore.Core.SaveSolution();
                SchematixCore.Core.UpdateExplorerPanel();
                if (System.IO.Directory.Exists(path) == true)
                    System.IO.Directory.Delete(path, true);
            }
            if (res == System.Windows.MessageBoxResult.No)
            {
                //только удалить с проекта
                (parent as ProjectFolder).RemoveElement(this);
                SchematixCore.Core.SaveSolution();
                SchematixCore.Core.UpdateExplorerPanel();
            }
            if (res == System.Windows.MessageBoxResult.Cancel)
            {
                //Ничего не делать, пользователь передумал
            }
        }

        #region ContextMenu
        /// <summary>
        /// Функция для создания контекстного меню
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        protected static ContextMenu CreateContextMenu(ProjectFolder elem, ProjectExplorerControl control)
        {
            ContextMenu res = new ContextMenu();

            MenuItem itemNew = new MenuItem();
            itemNew.Header = "New";
            res.Items.Add(itemNew);

            MenuItem itemNewItem = new MenuItem();
            itemNewItem.Header = "New Item";
            itemNewItem.Click += new System.Windows.RoutedEventHandler(elem.itemNewItem_Click);
            itemNew.Items.Add(itemNewItem);

            MenuItem itemNewFolder = new MenuItem();
            itemNewFolder.Header = "New Folder";
            itemNewFolder.Click += new System.Windows.RoutedEventHandler(elem.itemNewFolder_Click);
            itemNew.Items.Add(itemNewFolder);

            MenuItem itemAddExistingFile = new MenuItem();
            itemAddExistingFile.Header = "Add Existing Files";
            itemAddExistingFile.Click += new System.Windows.RoutedEventHandler(elem.itemAddExistingFile_Click);
            res.Items.Add(itemAddExistingFile);

            MenuItem itemAddExistingFolder = new MenuItem();
            itemAddExistingFolder.Header = "Add Existing Folder";
            itemAddExistingFolder.Click += new System.Windows.RoutedEventHandler(elem.itemAddExistingFolder_Click);
            res.Items.Add(itemAddExistingFolder);

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

        private void itemAddExistingFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingFolder(this as ProjectFolder);
        }

        private void itemAddExistingFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingItems(this as ProjectFolder);
        }

        private void itemNewFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProjectFolder folder = new ProjectFolder(System.IO.Path.Combine(Path, GenerateFileName("NewFolder")), this);
            folder.Save();
            AddElement(folder);
            Save();
            expanded = true;
            SchematixCore.Core.UpdateExplorerPanel();
            
            folder.Rename();
        }

        private void itemNewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {            
            AddNewFile dial = new AddNewFile(this);
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SchematixCore.Core.UpdateExplorerPanel();            
        }
        #endregion

        protected override void OnCaptionChange(string oldPath)
        {
            try
            {
                System.IO.Directory.Move(oldPath, path);
                SchematixCore.Core.Solution.Save();
            }
            catch (System.IO.IOException ex)
            {
                Schematix.Core.Logger.Log.Error("OnCaptionChange error.", ex);
                System.Windows.MessageBox.Show(string.Format("Could not rename file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);
            }
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ClipboardBufferData.CanGetFromClipboard() == true)
            {
                ClipboardBufferData data = ClipboardBufferData.GetFromClipboard();
                ClipboardBufferData.SendToClipboard(data);

                bool res = CanPaste(data);

                if (res == true)
                    e.CanExecute = true;
            }
        }

        private bool CanPaste(ClipboardBufferData data)
        {
            bool res1 = (data.GroupType == GroupType.ProjectElements);
            if (res1 == false)
                return false;
            foreach (ProjectElementBase paste_elem in data.Elements)
            {
                if (paste_elem is ProjectFolder)
                {
                    foreach (ProjectElementBase child in (paste_elem as ProjectFolder).GetProjectFilesRecursive())
                    {
                        if (child.Path == Path)
                            return false;
                    }
                }
            }
            foreach (ProjectElementBase elem in Childrens)
            {
                foreach (ProjectElementBase paste_elem in data.Elements)
                {
                    if ((paste_elem.Path.Equals(elem.Path)) || (paste_elem.Path.Equals(Path)))
                        return false;
                }
            }
            return true;
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ClipboardBufferData.CanGetFromClipboard() == true)
            {
                ClipboardBufferData data = ClipboardBufferData.GetFromClipboard();
                Paste(data);
            }
        }

        private void Paste(ClipboardBufferData data)
        {
            if (data.GroupType == GroupType.ProjectElements)
            {
                try
                {
                    //1. В зависимости от операции - переместить или скопировать элементы физически
                    if (data.OperationType == ClipboardOperationType.Copy)
                    {
                        CopyElementsToCurrentFolder(data);
                    }

                    if (data.OperationType == ClipboardOperationType.Cut)
                    {
                        MoveElementsToCurrentFolder(data);
                    }

                    //2. Добавление элементов в текущую папку(логически)
                    Project.Compiler.UpdateFileList();
                    SchematixCore.Core.UpdateExplorerPanel();
                }
                catch (System.IO.IOException ex)
                {
                    Schematix.Core.Logger.Log.Error("Paste error.", ex);
                    System.Windows.MessageBox.Show(string.Format("Paste error.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);
                }
            }
            
        }

        /// <summary>
        /// Скопировать элементы в текущую папку
        /// </summary>
        /// <param name="data"></param>
        private void CopyElementsToCurrentFolder(ClipboardBufferData data)
        {
            foreach (ProjectElementBase elem in data.Elements)
            {
                if (elem is ProjectFolder)
                {
                    FileOperations.DirectoryCopy((elem as ProjectFolder), this);
                }
                else
                {
                    if(elem is ProjectElement)
                        FileOperations.FileCopy((elem as ProjectElement), this);
                }
            }
        }

        /// <summary>
        /// Переместить элементы в текущую папку
        /// </summary>
        /// <param name="data"></param>
        private void MoveElementsToCurrentFolder(ClipboardBufferData data)
        {
            foreach (ProjectElementBase elem in data.Elements)
            {
                RemoveElementFromSolution(elem.Path, SchematixCore.Core.Solution);

                if (elem is ProjectFolder)
                {
                    FileOperations.DirectoryMove((elem as ProjectFolder), this);
                }
                else
                {
                    if (elem is ProjectElement)
                        FileOperations.FileMove((elem as ProjectElement), this);
                }
            }
        }        

        #region IElementProvider
        public void AddElement(ProjectElementBase element)
        {
            if (element is ProjectElement)
            {
                childrens.Add(element as ProjectElement);
                element.Parent = this;
            }
            else
                throw new Exception("Not correct type of item");
        }

        public void AddElementRange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if (e is ProjectElement)
                {
                    childrens.Add(e as ProjectElement);
                    e.Parent = this;
                }
                else
                    throw new Exception("Not correct type of item");
            }
        }

        public bool CanAddElement(ProjectElementBase element)
        {
            return (element is ProjectElement);
        }

        public bool CanAddElementrange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if ((e is ProjectElement) == false)
                    return false;
            }
            return true;
        }


        public void RemoveElement(ProjectElementBase element)
        {
            if (element is ProjectElement)
                childrens.Remove(element as ProjectElement);
            else
                throw new Exception("Not correct type of item");
        }

        public void RemoveElementRange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if (e is ProjectElement)
                    childrens.Remove(e as ProjectElement);
                else
                    throw new Exception("Not correct type of item");
            }
        }
        #endregion

        /// <summary>
        /// Получить список всех проектов (рекурсивный метод)
        /// </summary>
        /// <returns></returns>
        public List<ProjectElement> GetProjectFilesRecursive()
        {
            List<ProjectElement> res = new List<ProjectElement>();
            res.Add(this);
            foreach (ProjectElement elem in childrens)
            {
                if (!(elem is ProjectFolder))
                    res.Add(elem);
                else
                    res.AddRange((elem as ProjectFolder).GetProjectFilesRecursive());
            }

            return res;
        }

        /// <summary>
        /// Получить все элементы проекта
        /// </summary>
        /// <returns></returns>
        public List<ProjectElement> GetProjectElementsRecursive()
        {
            List<ProjectElement> res = new List<ProjectElement>();
            foreach (ProjectElement elem in childrens)
            {
                res.Add(elem);
                if(elem is ProjectFolder)
                    res.AddRange((elem as ProjectFolder).GetProjectFilesRecursive());
            }

            return res;
        }

        #region IDropable
        public bool CanDrop(ClipboardBufferData data)
        {
            return CanPaste(data);
        }

        public void Drop(ClipboardBufferData data)
        {
            Paste(data);
        }
        #endregion
    }
}