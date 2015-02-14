using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using System.ComponentModel;


namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// класс, который хранит данные об выделенном сигнале
    /// </summary>
    public class Selection :INotifyPropertyChanged
    {
        /// <summary>
        /// Первая координата
        /// </summary>
        private UInt64 x1;
        public UInt64 X1
        {
            get { return x1; }
            set 
            { 
                x1 = value;
                NotifyPropertyChanged("X1");
            }
        }

        /// <summary>
        /// Вторая координата
        /// </summary>
        private UInt64 x2;
        public UInt64 X2
        {
            get { return x2; }
            set 
            {
                x2 = value;
                NotifyPropertyChanged("X2");
            }
        }

        /// <summary>
        /// Длина выделенного временного интервала
        /// </summary>
        public UInt64 TimeInterval
        {
            get { return (UInt64) Math.Abs((Int64)x2 - (Int64)x1); }
        }

        /// <summary>
        /// Начальное время
        /// </summary>
        public UInt64 Start
        {
            get { return Math.Min(x2, x1); }
        }

        /// <summary>
        /// Конечное время
        /// </summary>
        public UInt64 End
        {
            get { return Math.Max(x2, x1); }
        }

        /// <summary>
        /// Переменная, которую выделяют
        /// </summary>
        private My_Variable variable;
        public My_Variable Variable
        {
            get { return variable; }
            set { variable = value; }
        }

        public Selection(UInt64 start, UInt64 end, My_Variable variable)
        {
            this.x1 = start;
            this.x2 = end;
            this.variable = variable;
        }

        public Selection()
        {
            x1 = 0;
            x2 = 0;
            variable = null;
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
