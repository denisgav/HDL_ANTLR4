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

namespace VHDL.parser
{
    using PositionInformation = VHDL.annotation.PositionInformation;
    using System.Collections.Generic;

    /// <summary>
    /// Parse error.
    /// </summary>
    [Serializable]
    public class ParseError
    {

        private readonly PositionInformation position;
        private readonly ParseErrorTypeEnum type;
        private readonly string message;

        /// <summary>
        /// Creates a new parse error.
        /// </summary>
        /// <param name="position">the position of the error</param>
        /// <param name="type">the error type</param>
        /// <param name="message">the message or identifier</param>
        public ParseError(PositionInformation position, ParseErrorTypeEnum type, string message)
        {
            this.position = position;
            this.type = type;
            this.message = message;
        }

        public virtual PositionInformation Position
        {
            get { return position; }
        }

        public virtual ParseErrorTypeEnum Type
        {
            get { return type; }
        }

        public virtual string Message
        {
            get { return message; }
        }

        public enum ParseErrorTypeEnum
        {
            UNKNOWN_CONFIGURATION,
            UNKNOWN_GENERATE_STATEMENT,
            UNKNOWN_CONSTANT,
            UNKNOWN_COMPONENT,
            UNKNOWN_ENTITY,
            UNKNOWN_ARCHITECTURE,
            UNKNOWN_FILE,
            UNKNOWN_SIGNAL,
            UNKNOWN_SIGNAL_ASSIGNMENT_TARGET,
            UNKNOWN_LOOP,
            UNKNOWN_PACKAGE,
            UNKNOWN_TYPE,
            UNKNOWN_VARIABLE,
            UNKNOWN_VARIABLE_ASSIGNMENT_TARGET,
            UNKNOWN_OTHER,
            PROCESS_TYPE_ERROR,
            UNKNOWN_CASE,
            UNKNOWN_IF,
            UNKNOWN_PROCESS
        }
    }

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