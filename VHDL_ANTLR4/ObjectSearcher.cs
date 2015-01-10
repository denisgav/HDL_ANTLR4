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
using VHDL.parser.antlr;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace VHDL
{
    public static class ObjectSearcher
    {
        public static Out Search<Out>([NotNull]List<IDeclarativeRegion> scopes, List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            foreach (IDeclarativeRegion current_scope in scopes)
            {
                Out res = Search<Out>(current_scope, parts, pred);
                if (res != null)
                    return res;
            }
            return null;
        }

        public static Out Search<Out>([NotNull]IDeclarativeRegion scope, List<Part> parts) where Out : class
        {
            return Search<Out>(scope, parts, o => true);
        }

        public static Out Search<Out>([NotNull]List<IDeclarativeRegion> scopes, List<Part> parts) where Out : class
        {
            return Search<Out>(scopes, parts, o => true);
        }

        public static Out Search<Out>([NotNull]IDeclarativeRegion scope, List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            if (parts.Count == 0)
                throw new Exception("Amount of suffixes is 0");
            if (parts.Count == 1)
            {
                return SearchIdentifier<Out>(scope, (parts[0] as Part.SelectedPart).Suffix, pred);
            }
            else
            {
                return SearchSelected<Out>(scope, parts, pred);
            }
        }

        public static Out SearchSelected<Out>([NotNull]IDeclarativeRegion scope, List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            if (parts.Count == 0)
                throw new Exception("Amount of suffixes is 0");

            IDeclarativeRegion current_scope = scope;
            for (int p = 0; p < parts.Count; p++)
            {
                Part part = parts[p];
                if (part.Type == Part.TypeEnum.SELECTED)
                {
                    List<object> objects = scope.Scope.resolveAllLocal((part as Part.SelectedPart).Suffix);
                    if ((objects == null) || (objects.Count == 0))
                        return null;

                    if (p == parts.Count - 1)
                    {
                        foreach (object o in objects)
                        {
                            Out res = o as Out;
                            if (pred(res) == true)
                                return res;
                        }
                    }
                    else
                    {
                        if (objects[0] is IDeclarativeRegion)
                        {
                            current_scope = objects[0] as IDeclarativeRegion;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                    return null;
            }

            return null;
        }

        public static Out SearchIdentifier<Out>(IDeclarativeRegion scope, string identifier, Predicate<Out> pred) where Out:class
        {            
            List<object> objects = scope.Scope.resolveAll(identifier);
            if ((objects == null) || (objects.Count == 0))
                return null;

            foreach (object o in objects)
            {
                Out res = o as Out;
                if (pred(res) == true)
                    return res;
            }
            
            return null;                
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------

        public static List<Out> SearchAll<Out>([NotNull]List<IDeclarativeRegion> scopes, List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            List<Out> res = new List<Out>();
            foreach (IDeclarativeRegion current_scope in scopes)
            {
                List<Out> candidates = SearchAll<Out>(current_scope, parts, pred);
                res.AddRange(candidates);
            }
            return res;
        }

        public static List<Out> SearchAll<Out>([NotNull]IDeclarativeRegion scope, List<Part> parts) where Out : class
        {
            return SearchAll<Out>(scope, parts, o => true);
        }

        public static List<Out> SearchAll<Out>([NotNull]List<IDeclarativeRegion> scopes, List<Part> parts) where Out : class
        {
            return SearchAll<Out>(scopes, parts, o => true);
        }

        public static List<Out> SearchAll<Out>([NotNull]IDeclarativeRegion scope, List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            if (parts.Count == 0)
                throw new Exception("Amount of suffixes is 0");
            if (parts.Count == 1)
            {
                return SearchAllIdentifier<Out>(scope, (parts[0] as Part.SelectedPart).Suffix, pred);
            }
            else
            {
                return SearchAllSelected<Out>(scope, parts, pred);
            }
        }

        public static List<Out> SearchAllIdentifier<Out>(IDeclarativeRegion scope, string identifier, Predicate<Out> pred) where Out : class
        {
            List<Out> res = new List<Out>();

            List<object> objects = scope.Scope.resolveAll(identifier);
            if ((objects == null) || (objects.Count == 0))
                return null;

            foreach (object o in objects)
            {
                Out candidate = o as Out;
                if (pred(candidate) == true)
                    res.Add(candidate);
            }

            return res;
        }

        public static List<Out> SearchAllSelected<Out>([NotNull]IDeclarativeRegion scope, [NotNull]List<Part> parts, [NotNull]Predicate<Out> pred) where Out : class
        {
            if (parts.Count == 0)
                throw new Exception("Amount of suffixes is 0");

            IDeclarativeRegion current_scope = scope;
            List<Out> res = new List<Out>();

            for (int p = 0; p < parts.Count; p++)
            {
                Part part = parts[p];
                if (part.Type == Part.TypeEnum.SELECTED)
                {
                    List<object> objects = scope.Scope.resolveAllLocal((part as Part.SelectedPart).Suffix);
                    if ((objects == null) || (objects.Count == 0))
                        return res;

                    if (p == parts.Count - 1)
                    {
                        
                        foreach (object o in objects)
                        {
                            Out candidate = o as Out;
                            if (pred(candidate) == true)
                                res.Add(candidate);
                            return res;
                        }
                    }
                    else
                    {
                        if (objects[0] is IDeclarativeRegion)
                        {
                            current_scope = objects[0] as IDeclarativeRegion;
                        }
                        else
                        {
                            return res;
                        }
                    }
                }
                else
                    return res;
            }
            return res;
        }
    }
}