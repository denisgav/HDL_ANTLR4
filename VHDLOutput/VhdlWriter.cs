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
using System.IO;
using System.Collections.Generic;

namespace VHDL.output
{
	using NamedEntity = VHDL.INamedEntity;    
    using VHDL.declaration;
    
    /// <summary>
    /// Allows the usage of java.io.Writer for VHDL output.
    /// </summary>
	internal class VhdlWriter
	{
		private const string NULL = "null";
		private readonly StreamWriter @out;
		private readonly IVhdlCodeFormat format;
		private bool firstAppend = true;
		private int IndentationLevel = 0;
		private bool inAlignBlock;
		private int lineAlignPosition;
		private int alignPosition;
		private List<Line> lines = new List<Line>();
		private System.Text.StringBuilder currentLine = new System.Text.StringBuilder(128);

        /// <summary>
        /// Creates a vhdl writer adapter.
        /// </summary>
        /// <param name="out">the base writer</param>
        /// <param name="format">the code format</param>
		public VhdlWriter(StreamWriter @out, IVhdlCodeFormat format)
		{
			this.@out = @out;
			this.format = format;
		}

		private void handleIndentation()
		{
			if (firstAppend)
			{
				for (int i = 0; i < IndentationLevel; i++)
				{
					@out.Write(format.IndentationString);
				}
				firstAppend = false;
			}
		}

        /// <summary>
        /// Appends a string.
        /// </summary>
        /// <param name="s">the string</param>
        /// <returns>this writer</returns>
		public virtual VhdlWriter Append(string s)
		{
			if (inAlignBlock)
			{
				currentLine.Append(s);
			}
			else
			{
				try
				{
					handleIndentation();
					@out.Write(s);
				}
				catch (IOException ex)
				{
				//TODO: report exception
				}
			}

			return this;
		}

        /// <summary>
        /// Appends an character.
        /// </summary>
        /// <param name="c">the character</param>
        /// <returns>this writer</returns>
		public virtual VhdlWriter Append(char c)
		{
			if (inAlignBlock)
			{
				currentLine.Append(c);
			}
			else
			{
				try
				{
					handleIndentation();
					@out.Write(c);
				}
				catch (IOException ex)
				{
				//TODO: report exception
				}
			}

			return this;
		}

        /// <summary>
        /// Appends a list of strings separated by a delimiter.
        /// </summary>
        /// <param name="strings">a list of strings</param>
        /// <param name="delimiter">the delimiter</param>
        /// <returns>this writer</returns>
		public virtual VhdlWriter AppendStrings(List<string> strings, string delimiter)
		{
			if (strings == null || delimiter == null)
			{
				Append(NULL);
			}
			else
			{
				bool first = true;
				foreach (string str in strings)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						Append(delimiter);
					}
					Append(str);
				}
			}

			return this;
		}

        /// <summary>
        /// Appends the identifiers of a list of named entities.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="elements">a list of named entities</param>
        /// <param nathis writerme="delimiter">the delimiter</param>
        /// <returns></returns>
		public virtual VhdlWriter AppendIdentifiers<T1>(IList<T1> elements, string delimiter) where T1 : NamedEntity
		{
			if (elements == null || delimiter == null)
			{
				Append(NULL);
			}
			else
			{
				bool first = true;
				foreach (INamedEntity element in elements)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						Append(delimiter);
					}
					AppendIdentifier(element);
				}
			}

			return this;
		}

        /// <summary>
        /// Appends a list of output enums.
        /// </summary>
        /// <param name="elements">the enums</param>
        /// <param name="delimiter">the delimiter</param>
        /// <returns>the writer</returns>
        public virtual VhdlWriter AppendOutputEnums(IList<EntityClass> elements, string delimiter)
		{
			if (elements == null || delimiter == null)
			{
				Append(NULL);
			}
			else
			{
				bool first = true;
                foreach (EntityClass element in elements)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						Append(delimiter);
					}
					Append(element.ToString());
				}
			}

			return this;
		}

        /// <summary>
        /// Appends the identifier of a named entity.
        /// </summary>
        /// <param name="namedEntity">the named entity</param>
        /// <returns>this writer</returns>
		public virtual VhdlWriter AppendIdentifier(NamedEntity namedEntity)
		{
			if (namedEntity != null)
			{
				Append(namedEntity.Identifier);
			}
			else
			{
				Append(NULL);
			}

			return this;
		}

        /// <summary>
        /// Appends a line break.
        /// </summary>
        /// <returns>this writer</returns>
		public virtual VhdlWriter NewLine()
		{
			if (inAlignBlock)
			{
				Line l = new Line(currentLine.ToString(), lineAlignPosition);
				lines.Add(l);
				currentLine = new System.Text.StringBuilder(100);
				lineAlignPosition = -1;
			}
			else
			{
				try
				{
					@out.Write(format.LineSeparator);
				}
				catch (IOException ex)
				{
				//TODO: report exception
				}
			}

			firstAppend = true;
			return this;
		}

        /// <summary>
        /// Increases the Indentation level.
        /// If the current line already contains non whitespace characters the current line
        /// is Indented, otherwise the Indentation is increased in the next line.
        /// </summary>
        /// <returns></returns>
		public virtual VhdlWriter Indent()
		{
			IndentationLevel++;

			return this;
		}

        /// <summary>
        /// Reduces the Indentation level.
        /// </summary>
        /// <returns>the writer</returns>
		public virtual VhdlWriter Dedent()
		{
			if (IndentationLevel > 0)
			{
				IndentationLevel--;
			}
			else
			{
				throw new Exception("too many Dedents");
			}

			return this;
		}

        /// <summary>
        /// Begins an alignment block.
        /// </summary>
        /// <returns>the writer</returns>
		public virtual VhdlWriter BeginAlign()
		{
			if (format.Align)
			{
				alignPosition = 0;
				lineAlignPosition = -1;
				inAlignBlock = true;
				lines.Clear();
			}

			return this;
		}

		private void AppendSpaces(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Append(' ');
			}
		}

        /// <summary>
        /// Ends an alignment block.
        /// </summary>
        /// <returns>the writer</returns>
		public virtual VhdlWriter EndAlign()
		{
			bool NewLineAfterLast = true;

			inAlignBlock = false;

			if (currentLine.Length > 0)
			{
				NewLine();
				NewLineAfterLast = false;
			}

			for (int i = 0; i < lines.Count; i++)
			{
				Line line = lines[i];

				int position = line.getAlignPosition();
				if (position >= 0)
				{
					Append(line.Text.Substring(0, position));
					AppendSpaces(alignPosition - position);
					Append(line.Text.Substring(position));
				}
				else
				{
					Append(line.Text);
				}

				if (NewLineAfterLast || i != lines.Count - 1)
				{
					NewLine();
				}
			}

			return this;
		}

        /// <summary>
        /// Inserts an alignement marker.
        /// </summary>
        /// <returns>the writer</returns>
		public virtual VhdlWriter Align()
		{
			lineAlignPosition = currentLine.Length;
			if (lineAlignPosition > alignPosition)
			{
				alignPosition = lineAlignPosition;
			}

			return this;
		}

        /// <summary>
        /// Returns the code format.
        /// </summary>
        /// <returns></returns>
		public virtual IVhdlCodeFormat Format
		{
            get { return format; }
		}

		private class Line
		{

			private readonly string text;
			private readonly int alignPosition;

			public Line(string text, int alignPosition)
			{
				this.text = text;
				this.alignPosition = alignPosition;
			}

			public virtual int getAlignPosition()
			{
				return alignPosition;
			}

			public virtual string Text
			{
				get
				{
					return text;
				}
			}
		}        
    }
}