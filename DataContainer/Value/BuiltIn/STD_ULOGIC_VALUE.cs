using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.builtin;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных STD_ULOGIC
    /// </summary>
    [System.Serializable]
    public class STD_ULOGIC_VALUE : EnumerationValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public STD_ULOGIC_VALUE(EnumerationLiteral enumerationLiteral)
            : base(StdLogic1164.STD_ULOGIC, enumerationLiteral)
        {            
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public STD_ULOGIC_VALUE()
            : base(StdLogic1164.STD_ULOGIC)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public STD_ULOGIC_VALUE(bool value)
            : base(StdLogic1164.STD_ULOGIC)
        {
        }

        public static STD_ULOGIC_VALUE Create_STD_ULOGIC_VALUE(bool value)
        {
            if (value == true)
                return new STD_ULOGIC_VALUE(StdLogic1164.STD_ULOGIC_1);
            else
                return new STD_ULOGIC_VALUE(StdLogic1164.STD_ULOGIC_0);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        protected STD_ULOGIC_VALUE(VHDL.type.EnumerationType type, EnumerationLiteral enumerationLiteral)
            : base(type, enumerationLiteral)
        {
        }

        ~STD_ULOGIC_VALUE()
        { }

        private static EnumerationLiteral[][] and_table = new EnumerationLiteral[][]
        {
                    //--      ----------------------------------------------------
                    //--      |  U    X    0    1    Z    W    L    H    -         |   |
                    //--      ----------------------------------------------------
                    //        ( 'U', 'U', '0', 'U', 'U', 'U', '0', 'U', 'U' ),  -- | U |
                    //        ( 'U', 'X', '0', 'X', 'X', 'X', '0', 'X', 'X' ),  -- | X |
                    //        ( '0', '0', '0', '0', '0', '0', '0', '0', '0' ),  -- | 0 |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | 1 |
                    //        ( 'U', 'X', '0', 'X', 'X', 'X', '0', 'X', 'X' ),  -- | Z |
                    //        ( 'U', 'X', '0', 'X', 'X', 'X', '0', 'X', 'X' ),  -- | W |
                    //        ( '0', '0', '0', '0', '0', '0', '0', '0', '0' ),  -- | L |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | H |
                    //        ( 'U', 'X', '0', 'X', 'X', 'X', '0', 'X', 'X' )   -- | - |
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0 },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_0 },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X } 
        };

        private static EnumerationLiteral[][] or_table = new EnumerationLiteral[][]
        {
                    //--      ----------------------------------------------------
                    //--      |  U    X    0    1    Z    W    L    H    -         |   |
                    //--      ----------------------------------------------------
                    //        ( 'U', 'U', 'U', '1', 'U', 'U', 'U', '1', 'U' ),  -- | U |
                    //        ( 'U', 'X', 'X', '1', 'X', 'X', 'X', '1', 'X' ),  -- | X |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | 0 |
                    //        ( '1', '1', '1', '1', '1', '1', '1', '1', '1' ),  -- | 1 |
                    //        ( 'U', 'X', 'X', '1', 'X', 'X', 'X', '1', 'X' ),  -- | Z |
                    //        ( 'U', 'X', 'X', '1', 'X', 'X', 'X', '1', 'X' ),  -- | W |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | L |
                    //        ( '1', '1', '1', '1', '1', '1', '1', '1', '1' ),  -- | H |
                    //        ( 'U', 'X', 'X', '1', 'X', 'X', 'X', '1', 'X' )   -- | - |
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_U },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1 },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_1 },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X } 
        };

        private static EnumerationLiteral[][] xor_table = new EnumerationLiteral[][]
        {
                    //--      ----------------------------------------------------
                    //--      |  U    X    0    1    Z    W    L    H    -         |   |
                    //--      ----------------------------------------------------
                    //        ( 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U' ),  -- | U |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' ),  -- | X |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | 0 |
                    //        ( 'U', 'X', '1', '0', 'X', 'X', '1', '0', 'X' ),  -- | 1 |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' ),  -- | Z |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' ),  -- | W |
                    //        ( 'U', 'X', '0', '1', 'X', 'X', '0', '1', 'X' ),  -- | L |
                    //        ( 'U', 'X', '1', '0', 'X', 'X', '1', '0', 'X' ),  -- | H |
                    //        ( 'U', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' )   -- | - |
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_U },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_1, StdLogic1164.STD_ULOGIC_0, StdLogic1164.STD_ULOGIC_X },
                    new EnumerationLiteral[] { StdLogic1164.STD_ULOGIC_U, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X, StdLogic1164.STD_ULOGIC_X } 
        };

        private static EnumerationLiteral[] not_table = new EnumerationLiteral[]
        {
                    //--  -------------------------------------------------
                    //--  |   U    X    0    1    Z    W    L    H    -   |
                    //--  -------------------------------------------------
                    //     ( 'U', 'X', '1', '0', 'X', 'X', '1', '0', 'X' );
                    StdLogic1164.STD_ULOGIC_U,
                    StdLogic1164.STD_ULOGIC_X,
                    StdLogic1164.STD_ULOGIC_1,
                    StdLogic1164.STD_ULOGIC_0,
                    StdLogic1164.STD_ULOGIC_X,
                    StdLogic1164.STD_ULOGIC_X,
                    StdLogic1164.STD_ULOGIC_1,
                    StdLogic1164.STD_ULOGIC_0,
                    StdLogic1164.STD_ULOGIC_X
        };

        /// <summary>
        /// Возвращает индекс по литералу в определении типа данных STD_LOGIC
        /// </summary>
        /// <param name="lieral"></param>
        /// <returns></returns>
        protected static int GetIndexOfLiteral(EnumerationLiteral literal)
        {
            switch ((literal as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral())
            {
                case 'U': return 0;
                case 'X': return 1;
                case '0': return 2;
                case '1': return 3;
                case 'Z': return 4;
                case 'W': return 5;
                case 'L': return 6;
                case 'H': return 7;
                case '-': return 8;
                default: return -1;
            }
        }

        public static STD_ULOGIC_VALUE AND(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return new STD_ULOGIC_VALUE(and_table[GetIndexOfLiteral(val1.Value)][GetIndexOfLiteral(val2.Value)]);
        }

        public static STD_ULOGIC_VALUE NAND(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return NOT(AND(val1, val2));
        }

        public static STD_ULOGIC_VALUE OR(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return new STD_ULOGIC_VALUE(or_table[GetIndexOfLiteral(val1.Value)][GetIndexOfLiteral(val2.Value)]);
        }

        public static STD_ULOGIC_VALUE NOR(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return NOT(OR(val1, val2));
        }

        public static STD_ULOGIC_VALUE XOR(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return new STD_ULOGIC_VALUE(xor_table[GetIndexOfLiteral(val1.Value)][GetIndexOfLiteral(val2.Value)]);
        }

        public static STD_ULOGIC_VALUE XNOR(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return NOT(XOR(val1, val2));
        }

        public static STD_ULOGIC_VALUE NOT(STD_ULOGIC_VALUE val)
        {
            return new STD_ULOGIC_VALUE(not_table[GetIndexOfLiteral(val.Value)]);
        }

        public static BOOLEAN_VALUE EQUALS(STD_ULOGIC_VALUE val1, STD_ULOGIC_VALUE val2)
        {
            return val1.Value.Equals(val2.Value) ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }
    }
}
