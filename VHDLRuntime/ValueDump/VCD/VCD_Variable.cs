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

/*
    $var
    The $var section lists the names and identifier codes of all the variables.

	Syntax:
		$var var_type size identifier reference $end
		var_type =  event | integer | parameter | real | reg | 
                            supply0 | supply1 | time | tri | triand | 
                            trior | trireg | tri0 | tri1 | wand | wire | wor
		size = decimal value of number of bits.
		identifier = name of the variable in printable ASCII characters.
		reference = bit or vector name mapped to the identifier
	Examples:
		$var wire 1 * en_q $end
		$var reg 8 ( data_q[7:0] $end
	*/

using System;
using System.Collections.Generic;
using System.Text;
using VHDLRuntime.Values;
using VHDLRuntime.MySortedDictionary;
using VHDLRuntime.Objects;

namespace VHDLRuntime.ValueDump
{
    public class VCD_Variable
    {
        public enum VCDVariableType
        {
            Event,
            Integer,
            Parameter,
            Real,
            Reg,
            Supply0,
            Supply1,
            Time,
            Tri,
            Triand,
            Trior,
            Trireg,
            Tri0,
            Tri1,
            Wand,
            Wire,
            Wor
        }

        /// <summary>
        /// Тип переменной
        /// </summary>
        private VCDVariableType var_type;
        public VCDVariableType VariableType
        {
            get
            {
                return var_type;
            }
        }


        /// <summary>
        /// Целое число, указывающее количество
        /// бит которое занимает переменная
        /// </summary>
        private uint size;
        public uint Size
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        /// Начальный индекс в шине
        /// </summary>
        private Int64 startIndex;
        public Int64 StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        /// <summary>
        /// Конечный индекс в шине
        /// </summary>
        private Int64 endIndex;
        public Int64 EndIndex
        {
            get { return endIndex; }
            set { endIndex = value; }
        }

        private string identifier;
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        private string reference;
        public string Reference
        {
            get
            {
                return reference;
            }
        }

        public string VCDVariableDeclaration
        {
            get
            {
                if ((startIndex == 0) && (endIndex == 0))
                    return string.Format("{0} {1} {2} {3}", var_type.ToString(), 1, identifier, reference);
                else
                    return string.Format("{0} {1} {2} {3}[{4}:{5}]", var_type.ToString(), size, identifier, reference, startIndex, endIndex);
            }
        }

        private VCDScope[] scope;
        public VCDScope[] Scope
        {
            get
            {
                return scope;
            }
            set
            {
                scope = value;
            }
        }

        /// <summary>
        /// Ссылка на обьект My_Variable к которому относится эта запись
        /// </summary>
        private Signal base_variable;
        public Signal Variable
        {
            get { return base_variable; }
            set { base_variable = value; }
        }

        #region constructors
        public VCD_Variable()
        {}

        public VCD_Variable(Signal var, string identifier)
        {
            this.base_variable = var;
            this.identifier = identifier;
            this.reference = var.Name;
            this._iterator = new NewSortedDictionaryIterator<VHDLBaseValue>(var.Dump);
            _iterator.Reset();
            //Определяем тип переменной
            Type type = var.Type;
            size = 1;
            if(type == typeof(VHDLRuntime.Values.BuiltIn.BIT))
            {
                var_type = VCDVariableType.Reg;
            }
            if(type == typeof(VHDLIntegerValue))
            {
                    var_type = VCDVariableType.Integer;
                    size = 32;
                    startIndex = 31;
                    endIndex = 0;
            }
            if(type == typeof(VHDLFloatingPointValue))
            {
                    var_type = VCDVariableType.Real;
            }
        }
        #endregion

        public static VCD_Variable Parse(string[] Words)
        {
            VCD_Variable var = new VCD_Variable();

            // var.var_type
            switch (Words[1].ToLower())
            {
                case "event": var.var_type = VCDVariableType.Event; break;
                case "integer": var.var_type = VCDVariableType.Integer;break;
                case "parameter":var.var_type = VCDVariableType.Parameter;break;
                case "real": var.var_type = VCDVariableType.Real;break;
                case "reg":var.var_type = VCDVariableType.Reg;break;
                case "supply0":var.var_type = VCDVariableType.Supply0;break;
                case "supply1":var.var_type = VCDVariableType.Supply1;break;
                case "time":var.var_type = VCDVariableType.Time;break;
                case "tri":var.var_type = VCDVariableType.Tri;break;
                case "triand":var.var_type = VCDVariableType.Triand;break;
                case "trior":var.var_type = VCDVariableType.Trior;break;
                case "trireg":var.var_type = VCDVariableType.Trireg;break;
                case "tri0":var.var_type = VCDVariableType.Tri0;break;
                case "tri1":var.var_type = VCDVariableType.Tri1;break;
                case "wand":var.var_type = VCDVariableType.Wand;break;
                case "wire":var.var_type = VCDVariableType.Wire;break;
                case "wor": var.var_type = VCDVariableType.Wor; break;
            }

            var.size = uint.Parse(Words[2]);
            var.identifier = Words[3];

            //Если встераем сложное имя переменной с учетом ее размерности
            if ((var.size != 1))
            {
                if (Words[4].Contains(":"))
                {
                    string[] temp = Words[4].Split(new char[] { ' ', '[', ']', ':' });
                    var.reference = temp[0];
                    bool parse_res = Int64.TryParse(temp[1], out var.startIndex);
                    parse_res = Int64.TryParse(temp[2], out var.endIndex);
                }
                else
                {
                    var.reference = Words[4];
                    var.endIndex = (int)var.size - 1;
                    var.startIndex = 0;
                }
            }
            else
            {
                var.reference = Words[4];
            }

            switch (var.VariableType)
            {
                case VCD_Variable.VCDVariableType.Integer:
                    var.base_variable = new Signal<VHDLIntegerValue>(new VHDLIntegerValue(0), var.Reference);
                    var.appendFunction = VCDConvertor.Append_Integer_VALUE;
                    break;

                case VCD_Variable.VCDVariableType.Real:
                    var.base_variable = new Signal<VHDLFloatingPointValue>(new VHDLFloatingPointValue(0), var.Reference);
                    var.appendFunction = VCDConvertor.Append_Real_VALUE;
                    break;

                case VCD_Variable.VCDVariableType.Time:
                    break;

                case VCD_Variable.VCDVariableType.Event:
                case VCD_Variable.VCDVariableType.Parameter:
                case VCD_Variable.VCDVariableType.Reg:
                case VCD_Variable.VCDVariableType.Supply0:
                case VCD_Variable.VCDVariableType.Supply1:
                case VCD_Variable.VCDVariableType.Tri:
                case VCD_Variable.VCDVariableType.Tri0:
                case VCD_Variable.VCDVariableType.Tri1:
                case VCD_Variable.VCDVariableType.Triand:
                case VCD_Variable.VCDVariableType.Trior:
                case VCD_Variable.VCDVariableType.Trireg:
                case VCD_Variable.VCDVariableType.Wand:
                case VCD_Variable.VCDVariableType.Wire:
                case VCD_Variable.VCDVariableType.Wor:
                    {
                        if (var.Size == 1)
                        {
                            new Signal<VHDLRuntime.Values.BuiltIn.BIT>(new VHDLRuntime.Values.BuiltIn.BIT(VHDLRuntime.Values.BuiltIn.BIT_Enum.item_0), var.Reference);
                            var.appendFunction = VCDConvertor.Append_BIT_VALUE;
                        }
                        else
                        {
                            
                        }
                    }
                    break;

            }

            return var;
        }

        /// <summary>
        /// Полное имя переменной с учетом Scope
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder res = new StringBuilder();
                for (int i = 0; i < scope.Length; i++)
                    res.AppendFormat("{0}.", scope[i].Name);
                res.Append(reference);
                return res.ToString();
            }
        }


        private IValueIterator<VHDLBaseValue> _iterator;
        public IValueIterator<VHDLBaseValue> Iterator
        {
            get { return _iterator; }
        }

        /// <summary>
        /// Функция для распарсивания строки
        /// </summary>
        private Action<string, Signal, UInt64> appendFunction;

        internal void AppendValue(UInt64 CurrentTime, string newvalue)
        {
            appendFunction(newvalue, Variable, CurrentTime);
        }
    }
}
