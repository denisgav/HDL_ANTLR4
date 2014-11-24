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

using System;
namespace VHDL.type
{
    /// <summary>
    /// Subtype indication with a resolution function.
    /// </summary>
    [Serializable]
    public class ResolvedSubtypeIndication : ISubtypeIndication
    {

        //TODO: don't use string for resolution function
        private string resolutionFunction;
        private ISubtypeIndication baseType;

        /// <summary>
        /// Creates a resolved subtype indication.
        /// </summary>
        /// <param name="resolutionFunction">the resolution function</param>
        /// <param name="baseType">the base type</param>
        public ResolvedSubtypeIndication(string resolutionFunction, ISubtypeIndication baseType)
        {
            this.resolutionFunction = resolutionFunction;
            this.baseType = baseType;
        }

        /// <summary>
        /// Returns/Sets the resolution function.
        /// </summary>
        public virtual string ResolutionFunction
        {
            get { return resolutionFunction; }
            set { resolutionFunction = value; }
        }

        /// <summary>
        /// Returns/Sets the base type.
        /// </summary>
        public virtual ISubtypeIndication BaseType
        {
            get { return baseType; }
            set { baseType = value; }
        }

        public void accept(VHDL.type.ISubtypeIndicationVisitor visitor)
        {
            visitor.visit(this);
        }

    }

}