using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.ValueDump;

namespace DataContainer
{
    /// <summary>
    /// Тип представление аналоговой величины
    /// </summary>
    public enum AnalogDataPresentation
    {
        None,         //Выводить в обычном режиме
        Step,         //В виде ступенек
    }

    /// <summary>
    /// Настройки для десятичного представления числа
    /// </summary>
    public enum DecimalDataPresentation
    {
        Unsigned,   //Беззнаковый
        Complement, //Обратный код
        Twos_Сomplement //Дополнительный код (Two's complement)
    }

    /// <summary>
    /// Класс для задания представления данных (для всех типов данных)
    /// </summary>
    [Serializable()]
    public class DataRepresentation
    {
        /// <summary>
        /// Инвертировать данные
        /// </summary>
        private bool isInverted;
        public bool IsInverted 
        {
            get { return isInverted; }
            set { isInverted = value; }
        }

        /// <summary>
        /// Вариант с отображением аналоговой величины
        /// </summary>
        private bool isAnalog;
        public bool IsAnalog
        {
            get { return isAnalog; }
            set { isAnalog = value; }
        }

        public DataRepresentation()
        {
            isInverted = false;
        }

        /// <summary>
        /// Представление данных по-умолчанию
        /// </summary>
        public static DataRepresentation DefaultDataRepresentation
        {
            get { return new DataRepresentation() { isInverted = false, isAnalog = false };; }
        }
    }

    /// <summary>
    /// Класс для задания представления данных (для вектора)
    /// </summary>
    [Serializable()]
    public class VectorDataRepresentation : DataRepresentation
    {
        public EnumerationSystem EnumerationSystem { get; set; }

        /// <summary>
        /// Настройки для десятичного представления числа
        /// </summary>
        private DecimalDataPresentation decimalDataPresentation;
        public DecimalDataPresentation DecimalDataPresentation 
        {
            get { return decimalDataPresentation; }
            set { decimalDataPresentation = value; }
        }

        /// <summary>
        /// Выводить биты в обратном порядке
        /// </summary>
        private bool isReorderedBits;
        public bool IsReorderedBits 
        { 
            get { return isReorderedBits; }
            set { isReorderedBits = value; }
        }

        #region Данные для типа данных Real
        /// <summary>
        /// Количество бит для задания основы
        /// </summary>
        public int NumBaseForReal { get; set; }

        /// <summary>
        /// Количество бит для указания показателя степени
        /// </summary>
        public int NumPowForReal { get; set; }
        #endregion

        public VectorDataRepresentation()
        {
            DecimalDataPresentation = DecimalDataPresentation.Unsigned;
            IsInverted = false;
            isReorderedBits = false;
            EnumerationSystem = EnumerationSystem.Bin;
            NumBaseForReal = 23;
            NumPowForReal = 9;
        }
    }
}
