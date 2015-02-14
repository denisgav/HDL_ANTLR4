using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.Compiler
{
    public class GHDLErrorStringParser
    {
        /** Type of system execution command */

        public GHDLErrorStringParser(string fileName)
        {
            this.fileName = fileName;
            diagnosticMsgs = new List<DiagnosticMessage>();
        }

        private string fileName;

        private List<DiagnosticMessage>  diagnosticMsgs;
        public List<DiagnosticMessage>  DiagnosticMsgs
        {
            get { return diagnosticMsgs; }
            set { diagnosticMsgs = value; }
        }
        
        /** Parse error string and collect diagnostic messages
         *
         *  \param _strErrorOutput the error output
         *  \param alDiagnosticMsgs the collection with diagnostic msgs structs
         *  \return none
         */
        public void ParseErrorString(string _strErrorOutput)
        {
            int strEnd = -1;
            int strBegin = 0;
            do
            {
                // Get last symbol number in string
                strEnd = _strErrorOutput.IndexOf("\n", strBegin);

                if (strEnd != -1)
                {
                    // Form error string
                    string strError = new string(_strErrorOutput.ToCharArray(), strBegin, strEnd - strBegin);

                    // Get diagnostic message from string
                    DiagnosticMessage diagMsg = GetDiagnosticMessageFromString(strError);
                    diagnosticMsgs.Add(diagMsg);

                    strBegin = strEnd + 1;
                }
            }
            while (strEnd++ != -1);
        }

        /** Get diagnostic message from string
         *
         *  \param _strError the error string
         *  \return DiagnosticMsg the diagnostic message represented in struct
         */
        private DiagnosticMessage GetDiagnosticMessageFromString(string _strError)
        {
            // File path on disc
            int first = _strError.IndexOf(":");
            if (first == -1) //без имени файла и позиции в файле
            {
                if (_strError.Contains("warning:"))
                {
                    _strError.Replace("warning:", "");
                    return new DiagnosticMessage(_strError.Trim(), true);
                }
                else
                {
                    return new DiagnosticMessage(_strError.Trim());
                }
            }

            _strError = _strError.Substring(first + 1);

            // First ":" symbol
            int second = _strError.IndexOf(":");

            // Second ":" symbol
            int third = _strError.IndexOf(":", second + 1);

            if (second == -1 || third == -1)//непонятное сообщение
            {
                if (_strError.Contains("warning:"))
                {
                    _strError = _strError.Replace("warning:", "");
                    return new DiagnosticMessage(_strError.Trim(), true);
                }
                else
                {
                    return new DiagnosticMessage(_strError.Trim());
                }
            }

            string strLine = _strError.Substring(0, second);
            string strColumn = _strError.Substring(second + 1, third - second - 1);

            SourcePosition errorLocation = new SourcePosition(fileName, 0, 0);
            try
            {
                errorLocation.LineNumber = Convert.ToInt32(strLine);
                errorLocation.ColumnNumber = Convert.ToInt32(strColumn);
            }
            catch
            {
                //не удалось сконвертировать координаты. принимаем что их нет
                if (_strError.Contains("warning:"))
                {
                    _strError = _strError.Replace("warning:", "");
                    return new DiagnosticMessage(_strError.Trim(), true);
                }
                else
                {
                    return new DiagnosticMessage(_strError.Trim());
                }
            }

            _strError = _strError.Substring(third + 1);

            if (_strError.Contains("warning:"))
            {
                _strError = _strError.Replace("warning:", "");
                return new DiagnosticMessage(errorLocation, _strError.Trim(), true);
            }
            else
            {
                return new DiagnosticMessage(errorLocation, _strError.Trim());
            }
        }


        /** Get line nr of error from selected string
         *
         *  \param _str the selected string
         *  \return int the line nr in file with error
         */
        public int GetLineNrFromSelectedString(string _strSelected)
        {
            int lineNr = -1;

            // Disc in path
            int first = _strSelected.IndexOf(":");

            // First ":" symbol
            int second = _strSelected.IndexOf(":", first + 1);

            if (second == -1)
                return -1;

            // Second ":" symbol
            int third = _strSelected.IndexOf(":", second + 1);
            if (third == -1)
                return -1;

            // Form string
            string str = new string(_strSelected.ToCharArray(), second + 1, third - second - 1);

            // Convert to integer
            lineNr = Convert.ToInt32(str);

            return lineNr;
        }


        /** Get diagnostic message line and column by index in collection 
         *  \param _idx index of diagnostic message object in the collection
         *  \param ref _iLineNr line number of diagnostic message
         *  \param ref _iColumnNr column number of dignostic message
         *  \return true/false is element with interesting index exist in the collection
         */
        public bool GetDiagnosticMsgDataByIdx(int _iIdx, ref int _iLineNr, ref int _iColumnNr)
        {
            if (!(DiagnosticMsgs.Count <= _iIdx))
            {
                _iLineNr = DiagnosticMsgs[_iIdx].Position.LineNumber;
                _iColumnNr = DiagnosticMsgs[_iIdx].Position.ColumnNumber;
                return true;
            }
            else
                return false;
        }        
    }
}
