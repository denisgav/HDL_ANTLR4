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

namespace VHDLCompiler.CodeTemplates.Declarations
{
    public partial class VariableDeclarationTemplate
    {
        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private object[] constructorParameters;
        public object[] ConstructorParameters
        {
            get { return constructorParameters; }
            set { constructorParameters = value; }
        }

        public VariableDeclarationTemplate(string typeName, string name, params object[] constructorParameters)
        {
            this.typeName = typeName;
            this.name = name;
            this.constructorParameters = constructorParameters;
        }

        public VariableDeclarationTemplate(string typeName, string name, object constructorParameter)
            : this(typeName, name, new object[] { constructorParameter })
        { }

        public VariableDeclarationTemplate(string typeName, string name)
            :this(typeName, name, new object[]{})
        { }
        
    }
}


/*
VHDLRuntime.Objects.Variable<<#= TypeName #>> <#= Name #> 
	= new VHDLRuntime.Objects.Variable<<#= TypeName #>>(
		<# for(int i=0; i<ConstructorParameters.Length; i++)
		{#>
			<#= ConstructorParameters[i] #>
			<#if(i != (ConstructorParameters.Length - 1) )
			{#>
			,
			<#}#>
		<# } // end of foreach #>
	);

*/