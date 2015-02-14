using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL;
using DataContainer.Objects;
using DataContainer;

namespace VHDLModelingSystem
{
    public class ConcurrentStatementRunner: IStartable
    {
        ExpressionEvaluator DefaultEvaluator;
        private IValueProviderContainer valueProviderContainer;
        public IValueProviderContainer ValueProviderContainer
        {
            get { return valueProviderContainer; }
            set { valueProviderContainer = value; }
        }

        private VHDL.concurrent.ConcurrentStatement statement;
        public VHDL.concurrent.ConcurrentStatement Statement
        {
            get { return statement; }
            set { statement = value; }
        }

        /// <summary>
        /// Список чувствительности
        /// </summary>
        private List<DataContainer.Objects.Signal> sensitivityList;
        public List<DataContainer.Objects.Signal> SensitivityList
        {
            get { return sensitivityList; }
            set { sensitivityList = value; }
        }


        public ConcurrentStatementRunner(VHDL.concurrent.ConcurrentStatement statement, IValueProviderContainer valueProviderContainer)
        {
            DefaultEvaluator = new ExpressionEvaluator(valueProviderContainer);
            this.valueProviderContainer = valueProviderContainer;
            this.statement = statement;
            FormSensitivityList();
        }

        /// <summary>
        /// Сформировать список чувствительности
        /// </summary>
        private void FormSensitivityList()
        {
            if (statement is VHDL.concurrent.ConditionalSignalAssignment)
            {
                FormSensitivityListConditionalSignaAsignment();
                return;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сформировать список чувствительности для параллельного условного оператора назначения сигнала
        /// </summary>
        private void FormSensitivityListConditionalSignaAsignment()
        {
            VHDL.concurrent.ConditionalSignalAssignment assignment = statement as VHDL.concurrent.ConditionalSignalAssignment;
            sensitivityList = new List<DataContainer.Objects.Signal>();
            List<VHDL.Object.Signal> signals = new List<VHDL.Object.Signal>();
            foreach (VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement conditionalWaveformElement in assignment.ConditionalWaveforms)
            {
                signals.AddRange(SensitivityListResolver.GetSensitivityList(conditionalWaveformElement.Condition));

                foreach (WaveformElement el in conditionalWaveformElement.Waveform)
                {
                    signals.AddRange(SensitivityListResolver.GetSensitivityList(el.Value));
                }
            }
            foreach (VHDL.Object.Signal s in signals.Distinct())
                if (s != null)
                    sensitivityList.Add(valueProviderContainer.GetValueProvider(s) as Signal);
        }

        /// <summary>
        /// Запустить на выполнение
        /// </summary>
        public void Start()
        {
            if (statement is VHDL.concurrent.ConditionalSignalAssignment)
            {
                StartConditionalSignalAsignment();
                return;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Запустить на выполнение
        /// </summary>
        private void StartConditionalSignalAsignment()
        {
            VHDL.concurrent.ConditionalSignalAssignment assignment = statement as VHDL.concurrent.ConditionalSignalAssignment;
            DataContainer.Objects.Signal target = valueProviderContainer.GetValueProvider((assignment.Target as VHDL.Object.RecordElement).getPrefix() as VHDL.Object.Signal) as DataContainer.Objects.Signal;

            foreach (VHDL.concurrent.ConditionalSignalAssignment.ConditionalWaveformElement conditionalWaveformElement in assignment.ConditionalWaveforms)
            {
                VHDL.expression.Expression condition = conditionalWaveformElement.Condition;
                bool isValidCondition = false;
                if (condition != null)
                {
                    DataContainer.Value.AbstractValue conditionResult = DefaultEvaluator.Evaluate(condition);
                    if (conditionResult is DataContainer.Value.BOOLEAN_VALUE)
                    {
                        if ((conditionResult as DataContainer.Value.BOOLEAN_VALUE).Value == VHDL.builtin.Standard.BOOLEAN_TRUE)
                        {
                            isValidCondition = true;
                        }
                    }
                }
                else
                    isValidCondition = true;

                if(isValidCondition == true)
                {

                    if (assignment.DelayMechanism is TRANSPORTDelayMechanism)
                    {
                        foreach (WaveformElement el in conditionalWaveformElement.Waveform)
                        {
                            DataContainer.Value.TIME_VALUE after;
                            if (el.After != null)
                                after = DataContainer.ExpressionEvaluator.DefaultEvaluator.Evaluate(el.After) as DataContainer.Value.TIME_VALUE;
                            else
                                after = new DataContainer.Value.TIME_VALUE(0);
                            DataContainer.Value.AbstractValue value = DefaultEvaluator.Evaluate(el.Value);
                            target.AddEvent((UInt64)valueProviderContainer.NOW.DoubleValue, (UInt64)after.DoubleValue, value, assignment.DelayMechanism);
                        }
                    }
                    else
                    {
                        List<WaveformElement> waveforms = conditionalWaveformElement.Waveform;
                        DataContainer.Value.TIME_VALUE after;
                        if (waveforms[0].After != null)
                            after = DefaultEvaluator.Evaluate(waveforms[0].After) as DataContainer.Value.TIME_VALUE;
                        else
                            after = new DataContainer.Value.TIME_VALUE(0);
                        DataContainer.Value.AbstractValue value = DefaultEvaluator.Evaluate(waveforms[0].Value);
                        target.AddEvent((UInt64)valueProviderContainer.NOW.DoubleValue, (UInt64)after.DoubleValue, value, assignment.DelayMechanism);

                        for (int i = 1; i < waveforms.Count; i++)
                        {
                            if (waveforms[i].After != null)
                                after = DefaultEvaluator.Evaluate(waveforms[i].After) as DataContainer.Value.TIME_VALUE;
                            else
                                after = new DataContainer.Value.TIME_VALUE(0);
                            value = DefaultEvaluator.Evaluate(waveforms[i].Value);
                            target.AddEvent((UInt64)valueProviderContainer.NOW.DoubleValue, (UInt64)after.DoubleValue, value, assignment.DelayMechanism);
                        }
                    }
                    return;
                }
            }
        }
    }
}
