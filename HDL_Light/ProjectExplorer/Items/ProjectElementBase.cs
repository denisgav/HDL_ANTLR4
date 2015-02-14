using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.ComponentModel;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Класс, представляющий элемент проекта
    /// </summary>
    [Serializable]
    public abstract class ProjectElementBase: INotifyPropertyChanged
    {
        /// <summary>
        /// Контекстное меню элемента
        /// </summary>
        public abstract ContextMenu CreateElementContextMenu (ProjectExplorerControl control);

        /// <summary>
        /// Иконка
        /// </summary>
        public abstract string Icon
        {
            get;
        }

        /// <summary>
        /// Путь к элементу (размещение в файловой системе)
        /// </summary>
        protected string path;
        public string Path
        {
            get { return path; }
            set 
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Абсолютный путь к элементу
        /// </summary>
        public string AbsolutePath
        {
            get { return path; }
        }

        /// <summary>
        /// Родительский элемент
        /// </summary>
        protected ProjectElementBase parent;
        public virtual ProjectElementBase Parent
        {
            get { return parent; }
            set 
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }
        
        /// <summary>
        /// Дочерние элементы
        /// </summary>
        public virtual List<ProjectElementBase> Childrens
        {
            get { return null; }
        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        protected string extention;
        public string Extention
        {
            get { return extention; }
            set { extention = value; }
        }


        /// <summary>
        /// Название элемента
        /// </summary>
        protected string caption;
        public virtual string Caption
        {
            get { return caption; }
            set 
            {
                if ((caption.Equals(value) == false) && (CheckFileName(value, extention, parent.path, true)))
                {
                    string old_path = Path;
                    int last_index = path.LastIndexOf(caption, StringComparison.InvariantCulture);
                    caption = value;
                    path = System.IO.Path.Combine(path.Substring(0, last_index), value + extention);

                    if (old_path.Equals(path) == false)
                        OnCaptionChange(old_path);
                }
            }
        }

        /// <summary>
        /// проверить правильность имени файла, перед тем как его менять
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Extention"></param>
        /// <param name="Path"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static bool CheckFileName(string FileName, string Extention, string Path, bool throwException = false)
        {
            //1. Пустое имя
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrWhiteSpace(FileName))
            {
                if (throwException == true)
                {
                    throw new Exception("File name is empty");
                }
                return false;
            }

            //2. Исп. неразрешенных символов
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in FileName)
                if (invalidChars.Contains(c))
                {
                    if (throwException == true)
                    {
                        throw new Exception(string.Format("Using of symbol '{0}' in file name is not alloved", c));
                    }
                    return false;
                }

            //3. Файл уже существует
            string fullPath = System.IO.Path.Combine(Path, FileName + Extention);
            if (System.IO.File.Exists(fullPath))
            {
                if (throwException == true)
                {
                    throw new Exception(string.Format("File {0} already exists", fullPath));
                }
                return false;
            }

            if (System.IO.Directory.Exists(fullPath))
            {
                if (throwException == true)
                {
                    throw new Exception(string.Format("Directory {0} already exists", fullPath));
                }
                return false;
            }

            return true;
        }        

        /// <summary>
        /// Метод, который вызывается в случае изменения имени элемента
        /// </summary>
        protected virtual void OnCaptionChange(string oldPath)
        {
            SchematixCore.Core.UpdateExplorerPanel();
            OnPropertyChanged("Caption");
        }

        /// <summary>
        /// Коструктор для элемента, чье имя не совадает с именем файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caption"></param>
        /// <param name="parent"></param>
        public ProjectElementBase(string path, string caption, ProjectElementBase parent)
        {
            this.parent = parent;
            this.path = path;
            this.caption = caption;

            if (System.IO.Directory.Exists(path))
                //Это папка, заглавие не меняем
            {
                extention = string.Empty;
            }
            else
            {
                if (System.IO.Path.HasExtension(path))
                    this.extention = System.IO.Path.GetExtension(path);
            }
        }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public ProjectElementBase(string path, ProjectElementBase parent)
        {
            this.parent = parent;
            this.path = path;

            if (System.IO.Directory.Exists(path))
                //Это папка, заглавие не меняем
            {
                extention = string.Empty;
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(path);
                caption = info.Name;
            }
            else
            {
                this.caption = System.IO.Path.GetFileNameWithoutExtension(path);
                if (System.IO.Path.HasExtension(path))
                    this.extention = System.IO.Path.GetExtension(path);
            }
        }

        /// <summary>
        /// Сохранение элемента
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Используется при открытии нового окна
        /// </summary>
        /// <returns></returns>
        public virtual Schematix.Windows.SchematixBaseWindow CreateNewWindow()
        {
            return null;
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// Используется для создания иконки для контектсного меню
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image CreateImageByPath(string path)
        {
            Image i = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(path, UriKind.Relative);
            src.EndInit();
            i.Source = src;
            i.Stretch = Stretch.Uniform;
            i.Width = 16;
            i.Height = 16;
            return i;
        }

        /// <summary>
        /// Используется для генерации иени (уникального)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// Хранение дополнительной информации
        /// </summary>
        [NonSerialized]
        private object tag;
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Обработка нажатия пункта меню Rename по-умолчанию
        /// </summary>
        protected void CustomRenameHandler(object sender, System.Windows.RoutedEventArgs e)
        {
            Rename();
        }

        /// <summary>
        /// Открыть элемент в проводнике Windows (по-умолчанию)
        /// </summary>
        protected void CustomOpenInWindowsExplorer()
        {
            System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(Path));
        }

        /// <summary>
        /// Обработчик события добавления существующего проекта по-умолчанию
        /// </summary>
        /// <param name="parent"></param>
        protected void CustomAddExsistingProject(SolutionFolder parent)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Schematix Project File|*.proj";
            ofd.InitialDirectory = Path;
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                //Пользователь выбрал проект, который необходимо добавить в решение
                if (System.IO.File.Exists(ofd.FileName))
                {
                    Project newProj = Project.LoadProjectFromXml(parent, ofd.FileName);
                    parent.AddElement(newProj);
                    Save();
                    SchematixCore.Core.UpdateExplorerPanel();
                }
            }            
        }


        /// <summary>
        /// Добавление существующей директории к проекту
        /// </summary>
        /// <param name="projectFolder"></param>
        protected void CustomAddExsistingFolder(ProjectFolder projectFolder, string searchPattern="*.*")
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = projectFolder.Path;
            fbd.ShowNewFolderButton = false;
            fbd.Description = "Select folder for inport data";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Directory.Exists(fbd.SelectedPath))
                {
                    ProjectFolder newFolder = CreateProjectFolderByPath(fbd.SelectedPath, projectFolder, searchPattern);
                    projectFolder.AddElement(newFolder);
                }
            }

            projectFolder.Project.Compiler.UpdateFileList();
            Save();
            SchematixCore.Core.UpdateExplorerPanel();
        }



        /// <summary>
        /// Добавление существующих файлов к проекту
        /// </summary>
        /// <param name="folder"></param>
        protected void CustomAddExsistingItems(ProjectFolder folder)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "All Files|*.*";
            ofd.InitialDirectory = folder.path;
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                List<ProjectElementBase> newElements = new List<ProjectElementBase>();
                //Пользователь выбрал проект, который необходимо добавить в решение
                foreach (string filePath in ofd.FileNames)
                {
                    bool IsInProject = false;
                    foreach (ProjectElement el in folder.SubElements)
                    {
                        if (el.Path.Equals(filePath) == true)
                        {
                            IsInProject = true;
                            break;
                        }
                    }

                    if (IsInProject == true)
                        continue;

                    ProjectElement newElement = CreateProjectElementByPath(filePath, folder);
                    if (newElement != null)
                        newElements.Add(newElement);

                }
                    
                folder.AddElementRange(newElements);
                folder.Project.Compiler.UpdateFileList();
                Save();
                SchematixCore.Core.UpdateExplorerPanel();
            }
        }

        protected static ProjectFolder CreateProjectFolderByPath(string folderPath, ProjectFolder folder, string searchPattern = "*.*")
        {
            ProjectFolder res = new ProjectFolder(folderPath, folder);

            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(folderPath);
            //Добавляем файлы
            List<ProjectElementBase> newElements = new List<ProjectElementBase>();
            foreach (System.IO.FileInfo file in info.GetFiles(searchPattern))
            {
                ProjectElement newElement = CreateProjectElementByPath(file.FullName, res);
                if (newElement != null)
                    newElements.Add(newElement);
            }
            foreach(System.IO.DirectoryInfo dir in info.GetDirectories())
            {
                ProjectElement newElement = CreateProjectFolderByPath(dir.FullName, res);
                if (newElement != null)
                    newElements.Add(newElement);
            }
            res.AddElementRange(newElements);

            return res;
        }

        /// <summary>
        /// Создание элемента проекта по его пути
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static ProjectElement CreateProjectElementByPath(string filePath, ProjectFolder folder = null)
        {
            switch (System.IO.Path.GetExtension(filePath).ToLower())
            {
                case ".v":
                case ".sv":
                    {
                        return new Verilog_Code_File(filePath, folder);
                    }
                    break;
                case ".vhdl":
                case ".vhd":
                    {
                        return new VHDL_Code_File(filePath, folder);
                    }
                    break;
                case ".fsm":
                    {
                        return new FSM_File(filePath, folder);
                    }
                    break;
                case ".vcd":
                    {
                        return new Waveform_File(filePath, folder);
                    }
                    break;
                case ".txt":
                    {
                        return new Text_File(filePath, folder);
                    }
                    break;
                default:
                    {
                        return null;
                    }
                    break;
            }
        }

        /// <summary>
        /// Удаление элемента с заданным путем с папки
        /// </summary>
        /// <param name="RmPath"></param>
        protected void RemoveElementFromSolution(string RmPath, ProjectElementBase elem)
        {
            //Проверяем путь
            if (elem.Path.Equals(RmPath) == true)
            {
                //Если можно - то удаляем элемент
                if ((elem.parent != null) && (elem.parent is IElementProvider))
                {
                    (elem.parent as IElementProvider).RemoveElement(elem);
                    return;
                }
            }
            //Перебираем оставшиеся дочерние элементы
            if (elem.Childrens != null)
                foreach (ProjectElementBase el in elem.Childrens)
                    RemoveElementFromSolution(RmPath, el);

            SchematixCore.Core.UpdateExplorerPanel();
        }

        /// <summary>
        /// Существует ли 'ktvtyn d afqkjdjq cbcntvt
        /// </summary>
        public virtual bool IsExistsInFileSystem 
        {
            get
            {
                if (string.IsNullOrEmpty(extention) == true)
                {
                    if (System.IO.Directory.Exists(path) == true)
                        return true;
                    if (System.IO.File.Exists(path) == true)
                        return true;
                }
                else
                {
                    if (System.IO.File.Exists(path) == true)
                        return true;
                }
                return false;
            }
        }

        public void Rename()
        {
            StackPanel panel = tag as StackPanel;
            if (panel != null)
            {
                foreach (object o in panel.Children)
                {
                    if (o is Schematix.Waveform.UserControls.EditableTextBlock)
                    {
                        Schematix.Waveform.UserControls.EditableTextBlock etb = o as Schematix.Waveform.UserControls.EditableTextBlock;
                        if (etb != null)
                        {
                            // Finally make sure that we are
                            // allowed to edit the TextBlock
                            if (etb.IsEditable)
                                etb.IsInEditMode = true;
                        }
                    }
                }
            }
        }

        #region CommandHandlers
        public virtual void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }
        public virtual void Paste_Executed(object sender, ExecutedRoutedEventArgs e) { }
        public virtual void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }
        public virtual void Remove_Executed(object sender, ExecutedRoutedEventArgs e) { Remove(); }
        public virtual void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }
        public virtual void Cut_Executed(object sender, ExecutedRoutedEventArgs e) { }
        public virtual void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }
        public virtual void Copy_Executed(object sender, ExecutedRoutedEventArgs e) { }
        #endregion

        #region INotifyPropertyChanged (Something chanded)
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        protected void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
            }
        }
        #endregion
    }
}
