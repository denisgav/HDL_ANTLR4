using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.TypeConstraint;
using VHDL.type;
using VHDL;
using DataContainer.Value;
using DataContainer.Generator;

namespace DataContainer
{
    /// <summary>
    /// Класс, исползуемый для хранения информации о типе данных
    /// </summary>
    [System.Serializable]
    public class ModellingType
    {
        /// <summary>
        /// Тип значения
        /// </summary>
        private readonly Type type;
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// Ограничения, накладываемые на тип данных
        /// </summary>
        private List<AbstractConstraint> constraints;
        public List<AbstractConstraint> Constraints
        {
            get { return constraints; }
        }

        /// <summary>
        /// Имя типа данных
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
        }

        private ModellingType(Type type, List<AbstractConstraint> constraints, ResolvedDiscreteRange[] dimension)
        {
            this.dimension = dimension;
            this.type = type;
            this.constraints = constraints;
            this.name = type.Identifier;
        }

        private ModellingType(Type type, List<AbstractConstraint> constraints)
        {
            this.type = type;
            this.constraints = constraints;
            this.dimension = GetDimension();
            this.resolvedRange = GetResolvedRange();
            this.name = type.Identifier;
        }

        private ModellingType(Type type)
            :this(type, new List<AbstractConstraint>())
        {
        }

        private ResolvedDiscreteRange[] resolvedRange;
        public ResolvedDiscreteRange[] ResolvedRange
        {
            get { return resolvedRange; }
        }

        private ResolvedDiscreteRange[] GetResolvedRange()
        {
            if (type is EnumerationType)
            {
                int count = (type as EnumerationType).Literals.Count;
                return new ResolvedDiscreteRange[] { ResolvedDiscreteRange.FormIntegerIndexes(1, count) };
            }
            return new ResolvedDiscreteRange[] { ResolvedDiscreteRange.Range1 };
        }

        private ResolvedDiscreteRange[] dimension;
        public ResolvedDiscreteRange[] Dimension
        {
            get { return dimension; }
        }

        public int SizeOf
        {
            get 
            {
                int res = 1;
                foreach (var v in dimension)
                    res *= v.Length;
                return res;
            }
        }

        private ResolvedDiscreteRange[] GetDimension()
        {
            if (type is ConstrainedArray)
            {
                List<ResolvedDiscreteRange> res = new List<ResolvedDiscreteRange>();
                foreach(DiscreteRange range in (type as ConstrainedArray).IndexRanges)
                    res.AddRange(ExpressionEvaluator.DefaultEvaluator.ResolveRange(range));
                return res.ToArray();
            }
            if (type is UnconstrainedArray)
            {
                List<ResolvedDiscreteRange> res = new List<ResolvedDiscreteRange>();
                foreach (DiscreteRange range in (type as ConstrainedArray).IndexRanges)
                    res.AddRange(ExpressionEvaluator.DefaultEvaluator.ResolveRange(range));
                return res.ToArray();
            }
            return new ResolvedDiscreteRange[]{ResolvedDiscreteRange.Range1};
        }

        /// <summary>
        /// Получение настроек генератора по заданному модельному типу данных
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GeneratorSettings GetGeneratorSettings(ModellingType type)
        {
            if (type.type is IntegerType)
            {
                return new GeneratorSettings(32, GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, GeneratorSettings.GeneratedValue.IntegerValue, new List<object>());
            }
            if (type.type is RealType)
            {
                return new GeneratorSettings(32, GeneratorSettings.GeneratedValue.DoubleValue, GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, new List<object>());
            }
            if (type.type is EnumerationType)
            {
                return new GeneratorSettings(1, GeneratorSettings.GeneratedValue.EnumerableValue, GeneratorSettings.GeneratedValue.EnumerableValue, new List<object>((type.type as EnumerationType).Literals));
            }
            if ((type.type == VHDL.builtin.StdLogic1164.STD_LOGIC_VECTOR) || (type.type == VHDL.builtin.StdLogic1164.STD_ULOGIC_VECTOR) || (type.type == VHDL.builtin.Standard.BIT_VECTOR))
            {
                return new GeneratorSettings((uint)type.Dimension[0].Length, GeneratorSettings.GeneratedValue.IntegerValue | GeneratorSettings.GeneratedValue.DoubleValue | GeneratorSettings.GeneratedValue.BoolArray, GeneratorSettings.GeneratedValue.BoolArray);
            }
            return null;
        }

        /// <summary>
        /// Создание ModellingType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ModellingType CreateModellingType(ArrayType type, List<AbstractConstraint> constraints, ResolvedDiscreteRange[] dimension)
        {
            foreach (ModellingType t in CreatedTypes)
                if ((t.type == type) && (ArraysEqual(t.constraints, constraints)) && (ArraysEqual(t.dimension, dimension)))
                    return t;
            ModellingType res = new ModellingType(type, constraints, dimension);
            CreatedTypes.Add(res);
            return res;
        }

        /// <summary>
        /// Создание ModellingType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static ModellingType CreateModellingType(Type type, List<AbstractConstraint> constraints)
        {
            foreach (ModellingType t in CreatedTypes)
                if ((t.type == type) && (ArraysEqual(t.constraints, constraints)))
                    return t;
            ModellingType res = new ModellingType(type, constraints);
            CreatedTypes.Add(res);
            return res;
        }

        /// <summary>
        /// Создание ModellingType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resolvedDiscreteRange"></param>
        /// <returns></returns>
        public static ModellingType CreateModellingType(ArrayType type, ResolvedDiscreteRange[] resolvedDiscreteRange)
        {
            return CreateModellingType(type, new List<AbstractConstraint>(), resolvedDiscreteRange);
        }

        /// <summary>
        /// Создание ModellingType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ModellingType CreateModellingType(Type type)
        {
            foreach (ModellingType t in CreatedTypes)
                if (t.type == type)
                    return t;
            ModellingType res = new ModellingType(type);
            CreatedTypes.Add(res);
            return res;
        }

        /// <summary>
        /// Уже созданные типы данных
        /// </summary>
        private static List<ModellingType> CreatedTypes = new List<ModellingType>();

        /// <summary>
        /// Сравнение 2-х массивов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Сравнение 2-х массивов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static bool ArraysEqual<T>(IList<T> a1, IList<T> a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Count != a2.Count)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Count; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }
    }
}
