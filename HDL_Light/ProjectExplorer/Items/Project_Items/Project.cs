using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using Schematix.Dialogs.NewFileDialogWizard;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, хранящий информацию о проекте
    /// </summary>
    [Serializable]
    public class Project : ProjectElement, IElementProvider, IDropable
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public Project(string path, SolutionElementBase parent)
            : base(path, parent)
        {
            parentSolutionElement = parent;
            rootFolder = new ProjectFolder(System.IO.Path.GetDirectoryName(path), this);
            rootFolder.Save();
            extention = ".proj";
        }


        /// <summary>
        /// Компилятор, используемый для данного проекта
        /// </summary>
        [NonSerialized]
        private Compiler compiler;
        public Compiler Compiler
        {
            get { return compiler; }
        }
        

        /// <summary>
        /// Родительский элемент
        /// </summary>
        private SolutionElementBase parentSolutionElement;

        /// <summary>
        /// Элементы проекта
        /// </summary>
        protected ProjectFolder rootFolder;
        public ProjectFolder RootFolder
        {
            get { return rootFolder; }
        }

        /// <summary>
        /// Папка в которой будут храниться результаты моделирования
        /// </summary>
        private ProjectFolder simulationFolder;
        public ProjectFolder SimulationFolder
        {
            get
            {
                if (simulationFolder != null)
                    return simulationFolder;

                //Search folder with name "Simulations"
                foreach (ProjectElement el in rootFolder.SubElements)
                {
                    if ((el is ProjectFolder) && (el.Caption == "Simulations"))
                    {
                        simulationFolder = el as ProjectFolder;
                        return simulationFolder;
                    }
                }

                //Folder not found
                //Create new...
                simulationFolder = new ProjectFolder(System.IO.Path.Combine(rootFolder.Path, "Simulations"), rootFolder);
                rootFolder.AddElement(simulationFolder);
                simulationFolder.Save();

                return simulationFolder;
            }
        }

        /// <summary>
        /// Обновить содержимое папки "Simulations"
        /// </summary>
        public void UpdateSimulationFolderContent()
        {
            if (simulationFolder != null)
            {
                rootFolder.RemoveElement(simulationFolder);
                simulationFolder = CreateProjectFolderByPath(simulationFolder.Path, rootFolder, "*.vcd");
                rootFolder.AddElement(simulationFolder);
            }
        }

        public override List<ProjectElementBase> Childrens
        {
            get
            {
                return rootFolder.Childrens;
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
            get { return "/HDL_Light;component/Images/Project.png"; }
        }

        /// <summary>
        /// Сохранение проекта
        /// </summary>
        public override void Save()
        {
            Project.Compiler.UpdateFileList();
            rootFolder.Save();
            SaveToXmlFile(this, path);
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

        #region Context Menu
        /// <summary>
        /// Функция для создания контекстного меню
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        private static ContextMenu CreateContextMenu(Project proj, ProjectExplorerControl control)
        {
            ContextMenu res = new ContextMenu();

            MenuItem itemNew = new MenuItem();
            itemNew.Header = "New";
            res.Items.Add(itemNew);

            MenuItem itemNewItem = new MenuItem();
            itemNewItem.Header = "New Item";
            itemNewItem.Click += new System.Windows.RoutedEventHandler(proj.itemNewItem_Click);
            itemNew.Items.Add(itemNewItem);

            MenuItem itemNewFolder = new MenuItem();
            itemNewFolder.Header = "New Folder";
            itemNewFolder.Click += new System.Windows.RoutedEventHandler(proj.itemNewFolder_Click);
            itemNew.Items.Add(itemNewFolder);

            MenuItem itemAddExistingProject = new MenuItem();
            itemAddExistingProject.Header = "Add Existing Files";
            itemAddExistingProject.Click += new System.Windows.RoutedEventHandler(proj.itemAddExistingFile_Click);
            res.Items.Add(itemAddExistingProject);

            MenuItem itemAddExistingFolder = new MenuItem();
            itemAddExistingFolder.Header = "Add Existing Folder";
            itemAddExistingFolder.Click += new System.Windows.RoutedEventHandler(proj.itemAddExistingFolder_Click);
            res.Items.Add(itemAddExistingFolder);

            MenuItem itemRename = new MenuItem();
            itemRename.Header = "Rename";
            itemRename.Click += new System.Windows.RoutedEventHandler(proj.itemRename_Click);
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
            itemOpenFolderInExplorer.Click += new System.Windows.RoutedEventHandler(proj.itemOpenFolderInExplorer_Click);
            res.Items.Add(itemOpenFolderInExplorer);

            return res;
        }

        /// <summary>
        /// Отфильтровать элементы по типу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetProjectElements<T>() where T:ProjectElement
        {
            List<T> res = new List<T>();

            foreach (ProjectElement el in rootFolder.GetProjectFilesRecursive())
                if(el is T)
                    res.Add(el as T);

            return res;
        }

        /// <summary>
        /// Плучить все элементы проекта
        /// </summary>
        /// <returns></returns>
        public List<ProjectElement> GetAllProjectElements()
        {
            return rootFolder.GetProjectFilesRecursive();
        }

        private void itemAddExistingFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingFolder(this.RootFolder as ProjectFolder);
        }

        private void itemAddExistingFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomAddExsistingItems(rootFolder);
        }

        private void itemNewFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProjectFolder folder = new ProjectFolder(System.IO.Path.Combine(RootFolder.Path, GenerateFileName("NewFolder")), this);
            folder.Save();
            rootFolder.AddElement(folder);
            Save();
            SchematixCore.Core.UpdateExplorerPanel();
            rootFolder.Expanded = true;
            folder.Rename();
        }

        private void itemNewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddNewFile dial = new AddNewFile(this.rootFolder);
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SchematixCore.Core.UpdateExplorerPanel();
        }

        private void itemRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (parent is SolutionFolder)
            {
                (parent as SolutionFolder).RemoveElement(this);
                SchematixCore.Core.Solution.Save();
                SchematixCore.Core.UpdateExplorerPanel();
            }
        }

        public override void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            rootFolder.Paste_CanExecute(sender, e);
        }
        public override void Paste_Executed(object sender, ExecutedRoutedEventArgs e) 
        {
            rootFolder.Paste_Executed(sender, e);
        }
        public override void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }
        public override void Cut_Executed(object sender, ExecutedRoutedEventArgs e) { }

        private new void itemOpenFolderInExplorer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomOpenInWindowsExplorer();
        }

        private new void itemRename_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CustomRenameHandler(sender, e);
            OnPropertyChanged("Caption");
        }

        #endregion

        /// <summary>
        /// Создание нового проекта
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Project CreateNewProject(string name, SolutionFolder parent)
        {
            string projectPath = System.IO.Path.Combine(parent.AbsolutePath, name, name + ".proj");
            Project res = new Project(projectPath, parent);
            res.compiler = new Compiler(Core.ModelingLanguage.VHDL_Combined, res);
            res.Save();
            return res;
        }

        #region XML methods for save/load data from XML
        /// <summary>
        /// Сохранение проекта в файл
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="path"></param>
        public static void SaveToXmlFile(Project proj, string path)
        {
            XmlDocument _doc = new XmlDocument();

            XmlElement rootNode = _doc.CreateElement("Project");
            XmlAttribute name_attribute = _doc.CreateAttribute("name");
            name_attribute.InnerText = proj.Caption;
            rootNode.Attributes.Append(name_attribute);

            AddFolderToXml(_doc, rootNode, proj, proj.rootFolder);

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
        /// <param name="proj"></param>
        /// <param name="folder"></param>
        private static void AddFolderToXml(XmlDocument _doc, XmlElement xml_el, Project proj, ProjectFolder folder)
        {
            XmlElement folderNode = _doc.CreateElement("ProjectFolder");
            XmlAttribute path_attribute = _doc.CreateAttribute("path");
            path_attribute.InnerText = FormRelativePath(folder.Parent.Path, folder.Path);
            folderNode.Attributes.Append(path_attribute);
            XmlAttribute expanded_attribute = _doc.CreateAttribute("expanded");
            expanded_attribute.InnerText = folder.Expanded.ToString();
            folderNode.Attributes.Append(expanded_attribute);
            foreach (ProjectElement el in folder.SubElements)
            {
                if (el is ProjectFolder)
                    AddFolderToXml(_doc, folderNode, proj, (el as ProjectFolder));
                else
                    AddElementToXml(_doc, folderNode, proj, el);
            }
            xml_el.AppendChild(folderNode);
        }

        /// <summary>
        /// Добавление документа к Xml документу
        /// </summary>
        /// <param name="_doc"></param>
        /// <param name="xml_el"></param>
        /// <param name="proj"></param>
        private static void AddElementToXml(XmlDocument _doc, XmlElement xml_el, Project proj, ProjectElement el)
        {
            XmlElement projNode = _doc.CreateElement(el.GetType().Name);
            XmlAttribute path_attribute = _doc.CreateAttribute("path");
            path_attribute.InnerText = FormRelativePath(el.Parent.Path, el.Path);
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
        public static Project LoadProjectFromXml(SolutionElementBase parent, string path)
        {
            Project res = new Project(path, parent);

            XmlDocument _doc = new XmlDocument();
            _doc.Load(path);
            XmlNode RootNode = _doc.FirstChild;
            XmlNode ProjectNode = _doc.LastChild;

            res.caption = ProjectNode.Attributes["name"].InnerText;
            res.rootFolder = LoadProjectFolderFromXmlDocument(res.rootFolder, ProjectNode.FirstChild, res);

            res.compiler = new Compiler(Core.ModelingLanguage.VHDL_Combined, res);

            return res;
        }

        /// <summary>
        /// Загрузка папки с Xml документа
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="folderElement"></param>
        /// <returns></returns>
        private static ProjectFolder LoadProjectFolderFromXmlDocument(ProjectElement parent, XmlNode folderElement, Project proj)
        {
            string path = folderElement.Attributes["path"].InnerText;
            if ((string.IsNullOrEmpty(path) == false) && (path[0] == '\\'))
                path = path.Substring(1);
            string expanded = folderElement.Attributes["expanded"].InnerText;
            path = System.IO.Path.Combine(parent.Path, path);
            ProjectFolder res = new ProjectFolder(path, parent) { Expanded = string.Equals(expanded, "True") };

            foreach (XmlNode node in folderElement.ChildNodes)
            {
                if (node.Name.Equals("ProjectFolder"))
                {
                    res.AddElement(LoadProjectFolderFromXmlDocument(res, node, proj));
                }
                else
                {
                    res.AddElement(LoadProjectElementXmlDocument(res, node, proj));
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
        private static ProjectElement LoadProjectElementXmlDocument(ProjectElement parent, XmlNode projElement, Project proj)
        {
            string path = projElement.Attributes["path"].InnerText;
            if ((string.IsNullOrEmpty(path) == false) && (path[0] == '\\'))
                path = path.Substring(1);
            path = System.IO.Path.Combine(parent.Path, path);

            switch (projElement.Name)
            {
                case "FSM_File":
                    return new FSM_File(path, parent);
                case "Verilog_Code_File":
                    return new Verilog_Code_File(path, parent);
                case "VHDL_Code_File":
                    return new VHDL_Code_File(path, parent);
                case "Waveform_File":
                    return new Waveform_File(path, parent);
                case "Text_File":
                    return new Text_File(path, parent);
                case "EDR_File":
                    return new EDR_File(path, parent);
                case "Schema_File":
                    return new Schema_File(path, parent);
            }

            throw new Exception("Project loading error");
        }
        #endregion

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

        public override string ToString()
        {
            return Caption;
        }

        private void UpdateCompilerData()
        {
            //compiler.UpdateFileList();
        }

        public static string CanCreateProject(string DirectoryPath, string ProjectName)
        {
            if (System.IO.Directory.Exists(DirectoryPath) == false)
                return string.Format("Directory {0} does not exist", DirectoryPath);
            foreach (string path in System.IO.Directory.EnumerateDirectories(DirectoryPath))
                if (System.IO.Path.GetFileName(path).Equals(ProjectName, StringComparison.InvariantCultureIgnoreCase))
                    return string.Format("Project {0} already exists", ProjectName);
            return null;
        }
    }
}