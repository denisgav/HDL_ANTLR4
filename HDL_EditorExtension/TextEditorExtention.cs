using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using Schematix.Core.Compiler;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.NRefactory;
using ICSharpCode.AvalonEdit.Highlighting;
using HDL_EditorExtension.Lexter;
using System.Windows.Input;
using System.Windows;
using HDL_EditorExtension.Folding;
using ICSharpCode.AvalonEdit.Indentation;
using My_Editor.CodeCompletion;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using HDL_EditorExtension.Indentation.Verilog;
using HDL_EditorExtension.Highlighting;

namespace HDL_EditorExtension
{
    public class TextEditorExtention : TextEditor
    {
        #region Constructors
		static TextEditorExtention()
		{
            ExtentionResources.RegisterBuiltInHighlightings(ExtendedHighlightingManager.Instance);
		}
		
		/// <summary>
		/// Creates a new TextEditor instance.
		/// </summary>
		public TextEditorExtention(string filePath) 
            : this(new TextArea())
		{
            FilePath = filePath;
		}

        /// <summary>
        /// Creates a new TextEditor instance.
        /// </summary>
        public TextEditorExtention()
            : this(new TextArea())
        {
        }
		
		/// <summary>
		/// Creates a new TextEditor instance.
		/// </summary>
        protected TextEditorExtention(TextArea textArea)
            : base(textArea)
		{
		}
		#endregion

        /// <summary>
        /// Компилятор, используемый для текущего документа
        /// </summary>
        public AbstractCompiler Compiler
        {
            get 
            {
                return Lexter.Compiler; 
            }
            set 
            {
                Lexter.Compiler = value; 
            }
        }

        public AbstractLexter Lexter { get; set; }

        private string CommentString
        {
            get
            {
                if (SyntaxHighlighting.Name == "VHDL")
                    return "--";
                else
                    return "//";
            }
        }

        //Комментирование строк
        public void CommentLines()
        {
            string selectedText = SelectedText;
            string[] lines = selectedText.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = CommentString + lines[i];
            }
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                res.Append(lines[i]);
                if (i != (lines.Length - 1))
                    res.Append('\n');
            }

            SelectedText = res.ToString();
        }

        public void UnCommentLines()
        {
            string selectedText = SelectedText;
            string[] lines = selectedText.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(CommentString, string.Empty);
            }
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                res.Append(lines[i]);
                if (i != (lines.Length - 1))
                    res.Append('\n');
            }

            SelectedText = res.ToString();
        }

        //Сдвиг влево/вправо
        public void IndentHS()
        {
            if (TextArea.Selection.Length != 0)
            {
                foreach (DocumentLine line in Document.Lines)
                {
                    int offset = line.EndOffset;
                    if (TextArea.Selection.Contains(offset) == true)
                    {
                        Document.Insert(line.Offset, "\t");
                    }
                }
            }
            else
            {
                foreach (DocumentLine line in Document.Lines)
                {
                    if ((CaretOffset >= line.Offset) && (CaretOffset <= line.Offset + line.Length))
                    {
                        Document.Insert(line.Offset, "\t");
                        break;
                    }
                }
            }
        }

        public void OutdentHS()
        {
            if (TextArea.Selection.Length != 0)
            {
                foreach (DocumentLine line in Document.Lines)
                {
                    int offset = line.EndOffset;
                    if (TextArea.Selection.Contains(offset) == true)
                    {
                        if ((Document.Text[line.Offset] == '\t') || (Document.Text[line.Offset] == ' '))
                        {
                            Document.Remove(line.Offset, 1);
                        }
                    }
                }
            }
            else
            {
                foreach (DocumentLine line in Document.Lines)
                {
                    if ((CaretOffset >= line.Offset) && (CaretOffset <= line.Offset + line.Length))
                    {
                        int offset = line.EndOffset;
                        if (TextArea.Selection.Contains(offset) == true)
                        {
                            if ((Document.Text[line.Offset] == '\t') || (Document.Text[line.Offset] == ' '))
                            {
                                Document.Remove(line.Offset, 1);
                            }
                        }
                    }
                }
            }
        }
               

        public TextLocation CurrentCaretLocation
        {
            get { return Document.GetLocation(CaretOffset); }
        }

        public void ChangeHighliting(string highlitingName)
        {
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(highlitingName);
            if (SyntaxHighlighting == null)
            {
                SyntaxHighlighting = ExtendedHighlightingManager.Instance.GetDefinition(highlitingName);
                if (SyntaxHighlighting == null)
                {
                    if (Lexter != null)
                        Lexter.FoldingStrategy = null;
                }
            }
            
            switch (highlitingName)
            {
                case "XML":
                    if (Lexter != null)
                        Lexter.Dispose();
                    Lexter = new CustomLexter(this, new XmlFoldingStrategy(), new DefaultIndentationStrategy(), new CodeCompletionList(TextArea));
                    break;
                case "C#":
                case "C++":
                case "PHP":
                case "Java":
                    if (Lexter != null)
                        Lexter.Dispose();
                    Lexter = new CustomLexter(this, new BraceFoldingStrategy(), new CSharpIndentationStrategy(Options), new CodeCompletionList(TextArea));
                    break;
                case "VHDL":
                    if (Lexter != null)
                        Lexter.Dispose();
                    Lexter = new VHDL_Lexter(this);
                    break;
                case "Verilog":
                    if (Lexter != null)
                        Lexter.Dispose();
                    Lexter = new CustomLexter(this, new VerilogFoldingStrategy(), new VerilogIndentionStrategy(), new CodeCompletionList(TextArea));
                    break;
                default:
                    if (Lexter != null)
                        Lexter.Dispose();
                    Lexter = new CustomLexter(this, null, new DefaultIndentationStrategy(), new CodeCompletionList(TextArea));
                    break;
            }
            
        }

        /// <summary>
        /// Путь к текущему файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Удалить лекстер
        /// </summary>
        public void DisposeLexter()
        {
            if (Lexter != null)
                Lexter.Dispose();
        }
    }
}
