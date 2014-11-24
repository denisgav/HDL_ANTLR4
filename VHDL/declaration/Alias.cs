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
using VHDL.Object;

namespace VHDL.declaration
{
    using Signature = VHDL.Signature;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    //TODO: replace dummy implementation
    /// <summary>
    /// Alias declaration.
    /// </summary>
    [Serializable]
	public class Alias : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem
	{
		private string designator;
		private SubtypeIndication subtypeIndication;
		private string aliased;
		private Signature signature;

        /// <summary>
        /// Creates an alias declartion.
        /// </summary>
        /// <param name="designator">the alias designator</param>
        /// <param name="subtypeIndication">the subtype indication</param>
		public Alias(string designator, SubtypeIndication subtypeIndication)
		{
			this.designator = designator;
			this.subtypeIndication = subtypeIndication;
		}

        /// <summary>
        /// Creates an alias declaration.
        /// </summary>
        /// <param name="designator">the alias designator</param>
        /// <param name="aliased">the identifier of the aliased object</param>
		public Alias(string designator, string aliased)
		{
			this.designator = designator;
			this.aliased = aliased;
		}

        /// <summary>
        /// Creates an alias declaration.
        /// </summary>
        /// <param name="designator">the alias designator</param>
        /// <param name="subtypeIndication">the subtype indication</param>
        /// <param name="aliased">the identifier of the aliased object</param>
		public Alias(string designator, SubtypeIndication subtypeIndication, string aliased)
		{
			this.designator = designator;
			this.subtypeIndication = subtypeIndication;
			this.aliased = aliased;
		}

        /// <summary>
        /// Returns/Sets the identifier of the aliased object.
        /// </summary>
		public virtual string Aliased
		{
            get { return aliased; }
            set { aliased = value; }
		}

        /// <summary>
        /// Returns/Sets the alias designator.
        /// </summary>
		public virtual string Designator
		{
            get { return designator; }
            set { designator = value; }
		}

        /// <summary>
        /// Returns/Sets the subtype indication.
        /// </summary>
		public virtual SubtypeIndication SubtypeIndication
		{
            get { return subtypeIndication; }
            set { subtypeIndication = value; }
		}

		/// <summary>
        /// Returns/Sets the signature.
		/// </summary>
        public virtual Signature Signature
		{
            get { return signature; }
            set { signature = value; }
		}

		internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitAliasDeclaration(this);
		}
	}
}