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
using Antlr4.Runtime;

namespace VHDL.ParseError
{
    public class vhdlParseException : Exception
    {
        private IRecognizer recognizer;
        public IRecognizer Recognizer
        {
            get { return recognizer; }
        }

        private IToken offendingSymbol;
        public IToken OffendingSymbol
        {
            get { return offendingSymbol; }
        }
        
        private int line;
        public int Line
        {
            get { return line; }
        }

        private int charPositionInLine;
        public int CharPositionInLine
        {
            get { return charPositionInLine; }
        }

        private RecognitionException recognitionException;
        public RecognitionException RecognitionException
        {
            get { return recognitionException; }
        }

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
        }
        
        public vhdlParseException(IRecognizer recognizer, IToken offendingSymbol, string filePath, int line, int charPositionInLine, string msg, RecognitionException e)
            :base(msg, e)
        {
            this.recognizer = recognizer;
            this.offendingSymbol = offendingSymbol;
            this.filePath = filePath;
            this.line = line;
            this.charPositionInLine = charPositionInLine;
            this.recognitionException = e;
        }
    }
}
