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

namespace VHDL.parser
{
    using PositionInformation = VHDL.annotation.PositionInformation;

    /// <summary>
    /// Parser settings.
    /// </summary>
    public class VhdlParserSettings
    {

        private bool createDummyObjects;
        private bool printErrors;
        private bool emitResolveErrors;
        private bool addPositionInformation;
        private bool parseComments;

        public VhdlParserSettings()
        {
            printErrors = false;
            createDummyObjects = true;
            emitResolveErrors = true;
            addPositionInformation = true;
            parseComments = true;
        }

        /// <summary>
        /// Returns/Sets if the parser should create dummy objects.
        /// </summary>
        public virtual bool CreateDummyObjects
        {
            get { return createDummyObjects; }
            set { createDummyObjects = value; }
        }

        /// <summary>
        /// Returns/Sets if resolve errors are emitted.
        /// </summary>
        /// <returns></returns>
        public virtual bool EmitResolveErrors
        {
            get { return emitResolveErrors; }
            set { emitResolveErrors = value; }
        }

        /// <summary>
        /// Returns/Sets if informations about the position in the source file should be stored in the meta class instances.
        /// </summary>
        public virtual bool AddPositionInformation
        {
            get { return addPositionInformation; }
            set { addPositionInformation = value; }
        }

        /// <summary>
        /// Returns/Sets if error messages should be printed to stderr.
        /// </summary>
        public virtual bool PrintErrors
        {
            get { return printErrors; }
            set { printErrors = value; }
        }

        /// <summary>
        /// Returns/Sets if comments in the input file are added as annotations to the meta classes.
        /// </summary>
        public virtual bool ParseComments
        {
            get { return parseComments; }
            set { parseComments = value; }
        }
    }
}