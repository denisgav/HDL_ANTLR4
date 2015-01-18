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
using VHDL.parser;
using VHDLCompiler.CodeTemplates.Helpers;

namespace VHDLCompiler.VHDLObserver
{
    public abstract class VHDLObserverBase
    {
        public abstract void Observe(VHDLCompilerInterface compiler);

        protected string GenReportStatement(LoggerMessageVerbosity verbosity, string str)
        {
            string verb = string.Format("{0}.{1}", "VHDLRuntime.LoggerMessageVerbosity", verbosity);
            FunctionCallTemplate template = new FunctionCallTemplate("Logger", "WriteLine", verb, "CurrentTime", "CurrentDutyCycle", str);
            return template.TransformText();
        }

        protected string GenReportStatement(string str)
        {
            return GenReportStatement(LoggerMessageVerbosity.Info, str);
        }

        protected string GenReportStatement(LoggerMessageVerbosity verbosity, object o)
        {
            string verb = string.Format("{0}.{1}", "VHDLRuntime.LoggerMessageVerbosity", verbosity);
            FunctionCallTemplate template = new FunctionCallTemplate("Logger", "WriteLine", verb, "CurrentTime", "CurrentDutyCycle", o);
            return template.TransformText();
        }

        protected string GenReportStatement(object o)
        {
            return GenReportStatement(LoggerMessageVerbosity.Info, o);
        }
    }
}
