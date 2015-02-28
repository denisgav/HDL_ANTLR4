using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.CodeCompletion;

using HDL_EditorExtension.CodeCompletion;
using HDL_EditorExtension.Folding;

namespace HDL_EditorExtension.Lexter
{
    public class CustomLexter : AbstractLexter
    {
        public CustomLexter(TextEditor textEditor, AbstractFoldingStrategy foldingStrategy, IIndentationStrategy indentationStrategy, CodeCompletionList codeCompletionList)
            :base (textEditor)
        {
            FoldingStrategy = foldingStrategy;
            IndentationStrategy = indentationStrategy;
            CodeCompletionList = codeCompletionList;
        }

        public override Schematix.Core.Model.CodeFile Code
        {
            get { return null; }
        }

        /// <summary>
        /// Обновить  представление
        /// </summary>
        public override void RenderData()
        {
            base.RenderData();
        }

        /// <summary>
        /// Получить описание для слова по его смещению от начала текста
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public override UIElement GetDefinitionForWord(int Offset, string text)
        {
            return null;
        }

        protected AbstractFoldingStrategy foldingStrategy;
        public override AbstractFoldingStrategy FoldingStrategy
        {
            get { return foldingStrategy; }
            set
            {
                foldingStrategy = value;
                UpdateFolding();
            }
        }

        /// <summary>
        /// Отступы
        /// </summary>
        protected IIndentationStrategy indentationStrategy;
        public override IIndentationStrategy IndentationStrategy
        {
            get { return indentationStrategy; }
            set { indentationStrategy = value; }
        }

        private CodeCompletionList codeCompletionList;
        public override CodeCompletionList CodeCompletionList
        {
            get
            {
                return codeCompletionList;
            }
            set
            {
                codeCompletionList = value;
            }
        }

        public override void Update(string text)
        {
        }

        protected override void UpdateCompiler()
        {
            //throw new NotImplementedException();
        }
    }
}
