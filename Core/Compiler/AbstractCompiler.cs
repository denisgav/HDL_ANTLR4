using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Schematix.Core.Model;
using System.Collections.Specialized;
using Schematix.Waveform;

namespace Schematix.Core.Compiler
{
    /// <summary>
    /// Базовый класс для компилятора
    /// </summary>
    public abstract class AbstractCompiler : IDisposable
    {
        /// <summary>
        /// Сообщения, выданные компилятором
        /// </summary>
        protected List<DiagnosticMessage> messages;
        public List<DiagnosticMessage> Messages
        {
            get 
            {
                List<DiagnosticMessage> res = new List<DiagnosticMessage>();
                if (messages.Count != 0)
                    res.AddRange(messages);
                else
                {
                    foreach (CodeFile file in files)
                        res.AddRange(file.GetMessages());
                }
                return res; 
            }
        }

        /// <summary>
        /// Использовать объект ProcessInterface для запуска внешнего приложения
        /// </summary>
        private ProcessInterface.ProcessInterface processInterface;
        public ProcessInterface.ProcessInterface ProcessInterface
        {
            get { return processInterface; }
            set 
            {
                if (processInterface != null)
                {
                    processInterface.OnProcessExit -= processInterface_OnProcessExit;
                    processInterface.OnProcessError -= processInterface_OnProcessError;
                }

                processInterface = value;

                processInterface.OnProcessExit += new global::ProcessInterface.ProcessEventHanlder(processInterface_OnProcessExit);
                processInterface.OnProcessError += new global::ProcessInterface.ProcessEventHanlder(processInterface_OnProcessError);
            }
        }

        /// <summary>
        /// Indicates that compiler is busy now
        /// </summary>
        protected bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
        }

        /// <summary>
        /// Если запустить компилятор, графический интерфейс пользователя заблокируется
        /// Необходимо выполнять процесс в отдельном потоке
        /// </summary>
        public virtual bool IsLockGUI
        {
            get { return processInterface != null; }
        }
        
        

        protected virtual void processInterface_OnProcessError(object sender, ProcessInterface.ProcessEventArgs args)
        {
            
        }

        protected virtual void processInterface_OnProcessExit(object sender, ProcessInterface.ProcessEventArgs args)
        {
        }
        

        protected event NotifyCollectionChangedEventHandler collectionChangedEvent;
        public event NotifyCollectionChangedEventHandler CollectionChangedEvent
        {
            add { collectionChangedEvent += value; }
            remove { collectionChangedEvent -= value; }
        }

        protected void NotifyCollectionChanged()
        {
            collectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Набор файлов с исходным кодом
        /// </summary>
        protected ObservableCollection<CodeFile> files;
        public ObservableCollection<CodeFile> Files
        {
            get { return files; }
            set { files = value; }
        }

        /// <summary>
        /// Добавление нового файла для компиляции
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public abstract CodeFile AddCodeFile(string filePath);

        /// <summary>
        /// Удаление файла с компилятора
        /// </summary>
        /// <param name="filePath"></param>
        public virtual void RemoveCodeFile(string filePath)
        {
            CodeFile file = GetFileByPath(filePath);
            if (file != null)
            {
                files.Remove(file);
            }
        }

        /// <summary>
        /// Возвращает обьект класса CodeFile, по пути к файлу
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public CodeFile GetFileByPath(string path)
        {
            foreach (CodeFile file in files)
                if (file.FilePath == path)
                    return file;
            return null;
        }

        /// <summary>
        /// Каталог с проектом
        /// </summary>
        private string projectDirectory;
        public string ProjectDirectory
        {
            get { return projectDirectory; }
            set { projectDirectory = value; }
        }

        /// <summary>
        /// Каталог с библиотекой
        /// </summary>
        private string libraryDirectory;
        public string LibraryDirectory
        {
            get { return libraryDirectory; }
            set { libraryDirectory = value; }
        }

        /// <summary>
        /// Каталог с исходниками
        /// </summary>
        private string sourceDirectory;
        public string SourceDirectory
        {
            get { return sourceDirectory; }
            set { sourceDirectory = value; }
        }

        public AbstractCompiler()
        {
            messages = new List<DiagnosticMessage>();
            files = new ObservableCollection<CodeFile>();
            collectionChangedEvent = new NotifyCollectionChangedEventHandler(AbstractCompiler_collectionChangedEvent);
        }

        /// <summary>
        /// Реагировать на изменение списка сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AbstractCompiler_collectionChangedEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Произвести обработку файла file
        /// </summary>
        /// <param name="file"></param>
        public virtual void ProcessCodeFile(CodeFile file)
        {
            messages.Clear();
            NotifyCollectionChanged();
        }

        public virtual void ProcessProject()
        {
            foreach (CodeFile file in files)
                ProcessCodeFile(file);
            NotifyCollectionChanged();
        }


        /// <summary>
        /// Компиляция файла
        /// </summary>
        /// <param name="filePath"></param>
        public virtual bool CompileOne(string filePath)
        {
            NotifyCollectionChanged();
            return true;
        }

        /// <summary>
        /// Проверка синтаксиса файла
        /// </summary>
        /// <param name="id"></param>
        public virtual bool CheckSyntaxOne(string fileName)
        {
            NotifyCollectionChanged();
            return true;
        }

        /// <summary>
        /// Запуск процесса моделирования
        /// </summary>
        /// <param name="vhdFile"></param>
        /// <param name="EntityName"></param>
        /// <param name="ArchitectureName"></param>
        /// <param name="vcdFile"></param>
        public abstract bool CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, bool noRun = false);

        /// <summary>
        /// Создание волновой диаграммы для test bench
        /// </summary>
        /// <param name="testBenchFilename"></param>
        /// <param name="vcdId"></param>
        /// <param name="outFile"></param>
        /// <param name="entity"></param>
        /// <param name="architecture"></param>
        public abstract bool CreateTestBenchDiagram(string testBenchEntity, string testBenchArchitecture, string testBenchFilename, string file, string entity, string architecture, string outFile);

        /// <summary>
        /// Создание волновой диаграммы для test bench
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entity"></param>
        /// <param name="architecture"></param>
        /// <param name="outFile"></param>
        /// <param name="Scope"></param>
        public abstract bool CreateTestBenchDiagram(string file, string entity, string architecture, string outFile, WaveformCore core);

        /// <summary>
        /// Очистить проект
        /// </summary>
        public virtual bool CleanProject()
        {
            return true;
        }

        /// <summary>
        /// Вернуть данные о библиотеке
        /// </summary>
        /// <returns></returns>
        public abstract SortedList<string, GHDLCompiledFile> ReparseLibrary();

        /// <summary>
        /// Организация очереди для компиляции
        /// </summary>
        public virtual List<CodeFile> CreateQueue()
        {
            List<CodeFile> queue = new List<CodeFile>();
            queue.AddRange(files);
            return queue;
        }

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
