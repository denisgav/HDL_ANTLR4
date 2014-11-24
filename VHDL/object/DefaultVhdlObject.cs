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

namespace VHDL.Object
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Default VHDL object.
    /// </summary>
    [Serializable]
	public abstract class DefaultVhdlObject : VhdlObject
	{
		private string identifier;
		private SubtypeIndication type;
		private ModeEnum mode;

		public DefaultVhdlObject(string identifier, SubtypeIndication type)
		{
			this.identifier = identifier;
			this.type = type;
			this.mode = ModeEnum.NONE;
		}

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        public override string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Returns the type of this object.
        /// </summary>
		public override SubtypeIndication Type
		{
            get { return type; }
		}

        /// <summary>
        /// Returns/Sets the mode of this vhdl object.
        /// </summary>
		public override ModeEnum Mode
		{
            get { return mode; }
            set
            {
                //if (value == Mode.NONE)
                //{
                //    throw new Exception("Setting the mode to NONE is not allowed");
                //}
                this.mode = value;
            }
		}
	}
}