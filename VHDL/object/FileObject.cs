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
    /// VHDL File Object.
    /// </summary>
    [Serializable]
    public class FileObject : DefaultVhdlObject
    {
        private Expression openKind;
        private Expression logicalName;

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="type">the type</param>
        /// <param name="openKind">the open kind</param>
        /// <param name="logicalName">the logical name</param>
        public FileObject(string identifier, SubtypeIndication type, Expression openKind, Expression logicalName)
            : base(identifier, type)
        {
            this.openKind = openKind;
            this.logicalName = logicalName;
        }

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="type">the type</param>
        /// <param name="logicalName">the logical name</param>
        public FileObject(string identifier, SubtypeIndication type, Expression logicalName)
            : base(identifier, type)
        {
            this.logicalName = logicalName;
        }

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="type">the type</param>
        public FileObject(string identifier, SubtypeIndication type)
            : base(identifier, type)
        {
        }

        /// <summary>
        /// Returns/Sets the logical name.
        /// </summary>
        public virtual Expression LogicalName
        {
            get { return logicalName; }
            set { logicalName = value; }
        }

        /// <summary>
        /// Returns/Sets the file open kind.
        /// </summary>
        public virtual Expression OpenKind
        {
            get { return openKind; }
            set { openKind = value; }
        }

        public override IList<VhdlObject> VhdlObjects
        {
            get { return new List<VhdlObject>(new VhdlObject[] { this }); }
        }

        public override ObjectClassEnum ObjectClass
        {
            get { return ObjectClassEnum.FILE; }
        }


        public override ModeEnum Mode
        {
            set
            {
                throw new Exception("Setting the mode is not supported for files");
            }
        }
    }

}