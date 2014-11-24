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

namespace VHDL.declaration
{
    /// <summary>
    /// Entity class enumeration.
    /// The entity class enumeration is used to describe the type of objects in attribute
    /// specifications and group template declartions.
    /// </summary>
    public enum EntityClass
    {

        /// Entity. 
        ENTITY,
        /// Architecture. 
        ARCHITECTURE,
        /// Configuration. 
        CONFIGURATION,
        /// Procedure. 
        PROCEDURE,
        /// Function. 
        FUNCTION,
        /// Package. 
        PACKAGE,
        /// Type. 
        TYPE,
        /// Subtype. 
        SUBTYPE,
        /// Constant. 
        CONSTANT,
        /// Signal. 
        SIGNAL,
        /// Variable. 
        VARIABLE,
        /// Component 
        COMPONENT,
        /// Label. 
        LABEL,
        /// Literal. 
        LITERAL,
        /// Units. 
        UNITS,
        /// Group. 
        GROUP,
        /// File. 
        FILE
    }
}