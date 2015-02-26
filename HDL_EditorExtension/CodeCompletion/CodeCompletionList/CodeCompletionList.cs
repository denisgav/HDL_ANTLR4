using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using HDL_EditorExtension.CodeCompletion;

namespace My_Editor.CodeCompletion
{
    /// <summary>
    /// test
    /// </summary>
    public class CodeCompletionList
    {
        public CompletionWindow completionWindow;
        protected TextArea TextArea;

        

        public CodeCompletionList(TextArea TextArea)
        {
            this.TextArea = TextArea;
        }

        public void Show()
        {
            this.completionWindow = new CompletionWindow(TextArea);
            LoadData();
            SetStartupWord();
            completionWindow.Show();
            completionWindow.Closed += delegate
            {
                completionWindow = null;
            };
        }

        public virtual void SetStartupWord()
        { }

        public virtual void LoadData()
        {
            System.Collections.Generic.IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
            data.Add(new MyCompletionData("Item1", "Reserved Word"));
            data.Add(new MyCompletionData("Item2", "Reserved Word"));
            data.Add(new MyCompletionData("Item3", "Reserved Word"));
            data.Add(new MyCompletionData("Another Item", "Reserved Word"));
        }
    }
}
