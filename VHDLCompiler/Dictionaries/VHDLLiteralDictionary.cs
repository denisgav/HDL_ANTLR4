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
using VHDL.literal;

namespace VHDLCompiler.CodeGenerator
{
    public class VHDLLiteralDictionary
    {
        private Dictionary<EnumBaseTypeInfo, string> internalDictionary;

        public VHDLLiteralDictionary()
        {
            internalDictionary = new Dictionary<EnumBaseTypeInfo, string>();
        }

        public void AddItem(EnumBaseTypeInfo key, string value)
        {
            if (internalDictionary.Keys.Contains(key) == false)
            {
                internalDictionary.Add(key, value);
            }
        }

        public void RemoveItem(EnumBaseTypeInfo key)
        {
            if (internalDictionary.Keys.Contains(key) == true)
            {
                internalDictionary.Remove(key);
            }
        }

        public void RemoveItem(EnumerationLiteral key)
        {
            EnumBaseTypeInfo keyToRemove = null;
            foreach (var i in internalDictionary)
            {
                if (i.Key.Key.Equals(key))
                {
                    keyToRemove = i.Key;
                    break;
                }
            }
            if (keyToRemove != null)
                internalDictionary.Remove(keyToRemove);
        }

        public string this[EnumerationLiteral key]
        {
            get
            {
                foreach (var i in internalDictionary)
                {
                    if (i.Key.Key.Equals(key))
                    {
                        return i.Value;
                    }
                }
                return null;
            }
        }

        public string this[string key]
        {
            get
            {
                foreach (var i in internalDictionary)
                {
                    if (i.Key.Value.Equals(key))
                    {
                        return i.Value;
                    }
                }
                return null;
            }
        }
    }
}
