using System.Collections.Generic;
using System.Linq;
using VHDL;
using Antlr4.Runtime;
using System;
using System.Windows;
using My_Editor.CodeCompletion;
using HDL_EditorExtension.Lexter;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace HDL_EditorExtension.CodeCompletion
{
    public class VHDLCodeCompletionList : CodeCompletionList
    {
        private VHDL_Lexter lexter;
        public VHDL_Lexter Lexter
        {
            get { return lexter; }
            set { lexter = value; }
        }

        public VHDLCodeCompletionList(TextArea TextArea, VHDL_Lexter parent)
            : base(TextArea)
        {
            this.lexter = parent;
        }

        public override void LoadData()
        {
            try
            {
                int cursor_offset = TextArea.Caret.Offset;
                VhdlElement element = lexter.GetElementByOffset(lexter.File, cursor_offset);

                List<object> obects = new List<object>();
                obects.AddRange(GetContextListOfObjects(element).Distinct());

                List<CompletionListItem> Data = LoadWordsFromObjects(obects);
                Data.AddRange(LoadReservedWords());

                IList<ICompletionData> CompletionData = completionWindow.CompletionList.CompletionData;
                foreach (CompletionListItem item in Data)
                {
                    CompletionData.Add(new MyCompletionData(item));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Code completion Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

     

        public override void SetStartupWord()
        {
            int cursor_offset = TextArea.Caret.Offset;
            IToken token = lexter.GetTreeElementByOffset(cursor_offset);
            if ((token != null) && ((token.StartIndex - 1) <= cursor_offset) && ((token.StopIndex + 1) >= cursor_offset))
            {
                string text = token.Text.Trim();
                if ((text.Length != 0) || (text.Length >= cursor_offset - token.StartIndex))
                    completionWindow.CompletionList.SelectItem(text.Substring(0, cursor_offset - token.StartIndex));
                completionWindow.StartOffset = token.StartIndex;
                completionWindow.EndOffset = token.StartIndex + text.Length;                
            }
        }

        private List<object> GetContextListOfObjects(VhdlElement element)
        {
            List<object> res = new List<object>();
            if (element == null)
                return res;

            IDeclarativeRegion parent = element.Parent;
            IScope scope = null;
            if (element is IDeclarativeRegion)
            {
                scope = (element as IDeclarativeRegion).Scope;
                res.AddRange(scope.GetListOfObjects());
            }
            else
            {
                if (parent != null)
                {
                    scope = parent.Scope;
                    res.AddRange(scope.GetListOfObjects());
                }
            }
            return res;
        }

        /// <summary>
        /// Загрузка зарезервированных слов
        /// </summary>
        /// <returns></returns>
        private List<CompletionListItem> LoadReservedWords()
        {
            List<CompletionListItem> res = new List<CompletionListItem>();
            foreach (string word in VHDL_Lexter.ReservedWords)
                res.Add(new CompletionListItem(word, "Reserved Word"));
            return res;
        }

        /// <summary>
        /// Загрузка данных для контекстного мень из обьектов
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        private List<CompletionListItem> LoadWordsFromObjects(List<object> objects)
        {
            List<CompletionListItem> res = new List<CompletionListItem>();
            foreach (object o in objects)
            {
                if (o is INamedEntity)
                {
                    res.Add(new CompletionListItem((o as INamedEntity).Identifier, o.GetType().Name.ToString()));
                }
                if (o is VHDL.Object.VhdlObject)
                {
                    res.Add(new CompletionListItem((o as VHDL.Object.VhdlObject).Identifier, o.GetType().Name.ToString()));
                }
            }
            return res;
        }
    }
}
