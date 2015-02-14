using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение массива
    /// </summary>
    [System.Serializable]
    public class ArrayValue:CompositeValue, IEnumerable<KeyValuePair<int[], AbstractValue>>
    {
        #region Constructors
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.ConstrainedArray type, IList<AbstractValue> _value, ResolvedDiscreteRange range)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[]{range}))
        {
            _values = CreateDictionary(_value, range);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.ConstrainedArray type, IList<AbstractValue> _value)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { ResolvedDiscreteRange.FormIntegerIndexes(_value.Count) }))
        {
            _values = CreateDictionary(_value);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.ConstrainedArray type, Dictionary<int[], AbstractValue> dictionary)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { ResolvedDiscreteRange.FormIntegerIndexes(dictionary.Count) }))
        {
            _values = dictionary;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.ConstrainedArray type, ResolvedDiscreteRange range)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] {range}))
        {
            _values = CreateDictionary(range);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.UnconstrainedArray type, IList<AbstractValue> _value, ResolvedDiscreteRange range)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { range }))
        {
            _values = CreateDictionary(_value, range);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.UnconstrainedArray type, IList<AbstractValue> _value)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { ResolvedDiscreteRange.FormIntegerIndexes(_value.Count) }))
        {
            _values = CreateDictionary(_value);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.UnconstrainedArray type, Dictionary<int[], AbstractValue> dictionary)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { ResolvedDiscreteRange.FormIntegerIndexes(dictionary.Count) }))
        {
            _values = dictionary;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(VHDL.type.UnconstrainedArray type, ResolvedDiscreteRange range)
            : base(ModellingType.CreateModellingType(type, new ResolvedDiscreteRange[] { range }))
        {
            _values = CreateDictionary(range);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(ModellingType modellingType)
            : base(modellingType)
        {
            _values = CreateDictionary(modellingType.Dimension);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ArrayValue(ModellingType modellingType, IList<AbstractValue> _value)
            : base(modellingType)
        {
            _values = CreateDictionary(_value, modellingType.Dimension);
        }

        #endregion

        /// <summary>
        /// Создание словаря по входным данным
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Dictionary<int[], AbstractValue> CreateDictionary(IList<AbstractValue> _value, ResolvedDiscreteRange range)
        {
            if (_value.Count != range.Length)
                throw new Exception("Invalid length of list");

            Dictionary<int[], AbstractValue> res = new Dictionary<int[], AbstractValue>();
            for (int i = 0; i <= range.Length; i++)
                res.Add(new int[]{i}, _value[i]);

            return res;
        }

        /// <summary>
        /// Создание словаря по входным данным
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private Dictionary<int[], AbstractValue> CreateDictionary(IList<AbstractValue> _value, ResolvedDiscreteRange[] resolvedDiscreteRange)
        {
            int[,] ranges = ResolvedDiscreteRange.CombineRanges(resolvedDiscreteRange);

            if (_value.Count != ranges.GetLength(0))
                throw new Exception("Invalid length of list");

            Dictionary<int[], AbstractValue> res = new Dictionary<int[], AbstractValue>();
            for (int i = 0; i < ranges.GetLength(0); i++)
            {
                int [] indexes = new int [ranges.GetLength(1)];
                for (int j = 0; j < ranges.GetLength(1); j++)
                    indexes[j] = ranges[i ,j];
                res.Add(indexes, _value[i]);
            }
            return res;
        }

        /// <summary>
        /// Создание словаря по входным данным
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private Dictionary<int[], AbstractValue> CreateDictionary(ResolvedDiscreteRange[] resolvedDiscreteRange)
        {
            int[,] ranges = ResolvedDiscreteRange.CombineRanges(resolvedDiscreteRange);
            Dictionary<int[], AbstractValue> res = new Dictionary<int[], AbstractValue>();
            for (int i = 0; i <= ranges.GetLength(0); i++)
            {
                int[] indexes = new int[ranges.GetLength(1)];
                for (int j = 0; j < ranges.GetLength(1); j++)
                    indexes[j] = ranges[i, j];
                res.Add(indexes, null);
            }
            return res;
        }

        /// <summary>
        /// Создание словаря по входным данным
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Dictionary<int[], AbstractValue> CreateDictionary(ResolvedDiscreteRange range)
        {
            Dictionary<int[], AbstractValue> res = new Dictionary<int[], AbstractValue>();
            for (int i = 0; i <= range.Length; i++)
                res.Add(new int[] { i }, null);
            return res;
        }

        /// <summary>
        /// Создание словаря по входным данным
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static Dictionary<int[], AbstractValue> CreateDictionary(IList<AbstractValue> _value)
        {
            Dictionary<int[], AbstractValue> res = new Dictionary<int[], AbstractValue>();
            int i = 1;

            foreach (AbstractValue v in _value)
            {
                res.Add(new int[]{i}, v);
                i++;
            }
            return res;
        }

        /// <summary>
        /// Хранимые значения
        /// </summary>
        private Dictionary<int[], AbstractValue> _values;
        public Dictionary<int[], AbstractValue> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        /// <summary>
        /// Перечисление хранимых данных
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<int[], AbstractValue>> GetEnumerator()
        {
            foreach (var v in _values)
                yield return v;
        }

        /// <summary>
        /// Перечисление хранимых данных
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var v in _values)
                yield return v;
        }

        public override IList<AbstractValue> GetChilds()
        {
            return new List<AbstractValue>(_values.Values);
        }

        public override void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (_values != null)
            {
                foreach (var v in _values)
                    v.Value.Dispose();
                _values.Clear();
            }
            _values = null;
        }

        ~ArrayValue()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }
    }
}
