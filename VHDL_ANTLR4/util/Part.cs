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
using VHDL.expression;

namespace VHDL.parser.antlr
{
    public abstract class Part : VhdlElement
    {
        //----------------------------------------------------------
        public class FunctionCallPart : Part
        {
            private List<string> selected_path;
            private List<AssociationElement> arguments;

            public FunctionCallPart(List<string> selected_path, List<AssociationElement> arguments)
                : base(TypeEnum.FUNCION_CALL)
            {
                this.selected_path = selected_path;
                this.arguments = arguments;
            }

            public virtual List<AssociationElement> Arguments
            {
                get { return arguments; }
            }

            public virtual List<string> SelectedPath
            {
                get { return selected_path; }
            }
        }
        //----------------------------------------------------------

        //----------------------------------------------------------
        public class AttributePart : Part
        {
            private string identifier;
            private List<AssociationElement> arguments;

            public AttributePart(string identifier, List<AssociationElement> arguments)
                : base(TypeEnum.ATTRIBUTE)
            {
                this.identifier = identifier;
                this.arguments = arguments;
            }

            public virtual List<AssociationElement> Arguments
            {
                get { return arguments; }
            }

            public virtual string Identifier
            {
                get { return identifier; }
            }
        }
        //----------------------------------------------------------

        //----------------------------------------------------------
        public class IndiciesPart : Part
        {
            private List<Expression> indices;

            public IndiciesPart(List<Expression> indices)
                : base(TypeEnum.INDEXED)
            {
                this.indices = indices;
            }

            public virtual List<Expression> Indices
            {
                get { return indices; }
            }
        }
        //----------------------------------------------------------

        //----------------------------------------------------------
        public class SelectedPart : Part
        {
            private string suffix;

            public SelectedPart(string suffix)
                : base(TypeEnum.SELECTED)
            {
                this.suffix = suffix;
            }

            public virtual string Suffix
            {
                get { return suffix; }
            }
        }
        //----------------------------------------------------------

        //----------------------------------------------------------
        public class SlicePart : Part
        {
            private DiscreteRange range;

            public SlicePart(DiscreteRange range)
                : base(TypeEnum.SLICE)
            {
                this.range = range;
            }

            public virtual DiscreteRange Range
            {
                get { return range; }
            }
        }
        //----------------------------------------------------------

        private readonly TypeEnum type;
        
        

        protected Part(TypeEnum type)
        {
            this.type = type;
        }

        public static Part CreateAttribute(string identifier, List<AssociationElement> arguments)
        {
            return new AttributePart(identifier, arguments);
        }

        public static Part CreateIndexed(List<Expression> indices)
        {
            return new IndiciesPart(indices);
        }

        public static Part CreateSelected(string suffix)
        {
            return new SelectedPart(suffix);
        }

        public static Part CreateSlice(DiscreteRange range)
        {
            return new SlicePart(range);
        }

        public static Part CreateFunctionCall(List<string> selected_path, List<AssociationElement> arguments)
        {
            return new FunctionCallPart(selected_path, arguments);
        }

        public virtual TypeEnum Type
        {
            get { return type; }
        }

        public enum TypeEnum
        {
            FUNCION_CALL,
            ATTRIBUTE,
            INDEXED,
            SELECTED,
            SLICE
        }
    }
}
