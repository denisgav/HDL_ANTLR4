using System;
using System.Collections.Generic;
using VHDL.parser;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Document;
using HDL_EditorExtension.Lexter;

namespace HDL_EditorExtension.Folding
{
    /// <summary>
    /// Allows producing foldings from a document based on braces.
    /// </summary>
    public class VHDLFoldingStrategy : AbstractFoldingStrategy
    {
        private VHDL_Lexter lexter;

        public VHDL_Lexter Lexter
        {
            get { return lexter; }
            set { lexter = value; }
        }

        public VHDLFoldingStrategy(VHDL_Lexter parent)
        {
            this.lexter = parent;
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document)
        {
            if (lexter.Tree != null)
            {
                List<NewFolding> newFoldings = new List<NewFolding>(){ };

                newFoldings.AddRange(CreateNewFoldings(lexter.Tree));

                newFoldings.Sort((f1, f2) => f1.StartOffset.CompareTo(f2.StartOffset));
                return newFoldings;
            }
            return new List<NewFolding>();
        }

        
        public IEnumerable<NewFolding> CreateNewFoldings(IParseTree tree)
        {
            List<NewFolding> res = new List<NewFolding>();
            ParserRuleContext rc = tree as ParserRuleContext;
            if(rc == null)
                return res;

            if (IsTreeElementToFolding(tree))
            {
                IToken tokenStart = rc.Start;
                IToken tokenEnd = rc.Stop;
                if ((tokenStart != null) && (tokenEnd != null) && (tokenStart != tokenEnd) && (tokenEnd.StopIndex > tokenStart.StartIndex))
                {
                    NewFolding folding = new NewFolding(tokenStart.StartIndex, tokenEnd.StopIndex);
                    folding.Name = tree.GetText();
                    res.Add(folding);
                }
            }

            for (int i = 0; i < rc.ChildCount; i++)
            {
                res.AddRange(CreateNewFoldings(rc.GetChild(i)));
            }

            return res;
        }

        private bool IsTreeElementToFolding(IParseTree t)
        {
            string text = t.GetText().ToLower();
            foreach(string s in Tree_Type_To_Folding)
                if(text.Contains(s))
                    return true;
            return false;
        }

        private static readonly string[] Tree_Type_To_Folding = new string[] 
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
            "loop"
        };

        public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = 0;
            return CreateNewFoldings(document);
        }
    }
}
