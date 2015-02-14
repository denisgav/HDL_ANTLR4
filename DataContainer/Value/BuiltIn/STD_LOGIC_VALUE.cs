using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.builtin;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных STD_LOGIC
    /// </summary>
    [System.Serializable]
    public class STD_LOGIC_VALUE:STD_ULOGIC_VALUE
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public STD_LOGIC_VALUE(EnumerationLiteral enumerationLiteral)
            : base(StdLogic1164.STD_ULOGIC, enumerationLiteral)
        {            
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public STD_LOGIC_VALUE()
        {
        }

        ~STD_LOGIC_VALUE()
        { }

        private static EnumerationLiteral[][] resolution_table = new EnumerationLiteral[][]
        {
                    //--      ---------------------------------------------------------
                    //--      |  U    X    0    1    Z    W    L    H    -        |   |
                    //--      ---------------------------------------------------------
                    //        ( 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U' ), -- | U |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' ), -- | X |
                    //        ( 'U', 'X', '0', 'X', '0', '0', '0', '0', 'X' ), -- | 0 |
                    //        ( 'U', 'X', 'X', '1', '1', '1', '1', '1', 'X' ), -- | 1 |
                    //        ( 'U', 'X', '0', '1', 'Z', 'W', 'L', 'H', 'X' ), -- | Z |
                    //        ( 'U', 'X', '0', '1', 'W', 'W', 'W', 'W', 'X' ), -- | W |
                    //        ( 'U', 'X', '0', '1', 'L', 'W', 'L', 'W', 'X' ), -- | L |
                    //        ( 'U', 'X', '0', '1', 'H', 'W', 'W', 'H', 'X' ), -- | H |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' )  -- | - |
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_Z, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_L, StdLogic1164.STD_ULOGIC_H, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_L, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_L, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_H, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_W, StdLogic1164.STD_ULOGIC_H, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X } 
        };

        public static STD_LOGIC_VALUE Resolve(STD_LOGIC_VALUE[] values)
        {
            if ((values == null) || (values.Length == 0))
                return new STD_LOGIC_VALUE(StdLogic1164.STD_ULOGIC_U);
            EnumerationLiteral res = values[0].Value;
            for (int i = 1; i < values.Length; i++)
                res = resolution_table[GetIndexOfLiteral(res)][GetIndexOfLiteral(values[i].Value)];
            return new STD_LOGIC_VALUE(res);
        }

        public static STD_LOGIC_VALUE CreateSTD_LOGIC_VALUE(char value)
        {
            switch (value)
            {
                case 'U': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_U);
                case 'X': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_X);
                case '0': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_0);
                case '1': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_1);
                case 'Z': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_Z);
                case 'W': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_W);
                case 'L': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_L);
                case 'H': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_H);
                case '-': return new STD_LOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_LOGIC_DONT_CARE);
                default: return null;
            }
            return null;
        }
    }
}
