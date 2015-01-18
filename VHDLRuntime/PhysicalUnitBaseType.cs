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
using System.Collections.ObjectModel;

namespace VHDLRuntime
{
    [Serializable]
    public class PhysicalBaseContainer
    {
        protected Int64 multiplier;
        protected string unitName;

        public Int64 Multiplier
        {
            get { return multiplier; }
        }

        public string UnitName
        {
            get { return unitName; }
        }

        public PhysicalBaseContainer(Int64 MultiplierIn, string UnitNameIn)
        {
            this.multiplier = MultiplierIn;
            this.unitName = UnitNameIn;
        }

        public PhysicalBaseContainer()
        {
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", multiplier, unitName);
        }
    }
    
    [Serializable]
    public class PhysicalUnitBaseType<T> : PhysicalBaseContainer where T : PhysicalUnitBaseType<T>
    {
        protected static List<T> UnitValues = new List<T>();
        protected static T BaseValue;

        public PhysicalUnitBaseType(Int64 MultiplierIn, string UnitNameIn)
            : base(MultiplierIn, UnitNameIn)
        {
            UnitValues.Add(this as T);
            RegisterBaseUnit(this as T);
        }

        public PhysicalUnitBaseType(Int64 MultiplierIn, string UnitNameIn, string BaseUnitNameIn)
            : base()
        {
            Int64 BaseUnitMultiplier = GetMultiplierByUnitName(BaseUnitNameIn);

            unitName = UnitNameIn;
            multiplier = BaseUnitMultiplier * MultiplierIn;

            RegisterUnit(this as T);
        }

        public static ReadOnlyCollection<T> GetBaseValues()
        {
            return UnitValues.AsReadOnly();
        }

        public static Int64 GetMultiplierByUnitName(string UnitNameIn)
        {
            foreach (T i in UnitValues)
            {
                if (i.UnitName.Equals(UnitNameIn))
                    return i.Multiplier;
            }
            throw new Exception(string.Format("Could not find unit {0}", UnitNameIn));
        }

        public static Int64 ConvertToInt(string UnitNameIn, Int64 Value)
        {
            return Value * GetMultiplierByUnitName(UnitNameIn);
        }

        public static void RegisterBaseUnit(T baseValue)
        {
            BaseValue = baseValue;
        }

        public static void RegisterUnit(T Value)
        {
            UnitValues.Add(Value);
        }

        public static string IntToString(Int64 Value)
        {
            T biggestMultiplier = BaseValue;
            foreach (T i in UnitValues)
            {
                if ((i.Multiplier < Value) && (i.Multiplier > biggestMultiplier.Multiplier) && (Value % i.Multiplier == 0))
                {
                    biggestMultiplier = i;
                }                    
            }

            if (biggestMultiplier != null)
            {
                return string.Format("{0} {1}", Value / biggestMultiplier.Multiplier, biggestMultiplier.UnitName);
            }
            else
            {
                return Value.ToString();
            }
        }

        public override string ToString()
        {
            return string.Format("1 {0} = {1}", unitName, IntToString(multiplier));
        }

    }
}
