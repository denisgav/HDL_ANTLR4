using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

namespace Schematix.Windows
{
    /// <summary>
    /// Базовый класс для окна в данном проекте
    /// </summary>
    public abstract class SchematixBaseWindow : LayoutDocument
    {
        private static System.Windows.Media.Brush savedBrush;
        public static System.Windows.Media.Brush SavedBrush
        {
            get { return savedBrush; }
            set { savedBrush = value; }
        }

        private static System.Windows.Media.Brush modifiedBrush;
        public static System.Windows.Media.Brush ModifiedBrush
        {
            get { return savedBrush; }
            set { savedBrush = value; }
        }

        static SchematixBaseWindow()
        {
            savedBrush = System.Windows.Media.Brushes.Black;
            modifiedBrush = System.Windows.Media.Brushes.Orange;
        }

        /// <summary>
        /// Срабатывает 1 раз в секунду
        /// </summary>
        DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Ссылка на главный узел проекта
        /// </summary>
        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }

        /// <summary>
        /// открытый элемент проекта
        /// </summary>
        private ProjectExplorer.ProjectElement projectElement;
        public ProjectExplorer.ProjectElement ProjectElement
        {
            get { return projectElement; }
        }

        /// <summary>
        /// Получение набора тулбаров компонента
        /// </summary>
        /// <returns></returns>
        public virtual IList<ToolBar> GetListOfToolBars()
        {
            return new List<ToolBar>();
        }

        /// <summary>
        /// Получить ToolBarTray
        /// </summary>
        /// <returns></returns>
        public virtual ToolBarTray GetToolBarTray()
        {
            return new ToolBarTray();
        }

        /// <summary>
        /// Получить StatusBar
        /// </summary>
        /// <returns></returns>
        public virtual System.Windows.Controls.Primitives.StatusBar GetStatusBar()
        {
            return new System.Windows.Controls.Primitives.StatusBar();
        }

        /// <summary>
        /// Сохранены ли данные
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSaved()
        {
            return (ProjectElement.Removed == false);
        }

        public bool IsModified
        {
            get { return ((IsSaved() == false) || (ProjectElement.Removed == true)); }
        }


        /// <summary>
        /// Загрузить (по-умолчанию)
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Сохранить (по-умолчанию)
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Сохранить по заданному пути
        /// </summary>
        /// <param name="filePath"></param>
        public abstract void Save(string filePath);

        /// <summary>
        /// Сохранить в поток
        /// </summary>
        /// <param name="stream"></param>
        public abstract void Save(System.IO.Stream stream);

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="projectElement"></param>
        /// <param name="core"></param>
        public SchematixBaseWindow(ProjectExplorer.ProjectElement projectElement, SchematixCore core)
        {
            this.core = core;
            this.projectElement = projectElement;
            this.Title = projectElement.Caption;
            this.IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(projectElement.Icon, UriKind.Relative));
            projectElement.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(projectElement_PropertyChanged);
            Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(SchematixBaseWindow_Closing);
            IsActiveChanged += new EventHandler(SchematixBaseWindow_IsActiveChanged);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }

        void SchematixBaseWindow_IsActiveChanged(object sender, EventArgs e)
        {
            CheckFileExist();
        }

        public virtual void CheckFileExist()
        {
            if ((projectElement.IsExistsInFileSystem == false))
            {
                MessageBoxResult res = MessageBox.Show(string.Format("File {0} does not exists on file system. Ceep this file opened?", projectElement.Path), "File not found", MessageBoxButton.YesNo, MessageBoxImage.Question);
                Schematix.Core.Logger.Log.Warn(string.Format("File {0} does not exists on file system.", projectElement.Path));
                if (res == MessageBoxResult.Yes)
                {
                    Save();
                    projectElement.Removed = false;
                    Schematix.Core.Logger.Log.Warn(string.Format("File {0} is steel opened.", projectElement.Path));
                }
                else
                {
                    try
                    {
                        Close();
                    }
                    catch
                    { }
                }
            }
            else
            {
                if ((projectElement.Removed == true))
                {
                    Load();
                    projectElement.Removed = false;
                }
            }
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Background = (IsSaved() == true) ? savedBrush : modifiedBrush;
        }

        public void SchematixBaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((IsSaved() == false) || (projectElement.IsExistsInFileSystem == false))
            {
                System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(string.Format("File: {0} is modefied. Save this file?", projectElement.Path), "File is modified", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);
                if (res == System.Windows.MessageBoxResult.Yes)  //Сохраняем и закрываем
                {
                    Save();
                    OnClose();
                    dispatcherTimer.Stop();
                }
                if (res == System.Windows.MessageBoxResult.No)  //Просто закрываем
                {
                    OnClose();
                    dispatcherTimer.Stop();
                }
                if (res == System.Windows.MessageBoxResult.Cancel)  //Придется отменить
                {
                    e.Cancel = true;
                }
            }
            else
            {
                OnClose();
                dispatcherTimer.Stop();
            }
        }

        /// <summary>
        /// Реакция на изменение какого-то параметра элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void projectElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Title = projectElement.Caption;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="projectElement"></param>
        public SchematixBaseWindow(ProjectExplorer.ProjectElement projectElement)
            : this(projectElement, SchematixCore.Core)
        {
        }


        #region Commands handler

        #region File submenu
        /// <summary>
        /// Возможно ли сохранение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsSaved();
        }

        /// <summary>
        /// сохранение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Вызывать при закрытии
        /// </summary>
        public abstract void OnClose();

        /// <summary>
        /// Доступно ли "сохранить как"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            e.CanExecute = true; 
        }

        /// <summary>
        /// "Сохранить как"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e) 
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.InitialDirectory = projectElement.Project.RootFolder.Path;
            if(SchematixCore.Core.Solution != null)
            {
                foreach(Schematix.ProjectExplorer.Project Project in SchematixCore.Core.Solution.ProjectList)
                    sfd.CustomPlaces.Add(new Microsoft.Win32.FileDialogCustomPlace(Project.Path));
            }
            sfd.FileName = System.IO.Path.GetFileName(projectElement.Path);
            sfd.ValidateNames = true;

            if (sfd.ShowDialog() == true)
            {
                Save(sfd.FileName);
                ProjectElement.Removed = false;
            }
        }

        /// <summary>
        /// Можно ли закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Вызвать при закрытии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Close_Executed(object sender, ExecutedRoutedEventArgs e) { }

        #endregion

        #region Edit submenu
        /// <summary>
        /// можно ли отменить действие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Отмена действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Undo_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли повторить действие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Повтор действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Redo_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли вырезать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Выполнение команды "вырезать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Cut_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли скопировать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Выполнение копирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Copy_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли вставить объект
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Вставка объекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Paste_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли удалить?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Выполнение удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Delete_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Можно ли выделить все
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e) { }

        /// <summary>
        /// Выделить все
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e) { }
        #endregion
        #endregion        
    }
}
