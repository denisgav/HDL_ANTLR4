using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Schematix.DesignBrowser
{
    public static class DesignChildProvider
    {
        /// <summary>
        /// Получение дочерних элементов от корневого элемента
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.RootDeclarativeRegion root)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(root.Libraries);
            return res;
        }

        /// <summary>
        /// Получение дочерних элементов entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.libraryunit.Entity entity)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(entity.Architectures);
            res.AddRange(FilterDesignElements(entity.Declarations));
            return res;
        }

        /// <summary>
        /// Получение дочерних элементов архитектуры
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.libraryunit.Architecture arch)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(FilterDesignElements(arch.Declarations));
            res.AddRange(FilterDesignElements(arch.Statements));
            return res;
        }

        /// <summary>
        /// Получение дочерних элементов архитектуры
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.libraryunit.PackageDeclaration pd)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(FilterDesignElements(pd.Declarations));
            return res;
        }

        /// <summary>
        /// Получение дочерних элементов архитектуры
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.libraryunit.PackageBody pb)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(FilterDesignElements(pb.Declarations));
            return res;
        }

        /// <summary>
        /// Получение дочерних элементов библиотеки
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.LibraryDeclarativeRegion lib)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();

            foreach (VHDL.VhdlFile file in lib.Files)
                res.AddRange(FilterDesignElements(GetSubElements(file)));

            return res;
        }

        /// <summary>
        /// Получение дочерних элементов библиотеки
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.VhdlFile file)
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            res.AddRange(file.Elements);
            return res;
        }

        /// <summary>
        /// Обобщенный метод получения дочерних элементов
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IList<VHDL.VhdlElement> GetSubElements(VHDL.VhdlElement element)
        {
            if (element is VHDL.RootDeclarativeRegion)
                return GetSubElements(element as VHDL.RootDeclarativeRegion);

            if (element is VHDL.VhdlFile)
                return GetSubElements(element as VHDL.VhdlFile);

            if (element is VHDL.LibraryDeclarativeRegion)
                return GetSubElements(element as VHDL.LibraryDeclarativeRegion);

            if (element is VHDL.libraryunit.Entity)
                return GetSubElements(element as VHDL.libraryunit.Entity);

            if (element is VHDL.libraryunit.Architecture)
                return GetSubElements(element as VHDL.libraryunit.Architecture);

            if (element is VHDL.libraryunit.PackageDeclaration)
                return GetSubElements(element as VHDL.libraryunit.PackageDeclaration);

            return new List<VHDL.VhdlElement>();
        }
        

        /// <summary>
        /// фильтрование дочерних элементов с сортировкой
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        private static IList<VHDL.VhdlElement> FilterDesignElements<T>(IList<T> elements) where T:class
        {
            List<VHDL.VhdlElement> res = new List<VHDL.VhdlElement>();
            foreach (T el in elements)
            {
                if (
                    (el is VHDL.libraryunit.Entity) ||
                    (el is VHDL.libraryunit.PackageDeclaration) ||
                    (el is VHDL.declaration.SubprogramDeclaration) ||
                    (el is VHDL.declaration.SubprogramBody) ||
                    (el is VHDL.declaration.ProcedureDeclaration) ||
                    (el is VHDL.declaration.ProcedureBody) ||
                    (el is VHDL.concurrent.ProcessStatement)
                    )
                    res.Add(el as VHDL.VhdlElement);
            }
            res = res.OrderBy(x => x.GetType().Name).ToList();
            return res;
        }
    }
}
