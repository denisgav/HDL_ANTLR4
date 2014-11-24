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

namespace VHDL.parser.annotation
{
    using ParseError = VHDL.parser.ParseError;
    /// <summary>
    /// Parse errors annotation.
    /// The parse errors annotation is used to store parse errors in a <code>VhdlFile</code> instance.
    /// </summary>
    [Serializable]
    public class ParseErrors
    {

        private readonly List<ParseError> errors;

        public ParseErrors(List<ParseError> errors)
        {
            this.errors = new List<ParseError>(errors);
        }

        public virtual List<ParseError> Errors
        {
            get { return errors; }
        }
    }

}