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

namespace VHDL.concurrent
{
    using Configuration = VHDL.libraryunit.Configuration;

    /// <summary>
    /// Configuration instantiation.
    /// </summary>
    [Serializable]
    public class ConfigurationInstantiation : AbstractComponentInstantiation
    {
        private Configuration configuration;

        /// <summary>
        /// Creates a configuration instantiation.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="configuration">the instantiated configuration</param>
        public ConfigurationInstantiation(string label, Configuration configuration)
            : base(label)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Returns/Sets the instantiated configuration.
        /// </summary>
        public virtual Configuration Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitConfigurationInstantiation(this);
        }
    }
}