using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using VHDL.declaration;
using VHDL;
using DataContainer.TypeConstraint;
using DataContainer.Objects;
using DataContainer.SignalDump;
using DataContainer.Value;
using VHDL.literal;
using VHDL.builtin;

namespace DataContainer
{
    /// <summary>
    /// Утилита для создания обьектов для выполнения операций
    /// </summary>
    public abstract class TypeCreator
    {
        public static ModellingType CreateType(ISubtypeIndication si)
        {
            if (si is ResolvedSubtypeIndication)
            {
                ModellingType res = CreateType((si as ResolvedSubtypeIndication).BaseType);
                res.Constraints.Add(new ResolvedTypeConstraint(si as ResolvedSubtypeIndication));
                return res;
            }

            if (si is RangeSubtypeIndication)
            {
                ModellingType res = CreateType((si as RangeSubtypeIndication).BaseType);
                res.Constraints.Add(new RangeTypeConstraint(si as RangeSubtypeIndication));
                return res;
            }

            if (si is IndexSubtypeIndication)
            {
                ModellingType res = CreateType((si as IndexSubtypeIndication).BaseType);
                res.Constraints.Add(new IndexTypeConstraint(si as IndexSubtypeIndication));
                return res;
            }

            if (si is Subtype)
            {
                return CreateType((si as Subtype).SubtypeIndication);
            }
            if (si is IntegerType)
            {
                return ModellingType.CreateModellingType(si as IntegerType);
            }
            if (si is RealType)
            {
                return ModellingType.CreateModellingType(si as RealType);
            }
            if (si is PhysicalType)
            {
                return ModellingType.CreateModellingType(si as PhysicalType);
            }
            if (si is RecordType)
            {
                return ModellingType.CreateModellingType(si as RecordType);
            }
            if (si is EnumerationType)
            {
                return ModellingType.CreateModellingType(si as EnumerationType);
            }

            if (si is UnconstrainedArray)
            {
                return ModellingType.CreateModellingType(si as UnconstrainedArray);
            }
            if (si is ConstrainedArray)
            {
                return ModellingType.CreateModellingType(si as ConstrainedArray);
            }

            return null;
        }

        public static Signal CreateSignal(string name, ISubtypeIndication si)
        {
            if (si is ResolvedSubtypeIndication)
            {
                Signal res = CreateSignal(name, (si as ResolvedSubtypeIndication).BaseType);
                return res;
            }

            if (si is RangeSubtypeIndication)
            {
                Signal res = CreateSignal(name, (si as RangeSubtypeIndication).BaseType);
                return res;
            }

            if (si is IndexSubtypeIndication)
            {
                Signal res = CreateUnconstrainedArraySignal(name, si as IndexSubtypeIndication);
                return res;
            }

            if (si is Subtype)
            {
                return CreateSignal(name, (si as Subtype).SubtypeIndication);
            }
            if (si is IntegerType)
            {
                ModellingType resType = ModellingType.CreateModellingType(si as IntegerType);
                IntegerAbstractValueConvertor conv = new IntegerAbstractValueConvertor(resType);
                AbstractSimpleSignalDump<int> resDump = new AbstractSimpleSignalDump<int>(name, resType, conv);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }
            if (si is RealType)
            {
                ModellingType resType = ModellingType.CreateModellingType(si as RealType);
                RealAbstractValueConvertor conv = new RealAbstractValueConvertor(resType);
                AbstractSimpleSignalDump<double> resDump = new AbstractSimpleSignalDump<double>(name, resType, conv);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }
            if (si is PhysicalType)
            {
                ModellingType resType = ModellingType.CreateModellingType(si as PhysicalType);
                PhysicalAbstractValueConvertor conv = new PhysicalAbstractValueConvertor(resType);
                AbstractSimpleSignalDump<PhysicalLiteral> resDump = new AbstractSimpleSignalDump<PhysicalLiteral>(name, resType, conv);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }
            if (si is RecordType)
            {
                RecordType recType = si as RecordType;
                ModellingType resType = ModellingType.CreateModellingType(recType);

                List<AbstractSignalDump> dumps = new List<AbstractSignalDump>();
                List<Signal> childrens = new List<Signal>();

                foreach (var el in recType.Elements)
                {
                    foreach (string s in el.Identifiers)
                    {
                        Signal newSignal = CreateSignal(s, el.Type);
                        childrens.Add(newSignal);
                        dumps.Add(newSignal.Dump);
                    }
                }

                SignalScopeDump resDump = new SignalScopeDump(name, resType, dumps);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }
            if (si is EnumerationType)
            {
                ModellingType resType = ModellingType.CreateModellingType(si as EnumerationType);
                EnumerationAbstractValueConvertor conv;

                if (si == StdLogic1164.STD_ULOGIC)
                {
                    conv = new STD_ULOGIC_AbstractValueConvertor(resType);
                }
                else
                {
                    if (si == StdLogic1164.STD_LOGIC)
                    {
                        conv = new STD_LOGIC_AbstractValueConvertor(resType);
                    }
                    else
                    {
                        conv = new EnumerationAbstractValueConvertor(resType);
                    }
                }
                
                AbstractSimpleSignalDump<EnumerationLiteral> resDump = new AbstractSimpleSignalDump<EnumerationLiteral>(name, resType, conv);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }

            if (si is ConstrainedArray)
            {
                ConstrainedArray arrayType = si as ConstrainedArray;
                ModellingType resType = ModellingType.CreateModellingType(arrayType);

                List<AbstractSignalDump> dumps = new List<AbstractSignalDump>();
                List<Signal> childrens = new List<Signal>();

                ResolvedDiscreteRange[] ranges = resType.Dimension;
                int[,] resIndexes = ResolvedDiscreteRange.CombineRanges(ranges);
                for (int i = 0; i < resIndexes.GetLength(0); i++)
                {
                    for (int j = 0; j < resIndexes.GetLength(1); j++)
                    {
                        Signal newSignal = CreateSignal(ranges[j][resIndexes[i, j]].ToString(), arrayType.ElementType);
                        childrens.Add(newSignal);
                        dumps.Add(newSignal.Dump);
                    }
                }

                SignalScopeDump resDump = new SignalScopeDump(name, resType, dumps);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }

            return null;
        }

        private static Signal CreateUnconstrainedArraySignal(string name, IndexSubtypeIndication si)
        {
            if (si.BaseType is UnconstrainedArray)
            {
                UnconstrainedArray arrayType = si.BaseType as UnconstrainedArray;
                
                List<ResolvedDiscreteRange> resolvedRanges = new List<ResolvedDiscreteRange>();
                foreach (DiscreteRange r in si.Ranges)
                {
                    if(r is Range)
                    {
                        int from = (ExpressionEvaluator.DefaultEvaluator.Evaluate((r as Range).From) as IntegerValue).Value;
                        int to = (ExpressionEvaluator.DefaultEvaluator.Evaluate((r as Range).To) as IntegerValue).Value;
                        ResolvedDiscreteRange newRange = ResolvedDiscreteRange.FormIntegerIndexes(from, to);
                        resolvedRanges.Add(newRange);
                    }
                }

                ModellingType resType = ModellingType.CreateModellingType(arrayType, resolvedRanges.ToArray());
                resType.Constraints.Add(new IndexTypeConstraint(si));
                

                List<AbstractSignalDump> dumps = new List<AbstractSignalDump>();
                List<Signal> childrens = new List<Signal>();

                int[,] resIndexes = ResolvedDiscreteRange.CombineRanges(resolvedRanges.ToArray());
                for (int i = 0; i < resIndexes.GetLength(0); i++)
                {
                    for (int j = 0; j < resIndexes.GetLength(1); j++)
                    {
                        Signal newSignal = CreateSignal(resolvedRanges[j][resIndexes[i, j]].ToString(), arrayType.IndexSubtypes[j]);
                        childrens.Add(newSignal);
                        dumps.Add(newSignal.Dump);
                    }
                }

                SignalScopeDump resDump = new SignalScopeDump(name, resType, dumps);
                Signal resSignal = new Signal(resDump);
                return resSignal;
            }
            return null;
        }
    }
}
