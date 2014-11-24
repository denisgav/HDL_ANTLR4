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

namespace VHDL.util
{

    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using LabeledElement = VHDL.LabeledElement;
    using NamedEntity = VHDL.INamedEntity;
    using VhdlElement = VHDL.VhdlElement;
    using DeclarativeItemMarker = VHDL.declaration.IDeclarativeItemMarker;
    using EnumerationLiteral = VHDL.literal.EnumerationLiteral;
    using PhysicalLiteral = VHDL.literal.PhysicalLiteral;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using EnumerationType = VHDL.type.EnumerationType;
    using PhysicalType = VHDL.type.PhysicalType;
    using System;
    using VHDL.Object;
    using VHDL.declaration;
    using VHDL.type;
    using VHDL.libraryunit;

    /// <summary>
    /// Vhdl collection utility class.
    /// </summary>
    [Serializable]
    public class VhdlCollections
    {

        private VhdlCollections()
        {
        }

        /// <summary>
        /// Returns a list that contains all elements in another list which are
        /// instances of the given class.
        /// </summary>
        /// <typeparam name="T">the type of the elements that should be returned</typeparam>
        /// <typeparam name="E">the type of the elements in the original list</typeparam>
        /// <param name="list">the original list</param>
        /// <returns>a list</returns>
        public static List<T> GetAll<T, E>(List<E> list) where T : E
        {
            List<T> result = new List<T>();

            foreach (E element in list)
            {
                if (element is T)
                {
                    //it is safe to remove this warning because the cast is checked
                    //by the surrounding if statement
                    T tmp = (T)element;

                    result.Add(tmp);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns an element by it's identifier.
        /// </summary>
        /// <typeparam name="T">the searched type</typeparam>
        /// <typeparam name="E">the list element type</typeparam>
        /// <param name="list">the list</param>
        /// <param name="identifier">the identifier</param>
        /// <returns>the object or null if no matching element exists</returns>
        public static T GetByIdentifier<T, E>(List<E> list, string identifier)
            where T : class, NamedEntity
            where E : class
        {
            foreach (E element in list)
            {
                if (element is T)
                {
                    //it is safe to remove this warning because the cast is checked
                    //by the surrounding if statement
                    T tmp = element as T;

                    if (identifier.Equals(tmp.Identifier, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return tmp;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a declaration list.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateDeclarationList<E>() where E : class, DeclarativeItemMarker
        {
            return new DeclarationList<E>();
        }

        /// <summary>
        /// Creates a declaration list.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="list">a list that is used to initialize the list</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateDeclarationList<E>(List<E> list) where E : class, DeclarativeItemMarker
        {
            return new DeclarationList<E>(list);
        }

        /// <summary>
        /// Creates a list of VHDL objects.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateVhdlObjectList<E>() where E : class, VhdlObjectProvider
        {
            return new VhdlObjectList<E>();
        }

        /// <summary>
        /// Creates a list of VHDL objects.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="list">a list that is used to initialize the list</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateVhdlObjectList<E>(List<E> list) where E : class, VhdlObjectProvider
        {
            return new VhdlObjectList<E>(list);
        }

        /// <summary>
        /// Creates a list of labeled elements.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="parent">the parent</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateLabeledElementList<E>(DeclarativeRegion parent) where E : LabeledElement
        {
            return new LabeledElementList<E>(parent);
        }

        /// <summary>
        /// Creates a list of labeled elements.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="parent">the parent</param>
        /// <param name="list">a list that is used to initialize the list</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateLabeledElementList<E>(DeclarativeRegion parent, IList<E> list) where E : LabeledElement
        {
            return new LabeledElementList<E>(parent, list);
        }

        /// <summary>
        /// Creates a list of named entities.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="parent">the parent</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> CreateNamedEntityList<E>(DeclarativeRegion parent) where E : VhdlElement
        {
            return new NamedEntityList<E>(parent);
        }

        /// <summary>
        /// Creates a list of named entities.
        /// </summary>
        /// <typeparam name="E">the element type</typeparam>
        /// <param name="parent">the parent</param>
        /// <param name="list">a list that is used to initialize the list</param>
        /// <returns>the list</returns>
        public static IResolvableList<E> createNamedEntityList<E>(DeclarativeRegion parent, List<E> list) where E : VhdlElement
        {
            return new NamedEntityList<E>(parent, list);
        }

        //TODO: use map to resolve identifiers
        /// <summary>
        /// Declaration list.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        [Serializable]
        public sealed class DeclarationList<E> : List<E>, IResolvableList<E> where E : class
        {

            public DeclarationList()
                : base(new List<E>())
            {
            }

            public DeclarationList(List<E> list)
                : base(new List<E>(list))
            {
            }

            public object Resolve(string identifier)
            {
                foreach (E declaration in this)
                {
                    if (declaration is EnumerationType)
                    {
                        EnumerationType type = declaration as EnumerationType;
                        if (identifier.Equals(type.Identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return type;
                        }

                        //TODO: support overloading
                        foreach (EnumerationLiteral literal in type.Literals)
                        {
                            if (identifier.Equals(literal.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                return literal;
                            }
                        }
                    }
                    else if (declaration is RecordType)
                    {
                        RecordType type = declaration as RecordType;
                        if (identifier.Equals(type.Identifier, StringComparison.InvariantCultureIgnoreCase))
                            return type;
                        foreach (VHDL.type.RecordType.ElementDeclaration el in type.Elements)
                        {
                            foreach (string name in el.Identifiers)
                            {
                                if (identifier.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return new Variable(name, el.Type);
                                }
                            }
                        }
                    }
                    else if (declaration is PhysicalType)
                    {
                        PhysicalType type = declaration as PhysicalType;
                        if (identifier.Equals(type.Identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return type;
                        }

                        //TODO: don't use strings for the physical literals
                        if (identifier.Equals(type.PrimaryUnit, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return new PhysicalLiteral(type.PrimaryUnit);
                        }

                        foreach (PhysicalType.Unit unit in type.Units)
                        {
                            if (identifier.Equals(unit.Identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return new PhysicalLiteral(unit.Identifier);
                            }
                        }
                    }
                    else if (declaration is NamedEntity)
                    {
                        INamedEntity identElement = (NamedEntity)declaration;
                        if (identifier.Equals(identElement.Identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return declaration;
                        }
                    }
                    else if (declaration is SignalDeclaration)
                    {
                        SignalDeclaration objDecl = declaration as SignalDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return o;
                            }
                        }
                    }
                    else if (declaration is ConstantDeclaration)
                    {
                        ConstantDeclaration objDecl = declaration as ConstantDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return o;
                            }
                        }
                    }
                    else if (declaration is VariableDeclaration)
                    {
                        VariableDeclaration objDecl = declaration as VariableDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return o;
                            }
                        }
                    }
                    else if (declaration is FileDeclaration)
                    {
                        FileDeclaration objDecl = declaration as FileDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return o;
                            }
                        }
                    }
                    else if (declaration is Alias)
                    {
                        Alias objDecl = declaration as Alias;
                        if (objDecl.Designator.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return new Variable(objDecl.Aliased, objDecl.SubtypeIndication);
                        }
                    }
                    else if (declaration is Subtype)
                    {
                        Subtype objDecl = declaration as Subtype;
                        if (objDecl.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return objDecl;
                        }
                    }
                    else if (declaration is UseClause)
                    {
                        UseClause use = declaration as UseClause;
                        object res = use.Scope.resolve(identifier);
                        if (res != null)
                            return res;
                    }
                    else if (declaration.GetType().BaseType == typeof(ObjectDeclaration<>))
                    {
                        ObjectDeclaration<VhdlObject> objDecl = declaration as ObjectDeclaration<VhdlObject>;
                        foreach (VhdlObject o in objDecl.Objects)
                        {
                            if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return o;
                            }
                        }
                    }
                }

                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (E declaration in this)
                {
                    if (declaration is EnumerationType)
                    {
                        EnumerationType type = declaration as EnumerationType;
                        res.Add(type);


                        //TODO: support overloading
                        foreach (EnumerationLiteral literal in type.Literals)
                        {
                            res.Add(literal);
                        }
                    }
                    else if (declaration is RecordType)
                    {
                        RecordType type = declaration as RecordType;
                        res.Add(type);
                        foreach (VHDL.type.RecordType.ElementDeclaration el in type.Elements)
                        {
                            foreach (string name in el.Identifiers)
                            {
                                res.Add(name);
                            }
                        }
                    }
                    else if (declaration is PhysicalType)
                    {
                        PhysicalType type = declaration as PhysicalType;
                        res.Add(type);
                        res.Add(new PhysicalLiteral(type.PrimaryUnit));

                        foreach (PhysicalType.Unit unit in type.Units)
                        {
                            res.Add(unit.Identifier);
                        }
                    }
                    else if (declaration is NamedEntity)
                    {
                        INamedEntity identElement = (NamedEntity)declaration;
                        res.Add(declaration);
                    }
                    else if (declaration is SignalDeclaration)
                    {
                        SignalDeclaration objDecl = declaration as SignalDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is ConstantDeclaration)
                    {
                        ConstantDeclaration objDecl = declaration as ConstantDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is VariableDeclaration)
                    {
                        VariableDeclaration objDecl = declaration as VariableDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is FileDeclaration)
                    {
                        FileDeclaration objDecl = declaration as FileDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is Alias)
                    {
                        res.Add(declaration);
                    }
                    else if (declaration is Subtype)
                    {
                        res.Add(declaration);
                    }
                    else if (declaration is UseClause)
                    {
                        UseClause use = declaration as UseClause;
                        res.AddRange(use.Scope.GetListOfObjects());
                    }
                    else if (declaration.GetType().BaseType == typeof(ObjectDeclaration<>))
                    {
                        ObjectDeclaration<VhdlObject> objDecl = declaration as ObjectDeclaration<VhdlObject>;
                        foreach (VhdlObject o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                }
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (E declaration in this)
                {
                    if (declaration is EnumerationType)
                    {
                        EnumerationType type = declaration as EnumerationType;
                        res.Add(type);


                        //TODO: support overloading
                        foreach (EnumerationLiteral literal in type.Literals)
                        {
                            res.Add(literal);
                        }
                    }
                    else if (declaration is RecordType)
                    {
                        RecordType type = declaration as RecordType;
                        res.Add(type);
                        foreach (VHDL.type.RecordType.ElementDeclaration el in type.Elements)
                        {
                            foreach (string name in el.Identifiers)
                            {
                                res.Add(name);
                            }
                        }
                    }
                    else if (declaration is PhysicalType)
                    {
                        PhysicalType type = declaration as PhysicalType;
                        res.Add(type);
                        res.Add(new PhysicalLiteral(type.PrimaryUnit));

                        foreach (PhysicalType.Unit unit in type.Units)
                        {
                            res.Add(unit.Identifier);
                        }
                    }
                    else if (declaration is NamedEntity)
                    {
                        INamedEntity identElement = (NamedEntity)declaration;
                        res.Add(declaration);
                    }
                    else if (declaration is SignalDeclaration)
                    {
                        SignalDeclaration objDecl = declaration as SignalDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is ConstantDeclaration)
                    {
                        ConstantDeclaration objDecl = declaration as ConstantDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is VariableDeclaration)
                    {
                        VariableDeclaration objDecl = declaration as VariableDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is FileDeclaration)
                    {
                        FileDeclaration objDecl = declaration as FileDeclaration;
                        foreach (var o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                    else if (declaration is Alias)
                    {
                        res.Add(declaration);
                    }
                    else if (declaration is Subtype)
                    {
                        res.Add(declaration);
                    }
                    else if (declaration is UseClause)
                    {
                        UseClause use = declaration as UseClause;
                        res.AddRange(use.Scope.GetLocalListOfObjects());
                    }
                    else if (declaration.GetType().BaseType == typeof(ObjectDeclaration<>))
                    {
                        ObjectDeclaration<VhdlObject> objDecl = declaration as ObjectDeclaration<VhdlObject>;
                        foreach (VhdlObject o in objDecl.Objects)
                        {
                            res.Add(o);
                        }
                    }
                }
                return res;
            }
        }

        //    *
        //     * VHDL object list.
        //     
        [Serializable]
        public sealed class VhdlObjectList<E> : List<E>, IResolvableList<E> where E : class, VhdlObjectProvider
        {

            public VhdlObjectList()
                : base(new List<E>())
            {
            }

            public VhdlObjectList(List<E> list)
                : base(new List<E>(list))
            {
            }

            public object Resolve(string identifier)
            {
                foreach (E provider in this)
                {
                    foreach (VhdlObject o in provider.VhdlObjects)
                    {
                        if (o.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return o;
                        }
                    }
                }

                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (E provider in this)
                    res.AddRange(provider.VhdlObjects);
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }

        //    *
        //     * Labeled element list.
        //     
        [Serializable]
        public sealed class LabeledElementList<E> : List<E>, IResolvableList<E> where E : LabeledElement
        {

            public LabeledElementList(DeclarativeRegion parent)
                : base(ParentSetList<E>.Create(parent))
            {
            }

            public LabeledElementList(DeclarativeRegion parent, IList<E> list)
                : base(ParentSetList<E>.Create(parent, list))
            {
            }

            public object Resolve(string identifier)
            {
                foreach (E element in this)
                {
                    if (identifier.Equals(element.Label, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return element;
                    }
                }
                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (E element in this)
                    res.Add(element);
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }

        //    *
        //     * Named entity list.
        //     
        [Serializable]
        public sealed class NamedEntityList<E> : List<E>, IResolvableList<E> where E : VhdlElement
        {

            public NamedEntityList(DeclarativeRegion parent)
                : base(ParentSetList<E>.Create(parent))
            {
            }

            public NamedEntityList(DeclarativeRegion parent, List<E> list)
                : base(ParentSetList<E>.Create(parent, list))
            {
            }

            public object Resolve(string identifier)
            {
                foreach (E element in this)
                {
                    if (element is NamedEntity)
                    {
                        INamedEntity namedEntity = (NamedEntity)element;
                        if (namedEntity.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return element;
                        }
                    }
                }
                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (E element in this)
                    res.Add(element);
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }
    }
}