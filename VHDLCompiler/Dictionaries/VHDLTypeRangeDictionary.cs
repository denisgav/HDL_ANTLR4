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
using VHDL.type;
using VHDL.declaration;

namespace VHDLCompiler.CodeGenerator
{
    public class TypeRangeInfo
    {
        private string rangeType;
        public string RangeType
        {
            get { return rangeType; }
            set { rangeType = value; }
        }
        
        private string rangeLeft;
	    public string RangeLeft
	    {
		    get { return rangeLeft;}
		    set { rangeLeft = value;}
	    }

        private string rangeRight;
	    public string RangeRight
	    {
		    get { return rangeRight;}
		    set { rangeRight = value;}
	    }

        private string rangeDirection;
	    public string RangeDirection
	    {
		    get { return rangeDirection;}
		    set { rangeDirection = value;}
	    }

        public TypeRangeInfo(string rangeType, string rangeLeft, string rangeRight, string rangeDirection)
        {
            this.rangeType = rangeType;
            this.rangeLeft = rangeLeft;
            this.rangeRight = rangeRight;
            this.rangeDirection = rangeDirection;
        }
    }

    public class VHDLTypeRangeDictionary
    {
        private Dictionary<ISubtypeIndication, TypeRangeInfo> intenalDictionary;

        public VHDLTypeRangeDictionary()
        {
            intenalDictionary = new Dictionary<ISubtypeIndication, TypeRangeInfo>(); 
        }

        public void AddItem(VHDL.type.Type key, TypeRangeInfo value)
        {
            if (intenalDictionary.Keys.Contains(key) == false)
            {
                intenalDictionary.Add(key, value);
            }
        }

        public void AddItem(Subtype key, TypeRangeInfo value)
        {
            if (intenalDictionary.Keys.Contains(key) == false)
            {
                intenalDictionary.Add(key, value);
            }
        }

        public void AddItem(VHDL.type.Type key, string rangeType, string rangeLeft, string rangeRight, string rangeDirection)
        {
            AddItem(key, new TypeRangeInfo(rangeType, rangeLeft, rangeRight, rangeDirection));
        }

        public void AddItem(Subtype key, string rangeType, string rangeLeft, string rangeRight, string rangeDirection)
        {
            AddItem(key, new TypeRangeInfo(rangeType, rangeLeft, rangeRight, rangeDirection));
        }

        public TypeRangeInfo this[VHDL.type.Type key]
        {
            get
            {
                if (intenalDictionary.Keys.Contains(key) == false)
                    return null;
                return intenalDictionary[key];
            }
        }

        public TypeRangeInfo this[ISubtypeIndication key]
        {
            get
            {
                if (intenalDictionary.Keys.Contains(key) == false)
                    return null;
                return intenalDictionary[key];
            }
        }
    }
}
