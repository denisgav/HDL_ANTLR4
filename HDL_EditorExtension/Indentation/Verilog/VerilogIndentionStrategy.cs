using System;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.NRefactory.Editor;

namespace HDL_EditorExtension.Indentation.Verilog
{
    /// <summary>
    /// Smart indentation for Verilog.
    /// </summary>
    public class VerilogIndentionStrategy : DefaultIndentationStrategy
    {
    /// <summary>
        /// Creates a new VerilogIndentionStrategy.
        /// </summary>
        public VerilogIndentionStrategy()
        {
        }

        /// <summary>
        /// Creates a new CSharpIndentationStrategy and initializes the settings using the text editor options.
        /// </summary>
        public VerilogIndentionStrategy(TextEditorOptions options)
        {
            this.IndentationString = options.IndentationString;
        }

        string indentationString = "\t";

        /// <summary>
        /// Gets/Sets the indentation string.
        /// </summary>
        public string IndentationString
        {
            get
            {
                return indentationString;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Indentation string must not be null or empty");
                indentationString = value;
            }
        }

        public override void IndentLine(TextDocument document, DocumentLine line)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (line == null)
                throw new ArgumentNullException("line");
            DocumentLine previousLine = line.PreviousLine;
            if (previousLine != null)
            {
                DocumentLine PrevPrevLine =  previousLine.PreviousLine;
                ISegment indentationSegmentPrevPrevLine = null;

                if(PrevPrevLine != null)
                    indentationSegmentPrevPrevLine = TextUtilities.GetWhitespaceAfter(document, PrevPrevLine.Offset);

                ISegment indentationSegmentPrevLine = TextUtilities.GetWhitespaceAfter(document, previousLine.Offset);

                bool isNeedToReplacePrevLine = (indentationSegmentPrevPrevLine != null) && (indentationSegmentPrevPrevLine.Length <= indentationSegmentPrevLine.Length);

                string indentation = document.GetText(indentationSegmentPrevLine);
                // copy indentation to line
                ISegment  indentationSegmentCurLine = TextUtilities.GetWhitespaceAfter(document, line.Offset);
                string text = document.GetText(previousLine.Offset, previousLine.Length);
                
                if (containsShiftRightKeyword(text) == true)
                {
                    document.Replace(indentationSegmentCurLine, indentation);
                    if (isNeedToReplacePrevLine == true)
                        document.Replace(indentationSegmentPrevLine, indentation.Remove(0, 1));
                    return;
                }

                //определяем, имеет ли строка открывающийся блок
                int countOpenBlock  = countStartBlock(text);
                //определяем, имеет ли строка закрывающийся блок
                int countCloseBlock = countEndBlock(text);

                if (countCloseBlock != 0)
                {
                    if (indentation.Length > 0)
                    {
                        if (isNeedToReplacePrevLine == true)
                        {
                            document.Insert(previousLine.EndOffset + 1, indentation.Remove(0, 1));
                            document.Replace(previousLine.Offset, indentation.Length, indentation.Remove(0, 1));
                        }
                        else
                        {
                            if (indentationSegmentCurLine.Length == indentationSegmentPrevLine.Length)
                                document.Replace(indentationSegmentCurLine, indentation.Remove(0, 1));
                            else
                                document.Replace(indentationSegmentCurLine, indentation);
                        }
                    }
                    return;
                }

                if (countOpenBlock!= 0)
                {
                    document.Replace(indentationSegmentCurLine, indentation + indentationString);
                    return;
                }
                document.Replace(indentationSegmentCurLine, indentation);
            }
        }

        private int countStartBlock(string text)
        {
            int res = 0;
            List<string> words = Folding.VerilogFoldingStrategy.MySplit(text);
            foreach (string s in words)
            {
                if (OpeningBrace.Contains(s))
                    res++;
            }
            return res;
        }

        private int countEndBlock(string text)
        {
            int res = 0;
            List<string> words = Folding.VerilogFoldingStrategy.MySplit(text);
            foreach (string s in words)
            {
                if (ClosingBrace.Contains(s))
                    res++;
            }
            return res;
        }

        private bool containsShiftRightKeyword(string text)
        {
            List<string> words = Folding.VerilogFoldingStrategy.MySplit(text);
            foreach (string s in words)
            {
                if (ShiftRightKeywords.Contains(s))
                    return true;
            }
            return false;
        }

        public override void IndentLines(TextDocument document, int beginLine, int endLine)
        {
        }

        private static readonly List<string> OpeningBrace = new List<string>
            ( 
                new string[]
                {
                    "begin", 
                    "module",
                    "function",
                    "case"
                }
            );

        private static readonly List<string> ShiftRightKeywords = new List<string>
        (
            new string []
            {
                "else"
            }
        );

        private static readonly List<string> ClosingBrace = new List<string>
        (
            new string[]
                    {
                        "end",
                        "endmodule",
                        "endfunction",
                        "endcase"
                    }
        );
    }
}
