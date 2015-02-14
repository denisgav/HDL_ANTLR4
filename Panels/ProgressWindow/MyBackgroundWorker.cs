using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core.UserControls
{
    /// <summary>
    /// Часный случай для использования ProgressWindow
    /// </summary>
    public class MyBackgroundWorker:ITask
    {
        private Action action;
        private Action cancelAction;

        public MyBackgroundWorker(Action action, Action cancelAction, string name)
        {
            this.cancelAction = cancelAction;
            this.action = action;
            this.name = name;
        }

        public void Start()
        {
            isComplete = false;
            action();
            isComplete = true;
        }

        public int PercentComplete
        {
            get { return 1; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        public bool IsIndeterminate
        {
            get { return true; }
        }

        private bool isComplete;
        public bool IsComplete
        {
            get { return isComplete; }
        }

        public void OnCancel()
        {
            if (cancelAction != null)
                cancelAction();
        }
    }
}
