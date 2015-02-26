using System;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media;
using ICSharpCode.NRefactory.Editor;


namespace HDL_EditorExtension.CodeCompletion
{
	/// <summary>
	/// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
	/// </summary>
	public class MyCompletionData : ICompletionData
	{
		public MyCompletionData(string text, string descriprion)
		{
			this.Text = text;
            this.Description = descriprion;
		}

        public MyCompletionData(CompletionListItem item)
        {
            this.Text = item.Text;
            this.Description = item.Description;
            this.Image = item.Image;
        }

        ImageSource image;
		public ImageSource Image
        {
			get { return image; }
            set { image = value; }
		}
		
		public string Text { get; private set; }
        private string description = "Description";
		
		// Use this property if you want to show a fancy UIElement in the drop down list.
		public object Content {
			get { return this.Text; }
		}
		
		public object Description
        {
			get { return description; }
            private set { description = value as string; }
		}
		
		public double Priority { get { return 0; } }
		
		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, this.Text);
		}
	}
}
