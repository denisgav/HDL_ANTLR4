//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDLRuntime.ValueDump;

namespace VHDLRuntime.MySortedDictionary
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
