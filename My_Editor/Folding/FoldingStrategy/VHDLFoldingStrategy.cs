using System;
using System.Collections.Generic;
using My_Editor.Document;
using My_Editor.Folding;
using VHDL.parser;
using My_Editor.Lexter;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace My_Editor.Folding
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
        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            //throw new NotSupportedException();
            /*
            if ((lexter.Tree != null) && (lexter.TokenStream != null))
            {
                List<NewFolding> newFoldings = new List<NewFolding>();
                CommonTreeNodeStream nodes = new CommonTreeNodeStream(lexter.Tree.Tree);
                nodes.TokenStream = lexter.TokenStream;

                object o = null;
                nodes.Reset();
                do
                {
                    o = nodes.NextElement();
                    if (o is CommonTree)
                    {
                        CommonTree elem = o as CommonTree;
                        if (IsTreeElementToFolding(elem) && (elem.Token != null))
                        {
                            IToken tokenStart = lexter.TokenStream.Get(elem.TokenStartIndex);
                            IToken tokenEnd = lexter.TokenStream.Get(elem.TokenStopIndex);
                            if ((tokenStart != null) && (tokenEnd != null) && (tokenStart != tokenEnd) && (tokenEnd.StopIndex > tokenStart.StartIndex))
                            {
                                NewFolding folding = new NewFolding(tokenStart.StartIndex, tokenEnd.StopIndex);
                                folding.Name = elem.Text;
                                newFoldings.Add(folding);
                            }
                        }
                    }
                }
                while(nodes.IsEndOfFile(o) == false);

                newFoldings.Sort((f1, f2) => f1.StartOffset.CompareTo(f2.StartOffset));
                return newFoldings;
            }
            */
            return new List<NewFolding>();
        }

        private bool IsTreeElementToFolding(ITree t)
        {
            foreach(string s in Tree_Type_To_Folding)
                if(s.Equals(t.ToString(), StringComparison.InvariantCultureIgnoreCase))
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
