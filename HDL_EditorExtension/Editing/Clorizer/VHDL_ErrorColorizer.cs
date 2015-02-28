using System;
using System.Windows;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Editing;
using HDL_EditorExtension.Lexter;

namespace HDL_EditorExtension.Editing
{
    /// <summary>
    /// 
    /// </summary>
    public class VHDL_ErrorColorizer : ColorizingTransformer
    {
        VHDL_Lexter Lexter;

        public VHDL_ErrorColorizer(VHDL_Lexter Lexter)
		{
            if (Lexter == null)
                throw new ArgumentNullException("Lexter");
            this.Lexter = Lexter;
		}
		
		protected override void Colorize(ITextRunConstructionContext context)
		{
			int lineStartOffset = context.VisualLine.FirstDocumentLine.Offset;
			int lineEndOffset = context.VisualLine.LastDocumentLine.Offset + context.VisualLine.LastDocumentLine.TotalLength;

            if (Lexter != null)
            {
                foreach (Exception_Information segment in Lexter.ErrorList)
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
