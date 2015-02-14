using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.declaration;
using VHDL.libraryunit;
using VHDL;
using VHDL.parser;

namespace VHDLModelingSystem
{
    //                  ***Operators***
    //**   exponentiation,  numeric ** integer,  result numeric
    //abs  absolute value,  abs numeric,  result numeric
    //not  complement,      not logic or boolean,  result same

    //*    multiplication,  numeric * numeric,  result numeric
    ///    division,        numeric / numeric,  result numeric
    //mod  modulo,          integer mod integer,  result integer
    //rem  remainder,       integer rem integer,  result integer

    //+    unary plus,      + numeric,  result numeric
    //-    unary minus,     - numeric,  result numeric

    //+    addition,        numeric + numeric,  result numeric
    //-    subtraction,     numeric - numeric,  result numeric
    //&    concatenation,   array or element & array or element,
    //                        result array

    //sll  shift left logical,     logical array sll integer,  result same
    //srl  shift right logical,    logical array srl integer,  result same
    //sla  shift left arithmetic,  logical array sla integer,  result same
    //sra  shift right arithmetic, logical array sra integer,  result same
    //rol  rotate left,            logical array rol integer,  result same
    //ror  rotate right,           logical array ror integer,  result same

    //=    test for equality, result is boolean
    ///=   test for inequality, result is boolean
    //<    test for less than, result is boolean
    //<=   test for less than or equal, result is boolean
    //>    test for greater than, result is boolean
    //>=   test for greater than or equal, result is boolean

    //and  logical and,                logical array or boolean,  result is same
    //or   logical or,                 logical array or boolean,  result is same
    //nand logical complement of and,  logical array or boolean,  result is same
    //nor  logical complement of or,   logical array or boolean,  result is same
    //xor  logical exclusive or,       logical array or boolean,  result is same
    //xnor logical complement of exclusive or,  logical array or boolean,  result is same


    /// <summary>
    /// Класс, используемый для построения списка
    /// простых выражений, которые можно вычислить
    /// </summary>
    public class FunctionAnalyser
    {
        private List<FunctionBody> functionsAbs;
        public List<FunctionBody> FunctionsAbs
        {
            get { return functionsAbs; }
        }

        private List<FunctionBody> functionsPlus;
        public List<FunctionBody> FunctionsPlus
        {
            get { return functionsPlus; }
        }

        private List<FunctionBody> functionsNot;
        public List<FunctionBody> FunctionsNot
        {
            get { return functionsNot; }
        }

        private List<FunctionBody> functionsMinus;
        public List<FunctionBody> FunctionsMinus
        {
            get { return functionsMinus; }
        }

        private List<FunctionBody> functionsTypeConversion;
        public List<FunctionBody> FunctionsTypeConversion
        {
            get { return functionsTypeConversion; }
        }

        private List<FunctionBody> functionsFunctionCall;
        public List<FunctionBody> FunctionsFunctionCall
        {
            get { return functionsFunctionCall; }
        }

        private List<FunctionBody> functionsAggregate;
        public List<FunctionBody> FunctionsAggregate
        {
            get { return functionsAggregate; }
        }

        private List<FunctionBody> functionsQualifiedExpression;
        public List<FunctionBody> FunctionsQualifiedExpression
        {
            get { return functionsQualifiedExpression; }
        }

        private List<FunctionBody> functionsQualifiedExpressionAllocator;
        public List<FunctionBody> FunctionsQualifiedExpressionAllocator
        {
            get { return functionsQualifiedExpressionAllocator; }
        }

        private List<FunctionBody> functionsParentheses;
        public List<FunctionBody> FunctionsParentheses
        {
            get { return functionsParentheses; }
        }

        private List<FunctionBody> functionsSubtypeIndicationAllocator;
        public List<FunctionBody> FunctionsSubtypeIndicationAllocator
        {
            get { return functionsSubtypeIndicationAllocator; }
        }

        private List<FunctionBody> functionsMod;
        public List<FunctionBody> FunctionsMod
        {
            get { return functionsMod; }
        }

        private List<FunctionBody> functionsRem;
        public List<FunctionBody> FunctionsRem
        {
            get { return functionsRem; }
        }

        private List<FunctionBody> functionsDivide;
        public List<FunctionBody> FunctionsDivide
        {
            get { return functionsDivide; }
        }

        private List<FunctionBody> functionsMultiply;
        public List<FunctionBody> FunctionsMultiply
        {
            get { return functionsMultiply; }
        }

        private List<FunctionBody> functionsPow;
        public List<FunctionBody> FunctionsPow
        {
            get { return functionsPow; }
        }

        private List<FunctionBody> functionsRor;
        public List<FunctionBody> FunctionsRor
        {
            get { return functionsRor; }
        }

        private List<FunctionBody> functionsSla;
        public List<FunctionBody> FunctionsSla
        {
            get { return functionsSla; }
        }

        private List<FunctionBody> functionsSll;
        public List<FunctionBody> FunctionsSll
        {
            get { return functionsSll; }
        }

        private List<FunctionBody> functionsSra;
        public List<FunctionBody> FunctionsSra
        {
            get { return functionsSra; }
        }

        private List<FunctionBody> functionsSrl;
        public List<FunctionBody> FunctionsSrl
        {
            get { return functionsSrl; }
        }

        private List<FunctionBody> functionsRol;
        public List<FunctionBody> FunctionsRol
        {
            get { return functionsRol; }
        }

        private List<FunctionBody> functionsAdd;
        public List<FunctionBody> FunctionsAdd
        {
            get { return functionsAdd; }
        }

        private List<FunctionBody> functionsConcatenate;
        public List<FunctionBody> FunctionsConcatenate
        {
            get { return functionsConcatenate; }
        }

        private List<FunctionBody> functionsSubtract;
        public List<FunctionBody> FunctionsSubtract
        {
            get { return functionsSubtract; }
        }

        private List<FunctionBody> functionsNand;
        public List<FunctionBody> FunctionsNand
        {
            get { return functionsNand; }
        }

        private List<FunctionBody> functionsXnor;
        public List<FunctionBody> FunctionsXnor
        {
            get { return functionsXnor; }
        }

        private List<FunctionBody> functionsNor;
        public List<FunctionBody> FunctionsNor
        {
            get { return functionsNor; }
        }

        private List<FunctionBody> functionsXor;
        public List<FunctionBody> FunctionsXor
        {
            get { return functionsXor; }
        }

        private List<FunctionBody> functionsQua;
        public List<FunctionBody> FunctionsQua
        {
            get { return functionsQua; }
        }

        private List<FunctionBody> functionsAnd;
        public List<FunctionBody> FunctionsAnd
        {
            get { return functionsAnd; }
        }

        private List<FunctionBody> functionsOr;
        public List<FunctionBody> FunctionsOr
        {
            get { return functionsOr; }
        }

        private List<FunctionBody> functionsEquals;
        public List<FunctionBody> FunctionsEquals
        {
            get { return functionsEquals; }
        }

        private List<FunctionBody> functionsGreaterThan;
        public List<FunctionBody> FunctionsGreaterThan
        {
            get { return functionsGreaterThan; }
        }

        private List<FunctionBody> functionsLessEquals;
        public List<FunctionBody> FunctionsLessEquals
        {
            get { return functionsLessEquals; }
        }

        private List<FunctionBody> functionsLessThan;
        public List<FunctionBody> FunctionsLessThan
        {
            get { return functionsLessThan; }
        }

        private List<FunctionBody> functionsNotEquals;
        public List<FunctionBody> FunctionsNotEquals
        {
            get { return functionsNotEquals; }
        }

        private List<FunctionBody> functionsGreaterEquals;
        public List<FunctionBody> FunctionsGreaterEquals
        {
            get { return functionsGreaterEquals; }
        }

        private List<FunctionBody> functionsResolved;
        public List<FunctionBody> FunctionsResolved
        {
            get { return functionsResolved; }
        }

        private List<FunctionBody> functions;
        public List<FunctionBody> Functions
        {
            get { return functions; }
        }

        private readonly IScope scope;
        public FunctionAnalyser(IScope scope)
        {
            this.scope = scope;
            functionsAbs = new List<FunctionBody>();
            functionsAdd = new List<FunctionBody>();
            functionsAnd = new List<FunctionBody>();
            functionsAggregate = new List<FunctionBody>();
            functionsConcatenate = new List<FunctionBody>();
            functionsDivide = new List<FunctionBody>();
            functionsEquals = new List<FunctionBody>();
            functionsFunctionCall = new List<FunctionBody>();
            functionsGreaterEquals = new List<FunctionBody>();
            functionsGreaterThan = new List<FunctionBody>();
            functionsLessEquals = new List<FunctionBody>();
            functionsLessThan = new List<FunctionBody>();
            functionsMinus = new List<FunctionBody>();
            functionsMod = new List<FunctionBody>();
            functionsMultiply = new List<FunctionBody>();
            functionsNand = new List<FunctionBody>();
            functionsNor = new List<FunctionBody>();
            functionsNot = new List<FunctionBody>();
            functionsNotEquals = new List<FunctionBody>();
            functionsOr = new List<FunctionBody>();
            functionsParentheses = new List<FunctionBody>();
            functionsPlus = new List<FunctionBody>();
            functionsPow = new List<FunctionBody>();
            functionsQua = new List<FunctionBody>();
            functionsQualifiedExpression = new List<FunctionBody>();
            functionsQualifiedExpressionAllocator = new List<FunctionBody>();
            functionsRem = new List<FunctionBody>();
            functionsRol = new List<FunctionBody>();
            functionsRor = new List<FunctionBody>();
            functionsSla = new List<FunctionBody>();
            functionsSll = new List<FunctionBody>();
            functionsSra = new List<FunctionBody>();
            functionsSrl = new List<FunctionBody>();
            functionsSubtract = new List<FunctionBody>();
            functionsSubtypeIndicationAllocator = new List<FunctionBody>();
            functionsTypeConversion = new List<FunctionBody>();
            functionsXnor = new List<FunctionBody>();
            functionsXor = new List<FunctionBody>();
            functions = new List<FunctionBody>();
            functionsResolved = new List<FunctionBody>();

            LoadFunctionBodies();
        }

        /// <summary>
        /// Подгрузка тел функций
        /// </summary>
        private void LoadFunctionBodies()
        {
            AnalyseUnits(scope.GetListOfObjects<FunctionBody>());
            foreach (PackageDeclaration decl in scope.GetListOfObjects<PackageDeclaration>())
                AnalysePackageDeclaration(decl);
        }

        /// <summary>
        /// Анализ набора тел функций
        /// </summary>
        /// <param name="objects"></param>
        private void AnalyseUnits(List<FunctionBody> objects)
        {
            foreach (FunctionBody decl in objects)
            {
                switch (decl.Identifier.ToLower())
                {
                    case "\"**\"" : functionsPow.Add(decl); break;
                    case "\"abs\"": functionsAbs.Add(decl); break;
                    case "\"not\"": functionsNot.Add(decl); break;

                    case "\"*\"": functionsMultiply.Add(decl); break;
                    case "\"/\"": functionsDivide.Add(decl); break;
                    case "\"mod\"": functionsMod.Add(decl); break;
                    case "\"rem\"": functionsRem.Add(decl); break;

                    case "\"+\"": functionsPlus.Add(decl); break;
                    case "\"-\"": functionsMinus.Add(decl); break;
                    case "\"&\"": functionsConcatenate.Add(decl); break;

                    case "\"sll\"": functionsSll.Add(decl); break;
                    case "\"srl\"": functionsSrl.Add(decl); break;
                    case "\"sla\"": functionsSla.Add(decl); break;
                    case "\"sra\"": functionsSra.Add(decl); break;
                    case "\"rol\"": functionsRol.Add(decl); break;
                    case "\"ror\"": functionsRor.Add(decl); break;

                    case "\"=\"": functionsEquals.Add(decl); break;
                    case "\"/=\"": functionsNotEquals.Add(decl); break;
                    case "\"\"<\"": functionsLessThan.Add(decl); break;
                    case "\"<=\"": functionsLessEquals.Add(decl); break;
                    case "\">\"": functionsGreaterThan.Add(decl); break;
                    case "\">=\"": functionsGreaterEquals.Add(decl); break;

                    case "\"and\"": functionsAnd.Add(decl); break;
                    case "\"or\"": functionsOr.Add(decl); break;
                    case "\"nand\"": functionsNand.Add(decl); break;
                    case "\"nor\"": functionsNor.Add(decl); break;
                    case "\"xor\"": functionsXor.Add(decl); break;
                    case "\"xnor\"": functionsXnor.Add(decl); break;
                    case "resolved": functionsResolved.Add(decl); break;

                    default: functions.Add(decl); break;
                }
            }
        }

        private void AnalysePackageDeclaration(PackageDeclaration decl)
        {
            //PackageBody packBody = VHDL_Library_Manager.GetPackageBody(decl);
            //AnalyseUnits(packBody.Scope.GetListOfObjects<FunctionBody>());            
        }
    }
}