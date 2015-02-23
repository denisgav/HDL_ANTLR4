using VHDL.type;
using VHDL.expression;
using VHDL.declaration;
using AssociationElement = VHDL.AssociationElement;
using System.Collections.Generic;
using Exception = System.Exception;
using VHDL.util;
using VHDL.Object;
using VHDL;
using VHDL.ParseError;

namespace VHDL.parser.typeinfer
{
    public class TypeInference
    {
        public ISubtypeIndication ResultType { get; internal set; }
        public ISubtypeIndication ExpectedType { get; internal set; }

        private TypeInference(ISubtypeIndication type)
        {
            this.ExpectedType = type;
        }

        public static IProcedure ResolveOverloadProcedure(IDeclarativeRegion scope, List<IProcedure> overloads,
            List<AssociationElement> arguments)
        {
            string id = "";
            var candidates = new List<IProcedure>(overloads);
            switch (candidates.Count)
            {
                case 0:
                    throw new Exception("Object is not declared");
                case 1:
                    return candidates[0];
                default:
                    id = candidates[0].Identifier;
                    for (int i = 0; i < candidates.Count; )
                    {
                        var decl = candidates[i];
                        // check by arguments count
                        if (decl.Parameters.Count != arguments.Count)
                        {
                            candidates.RemoveAt(i);
                            continue;
                        }                        
                        if (!CheckAssociationList(scope, decl.Parameters, arguments))
                        {
                            candidates.RemoveAt(i);
                            continue;
                        }
                        ++i;
                    }
                    switch (candidates.Count)
                    {
                        case 0:
                            throw new vhdlNoMatchSubprogramException(id, "procedure");
                        case 1:
                            return candidates[0];
                        case 2:
                            {
                                // TODO: link body with declaration
                                bool firstIsBody = candidates[0] is SubprogramBody;
                                bool secndIsBody = candidates[1] is SubprogramBody;
                                if (firstIsBody ^ secndIsBody)
                                    return candidates[0];
                            }
                            throw new vhdlAmbiguousCallException(id, "procedure");
                        default:
                            throw new vhdlAmbiguousCallException(id, "procedure");
                    }
            }
        }

        public static IFunction ResolveOverloadFunction(IDeclarativeRegion scope, List<IFunction> overloads,
            List<AssociationElement> arguments, ISubtypeIndication returnType)
        {
            string id = "";
            var candidates = new List<IFunction>(overloads);
            switch (candidates.Count)
            {
                case 0:
                    throw new Exception("Object is not declared");
                case 1:
                    return candidates[0];
                default:
                    id = candidates[0].Identifier;
                    for (int i = 0; i < candidates.Count;)
                    {
                        var decl = candidates[i];
                        // check by arguments count
                        if (decl.Parameters.Count != arguments.Count)
                        {
                            candidates.RemoveAt(i);
                            continue;
                        }
                        // check return type if so
                        if (returnType != null)
                        {
                            var funcDecl = decl as IFunction;
                            if (funcDecl == null)
                            {
                                candidates.RemoveAt(i);
                                continue;
                            }
                            else if (!AreTypesCompatible(funcDecl.ReturnType, returnType))
                            {
                                candidates.RemoveAt(i);
                                continue;
                            }
                        }
                        if (!CheckAssociationList(scope, decl.Parameters, arguments))
                        {
                            candidates.RemoveAt(i);
                            continue;
                        }
                        ++i;
                    }
                    switch (candidates.Count)
                    {
                        case 0:
                            throw new vhdlNoMatchSubprogramException(id, "function");
                        case 1:
                            return candidates[0];
                        case 2:
                            {
                                // TODO: link body with declaration
                                bool firstIsBody = candidates[0] is SubprogramBody;
                                bool secndIsBody = candidates[1] is SubprogramBody;
                                if (firstIsBody ^ secndIsBody)
                                    return candidates[0];
                            }
                            throw new vhdlAmbiguousCallException(id, "function");
                        default:
                            throw new vhdlAmbiguousCallException(id, "function"); // TODO: add location to exception
                    }
            }
        }

        public static ISubtypeIndication Infer(IDeclarativeRegion scope, Expression expr, ISubtypeIndication expectedType = null)
        {
            ISubtypeIndication baseType = TypeHelper.GetBaseType(expectedType);
            TypeInference baseInfer = new TypeInference(baseType);
            ExpressionInference infer = new ExpressionInference(scope, baseInfer);
            expr.accept(infer);
            return baseInfer.ResultType;
        }

        public static bool AreTypesCompatible(ISubtypeIndication left, ISubtypeIndication right)
        {
            // TODO: some types can be converted to other
            return AreTypesEqual(TypeHelper.GetBaseType(left), TypeHelper.GetBaseType(right));
        }

        public static bool AreTypesEqual(ISubtypeIndication left, ISubtypeIndication right)
        {
            string leftTypeName = TypeHelper.GetTypeName(TypeHelper.GetBaseType(left));
            string rightTypeName = TypeHelper.GetTypeName(TypeHelper.GetBaseType(right));
            return leftTypeName != "" && leftTypeName.EqualsIgnoreCase(rightTypeName);
        }

        private static bool CheckAssociationList(IDeclarativeRegion scope, IList<IVhdlObjectProvider> formals,
            List<AssociationElement> actuals)
        {
            // check by type of each argument
            for (int j = 0; j < formals.Count; ++j)
            {
                var param = formals[j];
                var actual = actuals[j].Actual;
                var actualType = Infer(scope, actual, param.VhdlObjects[0].Type);
                if (actualType == null)
                    return false;
            }
            return true;
        }

        public bool AnalyzeType(ISubtypeIndication type, bool strict = true)
        {
            bool success = strict ? AreTypesEqual(type, ExpectedType)
                : AreTypesCompatible(type, ExpectedType);
            if (success)
                ResultType = type;
            return success;
        }
    }
}
