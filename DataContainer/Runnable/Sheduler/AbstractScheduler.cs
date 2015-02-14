using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDLModelingSystem
{
    public abstract class AbstractScheduler : IStartable
    {
        protected List<ConcurrentStatementRunner> statementRunners;

        protected Dictionary<DataContainer.Objects.Signal, DataContainer.MySortedDictionary.IValueIterator> iterators;

        public void Start()
        {
            foreach (ConcurrentStatementRunner st in statementRunners)
            {
                StartConcurentStatement(st);
            }
            EndOfCycle();

            //Запускаем дельта-циклы
            while (true)
            {
                //Проверяем, есть ли ещо данные для выборки
                bool IsEndOfData = true;
                foreach (var i in iterators)
                    if (i.Value.IsEndOfIteration == false)
                    {
                        IsEndOfData = false;
                        break;
                    }
                if (IsEndOfData == true)
                    break;

                StartNextDeltaCycle();
            }
            foreach (var i in iterators)
                i.Value.Reset();
        }

        public void StartNextDeltaCycle()
        {
            UInt64 CurrentTime = CurrentVirtualTime;

            //выбираем первое событие (точнее минимальное время)
            CurrentTime = UInt64.MaxValue;
            foreach (var i in iterators)
            {
                if ((i.Value.IsEndOfIteration == false) && (i.Value.LastEvent < CurrentTime))
                    CurrentTime = i.Value.LastEvent;
            }

            //Выписываем время
            CurrentVirtualTime = CurrentTime;

            //передвигаем курсоры
            foreach (var i in iterators)
            {
                if (i.Value.LastEvent == CurrentTime)
                {
                    //Фиксируем результат
                    i.Value.MoveNext();
                    i.Key.CurrentValue = i.Value.CurrentValue.LastValue;
                    //Запускаем все процессы, у которых variable находится в списке чувствительности
                    foreach (ConcurrentStatementRunner runner in statementRunners)
                    {
                        if (runner.SensitivityList.Contains(i.Key))
                        {
                            StartConcurentStatement(runner);
                        }
                    }
                }
            }
            EndOfCycle();
        }

        /// <summary>
        /// Запуск на выполнение паралельного процесса
        /// </summary>
        /// <param name="runner"></param>
        protected abstract void StartConcurentStatement(ConcurrentStatementRunner runner);

        /// <summary>
        /// Обновление текущего модельного времени
        /// </summary>
        /// <param name="Time"></param>
        protected abstract UInt64 CurrentVirtualTime { get; set; }

        /// <summary>
        /// Конец дельта цикла
        /// </summary>
        protected virtual void EndOfCycle()
        {
            foreach (var i in iterators)
            {
                i.Value.UpdateLastEvent();
            }
        }
    }
}
