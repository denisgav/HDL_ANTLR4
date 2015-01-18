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
using System.Linq;
using System.Text;

namespace VHDLRuntime
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
