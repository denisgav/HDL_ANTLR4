using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDL_EditorExtension.CodeCompletion
{
    public class CompletionListItem
    {
        public CompletionListItem(string text, string descriprion)
        {
            this.Text = text;
            this.description = descriprion;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }
        private string description = "Description";

        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return description; }
        }
    }
}
