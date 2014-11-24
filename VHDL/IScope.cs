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
    /// Scope.
    /// </summary>
    public interface IScope
    {
        /// <summary>
        /// Resolves the given identifier in this scope and its parent scopes.
        /// </summary>
        /// <param name="identifier">identifier the identifier</param>
        /// <returns> the resolved object or <code>null</code></returns>
        object resolve(string identifier);

        /// <summary>
        /// Resolves the given identifier in this scope and its parent scopes with a specific type.
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="identifier">identifier the identifier</param>
        /// <returns>the resolved object or <code>null</code></returns>
        T resolve<T>(string identifier) where T : class;

        /// <summary>
        /// Resolves the given identifier in this scope.
        /// </summary>
        /// <param name="identifier">identifier the identifier</param>
        /// <returns>the resolved object or <code>null</code></returns>
        object resolveLocal(string identifier);

        /// <summary>
        /// Resolves the given identifier in this scope with a specific type.
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="identifier">identifier the identifier</param>
        /// <returns>the resolved object or <code>null</code></returns>
        T resolveLocal<T>(string identifier) where T : class;

        /// <summary>
        /// Получить список обьектов
        /// </summary>
        /// <returns></returns>
        List<object> GetListOfObjects();

        /// <summary>
        /// Получить список объектов определенного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetListOfObjects<T>() where T : class;

        /// <summary>
        /// Получить список обьектов без рекурсии
        /// </summary>
        /// <returns></returns>
        List<object> GetLocalListOfObjects();

        /// <summary>
        /// Получить список объектов определенного типа без рекурсии
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetLocalListOfObjects<T>() where T : class;

        List<object> resolveAll(string identifier);

        List<object> resolveAllLocal(string identifier);
    }
}