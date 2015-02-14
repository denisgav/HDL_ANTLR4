using System;
using System.Collections.Generic;
using My_Editor.Document;
using My_Editor.Folding;

namespace My_Editor.Folding
{
    /// <summary>
    /// 
    /// </summary>
    public class VerilogFoldingStrategy : AbstractFoldingStrategy
    {
        /// <summary>
        /// Gets/Sets the opening brace. 
        /// </summary>
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
        /// <summary>
        /// Gets/Sets the closing brace. The default value is '}'.
        /// </summary>
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
        /// <summary>
        /// Creates a new BraceFoldingStrategy.
        /// </summary>

        public static readonly char[] Delimiters = new char[] { '&', '<', '>', '~', '!', '%', '^', '*', '(', ')', '-', '+', '=', '|', '\\', '#', '/', '{', '}', '[', ']', ':', ';', ' ', '\n', '\r', '\t' };

        public VerilogFoldingStrategy()
        {
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            return CreateNewFoldings(document);
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            List<NewFolding> newFoldings = new List<NewFolding>();

            Stack<int> startOffsets = new Stack<int>();
            List<string> openingBrace = OpeningBrace;
            List<string> closingBrace = ClosingBrace;

            List<string> text = MySplit(document.Text.ToLower());
            int index = 0;
            for (int i = 0; i < text.Count; i++)
            {
                string s = text[i];
                //пропускаем комментарий
                if (s.Contains("/*"))
                {
                    index += s.Length;
                    continue;
                }
                //пропускаем разделительный символ
                if ((s.Length == 0) && (IsDelimeter(s[0])))
                {
                    index++;
                    continue;
                }
                //проверяем является ли слово закрывающим блок
                if (closingBrace.Contains(s))
                {
                    if (startOffsets.Count != 0)
                    {
                        int startOffset = startOffsets.Pop();
                        if ((startOffset < index + s.Length - 1))
                        {
                            newFoldings.Add(new NewFolding(startOffset, index + s.Length));
                        }
                    }
                }
                //проверяес являится ли слово открывающим блок
                if ((openingBrace.Contains(s)))
                {
                    startOffsets.Push(index);
                    index += s.Length;
                    continue;
                }
                index += s.Length;
            }

            newFoldings.Sort((f1, f2) => f1.StartOffset.CompareTo(f2.StartOffset));
            return newFoldings;
        }

        public static List<string> MySplit(string text)
        {
            List<string> res = new List<string>();
            int index = 0;
            System.Text.StringBuilder word = new System.Text.StringBuilder();

            while (index < text.Length)
            {
                char symbol = text[index];

                //вырезаем комментарий
                if ((symbol == '/') && (index + 1 < text.Length) && (text[index + 1] == '*'))
                {
                    if (word.Length != 0)
                        res.Add(word.ToString());
                    word.Clear();
                    while ((index < text.Length - 1) && (text[index] != '*') && (text[index+1] != '/'))
                    {
                        word.Append(text[index]);
                        index++;
                    }
                }

                //проверяем являится ли символ разделителем
                if (IsDelimeter(symbol) == true)
                {
                    if (word.Length != 0)
                        res.Add(word.ToString());
                    word.Clear();
                    res.Add(string.Empty + symbol);
                    index++;
                    continue;
                }
                word.Append(symbol);
                index++;
            }
            if (word.Length != 0)
                res.Add(word.ToString());
            return res;
        }

        private static bool IsDelimeter(char symbol)
        {
            foreach (char s in Delimiters)
                if (s.Equals(symbol))
                    return true;
            return false;
        }
    }
}