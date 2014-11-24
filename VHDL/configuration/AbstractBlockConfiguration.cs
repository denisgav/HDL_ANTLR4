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

using System.Collections.Generic;
using System;

namespace VHDL.configuration
{
    using UseClause = VHDL.libraryunit.UseClause;
    /// <summary>
    /// Abstract base class for block configurations.
    /// </summary>
    [Serializable]
	public abstract class AbstractBlockConfiguration : ConfigurationItem
	{
		private readonly List<UseClause> useClauses;
		private readonly List<ConfigurationItem> configurationItems;

        /// <summary>
        /// Creates an empty block configuration.
        /// </summary>
		public AbstractBlockConfiguration()
		{
            useClauses = new List<UseClause>();
            configurationItems = new List<ConfigurationItem>();
		}

        /// <summary>
        /// Returns the use clauses in this block configuration.
        /// </summary>
		public virtual List<UseClause> UseClauses
		{
            get { return useClauses; }
		}

        /// <summary>
        /// Returns the configuration items in this block configuration.
        /// </summary>
		public virtual List<ConfigurationItem> ConfigurationItems
		{
            get { return configurationItems; }
		}
	}
}