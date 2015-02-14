using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;

namespace DataContainer.Value
{
    public abstract class AbstractValueConvertor<T>
    {
        protected ModellingType modellingType;
        public ModellingType ModellingType
        {
            get { return modellingType; }
            set { modellingType = value; }
        }

        public AbstractValueConvertor(ModellingType modellingType)
        {
            this.modellingType = modellingType;
        }

        public abstract T GetValue(AbstractValue value);

        public abstract AbstractValue GetAbstractValue(T value);

        public virtual TimeStampInfo GetTimeStampInfo(AbstractTimeStampInfo<T> value)
        {
            Dictionary<int, AbstractValue> info = new Dictionary<int, AbstractValue>();
            foreach (KeyValuePair<int, T> i in value)
            {
                info.Add(i.Key, GetAbstractValue(i.Value));
            }

            return new TimeStampInfo(info);
        }

        public virtual AbstractTimeStampInfo<T> GetAbstractTimeStampInfo(TimeStampInfo value)
        {
            Dictionary<int, T> info = new Dictionary<int, T>();
            foreach (KeyValuePair<int, AbstractValue> i in value)
            {
                info.Add(i.Key, GetValue(i.Value));
            }

            return new AbstractTimeStampInfo<T>(info);
        }
    }

    public class IntegerAbstractValueConvertor : AbstractValueConvertor<int>
    {
        public IntegerAbstractValueConvertor(ModellingType modellingType)
            : base (modellingType)
        {            
        }

        public override int GetValue(AbstractValue value)
        {
            return (value as IntegerValue).Value;
        }

        public override AbstractValue GetAbstractValue(int value)
        {
            return new IntegerValue((modellingType.Type as VHDL.type.IntegerType), value);
        }
    }

    public class PhysicalAbstractValueConvertor : AbstractValueConvertor<PhysicalLiteral>
    {
        public PhysicalAbstractValueConvertor(ModellingType modellingType)
            : base(modellingType)
        {
        }

        public override PhysicalLiteral GetValue(AbstractValue value)
        {
            return (value as PhysicalValue).Value;
        }

        public override AbstractValue GetAbstractValue(PhysicalLiteral value)
        {
            return new PhysicalValue((modellingType.Type as VHDL.type.PhysicalType), value);
        }
    }

    public class RealAbstractValueConvertor : AbstractValueConvertor<double>
    {
        public RealAbstractValueConvertor(ModellingType modellingType)
            : base(modellingType)
        {
        }

        public override double GetValue(AbstractValue value)
        {
            return (value as RealValue).Value;
        }

        public override AbstractValue GetAbstractValue(double value)
        {
            return new RealValue((modellingType.Type as VHDL.type.RealType), value);
        }
    }

    public class EnumerationAbstractValueConvertor : AbstractValueConvertor<EnumerationLiteral>
    {
        public EnumerationAbstractValueConvertor(ModellingType modellingType)
            : base(modellingType)
        {
        }

        public override EnumerationLiteral GetValue(AbstractValue value)
        {
            return (value as EnumerationValue).Value;
        }

        public override AbstractValue GetAbstractValue(EnumerationLiteral value)
        {
            return new EnumerationValue((modellingType.Type as VHDL.type.EnumerationType), value);
        }
    }

    public class STD_ULOGIC_AbstractValueConvertor : EnumerationAbstractValueConvertor
    {
        public STD_ULOGIC_AbstractValueConvertor(ModellingType modellingType)
            : base(modellingType)
        {
        }

        public override EnumerationLiteral GetValue(AbstractValue value)
        {
            return (value as EnumerationValue).Value;
        }

        public override AbstractValue GetAbstractValue(EnumerationLiteral value)
        {
            return new STD_ULOGIC_VALUE(value);
        }
    }

    public class STD_LOGIC_AbstractValueConvertor : EnumerationAbstractValueConvertor
    {
        public STD_LOGIC_AbstractValueConvertor(ModellingType modellingType)
            : base(modellingType)
        {
        }

        public override EnumerationLiteral GetValue(AbstractValue value)
        {
            return (value as EnumerationValue).Value;
        }

        public override AbstractValue GetAbstractValue(EnumerationLiteral value)
        {
            return new STD_LOGIC_VALUE(value);
        }
    }
}
