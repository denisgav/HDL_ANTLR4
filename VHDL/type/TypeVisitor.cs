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
    /// Type visitor.
    /// </summary>
    [Serializable]
    public class TypeVisitor
    {
        /// <summary>
        /// Visits a type.
        /// No visit method is called when the parameter is <code>null</code>.
        /// </summary>
        /// <param name="type">the type</param>
        public virtual void visit(Type type)
        {
            if (type != null)
            {
                type.accept(this);
            }
        }

        /// <summary>
        /// Visits a access type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitAccessType(AccessType type)
        {
        }

        /// <summary>
        /// Visits a constrained array.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitConstrainedArray(ConstrainedArray type)
        {
        }

        /// <summary>
        /// Visits an enumeration type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitEnumerationType(EnumerationType type)
        {
        }

        /// <summary>
        /// Visits a file type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitFileType(FileType type)
        {
        }

        /// <summary>
        /// Visits an incomplete type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitIncompleteType(IncompleteType type)
        {
        }

        /// <summary>
        /// Visits an integer type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitIntegerType(IntegerType type)
        {
        }

        /// <summary>
        /// Visits an real type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitRealType(RealType type)
        {
        }

        /// <summary>
        /// Visits a physical type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitPhysicalType(PhysicalType type)
        {
        }

        /// <summary>
        /// Visits a record type.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitRecordType(RecordType type)
        {
        }

        /// <summary>
        /// Visits an unconstrained array.
        /// </summary>
        /// <param name="type"></param>
        protected internal virtual void visitUnconstrainedArray(UnconstrainedArray type)
        {
        }
    }

}