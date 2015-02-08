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
using System.Collections.Generic;

namespace VHDL.Object
{
    using Expression = VHDL.expression.Expression;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Signal.
    /// </summary>
    [Serializable]
    public class Signal : DefaultVhdlObject
    {
        private KindEnum kind;
        private Expression defaultValue;

        /// <summary>
        /// Creates a signal.
        /// </summary>
        /// <param name="identifier">the identifier of the signal</param>
        /// <param name="type">the type of the signal</param>
        public Signal(string identifier, SubtypeIndication type)
            : base(identifier, type)
        {
            Mode = ModeEnum.IN;
            kind = KindEnum.DEFAULT;
        }

        /// <summary>
        /// Creates a signal with a mode.
        /// </summary>
        /// <param name="identifier">the identifier of the signal</param>
        /// <param name="mode">the mode of the signal</param>
        /// <param name="type">the type of the signal</param>
        public Signal(string identifier, ModeEnum mode, SubtypeIndication type)
            : base(identifier, type)
        {
            Mode = mode;
            kind = KindEnum.DEFAULT;
        }

        /// <summary>
        /// Creates a signal with a default value.
        /// </summary>
        /// <param name="identifier">the identifier of the signal</param>
        /// <param name="type">the type of the signal</param>
        /// <param name="defaultValue">the default value of the signal</param>
        public Signal(string identifier, SubtypeIndication type, Expression defaultValue)
            : base(identifier, type)
        {
            Mode = ModeEnum.IN;
            this.defaultValue = defaultValue;
            kind = KindEnum.DEFAULT;
        }

        /// <summary>
        /// Creates a signal with a mode and a default value.
        /// </summary>
        /// <param name="identifier">the identifier of the signal</param>
        /// <param name="mode">the mode of the signal</param>
        /// <param name="type">the type of the signal</param>
        /// <param name="defaultValue">the default value of the signal</param>
        public Signal(string identifier, ModeEnum mode, SubtypeIndication type, Expression defaultValue)
            : base(identifier, type)
        {
            this.defaultValue = defaultValue;
            Mode = mode;
            kind = KindEnum.DEFAULT;
        }

        /// <summary>
        /// Returns/Sets the kind of this signal.
        /// </summary>
        public virtual KindEnum Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        /// <summary>
        /// Returns/Sets the default value.
        /// </summary>
        public virtual Expression DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public override IList<VhdlObject> VhdlObjects
        {
            get { return new List<VhdlObject>(new VhdlObject[] { this }); }
        }

        public override ObjectClassEnum ObjectClass
        {
            get { return ObjectClassEnum.SIGNAL; }
        }

        /// <summary>
        /// Signal kind.
        /// </summary>
        public enum KindEnum
        {

            /// Default signal kind. 
            DEFAULT,
            /// Register signal kind. 
            REGISTER,
            /// Bus signal kind. 
            BUS
        }
    }
}