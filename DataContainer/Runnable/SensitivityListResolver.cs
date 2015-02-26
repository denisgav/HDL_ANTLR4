using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.Object;
using VHDL.expression;

namespace VHDLModelingSystem
{
    public static class SensitivityListResolver
    {
        public static List<Signal> GetSensitivityList(BinaryExpression expr)
        {
            List<Signal> res = new List<Signal>();
            res.AddRange(GetSensitivityList(expr.Left));
            res.AddRange(GetSensitivityList(expr.Right));
            return res;
        }

        public static List<Signal> GetSensitivityList(UnaryExpression expr)
        {
            List<Signal> res = new List<Signal>();
            res.AddRange(GetSensitivityList(expr.Expression));
            return res;
        }



        public static List<Signal> GetSensitivityList(TypeConversion expr)
        {
            return GetSensitivityList(expr.Expression);
        }

        public static List<Signal> GetSensitivityList(FunctionCall expr)
        {
            List<Signal> res = new List<Signal>();
            foreach (VHDL.AssociationElement el in expr.Parameters)
            {
                res.AddRange(GetSensitivityList(el.Actual));
            }
            return res;
        }

        public static List<Signal> GetSensitivityList(QualifiedExpression expr)
        {
            return GetSensitivityList(expr.Operand);
        }
        /*
        public static List<Signal> GetSensitivityList(RecordElement expr)
        {
            return new List<Signal>() { expr.getPrefix() as Signal };
        }
        */
        public static List<Signal> GetSensitivityList(Parentheses expr)
        {
            return GetSensitivityList(expr.Expression);
        }

        public static List<Signal> GetSensitivityList(Aggregate expr)
        {
            List<Signal> res = new List<Signal>();
            foreach (Aggregate.ElementAssociation el in expr.Associations)
            {
                res.AddRange(GetSensitivityList(el.Expression));
            }
            return res;
        }

        public static List<Signal> GetSensitivityList(Expression expr)
        {
            if (expr is BinaryExpression)
                return GetSensitivityList(expr as BinaryExpression);
            if (expr is UnaryExpression)
                return GetSensitivityList(expr as UnaryExpression);
            if (expr is TypeConversion)
                return GetSensitivityList(expr as TypeConversion);
            if (expr is FunctionCall)
                return GetSensitivityList(expr as FunctionCall);
            if (expr is QualifiedExpression)
                return GetSensitivityList(expr as QualifiedExpression);
            /*if (expr is SelectedName)
                return GetSensitivityList(expr as SelectedName);*/
            if (expr is Parentheses)
                return GetSensitivityList(expr as Parentheses);
            if (expr is Aggregate)
                return GetSensitivityList(expr as Aggregate);
            return new List<Signal>();
        }
    }
}
