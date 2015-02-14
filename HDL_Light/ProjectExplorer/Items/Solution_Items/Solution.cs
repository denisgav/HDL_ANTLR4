using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Schematix.Dialogs;
using System.Windows.Input;
using System.Windows;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, представляющий решение
    /// </summary>
    [Serializable]
    public class Solution : SolutionElementBase, IElementProvider, IDropable
    {
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="path"></param>
        public Solution(string path)
            :base (path, null)
        {
            rootFolder = new SolutionFolder(System.IO.Path.GetDirectoryName(path), this);
            rootFolder.Save();
            extention = ".sol";
        }        
        
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public Solution(string path, string name)
            : base(System.IO.Path.Combine(path, name), null)
        {
            rootFolder = new SolutionFolder(path, this);
            rootFolder.Save();
            extention = ".sol";
        }

        /// <summary>
        /// Элементы решения
        /// </summary>
        protected SolutionFolder rootFolder;
        public SolutionFolder RootFolder
        {
            get { return rootFolder; }
        }

        /// <summary>
        /// Дочерние элементы
        /// </summary>
        public override List<ProjectElementBase> Childrens
        {
            get
            {
                return rootFolder.Childrens;
            }
        }

        /// <summary>
        /// Текущий активный проект
        /// </summary>
        private Project currentSelectedProject;
        public Project CurrentSelectedProject
        {
            get 
            {
                return currentSelectedProject; 
            }
            set 
            {
                currentSelectedProject = value; 
                //Обновить компилятор
            }
        }
        
        /// <summary>
        /// Получить список всех проектов в решении
        /// </summary>
        public List<Project> ProjectList
        {
            get 
            {
                List<Project> res = rootFolder.GetProjectListRecursive();
                res.Sort((x, y) => x.Caption.CompareTo(y.Caption));
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
            get { return "/HDL_Light;component/Images/Solution.png"; }
        }

        /// <summary>
        /// Особое имя проекта (нельзя менять разрешение)
        /// </summary>
        public override string Caption
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(path);
            }
        }

        /// <summary>
        /// Функция для создания решения
        /// </summary>
        /// <param name="solutionName"></param>
        /// <param name="solutionFolderPath"></param>
        /// <returns></returns>
        public static Solution CreateNewSolution(string solutionName, string solutionFolderPath)
        {
            string solutionPath = System.IO.Path.Combine(solutionFolderPath, solutionName);
            Solution res = new Solution(solutionPath, solutionName + ".sol");            
            res.Save();
            return res;
        }


        /// <summary>
        /// Сохранение решения
        /// </summary>
        public override void Save()
        {
            rootFolder.Save();
            SaveToXmlFile(this, path);
        }

        #region Context Menu
        /// <summary>
        /// Функция для создания контекстного меню
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        private static ContextMenu CreateContextMenu(Solution sol, ProjectExplorerControl control)
        {
            ContextMenu res = new ContextMenu();

            MenuItem itemNew = new MenuItem();
            itemNew.Header = "New";
            res.Items.Add(itemNew);

            MenuItem itemNewSolutionFolder = new MenuItem();
            itemNewSolutionFolder.Header = "New Solution Folder";
            itemNewSolutionFolder.Click += new System.Windows.RoutedEventHandler(sol.itemNewSolutionFolder_Click);
            itemNew.Items.Add(itemNewSolutionFolder);

            MenuItem itemNewProject = new MenuItem();
            itemNewProject.Header = "New Project";
            itemNewProject.Click += new System.Windows.RoutedEventHandler(sol.itemNewProject_Click);
            itemNew.Items.Add(itemNewProject);

            MenuItem itemAddExistingProject = new MenuItem();
            itemAddExistingProject.Header = "Add Existing Project";
            itemAddExistingProject.Click += new System.Windows.RoutedEventHandler(sol.itemAddExistingProject_Click);
            res.Items.Add(itemAddExistingProject);

            MenuItem itemRename = new MenuItem();
            itemRename.Header = "Rename";
            itemRename.Click += new System.Windows.RoutedEventHandler(sol.itemRename_Click);
            res.Items.Add(itemRename);


            MenuItem itemPaste = new MenuItem();
            itemPaste.Header = "Paste";
            itemPaste.Icon = CreateImageByPath("/HDL_Light;component/Images/Paste.png");
            itemPaste.Command = control.CommandPaste;
            itemPaste.CommandBindings.Add(control.BindingPasteCommand);
            res.Items.Add(itemPaste);

            MenuItem itemOpenFolderInExplorer = new MenuItem();
            itemOpenFolderInExplorer.Header = "Open Folder In Explorer";
            itemOpenFolderInExplorer.Click += new System.Windows.RoutedEventHandler(sol.itemOpenFolderInExplorer_Click);
            res.Items.Add(itemOpenFolderInExplorer);

            return res;
        }


        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            RootFolder.Paste_CanExecute(sender, e);
        }

        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RootFolder.Paste_Executed(sender, e);
        }


        private void itemAddExistingProject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingProject(rootFolder);
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
            NewProjectDialog dialog = new NewProjectDialog(this.rootFolder, this);
            dialog.ShowDialog();                
        }

        private void itemNewSolutionFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SolutionFolder folder = new SolutionFolder(System.IO.Path.Combine(RootFolder.Path, GenerateFileName("NewFolder")), this);
            folder.Save();
            rootFolder.AddElement(folder);
            Save();
            SchematixCore.Core.UpdateExplorerPanel();
            rootFolder.Expanded = true;
            folder.Rename();
        }
        #endregion

        /// <summary>
        /// Перечисляем вложенные компоненты
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<ProjectElementBase> GetEnumerator()
        {
            yield return this;
            foreach (ProjectElementBase el in rootFolder)
                yield return el;
        }


        #region XML methods for save/load data from XML
        /// <summary>
        /// Сохранение решения в файл
        /// </summary>
        /// <param name="sol"></param>
        /// <param name="path"></param>
        public static void SaveToXmlFile(Solution sol, string path)
        {
            XmlDocument _doc = new XmlDocument();

            XmlElement rootNode = _doc.CreateElement("Solution");
            XmlAttribute name_attribute = _doc.CreateAttribute("name");
            name_attribute.InnerText = sol.Caption;
            rootNode.Attributes.Append(name_attribute);

            AddSolutionFolderToXml(_doc, rootNode, sol, sol.rootFolder);

            _doc.AppendChild(rootNode);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            XmlWriter writer = XmlWriter.Create(path, settings);
            _doc.Save(writer);
            writer.Close();
        }

        /// <summary>
        /// Добавление папки к Xml документу
        /// </summary>
        /// <param name="_doc"></param>
        /// <param name="xml_el"></param>
        /// <param name="folder"></param>
        private static void AddSolutionFolderToXml(XmlDocument _doc, XmlElement xml_el, Solution sol, SolutionFolder folder)
        {
            XmlElement folderNode = _doc.CreateElement("SolutionFolder");
            XmlAttribute path_attribute = _doc.CreateAttribute("path");
            path_attribute.InnerText = FormRelativePath(sol.RootFolder.Path, folder.Path);
            folderNode.Attributes.Append(path_attribute);
            XmlAttribute expanded_attribute = _doc.CreateAttribute("expanded");
            expanded_attribute.InnerText = folder.Expanded.ToString();
            folderNode.Attributes.Append(expanded_attribute);
            foreach (SolutionFolder fold in folder.SubFolders)
            {
                AddSolutionFolderToXml(_doc, folderNode, sol, fold);
            }
            foreach (Project proj in folder.Projects)
            {
                AddProjectToXml(_doc, folderNode, sol, proj);
            }
            xml_el.AppendChild(folderNode);
        }

        /// <summary>
        /// Добавление проекта к Xml документу
        /// </summary>
        /// <param name="_doc"></param>
        /// <param name="xml_el"></param>
        /// <param name="proj"></param>
        private static void AddProjectToXml(XmlDocument _doc, XmlElement xml_el, Solution sol, Project proj)
        {
            XmlElement projNode = _doc.CreateElement("Project");
            XmlAttribute path_attribute = _doc.CreateAttribute("path");
            path_attribute.InnerText = FormRelativePath(proj.Parent.Path, proj.Path);
            projNode.Attributes.Append(path_attribute);
            xml_el.AppendChild(projNode);
        }

        /// <summary>
        /// Сформировать относительный путь к папке
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static string FormRelativePath(string BasePath, string Path)
        {
            return System.IO.Path.GetFullPath(Path).Replace(System.IO.Path.GetFullPath(BasePath), "");
        }

        /// <summary>
        /// Загрузка данных с Xml документа
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Solution LoadSolutionFromXml(string path)
        {
            Solution res = new Solution(path);

            XmlDocument _doc = new XmlDocument();
            try
            {
                _doc.Load(path);
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Could not open solution", ex);
                MessageBox.Show("Could not open solution", "Error during open solution", MessageBoxButton.OK);
                return null;
            }
            XmlNode RootNode = _doc.FirstChild;
            XmlNode SolutionNode = _doc.LastChild;

            res.caption = SolutionNode.Attributes["name"].InnerText;
            res.rootFolder = LoadSolutionFolderFromXmlDocument(res.rootFolder, SolutionNode.FirstChild, res);

            return res;
        }

        /// <summary>
        /// Загрузка папки решения с Xml документа
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="folderElement"></param>
        /// <returns></returns>
        private static SolutionFolder LoadSolutionFolderFromXmlDocument(SolutionFolder parent, XmlNode folderElement, Solution sol)
        {
            string expanded = folderElement.Attributes["expanded"].InnerText;
            string path = folderElement.Attributes["path"].InnerText;
            if ((string.IsNullOrEmpty(path) == false) && (path[0] == '\\'))
                path = path.Substring(1);
            path = System.IO.Path.Combine(parent.Path, path);
            SolutionFolder res = new SolutionFolder(path, parent) { Expanded = string.Equals(expanded, "True")};

            foreach (XmlNode node in folderElement.ChildNodes)
            {
                if (node.Name.Equals("SolutionFolder"))
                {
                    res.AddElement(LoadSolutionFolderFromXmlDocument(res, node, sol));
                }
                if (node.Name.Equals("Project"))
                {
                    res.AddElement(LoadProjectFromXmlDocument(res, node, sol));
                }
            }
            return res;
        }

        /// <summary>
        /// Загрузка проекта с Xml документа
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="projElement"></param>
        /// <returns></returns>
        private static Project LoadProjectFromXmlDocument(SolutionElementBase parent, XmlNode projElement, Solution sol)
        {
            string path = projElement.Attributes["path"].InnerText;
            if ((string.IsNullOrEmpty(path) == false) && (path[0] == '\\'))
                path = path.Substring(1);
            path = System.IO.Path.Combine(parent.Path, path);

            Project proj = Project.LoadProjectFromXml(parent, path);

            return proj;
        }
        #endregion

        /// <summary>
        /// Ничего не делать, вещь нужная
        /// </summary>
        public override void Remove()
        {
            
        }

        protected override void OnCaptionChange(string oldPath)
        {
            Save();
            try
            {
                System.IO.File.Delete(oldPath);
            }
            catch (System.IO.IOException ex)
            {
                Schematix.Core.Logger.Log.Error("OnCaptionChange error.", ex);
                System.Windows.MessageBox.Show(string.Format("Could not rename file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);

            }
            base.OnCaptionChange(oldPath);
        }

        public static string CanCreateSolution(string DirectoryPath, string SolutionName)
        {
            if (System.IO.Directory.Exists(DirectoryPath) == false)
                return string.Format("Directory {0} does not exist", DirectoryPath);
            foreach(string path in System.IO.Directory.EnumerateDirectories(DirectoryPath))
                if (System.IO.Path.GetFileName(path).Equals(SolutionName, StringComparison.InvariantCultureIgnoreCase))
                    return string.Format("Solution {0} already exists", SolutionName);
            return null;
        }

        #region IElementProvider
        public void AddElement(ProjectElementBase element)
        {
            rootFolder.AddElement(element);
        }

        public void RemoveElement(ProjectElementBase element)
        {
            rootFolder.RemoveElement(element);
        }

        public void AddElementRange(IList<ProjectElementBase> elements)
        {
            rootFolder.AddElementRange(elements);
        }

        public void RemoveElementRange(IList<ProjectElementBase> elements)
        {
            rootFolder.RemoveElementRange(elements);
        }

        public bool CanAddElement(ProjectElementBase element)
        {
            return rootFolder.CanAddElement(element);
        }

        public bool CanAddElementrange(IList<ProjectElementBase> elements)
        {
            return rootFolder.CanAddElementrange(elements);
        }
        #endregion

        #region IDropable
        public bool CanDrop(ClipboardBufferData data)
        {
            return rootFolder.CanDrop(data);
        }

        public void Drop(ClipboardBufferData data)
        {
            rootFolder.Drop(data);
        }
        #endregion
    }
}
