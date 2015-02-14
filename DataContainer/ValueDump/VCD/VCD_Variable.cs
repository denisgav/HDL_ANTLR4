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
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using DataContainer.Objects;
using VHDL;

namespace DataContainer.ValueDump
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
        private int startIndex;
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        /// <summary>
        /// Конечный индекс в шине
        /// </summary>
        private int endIndex;
        public int EndIndex
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
            this._iterator = var.Dump.Iterator;
            _iterator.Reset();
            //Определяем тип переменной
            string type = var.Type.Name;
            _iterator = var.Dump.Iterator;
            size = 1;
            switch (type.ToUpper())
            {
                case "BIT":
                case "STD_ULOGIC":
                case "STD_LOGIC":
                    var_type = VCDVariableType.Reg;
                    break;
                case "BIT_VECTOR":
                case "STD_ULOGIC_VECTOR":
                case "STD_LOGIC_VECTOR":
                    ResolvedDiscreteRange range = var.Type.Dimension[0];
                    startIndex = range.From;
                    endIndex = range.To;
                    size = (UInt32) (Math.Abs(endIndex - startIndex) + 1);
                    var_type = VCDVariableType.Reg;
                    break;                
                case "INTEGER":
                    var_type = VCDVariableType.Integer;
                    size = 32;
                    startIndex = 31;
                    endIndex = 0;
                    break;
                case "REAL":
                    var_type = VCDVariableType.Real;
                    break;
                default:
                    break;
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
                    bool parse_res = int.TryParse(temp[1], out var.startIndex);
                    parse_res = int.TryParse(temp[2], out var.endIndex);
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
                    var.base_variable = TypeCreator.CreateSignal(var.Reference, VHDL.builtin.Standard.INTEGER);
                    var.appendFunction = VCDConvertor.Append_Integer_VALUE;
                    break;

                case VCD_Variable.VCDVariableType.Real:
                    var.base_variable = TypeCreator.CreateSignal(var.Reference, VHDL.builtin.Standard.REAL);
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
                            var.base_variable = TypeCreator.CreateSignal(var.Reference, VHDL.builtin.StdLogic1164.STD_ULOGIC);
                            var.appendFunction = VCDConvertor.Append_STD_ULOGIC_VALUE;
                        }
                        else
                        {
                            ResolvedDiscreteRange range = ResolvedDiscreteRange.FormIntegerIndexes(var.StartIndex, var.EndIndex);
                            VHDL.type.ISubtypeIndication arrayType =  VHDL.builtin.StdLogic1164.Create_STD_ULOGIC_VECTOR(new Range(range.From, (range.RangeDirection == RangeDirection.To)?Range.RangeDirection.TO:Range.RangeDirection.DOWNTO, range.To));
                            var.base_variable = TypeCreator.CreateSignal(var.Reference, arrayType);
                            var.appendFunction = VCDConvertor.Append_STD_ULOGIC_VECTOR_VALUE;
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


        private IValueIterator _iterator;
        public IValueIterator Iterator
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
