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

namespace VHDL.configuration
{
    using BlockStatement = VHDL.concurrent.BlockStatement;

    /// <summary>
    /// Block statement configuration.
    /// </summary>
    [Serializable]
	public class BlockStatementConfiguration : AbstractBlockConfiguration
	{
		private BlockStatement block;

        /// <summary>
        /// Creates a block statement configuration.
        /// </summary>
        /// <param name="block">the configured block</param>
		public BlockStatementConfiguration(BlockStatement block)
		{
			this.block = block;
		}

        /// <summary>
        /// Returns/Sets the configured block.
        /// </summary>
		public virtual BlockStatement Block
		{
            get { return block; }
            set { block = value; }
		}

		internal override void accept(ConfigurationVisitor visitor)
		{
			visitor.visitBlockStatementConfiguration(this);
		}
	}
}