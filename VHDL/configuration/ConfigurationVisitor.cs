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

namespace VHDL.configuration
{
    /// <summary>
    /// Configuration visitor.
    /// </summary>
	public class ConfigurationVisitor
	{
        /// <summary>
        /// Visits a configuration item.
        /// No visit method is called if the parameter equals <code>null</code>.
        /// </summary>
        /// <param name="item"></param>
		public virtual void visit(ConfigurationItem item)
		{
			if (item != null)
			{
				item.accept(this);
			}
		}

        /// <summary>
        /// Visits a list of configuration items.
        /// null items in the list are ignored.
        /// The list parameter must not be null.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="items">the configuration items</param>
		public virtual void visit<T1>(List<T1> items) where T1 : ConfigurationItem
		{
			foreach (ConfigurationItem item in items)
			{
				if (item != null)
				{
					item.accept(this);
				}
			}
		}

        /// <summary>
        /// Visits an architecture configuration.
        /// </summary>
        /// <param name="configuration">the architecture configuration</param>
		protected internal virtual void visitArchitectureConfiguration(ArchitectureConfiguration configuration)
		{
		}

        /// <summary>
        /// Visits a block statement configuration.
        /// </summary>
        /// <param name="configuration">the block statement configuration</param>
		protected internal virtual void visitBlockStatementConfiguration(BlockStatementConfiguration configuration)
		{
		}

        /// <summary>
        /// Visits a component configuration.
        /// </summary>
        /// <param name="configuration">the component configuration</param>
		protected internal virtual void visitComponentConfiguration(ComponentConfiguration configuration)
		{
		}

        /// <summary>
        /// Visits a generate statement configuration.
        /// </summary>
        /// <param name="configuration">the generate statement configuration</param>
		protected internal virtual void visitGenerateStatementConfiguration(GenerateStatementConfiguration configuration)
		{
		}
	}
}