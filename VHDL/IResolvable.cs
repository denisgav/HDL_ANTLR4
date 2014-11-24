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
namespace VHDL
{
    /// <summary>
    /// Interface for objects which can be used to resolve an identifier.
    /// </summary>
    public interface IResolvable
	{
        /// <summary>
        /// Resolve an identivier.
        /// </summary>
        /// <param name="identifier">identifier the identifier</param>
        /// <returns>the matching object or <code>null</code> if no object was found</returns>
        object Resolve(string identifier);

        /// <summary>
        /// Получить список обьектов
        /// </summary>
        /// <returns></returns>
        List<object> GetListOfObjects();

        /// <summary>
        /// Получить список обьектов
        /// </summary>
        /// <returns></returns>
        List<object> GetLocalListOfObjects();
    }
}