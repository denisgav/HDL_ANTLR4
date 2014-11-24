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

namespace VHDL.declaration
{
    using Signature = VHDL.Signature;
    using VhdlElement = VHDL.VhdlElement;
    using Expression = VHDL.expression.Expression;

    /// <summary>
    /// Attribute specification.
    /// An attribute specification is used to set an attribute of a named entity.
    /// 
    /// @VHDL.example
    /// Attribute attrib = new Attribute("ATTRIB", Standard.STRING);
    /// AttributeSpecification s = new AttributeSpecification(attrib, EntityNameList.ALL,
    /// EntityClass.SIGNAL, new StringLiteral("value"));
    ///  ---
    /// attribute ATTRIB of all : signal is "value";
    /// </summary>
    [Serializable]
	public class AttributeSpecification : DeclarativeItem, IBlockDeclarativeItem, IConfigurationDeclarativeItem, IEntityDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem
	{
		private Attribute attribute;
		private EntityNameList entities;
		private EntityClass entityClass;
		private Expression @value;

        /// <summary>
        /// Creates an attribute specification.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <param name="entities">the list of entity names</param>
        /// <param name="entityClass">the class of the entities</param>
        /// <param name="value">the value</param>
		public AttributeSpecification(Attribute attribute, EntityNameList entities, EntityClass entityClass, Expression @value)
		{
			this.attribute = attribute;
			this.entities = entities;
			this.entityClass = entityClass;
			this.value = @value;
		}

        /// <summary>
        /// Returns/Sets the specified attribute.
        /// </summary>
		public virtual Attribute Attribute
		{
            get { return attribute; }
            set { attribute = value; }
		}

        /// <summary>
        /// Returns/Sets the list of entities.
        /// </summary>
		public virtual EntityNameList Entities
		{
            get { return entities; }
            set { entities = value; }
		}

        /// <summary>
        /// Returns/Sets the entity class.
        /// </summary>
		public virtual EntityClass EntityClass
		{
            get { return entityClass; }
            set { entityClass = value; }
		}

        /// <summary>
        /// Returns/Sets the value.
        /// </summary>
		public virtual Expression Value
		{
			get { return @value; }
			set	{ this.value = value; }
		}

    	internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitAttributeSpecification(this);
		}

        /// <summary>
        /// Entity name list.
        /// </summary>
        [Serializable]
		public class EntityNameList : VhdlElement
		{
			private readonly List<EntityDesignator> designators;
            /// <summary>
            /// OTHERS.
            /// </summary>
            public static readonly OTHERSEntityNameList OTHERS = new OTHERSEntityNameList();
            [Serializable]
			public class OTHERSEntityNameList: EntityNameList
			{
                public OTHERSEntityNameList():base(true)
                {}
			}

            /// <summary>
            /// ALL
            /// </summary>
            public static readonly AllEntityNameList ALL = new AllEntityNameList();
            [Serializable]
			public class AllEntityNameList: EntityNameList
			{
                public AllEntityNameList():base(true)
                {}
			}

            /// <summary>
            /// Creates an empty entity name list.
            /// </summary>
			public EntityNameList()
			{
				designators = new List<EntityDesignator>();
			}

            /// <summary>
            /// Creates a entity name list.
            /// </summary>
            /// <param name="designators">a list of designators used to initialize the entity name list</param>
			public EntityNameList(List<EntityDesignator> designators)
			{
				this.designators = new List<EntityDesignator>(designators);
			}

            /// <summary>
            /// Creates a entity name list.
            /// </summary>
            /// <param name="designators">a list of designators used to initialize the entity name list</param>
			public EntityNameList( params EntityDesignator[] designators):
                this(new List<EntityDesignator>(designators))
			{				
			}

			private EntityNameList(bool nullList)
			{
				if (nullList)
				{
					designators = null;
				}
				else
				{
					designators = new List<EntityDesignator>();
				}
			}

            /// <summary>
            /// Returns the list of designators in this entity name list.
            /// The method returns null if there is no list of designators for this type of entity name list.
            /// </summary>
			public List<EntityDesignator> Designators
			{
                get { return designators; }
			}

            /// <summary>
            /// Creates a entity designator and adds it to this entity name list.
            /// </summary>
            /// <param name="entityTag">the tag of the designator</param>
            /// <returns>the created entity designator</returns>
			public EntityDesignator CreateDesignator(string entityTag)
			{
				if (designators == null)
				{
					throw new System.Exception("cannot add designator to fixed entity name list");
				}

				EntityDesignator designator = new EntityDesignator(entityTag);
				designators.Add(designator);

				return designator;
			}

            /// <summary>
            /// Creates a entity designator with a signature and adds it to this entity name list.
            /// </summary>
            /// <param name="entityTag">the tag of the designator</param>
            /// <param name="signature">the signature of the designator</param>
            /// <returns>the created designator</returns>
			public EntityDesignator CreateDesignator(string entityTag, Signature signature)
			{
				if (designators == null)
				{
                    throw new System.Exception("cannot add designator to fixed entity name list");
				}

				EntityDesignator designator = new EntityDesignator(entityTag, signature);
				designators.Add(designator);

				return designator;
			}

            /// <summary>
            /// Entity designator.
            /// </summary>
            [Serializable]
			public class EntityDesignator : VhdlElement
			{
			    //TODO: don't use String?
				private string entityTag;
				private Signature signature;

                /// <summary>
                /// Creates a entity designator.
                /// </summary>
                /// <param name="entityTag">the tag of the designator</param>
				public EntityDesignator(string entityTag)
				{
					this.entityTag = entityTag;
				}

                /// <summary>
                /// Creates a entity designator with a signature.
                /// </summary>
                /// <param name="entityTag">the tag</param>
                /// <param name="signature">the signature</param>
				public EntityDesignator(string entityTag, Signature signature)
				{
					this.entityTag = entityTag;
					this.signature = signature;
				}

                /// <summary>
                /// Returns/Sets the tag.
                /// </summary>
				public string EntityTag
				{
                    get { return entityTag; }
                    set { entityTag = value; }
				}

                /// <summary>
                /// Returns/Sets the signature.
                /// </summary>
				public Signature Signature
				{
                    get { return signature; }
                    set { signature = value; }
				}
			}
		}
	}
}