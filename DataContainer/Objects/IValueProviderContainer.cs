using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.Objects
{
    /// <summary>
    /// Позволяет получить объект IValueProvider по его имени или описанию
    /// </summary>
    public interface IValueProviderContainer
    {
        /// <summary>
        /// Получение IValueProvider по его описанию
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        IValueProvider GetValueProvider(VHDL.Object.VhdlObject o);

        /// <summary>
        /// Получение IValueProvider по его имени
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        IValueProvider GetValueProvider(string identifier);

        /// <summary>
        /// Получение текущего времени
        /// </summary>
        TIME_VALUE NOW { get; set; }
    }
}
