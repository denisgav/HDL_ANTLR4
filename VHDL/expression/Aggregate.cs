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

using System.Collections.Generic;
using System;

namespace VHDL.expression
{
    using Choice = VHDL.Choice;
    using Choices = VHDL.Choices;
    using VhdlElement = VHDL.VhdlElement;
    using SignalAssignmentTarget = VHDL.Object.ISignalAssignmentTarget;
    using VariableAssignmentTarget = VHDL.Object.IVariableAssignmentTarget;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    //TODO: check if aggregate is a valid signal assignment or variable assignment target
    /// <summary>
    /// Aggregate.
    /// </summary>
    [Serializable]
    public class Aggregate : Primary, SignalAssignmentTarget, VariableAssignmentTarget
    {
        private readonly List<ElementAssociation> associations = new List<ElementAssociation>();

        /// <summary>
        /// Creates an empty aggregate.
        /// </summary>
        public Aggregate()
        {
        }

        /// <summary>
        /// Creates an aggregate that contains the given expressions.
        /// </summary>
        /// <param name="expressions">the epxressions</param>
        public Aggregate(params Expression[] expressions)
            : this(new List<Expression>(expressions))
        {
        }

        /// <summary>
        /// Creates an aggregate that contains the given expressions.
        /// </summary>
        /// <param name="expressions">the epxressions</param>
        public Aggregate(List<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                CreateAssociation(expression);
            }
        }

        /// <summary>
        /// Returns the associations.
        /// </summary>
        public virtual List<ElementAssociation> Associations
        {
            get { return associations; }
        }

        /// <summary>
        /// Creates a new positional element association and adds it to this aggregate.
        /// </summary>
        /// <param name="expression">the expression</param>
        /// <returns>the created element association</returns>
        public virtual ElementAssociation CreateAssociation(Expression expression)
        {
            ElementAssociation association = new ElementAssociation(expression);
            associations.Add(association);
            return association;
        }

        /// <summary>
        /// Creates a new named element association and adds it to this aggregate.
        /// </summary>
        /// <param name="expression">the expression</param>
        /// <param name="choices">the choices</param>
        /// <returns>the created element association</returns>
        public virtual ElementAssociation CreateAssociation(Expression expression, List<Choice> choices)
        {
            ElementAssociation association = new ElementAssociation(expression, choices);
            associations.Add(association);
            return association;
        }

        /// <summary>
        /// Creates a new named element association and adds it to this aggregate.
        /// </summary>
        /// <param name="expression">the expression</param>
        /// <param name="choices">the choices</param>
        /// <returns>the created element association</returns>
        public virtual ElementAssociation CreateAssociation(Expression expression, params Choice[] choices)
        {
            return CreateAssociation(expression, new List<Choice>(choices));
        }

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitAggregate(this);
        }

        public override Choice copy()
        {
            Aggregate a = new Aggregate();
            foreach (ElementAssociation association in associations)
            {
                ElementAssociation associationCopy = new ElementAssociation(association.Expression.copy() as Expression, association.Choices);
                a.associations.Add(associationCopy);
            }

            return a;
        }

        /// <summary>
        /// Creates a new aggregate of the type "(others => expression)".
        /// </summary>
        /// <param name="expression">the expression</param>
        /// <returns>the created aggregate</returns>
        public static Aggregate OTHERS(Expression expression)
        {
            Aggregate aggregate = new Aggregate();
            aggregate.CreateAssociation(expression, Choices.OTHERS);
            return aggregate;
        }

        /// <summary>
        /// An ElementAssociation associates choices with and expression.
        /// </summary>
        [Serializable]
        public class ElementAssociation : VhdlElement
        {
            private readonly List<Choice> choices = new List<Choice>();
            private Expression expression;

            /// <summary>
            /// Creates a positional element association.
            /// </summary>
            /// <param name="expression">the expression</param>
            public ElementAssociation(Expression expression)
            {
                this.expression = expression;
            }

            /// <summary>
            /// Creates a named element association.
            /// </summary>
            /// <param name="expression">the associated epxression</param>
            /// <param name="choices">the choices</param>
            public ElementAssociation(Expression expression, List<Choice> choices)
            {
                this.expression = expression;
                this.choices.AddRange(choices);
            }

            /// <summary>
            /// Returns the list of choices.
            /// A positional element association returns an empty list.
            /// </summary>
            public virtual List<Choice> Choices
            {
                get { return choices; }
            }

            /// <summary>
            /// Returns/Sets the associated expression.
            /// </summary>
            public virtual Expression Expression
            {
                get { return expression; }
                set { expression = value; }
            }
        }
    }
}