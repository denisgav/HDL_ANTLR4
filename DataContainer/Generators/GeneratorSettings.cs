using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Generator
{
    /// <summary>
    /// Структура, хранящая в себе настройки генератора
    /// </summary>
    public class GeneratorSettings
    {
        /// <summary>
        /// Тип генерируемых значений
        /// </summary>
        [Flags]
        public enum GeneratedValue
        {
            EnumerableValue = 0x1,
            IntegerValue = 0x2,
            DoubleValue = 0x4,
            BoolArray = 0x8,
            String = 0x16
        }


        /// <summary>
        /// Размер данных в битах
        /// </summary>
        private uint size;
        public uint Size
        {
            get { return size; }
            set
            {
                if (value == 0)
                    throw new Exception("Invalid size");
                else
                    size = value;
            }
        }

        private GeneratedValue genValue;
        public GeneratedValue GenValue
        {
            get { return genValue; }
            set { genValue = value; }
        }

        /// <summary>
        /// Наиболее удобный тип данных
        /// </summary>
        private GeneratedValue privilegeValue;
        public GeneratedValue PrivilegeValue
        {
            get { return privilegeValue; }
            set { privilegeValue = value; }
        }
        

        /// <summary>
        /// Если тип данных перечислимый - то возможные его значения
        /// (хранится в виде строки)
        /// </summary>
        private List<object> enumerableValues;
        public List<object> EnumerableValues
        {
            get { return enumerableValues; }
            set { enumerableValues = value; }
        }

        public GeneratorSettings(uint sizeInBits, GeneratedValue GenValue, GeneratedValue PrivilegeValue)
        {
            this.size = sizeInBits;
            genValue = GenValue;
            privilegeValue = PrivilegeValue;
            enumerableValues = new List<object>();
        }

        public GeneratorSettings(uint sizeInBits, GeneratedValue GenValue, GeneratedValue PrivilegeValue, List<object> EnumerableValues)
        {
            this.size = sizeInBits;
            genValue = GenValue;
            privilegeValue = PrivilegeValue;
            enumerableValues = EnumerableValues;
        }
    }
}
