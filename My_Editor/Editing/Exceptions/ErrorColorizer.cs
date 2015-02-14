using System;
using System.Windows;
using My_Editor.Document;
using My_Editor.Rendering;

namespace My_Editor.Editing
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorColorizer : ColorizingTransformer
    {
        TextArea textArea;

        public ErrorColorizer(TextArea textArea)
		{
			if (textArea == null)
				throw new ArgumentNullException("textArea");
			this.textArea = textArea;
		}
		
		protected override void Colorize(ITextRunConstructionContext context)
		{
			int lineStartOffset = context.VisualLine.FirstDocumentLine.Offset;
			int lineEndOffset = context.VisualLine.LastDocumentLine.Offset + context.VisualLine.LastDocumentLine.TotalLength;

            if (textArea.Lexter != null)
            {
                foreach (Exception_Information segment in textArea.Lexter.ErrorList)
                {
                    int segmentStart = segment.Offset;
                    int segmentEnd = segment.Offset + segment.Length;
                    if (segmentEnd <= lineStartOffset)
                        continue;
                    if (segmentStart >= lineEndOffset)
                        continue;
                    int startColumn = context.VisualLine.GetVisualColumn(Math.Max(0, segmentStart - lineStartOffset));
                    int endColumn = context.VisualLine.GetVisualColumn(segmentEnd - lineStartOffset);

                    ChangeVisualElements(
                        startColumn, endColumn,
                        element =>
                        {
                            TextDecorationCollection coll = new TextDecorationCollection();
                            TextDecoration decor = new TextDecoration();
                            decor.Pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 3);
                            coll.Add(decor);
                            element.TextRunProperties.SetTextDecorations(coll);
                        });
                }
            }
		}
    }
}
