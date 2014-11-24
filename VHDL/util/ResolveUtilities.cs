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


using DeclarativeRegion = VHDL.IDeclarativeRegion;
using Scope = VHDL.IScope;

namespace VHDL.util
{
    /// <summary>
    /// Resolve utilities.
    /// </summary>
    public class ResolveUtilities
    {
        private ResolveUtilities()
        {
        }

        /// <summary>
        /// Resolves an path in a scope.
        /// </summary>
        /// <param name="root">the scope</param>
        /// <param name="path">the path</param>
        /// <returns>the object or null</returns>
        public static object ResolvePath(IDeclarativeRegion root, string path)
        {
            string[] parts = path.Split(':');
            IScope scope = GetPathScope(root, parts);
            return (scope == null ? null : scope.resolve(parts[parts.Length - 1]));
        }

        /// <summary>
        /// Resolves a path in a scope.
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="root">the scope</param>
        /// <param name="path">the path</param>
        /// <returns>the object or null<</returns>
        public static T ResolvePath<T>(IDeclarativeRegion root, string path) where T : class
        {
            string[] parts = path.Split(':');
            IScope scope = GetPathScope(root, parts);
            return (scope == null ? null : scope.resolve<T>(parts[parts.Length - 1]));
        }

        private static IScope GetPathScope(IDeclarativeRegion root, string[] parts)
        {
            IScope scope = root.Scope;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                IDeclarativeRegion r = scope.resolve<IDeclarativeRegion>(parts[i]);
                if (r == null)
                {
                    return null;
                }
                scope = r.Scope;
                if (scope == null)
                {
                    return null;
                }
            }

            return scope;
        }
    }

}