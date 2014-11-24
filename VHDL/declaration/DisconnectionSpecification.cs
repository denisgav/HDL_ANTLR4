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

namespace VHDL.declaration
{
    using VhdlElement = VHDL.VhdlElement;
    using Expression = VHDL.expression.Expression;
    using Signal = VHDL.Object.Signal;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Disconnection specification.
    /// </summary>
    [Serializable]
    public class DisconnectionSpecification : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageDeclarativeItem
    {
        private SignalList signals;
        //TODO: use type_mark
        private SubtypeIndication type;
        private Expression after;

        /// <summary>
        /// Creates a disconnection specification.
        /// </summary>
        /// <param name="signals">a list of guarded signals</param>
        /// <param name="type">the type of the signals</param>
        /// <param name="after">the disconnection delay</param>
        public DisconnectionSpecification(SignalList signals, SubtypeIndication type, Expression after)
        {
            this.signals = signals;
            this.type = type;
            this.after = after;
        }

        /// <summary>
        /// Returns/Sets the disconnection delay.
        /// </summary>
        public virtual Expression After
        {
            get { return after; }
            set { after = value; }
        }

        /// <summary>
        /// Returns/Sets the list of signals.
        /// </summary>
        public virtual SignalList Signals
        {
            get { return signals; }
            set { signals = value; }
        }

        /// <summary>
        /// Returns/Sets the type of the signals.
        /// </summary>
        public virtual SubtypeIndication Type
        {
            get { return type; }
            set { type = value; }
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitDisconnectionSpecification(this);
        }

        /// <summary>
        /// Signal list for disconnection specification.
        /// </summary>
        public class SignalList : VhdlElement
        {
            private readonly List<Signal> signals;
            /// <summary>
            /// ALL
            /// </summary>
            public static readonly AllSignalList ALL = new AllSignalList();
            public class AllSignalList : SignalList
            {
                public AllSignalList()
                    : base(true)
                { }
            }

            /// <summary>
            /// OTHERS
            /// </summary>
            public static readonly OTHERSSignalList OTHERS = new OTHERSSignalList();
            public class OTHERSSignalList : SignalList
            {
                public OTHERSSignalList()
                    : base(true)
                { }
            }

            /// <summary>
            /// Creates a signal list.
            /// </summary>
            /// <param name="signals">a list of signals</param>
            public SignalList(List<Signal> signals)
            {
                this.signals = new List<Signal>(signals);
            }

            /// <summary>
            /// Creates a signal list.
            /// </summary>
            /// <param name="signals">a list of signals</param>
            public SignalList(params Signal[] signals)
                : this(new List<Signal>(signals))
            {
            }

            /// <summary>
            /// Creates a signal list.
            /// </summary>
            /// <param name="nullList"></param>
            private SignalList(bool nullList)
            {
                if (nullList)
                {
                    signals = null;
                }
                else
                {
                    signals = new List<Signal>();
                }
            }

            /// <summary>
            /// Returns the list of signals in this signal list.
            /// This method returns null if this type signal list containt no signals.
            /// </summary>
            public List<Signal> Signals
            {
                get { return signals; }
            }
        }
    }

}