using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class Range
    {
        public int leftRange, rightRange;
        public Range()
        {
            leftRange = 0;
            rightRange = 0;
        }

        public Range(ref List<string> str)
        {
            isReverseRange = str.Contains("downto");
            int index_of_devide;
            if (isReverseRange)
                index_of_devide = str.IndexOf("downto");
            else
                index_of_devide = str.IndexOf("to");

            List<string> leftExpression = new List<string>(str); leftExpression.RemoveRange(index_of_devide, str.Count - index_of_devide); leftExpression.RemoveAll(Globals.isParen);
            List<string> rightExpression = new List<string>(str); rightExpression.RemoveRange(0, index_of_devide + 1); rightExpression.RemoveAll(Globals.isParen);

            leftRange = (int)Globals.Calculate(ref leftExpression);
            rightRange = (int)Globals.Calculate(ref rightExpression);

        }
        public bool isReverseRange;//true => downto, false => to
    }
}
