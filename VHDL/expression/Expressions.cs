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
namespace VHDL.expression
{

    using ElementAssociation = VHDL.expression.Aggregate.ElementAssociation;
    using AssociationElement = VHDL.AssociationElement;
    using SignalAttributes = VHDL.builtin.SignalAttributes;
    using StdLogic1164 = VHDL.builtin.StdLogic1164;
    using CharacterLiteral = VHDL.literal.CharacterLiteral;
    using AttributeExpression = VHDL.Object.AttributeExpression;
    using Signal = VHDL.Object.Signal;

    /// <summary>
    /// Methods for expression creation.
    /// </summary>
    public class Expressions
    {

        private Expressions()
        {
        }

        private static Expression clockEdge(Signal clock, bool rising, bool useFunction)
        {
            if (useFunction)
            {
                FunctionCall call;
                if (rising)
                {
                    call = new FunctionCall(StdLogic1164.RISING_EDGE);
                }
                else
                {
                    call = new FunctionCall(StdLogic1164.FALLING_EDGE);
                }
                call.Parameters.Add(new AssociationElement(clock));
                return call;
            }
            else
            {
                Expression condition1 = new AttributeExpression(clock, SignalAttributes.EVENT);
                Expression state = rising ? StdLogic1164.STD_LOGIC_1 : StdLogic1164.STD_LOGIC_0;
                Expression condition2 = new Equals(clock, state);

                return new And(condition1, condition2);
            }
        }

        /// <summary>
        /// Creates a rising edge clock condition.
        /// Generated VHDL: clock'event and clock = '1'
        /// </summary>
        /// <param name="clock">the clock signal</param>
        /// <returns>the risign edge clock condition</returns>
        public static Expression risingEdge(Signal clock)
        {
            return clockEdge(clock, true, false);
        }

        /// <summary>
        /// Creates a rising edge clock condition with or without using a function call.
        /// Generated VHDL: <code>clock'event and clock = '1'</code> or <code>rising_edge(clock)</code>
        /// </summary>
        /// <param name="clock">the clock signal</param>
        /// <param name="useFunction"><code>true</code>, if the <code>rising_edge</code> should be used</param>
        /// <returns>the risign edge clock condition</returns>
        public static Expression risingEdge(Signal clock, bool useFunction)
        {
            return clockEdge(clock, true, useFunction);
        }

        /// <summary>
        /// Creates a falling edge clock condition.
        /// Generated VHDL: <code>clock'event and clock = '0'</code>
        /// </summary>
        /// <param name="clock">the clock signal</param>
        /// <returns>the risign edge clock condition</returns>
        public static Expression fallingEdge(Signal clock)
        {
            return clockEdge(clock, false, false);
        }

        /// <summary>
        /// Creates a falling edge clock condition with or without using a function call.
        /// Generated VHDL: <code>clock'event and clock = '0'</code> or <code>falling_edge(clock)</code>
        /// </summary>
        /// <param name="clock">the clock signal</param>
        /// <param name="useFunction"><code>true</code>, if the <code>falling_edge</code> should be used</param>
        /// <returns>the risign edge clock condition</returns>
        public static Expression fallingEdge(Signal clock, bool useFunction)
        {
            return clockEdge(clock, false, useFunction);
        }

        /// <summary>
        /// Returns the clock signal in an edge condition.
        /// If the expression is no edge condition <code>null</code> is returned.
        /// <p>Recognized expressions:
        /// <code><ul>
        /// <li>clock'event and clock = '0'
        /// <li>clock'event and clock = '1'
        /// <li>clock = '0' and clock'event
        /// <li>clock = '1' and clock'event
        /// <li>not clock'stable and clock = '0'
        /// <li>not clock'stable and clock = '1'
        /// <li>clock = '0' and not clock'stable
        /// <li>clock = '1' and not clock'stable
        /// <li>falling_edge(clock)
        /// <li>rising_edge(clock)
        /// </ul></code>
        /// </summary>
        /// <param name="expression">the expression</param>
        /// <returns>the clock signal or <code>null</code></returns>
        public static Signal getEdgeConditionClock(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                BinaryExpression binExpr = toBinaryExpression(expression, ExpressionKind.AND);
                if (binExpr == null)
                {
                    return null;
                }

                Signal clock = clockLevelToSignal(binExpr.Left);
                if (clock != null)
                {
                    return isEventExpression(binExpr.Right, clock) ? clock : null;
                }

                clock = clockLevelToSignal(binExpr.Right);
                if (clock != null)
                {
                    return isEventExpression(binExpr.Left, clock) ? clock : null;
                }

            }
            else if (expression is FunctionCall)
            {
                FunctionCall call = (FunctionCall)expression;
                if (call.Function.Equals(StdLogic1164.FALLING_EDGE) || call.Function.Equals(StdLogic1164.RISING_EDGE))
                {
                    if (call.Parameters.Count == 1)
                    {
                        AssociationElement ae = call.Parameters[0];
                        if (ae.Actual is Signal)
                        {
                            Signal s = (Signal)ae.Actual;
                            return s;
                        }
                    }
                }
            }
            else if (expression is Parentheses)
            {
                Parentheses p = (Parentheses)expression;
                return getEdgeConditionClock(p.Expression);
            }
            else if (expression is Aggregate)
            {
                Aggregate a = (Aggregate)expression;
                if (a.Associations.Count != 1)
                {
                    return null;
                }
                ElementAssociation association = a.Associations[0];
                if (association.Choices.Count == 0)
                {
                    return getEdgeConditionClock(association.Expression);
                }
            }

            return null;
        }

        private static Signal clockLevelToSignal(Expression expression)
        {
            BinaryExpression binExpr = toBinaryExpression(expression, ExpressionKind.EQUALS);

            if (binExpr != null)
            {
                if (binExpr.Right is CharacterLiteral)
                {
                    CharacterLiteral literal = (CharacterLiteral)binExpr.Right;
                    if (literal.Character != '0' && literal.Character != '1')
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                if (binExpr.Left is Signal)
                {
                    Signal clock = (Signal)binExpr.Left;
                    return clock;
                }
            }

            return null;
        }

        private static bool isEventExpression(Expression expression, Signal clock)
        {
            if (expression is AttributeExpression)
            {
                AttributeExpression ae = (AttributeExpression)expression;
                if (!ae.Attribute.Identifier.EqualsIdentifier("event"))
                {
                    return false;
                }

                return ae.Prefix.Equals(clock);
            }
            else if (expression is Not)
            {
                Not not = (Not)expression;
                if (not.Expression is AttributeExpression)
                {
                    AttributeExpression ae = (AttributeExpression)not.Expression;
                    if (!ae.Attribute.Identifier.EqualsIdentifier("stable"))
                    {
                        return false;
                    }

                    return ae.Prefix.Equals(clock);
                }
            }

            return false;
        }

        private static BinaryExpression toBinaryExpression(Expression expr, ExpressionKind kind)
        {
            if (expr is BinaryExpression)
            {
                BinaryExpression binExpr = (BinaryExpression)expr;

                if (binExpr.ExpressionKind == kind)
                {
                    return binExpr;
                }
            }

            return null;
        }
    }
}