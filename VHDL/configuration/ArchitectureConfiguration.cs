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
using System.Runtime.Serialization;

namespace VHDL.configuration
{
    using Architecture = VHDL.libraryunit.Architecture;

    /// <summary>
    /// Architecuture configuration.
    /// </summary>
    [Serializable]
	public class ArchitectureConfiguration : AbstractBlockConfiguration
	{
		private Architecture architecture;

        /// <summary>
        /// Creates a architecture configuration.
        /// </summary>
        /// <param name="architecture">the configured architecture</param>
		public ArchitectureConfiguration(Architecture architecture)
		{
			this.architecture = architecture;
		}

        /// <summary>
        /// Returns/Sets the configured architecture.
        /// </summary>
		public virtual Architecture Architecture
		{
            get { return architecture; }
            set { architecture = value; }
		}

		internal override void accept(ConfigurationVisitor visitor)
		{
			visitor.visitArchitectureConfiguration(this);
		}
	}
}