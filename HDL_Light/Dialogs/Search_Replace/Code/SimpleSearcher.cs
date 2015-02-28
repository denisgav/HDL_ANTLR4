using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Schematix
{
    public class SimpleSearcher : CustomSearcher
    {
        public SimpleSearcher(string Text, string SearchedText, int StartOffset)
            :base(Text, SearchedText, StartOffset)
        {
        }

        public override ICSharpCode.AvalonEdit.Document.TextSegment FindNext()
        {
            if (base.SearchUp == false)
                return Next();
            else
                return Previous();
        }

        private ICSharpCode.AvalonEdit.Document.TextSegment Next()
        {
            ICSharpCode.AvalonEdit.Document.TextSegment res = new ICSharpCode.AvalonEdit.Document.TextSegment();
            Regex pattern = CreateRegularExpression();
            foreach (Match m in pattern.Matches(Text))
                if ((m.Success) && (StartOffset <= m.Index))
                {
                    res.StartOffset = m.Index;
                    res.Length = m.Length;
                    StartOffset = m.Index;
                    return res;
                }
            return res;
        }

        private ICSharpCode.AvalonEdit.Document.TextSegment Previous()
        {
            ICSharpCode.AvalonEdit.Document.TextSegment res = new ICSharpCode.AvalonEdit.Document.TextSegment();
            Regex pattern = CreateRegularExpression();
            foreach (Match m in pattern.Matches(Text))
                if ((m.Success) && (StartOffset >= (m.Index + m.Length)))
                {
                    res.StartOffset = m.Index;
                    res.Length = m.Length;
                }
            return res;
        }

        protected override Regex CreateRegularExpression()
        {
            Regex pattern = null;
            if (MatchCase == true)
            {
                pattern = new Regex(SearchedText);
            }
            else
            {
                pattern = new Regex(SearchedText, RegexOptions.IgnoreCase);
            }

            return pattern;
        }
    }
}
