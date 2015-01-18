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
using System.Linq;
using System.Text;

namespace VHDLCompiler.VHDLObserver
{
    public static class BuiltInTypesDictionary
    {
        private static Dictionary<VHDL.type.Type, System.Type> typeDictionary;
        private static Dictionary<VHDL.declaration.Subtype, System.Type> subTypeDictionary;
        
        public static bool ContainsBuiltInType(VHDL.type.Type type)
        {
            return typeDictionary.ContainsKey(type);
        }

        public static System.Type GetBuiltInType(VHDL.type.Type type)
        {
            return typeDictionary[type];
        }

        public static bool ContainsBuiltInSubType(VHDL.declaration.Subtype type)
        {
            return subTypeDictionary.ContainsKey(type);
        }

        public static System.Type GetBuiltInSubType(VHDL.declaration.Subtype type)
        {
            return subTypeDictionary[type];
        }

        public static void RegisterType(VHDL.type.Type vhdlType, System.Type runtimeType)
        {
            if (typeDictionary == null)
            {
                typeDictionary = new Dictionary<VHDL.type.Type, System.Type>();
            }

            if (typeDictionary.ContainsKey(vhdlType) == false)
                typeDictionary.Add(vhdlType, runtimeType);
            else
                typeDictionary[vhdlType] = runtimeType;
        }

        public static void RegisterType(VHDL.declaration.Subtype vhdlType, System.Type runtimeType)
        {
            if (subTypeDictionary == null)
            {
                subTypeDictionary = new Dictionary<VHDL.declaration.Subtype, System.Type>();
            }

            if (subTypeDictionary.ContainsKey(vhdlType) == false)
                subTypeDictionary.Add(vhdlType, runtimeType);
            else
                subTypeDictionary[vhdlType] = runtimeType;
        }

        static BuiltInTypesDictionary()
        {
            Init();
        }

        public static void Init()
        {
            //Type
            RegisterType(VHDL.builtin.Standard.BIT, typeof(VHDLRuntime.Values.BuiltIn.BIT));
            RegisterType(VHDL.builtin.Standard.BOOLEAN, typeof(VHDLRuntime.Values.BuiltIn.BOOLEAN));
            RegisterType(VHDL.builtin.Standard.CHARACTER, typeof(VHDLRuntime.Values.BuiltIn.CHARACTER));
            RegisterType(VHDL.builtin.Standard.FILE_OPEN_KIND, typeof(VHDLRuntime.Values.BuiltIn.FILE_OPEN_KIND));
            RegisterType(VHDL.builtin.Standard.FILE_OPEN_STATUS, typeof(VHDLRuntime.Values.BuiltIn.FILE_OPEN_STATUS));
            RegisterType(VHDL.builtin.Standard.INTEGER, typeof(VHDLRuntime.Values.BuiltIn.INTEGER));
            RegisterType(VHDL.builtin.Standard.REAL, typeof(VHDLRuntime.Values.BuiltIn.REAL));
            RegisterType(VHDL.builtin.Standard.SEVERITY_LEVEL, typeof(VHDLRuntime.Values.BuiltIn.SEVERITY_LEVEL));
            RegisterType(VHDL.builtin.Standard.TIME, typeof(VHDLRuntime.Values.BuiltIn.TIME));

            //Subtype
            RegisterType(VHDL.builtin.Standard.NATURAL, typeof(VHDLRuntime.Values.BuiltIn.NATURAL));
            RegisterType(VHDL.builtin.Standard.POSITIVE, typeof(VHDLRuntime.Values.BuiltIn.POSITIVE));
        }

    }
}
