using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.expression;

namespace VHDL
{
    /// <summary>
    /// Направление индексирования
    /// </summary>
    public enum RangeDirection
    {
        To,
        DownTo
    }

    /// <summary>
    /// Класс, представляющий размерность
    /// </summary>
    [System.Serializable]
    public class ResolvedDiscreteRange
    {
        private static ResolvedDiscreteRange range1;
        static ResolvedDiscreteRange()
        {
            range1 = FormIntegerIndexes(0, 0);
        }
        public static ResolvedDiscreteRange Range1
        {
            get { return range1; }
        }

        /// <summary>
        /// Нижняя граница
        /// </summary>
        private int from;
        public int From
        {
            get { return from; }
        }

        /// <summary>
        /// Верхняя граница
        /// </summary>
        private int to;
        public int To
        {
            get { return to; }
        }
        
        /// <summary>
        /// Направление индексирования
        /// </summary>
        public RangeDirection RangeDirection
        {
            get { return (from>to)?RangeDirection.DownTo:RangeDirection.To; }
        }
        
        /// <summary>
        /// Длина диапазона
        /// </summary>
        public int Length
        {
            get { return Math.Abs(from-to)+1; }
        }

        private ResolvedDiscreteRange(int from, int to, List<Literal> indexes)
        {
            this.from = from;
            this.to = to;
            this.indexes = indexes;
        }

        public bool IsContainIndex(int index)
        {
            if (RangeDirection == RangeDirection.To)
                return (index >= from) && (index <= to);
            else
                return (index <= from) && (index >= to);
        }

        public Range CreateRange()
        {
            if (RangeDirection == VHDL.RangeDirection.To)
                return new Range(from, Range.RangeDirection.TO, to);
            else
                return new Range(from, Range.RangeDirection.DOWNTO, to);
        }

        public bool IsContainSubRange(ResolvedDiscreteRange subrange)
        {
            if (subrange.RangeDirection != RangeDirection)
                return false;
            else
            {
                if (RangeDirection == RangeDirection.To)
                    return (from <= subrange.from) && (to >= subrange.to);
                else
                    return (to <= subrange.to) && (from >= subrange.from);
            }
        }

        public int GetNormalizedIndex(int index)
        {
            if (RangeDirection == RangeDirection.To)
            {
                if ((index < from) || (index > to))
                    throw new Exception("Invalid Index");
                else
                    return index - from;
            }
            else
            {
                if ((index < to) || (index > from))
                    throw new Exception("Invalid Index");
                else
                    return index - to;
            }
        }

        public override string ToString()
        {
            if (RangeDirection == VHDL.RangeDirection.To)
                return string.Format("({0} to {1})", from, to);
            else
                return string.Format("({0} downto {1})", from, to);
        }

        /// <summary>
        /// Возвращение литерала по его индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Literal this[int index]
        {
            get { return indexes[index];}
        }


        #region Работа с элементами этого диапазона
        /// <summary>
        /// Элементы, которые входят в данный диапазон
        /// </summary>
        private List<Literal> indexes;
        public List<Literal> Indexes
        {
            get { return indexes; }
        }

        /// <summary>
        /// Использовать в случае целочисленных индексов
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public static ResolvedDiscreteRange FormIntegerIndexes(int From, int To)
        {
            foreach (var v in FormedIntegerRanges)
            {
                if ((v.Key[0] == From) && (v.Key[1] == To))
                    return v.Value;
            }

            List<Literal> indexes = new List<Literal>();
            if (From == To)
            {
                indexes.Add(new DecBasedInteger(From));
            }
            else
            {
                if (From < To)
                {
                    for (int i = From; i <= To; i++)
                        indexes.Add(new DecBasedInteger(i));
                }
                else
                {
                    for (int i = From; i >= To; i--)
                        indexes.Add(new DecBasedInteger(i));
                }
            }

            ResolvedDiscreteRange res = new ResolvedDiscreteRange(From, To, indexes);
            FormedIntegerRanges.Add(new int [] {From, To}, res);
            return res;
        }

        /// <summary>
        /// Использовать в случае целочисленных индексов
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static ResolvedDiscreteRange FormIntegerIndexes(int Length)
        {
            return FormIntegerIndexes(1, Length);
        }

        /// <summary>
        /// Уже сформированные объекты ResolvedDiscreteRange
        /// </summary>
        private static Dictionary<int[], ResolvedDiscreteRange> FormedIntegerRanges = new Dictionary<int[],ResolvedDiscreteRange>();

        /// <summary>
        /// Использовать в случае перечислимого индекса
        /// </summary>
        /// <param name="literals"></param>
        /// <returns></returns>
        public static ResolvedDiscreteRange FormEnumerationIndexes(List<EnumerationLiteral> literals)
        {
            foreach (var v in FormedEnumerationRanges)
                if (v.Key == literals)
                    return v.Value;

            List<Literal> indexes = new List<Literal>();
            indexes.AddRange(literals);
            ResolvedDiscreteRange res = new ResolvedDiscreteRange(1, literals.Count, indexes);
            FormedEnumerationRanges.Add(literals, res);
            return res;
        }

        /// <summary>
        /// Уже сформированные объекты ResolvedDiscreteRange
        /// </summary>
        private static Dictionary<List<EnumerationLiteral>, ResolvedDiscreteRange> FormedEnumerationRanges = new Dictionary<List<EnumerationLiteral>, ResolvedDiscreteRange>();
        #endregion

        /// <summary>
        /// Формирование массива литералов при формировании массива
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static int[,] CombineRanges(ResolvedDiscreteRange[] ranges)
        {
            int length = 1;
            for (int i = 0; i < ranges.Length; i++)
            {
                length *= ranges[i].Length;
            }
            int[,] res = new int[length, ranges.Length];
            int[] indexes = new int[ranges.Length];
            for (int i = 0; i < ranges.Length; i++)
            {
                indexes[i] = 0;
            }

            for(int index = 0; index <length; index++)
            {
                //Добавляем индексы к результующему набору литералов
                for (int i = 0; i < ranges.Length; i++)
                    res[index, i] = indexes[i];

                indexes[ranges.Length - 1]++;

                for (int i = ranges.Length - 1; i >= 1; i--)
                {
                    if (ranges[i].Length <= indexes[i])
                    {
                        indexes[i - 1]++;
                        indexes[i] = 0;
                    }
                }
            }

            return res;
        }
    }
}
