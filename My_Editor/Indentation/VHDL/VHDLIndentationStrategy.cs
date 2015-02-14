using System;
using System.Collections.Generic;
using My_Editor.Document;
using System.Text;
using My_Editor.Lexter;
using Antlr4.Runtime;

namespace My_Editor.Indentation.VHDL
{
    /// <summary>
    /// Smart indentation for VHDL.
    /// </summary>
    public class VHDLIndentationStrategy : DefaultIndentationStrategy
    {
        private VHDL_Lexter lexter;

        public VHDL_Lexter Lexter
        {
            get { return lexter; }
            set { lexter = value; }
        }


        /// <summary>
        /// Creates a new VHDLIndentationStrategy.
        /// </summary>
        public VHDLIndentationStrategy(VHDL_Lexter parent)
        {
            this.lexter = parent;
        }

        /// <summary>
        /// Creates a new CSharpIndentationStrategy and initializes the settings using the text editor options.
        /// </summary>
        public VHDLIndentationStrategy(TextEditorOptions options, VHDL_Lexter parent)
        {
            this.lexter = parent;
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
                DocumentLine PrevPrevLine = previousLine.PreviousLine;
                ISegment indentationSegmentPrevPrevLine = null;

                if (PrevPrevLine != null)
                    indentationSegmentPrevPrevLine = TextUtilities.GetWhitespaceAfter(document, PrevPrevLine.Offset);

                ISegment indentationSegmentPrevLine = TextUtilities.GetWhitespaceAfter(document, previousLine.Offset);

                string indentation = document.GetText(indentationSegmentPrevLine);
                ISegment indentationSegmentCurLine = TextUtilities.GetWhitespaceAfter(document, line.Offset);


                IToken elem = lexter.GetTreeElementByOffset(line);
                IToken prev_elem = (PrevPrevLine != null) ? lexter.GetTreeElementByOffset(PrevPrevLine) : null;

                if ((elem != null))
                {
                    //нашли нужный токен, ищем отступ, который необходимо сделать
                    string indent = document.GetText(TextUtilities.GetWhitespaceBefore(document, elem.StartIndex));
                    if (prev_elem != null)
                    {

                        if ((prev_elem.StartIndex >= elem.StartIndex) && (prev_elem.StopIndex <= elem.StopIndex))
                        {
                            document.Replace(indentationSegmentCurLine, indent);
                        }
                    }
                    document.Replace(indentationSegmentCurLine, indent);
                }
            }
        }

        private bool IsTreeElementToIndent(IToken t)
        {
            foreach (string s in Tree_Type_To_Indent)
                if (s.Equals(t.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        private static readonly string[] Tree_Type_To_Indent = new string[] 
        {
            "entity", 
            "architecture",
            "process",
            "block",
            "package",
            "PACKAGE_BODY",
            "SUBPROGRAM_BODY",

            "case",
            "if",
            "for",
            "while"
        };


        public override void IndentLines(TextDocument document, int beginLine, int endLine)
        {
            for (int i = beginLine; i <= endLine; i++)
            {
                DocumentLine line = document.Lines[i];
                IndentLine(document, line);
            }
        }
    }
}
