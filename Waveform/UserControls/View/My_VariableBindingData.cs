using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using Schematix.Waveform.UserControls;
using System.ComponentModel;

namespace Schematix.Waveform
{
    /// <summary>
    /// Класс, который используется во время привязки данных к LisiViewSignals
    /// </summary>
    public class My_VariableBindingData : INotifyPropertyChanged
    {
        /// <summary>
        /// Отображаемая переменная
        /// </summary>
        private My_Variable variable;
        public My_Variable Variable
        {
            get { return variable; }
        }

        /// <summary>
        /// Имя, которое используется при привязке данных
        /// </summary>
        public string Name
        {
            get 
            {
                if (variable.IsEditableName == true)
                    return variable.Name;
                else
                    return variable.FullName;
            }
            set
            {
                if (variable.IsEditableName == true)
                    variable.Name = value;
                else
                    throw new Exception("Variable is Write proptected");
            }
        }
        

        /// <summary>
        /// Ядро системы
        /// </summary>
        private WaveformCore core;
        public WaveformCore Core
        {
            get { return core; }
        }

        /// <summary>
        /// Класс, отвечающий за масштабирование
        /// </summary>
        private ScaleManager scaleManager;
        public ScaleManager ScaleManager
        {
            get { return scaleManager; }
        }

        /// <summary>
        /// Класс, отвечающий за работу с курсором
        /// </summary>
        private CursorViewer cursorViewer;
        public CursorViewer CursorViewer
        {
            get { return cursorViewer; }
        }

        public My_VariableBindingData(My_Variable variable, WaveformCore core, ScaleManager scaleManager, CursorViewer cursorViewer)
        {
            this.variable = variable;
            this.cursorViewer = cursorViewer;
            this.core = core;
            this.scaleManager = scaleManager;

            //Реакция на изменение переменной
            variable.PropertyChanged += new PropertyChangedEventHandler(variable_PropertyChanged);
        }

        void variable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Name");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
