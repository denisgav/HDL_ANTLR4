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
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    /// <summary>
    /// Function
    /// </summary>
    public interface IFunction : ISubprogram
    {

        /// <summary>
        /// Returns/Sets if this function if impure.
        /// </summary>
        bool Impure { get; set; }

        /// <summary>
        /// Returns/Sets the return type of this function.
        /// </summary>
        SubtypeIndication ReturnType { get; set; }
    }
}