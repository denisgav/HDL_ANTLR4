using VHDL.type;
using VHDL.expression;
using VHDL.declaration;
using AssociationElement = VHDL.AssociationElement;
using System.Collections.Generic;
using Exception = System.Exception;
using VHDL.util;
using VHDL.Object;
using VHDL;

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

        public static VHDL.declaration.ProcedureDeclaration ResolveOverloadProcedure(IDeclarativeRegion scope, List<VHDL.declaration.ProcedureDeclaration> overloads_in,
            List<AssociationElement> arguments)
        {
            List<VHDL.declaration.ProcedureDeclaration> overloads = new List<VHDL.declaration.ProcedureDeclaration>(overloads_in);
            switch (overloads.Count)
            {
                case 0:
                    throw new Exception("Object is not declared");
                case 1:
                    return overloads[0];
                default:
                    for (int i = 0; i < overloads.Count; )
                    {
                        var decl = overloads[i];
                        // check by arguments count
                        if (decl.Parameters.Count != arguments.Count)
                        {
                            overloads.RemoveAt(i);
                            continue;
                        }                        
                        if (!CheckAssociationList(scope, decl.Parameters, arguments))
                        {
                            overloads.RemoveAt(i);
                            continue;
                        }
                        ++i;
                    }
                    switch (overloads.Count)
                    {
                        case 0:
                            throw new Exception("None of overloads matches procedure call");
                        case 1:
                            return overloads[0];
                        default:
                            throw new Exception("Ambiguous call");
                    }
            }
        }

        public static VHDL.declaration.FunctionDeclaration ResolveOverloadFunction(IDeclarativeRegion scope, List<VHDL.declaration.FunctionDeclaration> overloads_in,
            List<AssociationElement> arguments, ISubtypeIndication returnType)
        {
            List<VHDL.declaration.FunctionDeclaration> overloads = new List<VHDL.declaration.FunctionDeclaration>(overloads_in);
            switch (overloads.Count)
            {
                case 0:
                    throw new Exception("Object is not declared");
                case 1:
                    return overloads[0];
                default:
                    for (int i = 0; i < overloads.Count;)
                    {
                        var decl = overloads[i];
                        // check by arguments count
                        if (decl.Parameters.Count != arguments.Count)
                        {
                            overloads.RemoveAt(i);
                            continue;
                        }
                        // check return type if so
                        if (returnType != null)
                        {
                            var funcDecl = decl as IFunction;
                            if (funcDecl == null)
                            {
                                overloads.RemoveAt(i);
                                continue;
                            }
                            else if (!AreTypesCompatible(funcDecl.ReturnType, returnType))
                            {
                                overloads.RemoveAt(i);
                                continue;
                            }
                        }
                        if (!CheckAssociationList(scope, decl.Parameters, arguments))
                        {
                            overloads.RemoveAt(i);
                            continue;
                        }
                        ++i;
                    }
                    switch (overloads.Count)
                    {
                        case 0:
                            throw new Exception("None of overloads matches function call");
                        case 1:
                            return overloads[0];
                        default:
                            throw new Exception("Ambiguous call");
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
            string leftTypeName = TypeHelper.GetTypeName(TypeHelper.GetBaseType(left));
            string rightTypeName = TypeHelper.GetTypeName(TypeHelper.GetBaseType(right));
            return leftTypeName != "" && leftTypeName == rightTypeName;
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
    }
}
