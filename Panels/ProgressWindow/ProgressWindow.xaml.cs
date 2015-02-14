using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace Schematix.Core.UserControls
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private ITask task;
        private Thread thread;
        private DateTime startTime;
        private ProgressWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// статическая переменная, хранящая текущую конфигурацию
        /// </summary>
        static ProgressWindow pw = null;
        public static ProgressWindow Window
        {
            get
            {
                return new ProgressWindow();
            }
        }
        static ProgressWindow()
        {
            //pw = new ProgressWindow();
        }


        public string CurrentAction
        {
            get { return Title; }
            set { Title = value; }
        }
        

        public void RunProcess(ITask task)
        {
            this.task = task;
            this.InitializeComponent();
            ShowDialog();
        }

        private void Run()
        {
            startTime = DateTime.Now;
            CurrentAction = string.Format("Current Action: {0}", task.Name);
            thread = new Thread(task.Start);
            thread.Start();

            ButtonPause.Visibility = System.Windows.Visibility.Visible;
            ButtonResume.Visibility = System.Windows.Visibility.Collapsed;


            ProgressIndicator1.IsRunning = true;
            ProgressIndicator1.IsIndeterminate = task.IsIndeterminate;
            ProgressBar1.IsIndeterminate = task.IsIndeterminate;

            if (task.IsIndeterminate == false)
            {

                ThreadPool.QueueUserWorkItem(new WaitCallback(
                    (object o) =>
                    {
                        int percent = 0;
                        while (task.IsComplete == false)
                        {
                            percent = task.PercentComplete;
                            ProgressIndicator1.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    ProgressIndicator1.Value = percent;
                                }
                            ), null);
                            ProgressBar1.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    ProgressBar1.Value = percent;
                                }
                            ), null);

                            TextBlockTimeElapsed.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    TextBlockTimeElapsed.Text = string.Format("Time elapsed = {0}", DateTime.Now - startTime);
                                }
                            ), null);

                            TextBlockTimeRequired.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    TimeSpan delta = DateTime.Now - startTime;
                                    double miliseconds = delta.TotalMilliseconds;
                                    if (percent != 0)
                                    {
                                        miliseconds *= (100.0 - (double)percent) / (double)percent;
                                    }
                                    TextBlockTimeRequired.Text = string.Format("Time requited = {0}", TimeSpan.FromMilliseconds(miliseconds));
                                }
                            ), null);

                            TextBlockPercentCompleted.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    TextBlockPercentCompleted.Text = string.Format("Completed = {0} %", percent);
                                }
                            ), null);
                            Thread.Sleep(500);
                        }

                        ProgressIndicator1.Dispatcher.Invoke(new Action(
                            () =>
                            {
                                ProgressIndicator1.IsRunning = false;
                            }
                        ), null);
                        this.Dispatcher.Invoke(new Action(
                            () =>
                            {
                                Close();
                            }
                            ), null);
                    }
                ));
            }
            else
            {
                TextBlockPercentCompleted.Visibility = System.Windows.Visibility.Collapsed;
                TextBlockTimeRequired.Visibility = System.Windows.Visibility.Collapsed;
                ThreadPool.QueueUserWorkItem(new WaitCallback(
                    (object o) =>
                    {
                        while (task.IsComplete == false)
                        {
                            TextBlockTimeElapsed.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    TextBlockTimeElapsed.Text = string.Format("Time elapsed = {0}", DateTime.Now - startTime);
                                }
                            ), null);

                            Thread.Sleep(500);
                        }

                        ProgressIndicator1.Dispatcher.Invoke(new Action(
                            () =>
                            {
                                ProgressIndicator1.IsRunning = false;
                            }
                        ), null);
                        this.Dispatcher.Invoke(new Action(
                            () =>
                            {
                                Close();
                            }
                            ), null);
                    }
                ));
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            Close();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            ButtonPause.Visibility = System.Windows.Visibility.Collapsed;
            ButtonResume.Visibility = System.Windows.Visibility.Visible;
            ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
            thread.Suspend();
        }

        private void ButtonResume_Click(object sender, RoutedEventArgs e)
        {
            ButtonPause.Visibility = System.Windows.Visibility.Visible;
            ButtonResume.Visibility = System.Windows.Visibility.Collapsed;
            ButtonCancel.Visibility = System.Windows.Visibility.Visible;
            thread.Resume();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Run();
        }
    }
}
