using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.expression;
using DataContainer.Value;
using VHDL.Object;
using VHDL.literal;
using VHDL.declaration;
using VHDL;
using DataContainer.Objects;

namespace DataContainer
{
    /// <summary>
    /// Класс, используемый для вычисления результата выражения
    /// </summary>
    public class ExpressionEvaluator
    {
        private  IValueProviderContainer valueProvider;
        public  IValueProviderContainer ValueProvider
        {
            get { return valueProvider; }
            set { valueProvider = value; }
        }

        public ExpressionEvaluator(IValueProviderContainer valueProvider)
        {
            this.valueProvider = valueProvider;
        }

        static ExpressionEvaluator()
        {
            defaultEvaluator = new ExpressionEvaluator(null);
        }

        private static ExpressionEvaluator defaultEvaluator;
        public static ExpressionEvaluator DefaultEvaluator
        {
            get { return defaultEvaluator; }
            set { defaultEvaluator = value; }
        }


        #region Функции по обработке выражения
        public  AbstractValue Evaluate(Abs expr)
        {
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Plus expr)
        {
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Not expr)
        {
            AbstractValue val = Evaluate(expr.Expression);

            if ((val != null) && (val is CHARACTER_VALUE))
            {
                STD_LOGIC_VALUE v = STD_LOGIC_VALUE.CreateSTD_LOGIC_VALUE(((val as CHARACTER_VALUE).Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());
                return STD_LOGIC_VALUE.NOT(v as STD_LOGIC_VALUE);
            }

            if ((val != null) && (val is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.NOT(val as STD_LOGIC_VALUE);
            }

            if ((val != null) && (val is STD_ULOGIC_VALUE))
            {
                return STD_ULOGIC_VALUE.NOT(val as STD_ULOGIC_VALUE);
            }
            return null;
        }

        public  AbstractValue Evaluate(Minus expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(TypeConversion expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(FunctionCall expr)
        {
            if (expr.Function.Identifier.Equals("NOW", StringComparison.InvariantCultureIgnoreCase))
                return valueProvider.NOW;
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(QualifiedExpression expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(QualifiedExpressionAllocator expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Parentheses expr)
        {
            return Evaluate(expr.Expression);
        }

        public  AbstractValue Evaluate(SubtypeIndicationAllocator expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Mod expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Rem expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Divide expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Multiply expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Pow expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Ror expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Sla expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Sll expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Sra expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Srl expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Rol expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Add expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Concatenate expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Subtract expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(Nand expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_LOGIC_VALUE) && (right is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.NAND(left as STD_LOGIC_VALUE, right as STD_LOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Xnor expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_LOGIC_VALUE) && (right is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.XNOR(left as STD_LOGIC_VALUE, right as STD_LOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Nor expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_LOGIC_VALUE) && (right is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.NOR(left as STD_LOGIC_VALUE, right as STD_LOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Xor expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_LOGIC_VALUE) && (right is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.XOR(left as STD_LOGIC_VALUE, right as STD_LOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(And expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_ULOGIC_VALUE) && (right is STD_ULOGIC_VALUE))
            {
                return STD_ULOGIC_VALUE.AND(left as STD_ULOGIC_VALUE, right as STD_ULOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Or expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_ULOGIC_VALUE) && (right is STD_ULOGIC_VALUE))
            {
                return STD_ULOGIC_VALUE.OR(left as STD_ULOGIC_VALUE, right as STD_ULOGIC_VALUE);
            }
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Equals expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is STD_LOGIC_VALUE) && (right is STD_LOGIC_VALUE))
            {
                return STD_LOGIC_VALUE.EQUALS(left as STD_LOGIC_VALUE, right as STD_LOGIC_VALUE);
            }
            if ((left != null) && (right != null) && (left is TIME_VALUE) && (right is TIME_VALUE))
            {
                return TIME_VALUE.EQUALS(left as TIME_VALUE, right as TIME_VALUE);
            }
            if ((left != null) && (right != null) && (left is TIME_VALUE) && (right is PhysicalValue))
            {
                return TIME_VALUE.EQUALS(left as TIME_VALUE, right as PhysicalValue);
            }
            throw new NotImplementedException(); 
        }

        public  AbstractValue Evaluate(GreaterThan expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is TIME_VALUE) && (right is TIME_VALUE))
            {
                return TIME_VALUE.GREATE_THAN(left as TIME_VALUE, right as TIME_VALUE);
            }

            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(LessEquals expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is TIME_VALUE) && (right is TIME_VALUE))
            {
                return TIME_VALUE.LESS_EQUALS_THAN(left as TIME_VALUE, right as TIME_VALUE);
            }

            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(LessThan expr)
        {
            AbstractValue left = Evaluate(expr.Left);
            AbstractValue right = Evaluate(expr.Right);

            if ((left != null) && (right != null) && (left is TIME_VALUE) && (right is TIME_VALUE))
            {
                return TIME_VALUE.LESS_THAN(left as TIME_VALUE, right as TIME_VALUE);
            }

            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(NotEquals expr)
        { throw new NotImplementedException(); }

        public  AbstractValue Evaluate(GreaterEquals expr)
        { throw new NotImplementedException(); }

        /*
        public  AbstractValue Evaluate(RecordElement expr)
        {
            //throw new NotImplementedException();
            IValueProvider value = valueProvider.GetValueProvider(expr.getPrefix() as VHDL.Object.VhdlObject);
            return value.CurrentValue;
        }
        */
        public AbstractValue Evaluate(IntegerLiteral expr)
        {
            return new IntegerValue(VHDL.builtin.Standard.INTEGER, (int)expr.IntegerValue);
        }

        public  AbstractValue Evaluate(CharacterLiteral expr)
        {
            return STD_LOGIC_VALUE.CreateSTD_LOGIC_VALUE(expr.Character);
            //return new CHARACTER_VALUE(expr.getCharacter());
        }

        public  AbstractValue Evaluate(PhysicalLiteral expr)
        {
            //if (expr.GetPhysicalType().Equals(VHDL.builtin.Standard.TIME))
                return new TIME_VALUE(expr);
            //return new PhysicalValue(expr.getType() as VHDL.type.PhysicalType, expr);
        }

        public  AbstractValue Evaluate(Aggregate expr)
        {
            throw new NotImplementedException();
        }

        public  AbstractValue Evaluate(Expression expr)
        {
            if (expr is Aggregate)
                return Evaluate(expr as Aggregate);
            if (expr is Abs)
                return Evaluate(expr as Abs);
            if (expr is Plus)
                return Evaluate(expr as Plus);
            if (expr is Not)
                return Evaluate(expr as Not);
            if (expr is Minus)
                return Evaluate(expr as Minus);
            if (expr is TypeConversion)
                return Evaluate(expr as TypeConversion);
            if (expr is FunctionCall)
                return Evaluate(expr as FunctionCall);
            if (expr is Aggregate)
                return Evaluate(expr as Aggregate);
            if (expr is QualifiedExpression)
                return Evaluate(expr as QualifiedExpression);
            if (expr is QualifiedExpressionAllocator)
                return Evaluate(expr as QualifiedExpressionAllocator);
            if (expr is Parentheses)
                return Evaluate(expr as Parentheses);
            if (expr is SubtypeIndicationAllocator)
                return Evaluate(expr as SubtypeIndicationAllocator);
            if (expr is Mod)
                return Evaluate(expr as Mod);
            if (expr is Rem)
                return Evaluate(expr as Rem);
            if (expr is Divide)
                return Evaluate(expr as Divide);
            if (expr is Multiply)
                return Evaluate(expr as Multiply);
            if (expr is Pow)
                return Evaluate(expr as Pow);
            if (expr is Ror)
                return Evaluate(expr as Ror);
            if (expr is Sla)
                return Evaluate(expr as Sla);
            if (expr is Sll)
                return Evaluate(expr as Sll);
            if (expr is Rol)
                return Evaluate(expr as Rol);
            if (expr is Add)
                return Evaluate(expr as Add);
            if (expr is Concatenate)
                return Evaluate(expr as Concatenate);
            if (expr is Subtract)
                return Evaluate(expr as Subtract);
            if (expr is Nand)
                return Evaluate(expr as Nand);
            if (expr is Xnor)
                return Evaluate(expr as Xnor);
            if (expr is Nor)
                return Evaluate(expr as Nor);
            if (expr is And)
                return Evaluate(expr as And);
            if (expr is Or)
                return Evaluate(expr as Or);
            if (expr is Equals)
                return Evaluate(expr as Equals);
            if (expr is GreaterThan)
                return Evaluate(expr as GreaterThan);
            if (expr is LessEquals)
                return Evaluate(expr as LessEquals);
            if (expr is LessThan)
                return Evaluate(expr as LessThan);
            if (expr is NotEquals)
                return Evaluate(expr as NotEquals);
            if (expr is GreaterEquals)
                return Evaluate(expr as GreaterEquals);
            /*if (expr is RecordElement)
                return Evaluate(expr as RecordElement);*/
            if (expr is IntegerLiteral)
                return Evaluate(expr as IntegerLiteral);
            if (expr is CharacterLiteral)
                return Evaluate(expr as CharacterLiteral);
            if (expr is PhysicalLiteral)
                return Evaluate(expr as PhysicalLiteral);
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Запуск на выполнение тела функции
        /// </summary>
        /// <param name="func"></param>
        private void ExecuteFunction(FunctionBody func)
        { }

        #region Evaluate Range

        public  ResolvedDiscreteRange [] ResolveRange(DiscreteRange range)
        {
            if (range is Range) return new ResolvedDiscreteRange []{ResolveSimpleRange(range as Range)};
            if (range is SubtypeDiscreteRange) return ResolveRange(range as SubtypeDiscreteRange);
            return null;
        }

        public  ResolvedDiscreteRange ResolveSimpleRange(Range range)
        {
            int from = (int)(Evaluate(range.From) as IntegerValue).Value;
            int to = (int)(Evaluate(range.To) as IntegerValue).Value;
            return ResolvedDiscreteRange.FormIntegerIndexes(from, to);
        }

        public  ResolvedDiscreteRange [] ResolveRange(SubtypeDiscreteRange range)
        {
            ModellingType modelType = TypeCreator.CreateType(range.SubtypeIndication);
            if (modelType != null)
            {
                return modelType.ResolvedRange;
            }
            return null;
        }
        #endregion
    }
}
