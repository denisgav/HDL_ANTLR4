using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение массива
    /// </summary>
    [System.Serializable]
    public class RecordValue : CompositeValue, IEnumerable<KeyValuePair<string, AbstractValue>>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RecordValue(VHDL.type.RecordType type, IDictionary<string, AbstractValue> _value)
            : base(ModellingType.CreateModellingType(type))
        {
            CreateRecordValue(type, _values);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RecordValue(VHDL.type.RecordType type)
            : base(ModellingType.CreateModellingType(type))
        {
            CreateRecordValue(type);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RecordValue(ModellingType type)
            : base (type)
        {
            if (type.Type is VHDL.type.RecordType)
                CreateRecordValue(type.Type as VHDL.type.RecordType);
            else
                throw new Exception("Invalid type");
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RecordValue(ModellingType type, IList<AbstractValue> _value)
            : base(type)
        {
            if (type.Type is VHDL.type.RecordType)
                CreateRecordValue(type.Type as VHDL.type.RecordType, _value);
            else
                throw new Exception("Invalid type");
        }

        private void CreateRecordValue(VHDL.type.RecordType recordType, IList<AbstractValue> _value)
        {
            _values = new Dictionary<string, AbstractValue>();
            int index = 0;
            foreach (var v in (Type.Type as VHDL.type.RecordType).Elements)
            {
                foreach (string name in v.Identifiers)
                {
                    _values.Add(name, _value[index]);
                    index++;
                }
            }
        }

        private void CreateRecordValue(VHDL.type.RecordType type, IDictionary<string, AbstractValue> _value)
        {
            _values = new Dictionary<string,AbstractValue>();
            foreach (var v in _value)
            {
                if (IsContainField(v.Key))
                    _values.Add(v.Key, v.Value);
                else
                    throw new Exception(string.Format("Invalid field name {0}.", v.Key));
            }
        }


        private void CreateRecordValue(VHDL.type.RecordType type)
        {
            _values = new Dictionary<string, AbstractValue>();
            foreach (var v in type.Elements)
            {
                foreach (string name in v.Identifiers)
                    _values.Add(name, null);
            }
        }

        private bool IsContainField(string name)
        {
            VHDL.type.RecordType type = Type.Type as VHDL.type.RecordType;
            foreach (var v in type.Elements)
            {
                foreach(string fieldName in v.Identifiers)
                    if(fieldName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return true;
            }
            return false;
        }


        /// <summary>
        /// Хранимые значения
        /// </summary>
        private Dictionary<string, AbstractValue> _values;
        public Dictionary<string, AbstractValue> Values
        {
            get { return _values; }
            set { _values = value; }
        }


        /// <summary>
        /// Перечисление хранимых данных
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, AbstractValue>> GetEnumerator()
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

        /// <summary>
        /// Получение элемента по его индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AbstractValue this[string index]
        {
            get
            {
                if (_values.ContainsKey(index))
                    return _values[index];
                else
                    throw new Exception("Invalid Index");
            }
            set
            {
                if (_values.ContainsKey(index))
                    _values[index] = value;
                else
                    throw new Exception("Invalid Index");
            }
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

        ~RecordValue()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }
    }
}
