using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using DataContainer.TypeConstraint;
using VHDL.declaration;

namespace DataContainer.Value
{
    /// <summary>
    /// Утилита для создания обьектов для выполнения операций
    /// </summary>
    public abstract class ValueCreator
    {
        /// <summary>
        /// Создание контейнера по заданному типу
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public static AbstractValue CreateValue(ISubtypeIndication si)
        {
            if (si == VHDL.builtin.Standard.TIME)
            {
                return new TIME_VALUE(0);
            }
            if (si is ResolvedSubtypeIndication)
            {
                AbstractValue res = CreateValue((si as ResolvedSubtypeIndication).BaseType);
                res.Type.Constraints.Add(new ResolvedTypeConstraint(si as ResolvedSubtypeIndication));
                return res;
            }

            if (si is RangeSubtypeIndication)
            {
                AbstractValue res = CreateValue((si as RangeSubtypeIndication).BaseType);
                res.Type.Constraints.Add(new RangeTypeConstraint(si as RangeSubtypeIndication));
                return res;
            }

            if (si is IndexSubtypeIndication)
            {
                ArrayValue res = CreateValue((si as IndexSubtypeIndication).BaseType) as ArrayValue;
                res.Type.Constraints.Add(new IndexTypeConstraint(si as IndexSubtypeIndication));
                return res;
            }

            if (si is Subtype)
            {
                return CreateValue((si as Subtype).SubtypeIndication);
            }
            if (si is IntegerType)
            {
                return new IntegerValue(si as IntegerType);
            }
            if (si is RealType)
            {
                return new RealValue(si as RealType);
            }
            if (si is EnumerationType)
            {
                EnumerationType en = si as EnumerationType;
                switch (en.Identifier.ToLower())
                {
                    case "std_logic": return new STD_LOGIC_VALUE();
                    case "std_ulogic": return new STD_LOGIC_VALUE();
                    case "bit": return new BIT_VALUE();
                    case "character": return new CHARACTER_VALUE();
                    default: return new EnumerationValue(si as EnumerationType);
                }
            }
            if (si is PhysicalType)
            {
                return new PhysicalValue(si as PhysicalType);
            }
            if (si is UnconstrainedArray)
            {
                throw new Exception("Undefined bounds of array");
            }
            if (si is ConstrainedArray)
            {
                //ArrayValue val = new ArrayValue(si as ConstrainedArray, );
                //return val;
            }
            if (si is RecordType)
            {
                return new RecordValue(si as RecordType);
            }
            return null;
        }

        /// <summary>
        /// Создание контейнера по заданному имени типа данных
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static AbstractValue CreatePredefinedValue(string typeName, string value)
        {
            return null;
        }
    }
}
