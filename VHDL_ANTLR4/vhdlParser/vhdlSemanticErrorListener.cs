using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using VHDL.ParseError;

namespace VHDL.parser
{
    public class vhdlSemanticErrorListener : IAntlrErrorListener<IToken>
    {
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
        }

        public vhdlSemanticErrorListener(string filePath)
        {
            this.filePath = filePath;
        }

        void IAntlrErrorListener<IToken>.SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new vhdlParseException(recognizer, offendingSymbol, filePath, line, charPositionInLine, msg, e);
        }
    }
}
