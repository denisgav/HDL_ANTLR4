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

        public override My_Editor.Document.ISegment FindNext()
        {
            if (base.SearchUp == false)
                return Next();
            else
                return Previous();
        }

        private My_Editor.Document.ISegment Next()
        {
            My_Editor.Document.TextSegment res = new My_Editor.Document.TextSegment();
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

        private My_Editor.Document.ISegment Previous()
        {
            My_Editor.Document.TextSegment res = new My_Editor.Document.TextSegment();
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
