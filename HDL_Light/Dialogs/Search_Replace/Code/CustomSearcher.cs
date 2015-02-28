using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Schematix
{
    public class CustomSearcher
    {
        public string Text { get; set; }
        public string SearchedText { get; set; }
        public int StartOffset { get; set; }
        public bool MatchCase { get; set; }
        public bool SearchUp { get; set; }

        public virtual ICSharpCode.AvalonEdit.Document.TextSegment FindNext()
        {
            return new ICSharpCode.AvalonEdit.Document.TextSegment();
        }
        protected virtual Regex CreateRegularExpression()
        {
            return new Regex(string.Empty);
        }

        public CustomSearcher(string Text, string SearchedText, int StartOffset)
        {
            this.Text = Text;
            this.SearchedText = SearchedText;
            this.StartOffset = StartOffset;
            SearchUp = false;
        }
    }
}
