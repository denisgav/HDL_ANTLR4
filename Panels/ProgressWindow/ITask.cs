using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.UserControls
{
    public interface ITask
    {
        /// <summary>
        /// Запуск на выполнение
        /// </summary>
        void Start();
        /// <summary>
        /// Процент выполненого задания
        /// </summary>
        /// <returns></returns>
        int PercentComplete
        { get; }
        /// <summary>
        /// Имя задания
        /// </summary>
        string Name
        { get; }

        /// <summary>
        /// Можно ли определить процент выполненой работы
        /// </summary>
        bool IsIndeterminate
        { get; }

        /// <summary>
        /// Завершились ли вычисления
        /// </summary>
        bool IsComplete
        { get; }

        /// <summary>
        /// Что делать по отмене задания
        /// </summary>
        void OnCancel();
    }
}
