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
namespace VHDL.annotation
{
    /// <summary>
    /// Information about the position of a meta class in the parsed file.
    /// </summary>
    [Serializable]
	public class PositionInformation
	{
		private readonly SourcePosition begin;
		private readonly SourcePosition end;

        private string fileName;
        public string FileName
        {
            get { return fileName; }
         }
        

		public PositionInformation(string fileName, SourcePosition begin, SourcePosition end)
		{
            this.fileName = fileName;
			this.begin = begin;
			this.end = end;
		}

		public virtual SourcePosition Begin
		{
            get { return begin; }
		}

		public virtual SourcePosition End
		{
            get { return end; }
		}
	}

}