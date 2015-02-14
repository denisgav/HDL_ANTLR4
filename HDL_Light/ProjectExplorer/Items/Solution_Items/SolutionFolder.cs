using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Папка, которая групирует проекты
    /// </summary>
    [Serializable]
    public class SolutionFolder : SolutionElementBase, IElementProvider, IDropable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public SolutionFolder(string path, SolutionElementBase parent)
            : base(path, parent)
        {
            parentSolutionElement = parent;
            subFolders = new List<SolutionFolder>();
            projects = new List<Project>();
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
        private SolutionElementBase parentSolutionElement;

        /// <summary>
        /// Дочерние элементы
        /// </summary>
        private List<SolutionFolder> subFolders;
        public List<SolutionFolder> SubFolders
        {
            get { return subFolders; }
        }

        /// <summary>
        /// Вложенные проекты
        /// </summary>
        private List<Project> projects;
        public List<Project> Projects
        {
            get { return projects; }
        }
        

        public override List<ProjectElementBase> Childrens
        {
            get
            {
                /*
                List<ProjectElementBase> res = new List<ProjectElementBase>();
                res.AddRange(subFolders);
                res.AddRange(projects);
                */

                List<ProjectElementBase> res = new List<ProjectElementBase>();
                res.AddRange(subFolders);
                res.AddRange(projects);
                res.Sort((x, y) => x.Caption.CompareTo(y.Caption));
                res = res.OrderBy(x => x.GetType().Name).ToList();
                return res;
            }
        }

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
        /// Сохранение папки
        /// </summary>
        public override void Save()
        {
            if (System.IO.Directory.Exists(path) == false)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            foreach (SolutionFolder el in subFolders)
                el.Save();
            foreach (Project proj in projects)
                proj.Save();
        }

        #region Context Menu
        /// <summary>
        /// Функция для создания контекстного меню
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        private static ContextMenu CreateContextMenu(SolutionFolder folder, ProjectExplorerControl control)
        {
            ContextMenu res = new ContextMenu();

            MenuItem itemNew = new MenuItem();
            itemNew.Header = "New";
            res.Items.Add(itemNew);

            MenuItem itemNewSolutionFolder = new MenuItem();
            itemNewSolutionFolder.Header = "New Solution Folder";
            itemNewSolutionFolder.Click += new System.Windows.RoutedEventHandler(folder.itemNewSolutionFolder_Click);
            itemNew.Items.Add(itemNewSolutionFolder);

            MenuItem itemNewProject = new MenuItem();
            itemNewProject.Header = "New Project";
            itemNewProject.Click += new System.Windows.RoutedEventHandler(folder.itemNewProject_Click);
            itemNew.Items.Add(itemNewProject);

            MenuItem itemAddExistingProject = new MenuItem();
            itemAddExistingProject.Header = "Add Existing Project";
            itemAddExistingProject.Click += new System.Windows.RoutedEventHandler(folder.itemAddExistingProject_Click);
            res.Items.Add(itemAddExistingProject);

            MenuItem itemRename = new MenuItem();
            itemRename.Header = "Rename";
            itemRename.Click += new System.Windows.RoutedEventHandler(folder.itemRename_Click);
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

            MenuItem itemOpenFolderInExplorer = new MenuItem();
            itemOpenFolderInExplorer.Header = "Open Folder In Explorer";
            itemOpenFolderInExplorer.Click += new System.Windows.RoutedEventHandler(folder.itemOpenFolderInExplorer_Click);
            res.Items.Add(itemOpenFolderInExplorer);

            return res;
        }

        private void itemAddExistingProject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingProject(this);
        }

        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            base.Cut_CanExecute(sender, e);
        }

        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e) { }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ClipboardBufferData.CanGetFromClipboard() == true)
            {
                ClipboardBufferData data = ClipboardBufferData.GetFromClipboard();
                ClipboardBufferData.SendToClipboard(data);

                bool res = CanPaste(data);
                if (res == true)
                {
                    e.CanExecute = true;
                }
            }
        }

        private bool CanPaste(ClipboardBufferData data)
        {
            bool res1 = (data.GroupType == GroupType.ProjectsAndSolutionsFolder);
            if(res1 == false)
                return false;
            foreach (ProjectElementBase paste_elem in data.Elements)
            {
                if (paste_elem is SolutionFolder)
                {
                    foreach (ProjectElementBase child in (paste_elem as SolutionFolder).GetSolutionItemsRecursive())
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
            if (data.GroupType == GroupType.ProjectsAndSolutionsFolder)
            {
                //1. Удаление элементов (логически)
                foreach (ProjectElementBase elem in data.Elements)
                    RemoveElementFromSolution(elem.Path, SchematixCore.Core.Solution);

                //2. Добавление элементов в текущую папку(логически)
                AddElementRange(data.Elements);
            }
            SchematixCore.Core.UpdateExplorerPanel();
        }

        private void itemOpenFolderInExplorer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomOpenInWindowsExplorer();
        }

        private void itemRename_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomRenameHandler(sender, e);
        }

        private void itemNewProject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Schematix.Dialogs.NewProjectDialog dialog = new Schematix.Dialogs.NewProjectDialog(this, SchematixCore.Core.Solution);
            dialog.ShowDialog();
                //HDL_LightCore.Core.Solution = dialog.Solution;
        }

        private void itemNewSolutionFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SolutionFolder folder = new SolutionFolder(System.IO.Path.Combine(Path, GenerateFileName("NewFolder")), this);
            folder.Save();
            (parent as SolutionFolder).SubFolders.Add(folder);
            Save();
            SchematixCore.Core.Solution.Save();
            SchematixCore.Core.UpdateExplorerPanel();
            expanded = true;
            folder.Rename();
        }
        #endregion

        /// <summary>
        /// Перечисляем вложенные компоненты
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<ProjectElementBase> GetEnumerator()
        {
            foreach (SolutionFolder el in subFolders)
                yield return el;
            foreach (Project proj in projects)
                yield return proj;
        }

        /// <summary>
        /// Удаление
        /// </summary>
        public override void Remove()
        {
            try
            {
                var res = System.Windows.MessageBox.Show(string.Format("Remove folder {0} from file system permanently?", path), "Remove folder", System.Windows.MessageBoxButton.YesNoCancel);
                if (res == System.Windows.MessageBoxResult.Yes)
                {
                    //уладить с проекта,
                    //удалить папку
                    (parent as SolutionFolder).SubFolders.Remove(this);
                    SchematixCore.Core.SaveSolution();
                    SchematixCore.Core.UpdateExplorerPanel();
                    if (System.IO.Directory.Exists(path) == true)
                        System.IO.Directory.Delete(path);
                }
                if (res == System.Windows.MessageBoxResult.No)
                {
                    //только удалить с проекта
                    (parent as SolutionFolder).SubFolders.Remove(this);
                    SchematixCore.Core.SaveSolution();
                    SchematixCore.Core.UpdateExplorerPanel();
                }
                if (res == System.Windows.MessageBoxResult.Cancel)
                {
                    //Ничего не делать, пользователь передумал
                }
            }
            catch (System.IO.IOException ex)
            {
                Schematix.Core.Logger.Log.Error("Remove error.", ex);
                System.Windows.MessageBox.Show(string.Format("Could not Remove file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);
            }
        }

        protected override void OnCaptionChange(string oldPath)
        {
            try
            {
                System.IO.Directory.Move(oldPath, path);
            }
            catch (System.IO.IOException ex)
            {
                Schematix.Core.Logger.Log.Error("OnCaptionChange error.", ex);
                System.Windows.MessageBox.Show(string.Format("Could not rename file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);

            }
            SchematixCore.Core.Solution.Save();
            base.OnCaptionChange(oldPath);
        }

        #region IElementProvider
        public void AddElement(ProjectElementBase element)
        {
            if (element is Project)
            {
                Projects.Add(element as Project);
                element.Parent = this;
                return;
            }

            if (element is SolutionFolder)
            {
                SubFolders.Add(element as SolutionFolder);
                element.Parent = this;
                return;
            }
            throw new Exception("Not correct type of item");
        }

        public void AddElementRange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if (e is Project)
                {
                    Projects.Add(e as Project);
                    e.Parent = this;
                    continue;
                }

                if (e is SolutionFolder)
                {
                    SubFolders.Add(e as SolutionFolder);
                    e.Parent = this;
                    continue;
                }
                throw new Exception("Not correct type of item");
            }
        }

        public bool CanAddElement(ProjectElementBase element)
        {
            return ((element is Project) || (element is SolutionFolder));
        }

        public bool CanAddElementrange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if (((e is Project) || (e is SolutionFolder)) == false)
                    return false;
            }
            return true;
        }


        public void RemoveElement(ProjectElementBase element)
        {
            if (element is Project)
            {
                Projects.Remove(element as Project);
                return;
            }

            if (element is SolutionFolder)
            {
                SubFolders.Remove(element as SolutionFolder);
                return;
            }
            throw new Exception("Not correct type of item");
        }

        public void RemoveElementRange(IList<ProjectElementBase> elements)
        {
            foreach (ProjectElementBase e in elements)
            {
                if (e is Project)
                {
                    Projects.Remove(e as Project);
                    continue;
                }

                if (e is SolutionFolder)
                {
                    SubFolders.Remove(e as SolutionFolder);
                    continue;
                }
                throw new Exception("Not correct type of item");
            }
        }
        #endregion

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

        /// <summary>
        /// Получить список всех проектов (рекурсивный метод)
        /// </summary>
        /// <returns></returns>
        public List<Project> GetProjectListRecursive()
        {
            List<Project> res = new List<Project>();
            res.AddRange(projects);

            foreach (SolutionFolder folder in subFolders)
                res.AddRange(folder.GetProjectListRecursive());

            return res;
        }

        /// <summary>
        /// Получить список всех элементов решения (рекурсивный метод)
        /// </summary>
        /// <returns></returns>
        public List<ProjectElementBase> GetSolutionItemsRecursive()
        {
            List<ProjectElementBase> res = new List<ProjectElementBase>();
            res.Add(this);
            res.AddRange(projects);

            foreach (SolutionFolder folder in subFolders)
                res.AddRange(folder.GetSolutionItemsRecursive());

            return res;
        }
    }
}
