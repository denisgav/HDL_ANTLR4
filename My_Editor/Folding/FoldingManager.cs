// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5235 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using My_Editor.Document;
using My_Editor.Editing;
using My_Editor.Rendering;
using My_Editor.Utils;

namespace My_Editor.Folding
{
	/// <summary>
	/// Stores a list of foldings for a specific TextView and TextDocument.
	/// </summary>
	public class FoldingManager : IWeakEventListener
	{
		internal readonly TextView textView;
		internal readonly TextDocument document;
		
		readonly TextSegmentCollection<FoldingSection> foldings;
		
		#region Constructor
		/// <summary>
		/// Creates a new FoldingManager instance.
		/// </summary>
		public FoldingManager(TextView textView, TextDocument document)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			if (document == null)
				throw new ArgumentNullException("document");
			this.textView = textView;
			this.document = document;
			this.foldings = new TextSegmentCollection<FoldingSection>();
			document.VerifyAccess();
			TextDocumentWeakEventManager.Changed.AddListener(document, this);
		}
		#endregion
		
		#region ReceiveWeakEvent
		/// <inheritdoc cref="IWeakEventListener.ReceiveWeakEvent"/>
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(TextDocumentWeakEventManager.Changed)) {
				OnDocumentChanged((DocumentChangeEventArgs)e);
				return true;
			}
			return false;
		}
		
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return ReceiveWeakEvent(managerType, sender, e);
		}
		
		void OnDocumentChanged(DocumentChangeEventArgs e)
		{
			foldings.UpdateOffsets(e);
			FoldingSection s = foldings.FindFirstSegmentWithStartAfter(e.Offset);
			while (s != null && s.StartOffset == e.Offset) {
				FoldingSection next = foldings.GetNextSegment(s);
				if (s.Length == 0) {
					RemoveFolding(s);
				}
				s = next;
			}
		}
		#endregion
		
		#region Create / Remove / Clear
		/// <summary>
		/// Creates a folding for the specified text section.
		/// </summary>
		public FoldingSection CreateFolding(int startOffset, int endOffset)
		{
			if (startOffset >= endOffset)
				throw new ArgumentException("startOffset must be less than endOffset");
			FoldingSection fs = new FoldingSection(this, startOffset, endOffset);
			foldings.Add(fs);
			textView.Redraw(fs, DispatcherPriority.Normal);
			return fs;
		}
		
		/// <summary>
		/// Removes a folding section from this manager.
		/// </summary>
		public void RemoveFolding(FoldingSection fs)
		{
			if (fs == null)
				throw new ArgumentNullException("fs");
			fs.IsFolded = false;
			foldings.Remove(fs);
			textView.Redraw(fs, DispatcherPriority.Normal);
		}
		
		/// <summary>
		/// Removes all folding sections.
		/// </summary>
		public void Clear()
		{
			document.VerifyAccess();
			foreach (FoldingSection s in foldings)
				s.IsFolded = false;
			foldings.Clear();
			textView.Redraw();
		}
		#endregion
		
		#region Get...Folding
		/// <summary>
		/// Gets all foldings in this manager.
		/// The foldings are returned sorted by start offset;
		/// for multiple foldings at the same offset the order is undefined.
		/// </summary>
		public IEnumerable<FoldingSection> AllFoldings {
			get { return foldings; }
		}
		
		/// <summary>
		/// Gets the first offset greater or equal to <paramref name="startOffset"/> where a folded folding starts.
		/// Returns -1 if there are no foldings after <paramref name="startOffset"/>.
		/// </summary>
		public int GetNextFoldedFoldingStart(int startOffset)
		{
			FoldingSection fs = foldings.FindFirstSegmentWithStartAfter(startOffset);
			while (fs != null && !fs.IsFolded)
				fs = foldings.GetNextSegment(fs);
			return fs != null ? fs.StartOffset : -1;
		}
		
		/// <summary>
		/// Gets the first folding with a <see cref="TextSegment.StartOffset"/> greater or equal to
		/// <paramref name="startOffset"/>.
		/// Returns null if there are no foldings after <paramref name="startOffset"/>.
		/// </summary>
		public FoldingSection GetNextFolding(int startOffset)
		{
			// TODO: returns the longest folding instead of any folding at the first position after startOffset
			return foldings.FindFirstSegmentWithStartAfter(startOffset);
		}
		
		/// <summary>
		/// Gets all foldings that start exactly at <paramref name="startOffset"/>.
		/// </summary>
		public ReadOnlyCollection<FoldingSection> GetFoldingsAt(int startOffset)
		{
			List<FoldingSection> result = new List<FoldingSection>();
			FoldingSection fs = foldings.FindFirstSegmentWithStartAfter(startOffset);
			while (fs != null && fs.StartOffset == startOffset) {
				result.Add(fs);
				fs = foldings.GetNextSegment(fs);
			}
			return result.AsReadOnly();
		}
		
		/// <summary>
		/// Gets all foldings that contain <paramref name="offset" />.
		/// </summary>
		public ReadOnlyCollection<FoldingSection> GetFoldingsContaining(int offset)
		{
			return foldings.FindSegmentsContaining(offset);
		}
		#endregion
		
		#region UpdateFoldings
		/// <summary>
		/// Updates the foldings in this <see cref="FoldingManager"/> using the given new foldings.
		/// This method will try to detect which new foldings correspond to which existing foldings; and will keep the state
		/// (<see cref="FoldingSection.IsFolded"/>) for existing foldings.
		/// </summary>
		/// <param name="newFoldings">The new set of foldings. These must be sorted by starting offset.</param>
		/// <param name="firstErrorOffset">The first position of a parse error. Existing foldings starting after
		/// this offset will be kept even if they don't appear in <paramref name="newFoldings"/>.
		/// Use -1 for this parameter if there were no parse errors.</param>
		public void UpdateFoldings(IEnumerable<NewFolding> newFoldings, int firstErrorOffset)
		{
			if (newFoldings == null)
				throw new ArgumentNullException("newFoldings");
			
			if (firstErrorOffset < 0)
				firstErrorOffset = int.MaxValue;
			
			var oldFoldings = this.AllFoldings.ToArray();
			int oldFoldingIndex = 0;
			int previousStartOffset = 0;
			// merge new foldings into old foldings so that sections keep being collapsed
			// both oldFoldings and newFoldings are sorted by start offset
			foreach (NewFolding newFolding in newFoldings) {
				// ensure newFoldings are sorted correctly
				if (newFolding.StartOffset < previousStartOffset)
					throw new ArgumentException("newFoldings must be sorted by start offset");
				previousStartOffset = newFolding.StartOffset;
				
				if (newFolding.StartOffset == newFolding.EndOffset)
					continue; // ignore zero-length foldings
				
				// remove old foldings that were skipped
				while (oldFoldingIndex < oldFoldings.Length && newFolding.StartOffset > oldFoldings[oldFoldingIndex].StartOffset) {
					this.RemoveFolding(oldFoldings[oldFoldingIndex++]);
				}
				FoldingSection section;
				// reuse current folding if its matching:
				if (oldFoldingIndex < oldFoldings.Length && newFolding.StartOffset == oldFoldings[oldFoldingIndex].StartOffset) {
					section = oldFoldings[oldFoldingIndex++];
					section.Length = newFolding.EndOffset - newFolding.StartOffset;
				} else {
					// no matching current folding; create a new one:
					section = this.CreateFolding(newFolding.StartOffset, newFolding.EndOffset);
					// auto-close #regions only when opening the document
					section.IsFolded = newFolding.DefaultClosed;
					section.Tag = newFolding;
				}
				section.Title = newFolding.Name;
			}
			// remove all outstanding old foldings:
			while (oldFoldingIndex < oldFoldings.Length) {
				FoldingSection oldSection = oldFoldings[oldFoldingIndex++];
				if (oldSection.StartOffset >= firstErrorOffset)
					break;
				this.RemoveFolding(oldSection);
			}
		}
		#endregion
		
		#region Install
		/// <summary>
		/// Adds Folding support to the specified text area.
		/// Warning: The folding manager is only valid for the text area's current document. The folding manager
		/// must be uninstalled before the text area is bound to a different document.
		/// </summary>
		/// <returns>The <see cref="FoldingManager"/> that manages the list of foldings inside the text area.</returns>
		public static FoldingManager Install(TextArea textArea)
		{
			if (textArea == null)
				throw new ArgumentNullException("textArea");
			return new FoldingManagerInstallation(textArea);
		}
		
		/// <summary>
		/// Uninstalls the folding manager.
		/// </summary>
		/// <exception cref="ArgumentException">The specified manager was not created using <see cref="Install"/>.</exception>
		public static void Uninstall(FoldingManager manager)
		{
			if (manager == null)
				throw new ArgumentNullException("manager");
			FoldingManagerInstallation installation = manager as FoldingManagerInstallation;
			if (installation != null) {
				installation.Uninstall();
			} else {
				throw new ArgumentException("FoldingManager was not created using FoldingManager.Install");
			}
		}
		
		sealed class FoldingManagerInstallation : FoldingManager
		{
			TextArea textArea;
			FoldingMargin margin;
			FoldingElementGenerator generator;
			
			public FoldingManagerInstallation(TextArea textArea) : base(textArea.TextView, textArea.Document)
			{
				this.textArea = textArea;
				margin = new FoldingMargin() { FoldingManager = this };
				generator = new FoldingElementGenerator() { FoldingManager = this };
				textArea.LeftMargins.Add(margin);
				textArea.TextView.ElementGenerators.Add(generator);
				textArea.Caret.PositionChanged += textArea_Caret_PositionChanged;
			}
			
			/*
			void DemoMode()
			{
				foldingGenerator = new FoldingElementGenerator() { FoldingManager = fm };
				foldingMargin = new FoldingMargin { FoldingManager = fm };
				foldingMarginBorder = new Border {
					Child = foldingMargin,
					Background = new LinearGradientBrush(Colors.White, Colors.Transparent, 0)
				};
				foldingMarginBorder.SizeChanged += UpdateTextViewClip;
				textEditor.TextArea.TextView.ElementGenerators.Add(foldingGenerator);
				textEditor.TextArea.LeftMargins.Add(foldingMarginBorder);
			}
			
			void UpdateTextViewClip(object sender, SizeChangedEventArgs e)
			{
				textEditor.TextArea.TextView.Clip = new RectangleGeometry(
					new Rect(-foldingMarginBorder.ActualWidth,
					         0,
					         textEditor.TextArea.TextView.ActualWidth + foldingMarginBorder.ActualWidth,
					         textEditor.TextArea.TextView.ActualHeight));
			}
			 */
			
			public void Uninstall()
			{
				Clear();
				if (textArea != null) {
					textArea.Caret.PositionChanged -= textArea_Caret_PositionChanged;
					textArea.LeftMargins.Remove(margin);
					textArea.TextView.ElementGenerators.Remove(generator);
					margin = null;
					generator = null;
					textArea = null;
				}
			}
			
			void textArea_Caret_PositionChanged(object sender, EventArgs e)
			{
				// Expand Foldings when Caret is moved into them.
				int caretOffset = textArea.Caret.Offset;
				foreach (FoldingSection s in GetFoldingsContaining(caretOffset)) {
					if (s.IsFolded && s.StartOffset < caretOffset && caretOffset < s.EndOffset) {
						s.IsFolded = false;
					}
				}
			}
		}
		#endregion
	}
}
