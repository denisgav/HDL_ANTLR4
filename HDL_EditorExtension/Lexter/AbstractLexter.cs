using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

using Schematix.Core.Compiler;
using Schematix.Core.Model;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation;

using HDL_EditorExtension.CodeCompletion;
using HDL_EditorExtension.Folding;

namespace HDL_EditorExtension.Lexter
{
    public abstract class AbstractLexter : IDisposable 
    {
        protected TextEditor textEditor;
        private Thread updateThread;

        /// <summary>
        /// Компилятор, используемый для текущего документа
        /// </summary>
        private AbstractCompiler compiler;
        public AbstractCompiler Compiler
        {
            get { return compiler; }
            set 
            {
                compiler = value;
                UpdateCompiler();
            }
        }

        /// <summary>
        /// Обновление данных для компилятора
        /// </summary>
        protected abstract void UpdateCompiler();

        public abstract CodeFile Code { get; }

        public AbstractLexter(TextEditor textEditor)
        {
            this.textEditor = textEditor;
            errorList = new List<Exception_Information>();
            warningList = new List<Exception_Information>();
            updateThread = new Thread(UpdateThread);
            updateThread.Priority = ThreadPriority.BelowNormal;
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        /// <summary>
        /// Функция (поток) для регулярного обновления отображения текста
        /// </summary>
        private void UpdateThread()
        {
            string old_text = string.Empty;

            while (true)
            {
                string new_text = GetText();
                if (new_text.Equals(old_text) == false)
                {
                    try
                    {
                        Update(new_text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Message: {0}\n Source: {1}\n StackTrace: {2}\n HelpLink: {3}", ex.Message, ex.Source, ex.StackTrace, ex.HelpLink), "Some Error :(", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (Code != null)
                    {
                        foreach (CodeFile code in Code.Dependencies)
                        {
                            code.NeedForUpdate = true;
                        }
                        foreach (CodeFile code in compiler.Files)
                        {
                            if (code.Dependencies.Count == 0)
                            {
                                code.NeedForUpdate = true;
                            }
                        }
                        Code.NeedForUpdate = false;
                    }

                    textEditor.Dispatcher.Invoke
                    (
                       new Action(
                         delegate()
                         {
                             RenderData();
                         }
                        ),
                        new TimeSpan(0, 0, 25),
                        System.Windows.Threading.DispatcherPriority.Background
                    );
                    old_text = new_text;
                }

                if ((Code != null) && (Code.NeedForUpdate == true))
                {
                    Code.NeedForUpdate = false;
                    Update(new_text);

                    textEditor.Dispatcher.Invoke
                    (
                       new Action(
                         delegate()
                         {
                             RenderData();
                         }
                        ),
                        new TimeSpan(0, 0, 25),
                        System.Windows.Threading.DispatcherPriority.Background
                    );
                }

                Thread.Sleep(new TimeSpan(0, 0, 2));
            }
        }

        protected string GetText()
        {
            string text = string.Empty;

            textEditor.Dispatcher.Invoke
            (
                new Action(
                    delegate()
                    {
                        text = textEditor.Text;
                    }
                ),
                System.Windows.Threading.DispatcherPriority.Background
            );

            return text;
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <param name="text"></param>
        public abstract void Update(string text);

        /// <summary>
        /// Обновить  представление
        /// </summary>
        public virtual void RenderData()
        {
            if ((foldingManager != null) && (FoldingStrategy != null))
                FoldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
            textEditor.TextArea.TextView.Redraw();
        }
        
        /// <summary>
        /// Получить описание для слова по его смещению от начала текста
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract UIElement GetDefinitionForWord(int Offset, string text);
        

        #region IDisposable
        private bool disposed = false;
        public virtual void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }
                updateThread.Abort();

                if((foldingManager != null) && (FoldingStrategy != null))
                    FoldingManager.Uninstall(FoldingManager);

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~AbstractLexter()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        #region Errors & Warnings
        /// <summary>
        /// список ошибок
        /// </summary>
        protected List<Exception_Information> errorList;
        public List<Exception_Information> ErrorList
        {
            get { return errorList; }
            set { errorList = value; }
        }
        
        /// <summary>
        /// список предупреждений
        /// </summary>
        protected List<Exception_Information> warningList;
        public List<Exception_Information> WarningList
        {
            get { return warningList; }
            set { warningList = value; }
        }
        #endregion

        #region CodeCompletionList
        /// <summary>
        /// Автодополнение
        /// </summary>
        public abstract CodeCompletionList CodeCompletionList
        {
            get;
            set;
        }      

        #endregion

        #region Folding
        public abstract AbstractFoldingStrategy FoldingStrategy
        {
            get;
            set;
        }


        protected FoldingManager foldingManager;
        public FoldingManager FoldingManager
        {
            get { return foldingManager; }
            set { foldingManager = value; }
        }

        /// <summary>
        /// Обновление foldingManager
        /// </summary>
        protected void UpdateFolding()
        {
            if (FoldingStrategy != null)
            {
                if (foldingManager == null)
                    foldingManager = FoldingManager.Install(textEditor.TextArea);
                FoldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
            }
            else
            {
                if (foldingManager != null)
                {
                    FoldingManager.Uninstall(foldingManager);
                    foldingManager = null;
                }
            }
        }

        #endregion


        #region IndentationStrategy property
        /// <summary>
        /// Отступы
        /// </summary>
        public abstract IIndentationStrategy IndentationStrategy
        {
            get;
            set;
        }
        
        #endregion
    }    
}
