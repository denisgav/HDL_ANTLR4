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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Schematix;
using Schematix.Dialogs.Search_Replace.Code;

namespace Schematix
{
    /// <summary>
    /// Interaction logic for SearchReplace.xaml
    /// </summary>
    public partial class SearchReplaceUserControl : UserControl
    {
        private SchematixCore core;       
        private SimpleSearcher searcher;
        private TextSearcher textSearcher;
        private SearchResult curSearchResult;

        public SearchReplaceUserControl()
            : this(SchematixCore.Core, null)
        { }

        public SearchReplaceUserControl(Schematix.Windows.Code.Code code)
            : this(SchematixCore.Core, code)
        { }

        public SearchReplaceUserControl(SchematixCore core, Schematix.Windows.Code.Code code)
        {
            this.core = core;
            textSearcher = new TextSearcher();
            InitializeComponent();            
        }                       

        private void ButtonFindNext1_Click(object sender, RoutedEventArgs e)
        {
            textSearcher.Searcher.MatchCase = CheckBoxMatchCase1.IsChecked == true;
            textSearcher.Searcher.SearchUp = CheckBoxSearchUp1.IsChecked == true;
            textSearcher.Searcher.SearchedText = ComboboxQuickFind.Text;
            curSearchResult = textSearcher.GetNextSearchResult();
            if (curSearchResult != null)
            {
                Schematix.Windows.Code.Code window = core.OpenNewWindow(curSearchResult.Code.Path) as Schematix.Windows.Code.Code;
                if (window != null)
                {
                    if (window.textEditor.IsLoaded == false)
                    {
                        window.textEditor.Loaded += new RoutedEventHandler(delegate { window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length); });
                    }
                    else
                    {
                        window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length);
                    }
                    textSearcher.Searcher.Text = window.textEditor.Text;
                    textSearcher.Searcher.StartOffset += curSearchResult.Segment.Length;
                }
            }
            else
            {
                textSearcher.Searcher.StartOffset = 0;
                UpdateSearchType1();
            }
        }

        private void ButtonFindNext2_Click(object sender, RoutedEventArgs e)
        {
            textSearcher.Searcher.MatchCase = CheckBoxMatchCase2.IsChecked == true;
            textSearcher.Searcher.SearchUp = CheckBoxSearchUp2.IsChecked == true;
            textSearcher.Searcher.SearchedText = ComboboxQuickSource.Text;
            curSearchResult = textSearcher.GetNextSearchResult();
            if (curSearchResult != null)
            {
                Schematix.Windows.Code.Code window = core.OpenNewWindow(curSearchResult.Code.Path) as Schematix.Windows.Code.Code;
                if (window != null)
                {
                    if (window.textEditor.IsLoaded == false)
                    {
                        window.textEditor.Loaded += new RoutedEventHandler(delegate { window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length); });
                    }
                    else
                    {
                        window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length);
                    }
                    textSearcher.Searcher.Text = window.textEditor.Text;
                    textSearcher.Searcher.StartOffset += curSearchResult.Segment.Length;
                }
            }
            else
            {
                textSearcher.Searcher.StartOffset = 0;
                UpdateSearchType2();
            }
        }

        

        private void ButtonReplace2_Click(object sender, RoutedEventArgs e)
        {
            if ((curSearchResult != null) && (curSearchResult.Segment.Length != 0))
            {
                if ((core.MainWindow.CurrentActiveWindow != null) && (core.MainWindow.CurrentActiveWindow is Schematix.Windows.Code.Code))
                {
                    Schematix.Windows.Code.Code window = core.MainWindow.CurrentActiveWindow as Schematix.Windows.Code.Code;
                    StringBuilder res = new StringBuilder();
                    res.Append(window.textEditor.Text.Substring(0, curSearchResult.Segment.StartOffset));
                    res.Append(ComboboxQuickReplace.Text);
                    res.Append(window.textEditor.Text.Substring(curSearchResult.Segment.EndOffset));
                    window.textEditor.Text = res.ToString();
                    textSearcher.Searcher.Text = window.textEditor.Text;
                    textSearcher.Searcher.StartOffset += ComboboxQuickReplace.Text.Length;
                }
            }
        }

        private void ButtonReplaceAll2_Click(object sender, RoutedEventArgs e)
        {
            UpdateSearchType2();
            textSearcher.Searcher.MatchCase = CheckBoxMatchCase2.IsChecked == true;
            textSearcher.Searcher.SearchUp = CheckBoxSearchUp2.IsChecked == true;
            textSearcher.Searcher.SearchedText = ComboboxQuickSource.Text;            
            while(true)
            {
                curSearchResult = textSearcher.GetNextSearchResult();
                if ((curSearchResult != null) && (curSearchResult.Code != null))
                {
                    Schematix.Windows.Code.Code window = core.OpenNewWindow(curSearchResult.Code.Path) as Schematix.Windows.Code.Code;
                    if (window != null)
                    {
                        if (window.textEditor.IsLoaded == false)
                        {
                            window.textEditor.Loaded += new RoutedEventHandler(delegate {
                                StringBuilder res = new StringBuilder();
                                res.Append(window.textEditor.Text.Substring(0, curSearchResult.Segment.StartOffset));
                                res.Append(ComboboxQuickReplace.Text);
                                res.Append(window.textEditor.Text.Substring(curSearchResult.Segment.EndOffset));
                                window.textEditor.Text = res.ToString();
                                textSearcher.Searcher.Text = window.textEditor.Text;
                                textSearcher.Searcher.StartOffset += ComboboxQuickReplace.Text.Length;
                            });
                        }
                        else
                        {
                            StringBuilder res = new StringBuilder();
                            res.Append(window.textEditor.Text.Substring(0, curSearchResult.Segment.StartOffset));
                            res.Append(ComboboxQuickReplace.Text);
                            res.Append(window.textEditor.Text.Substring(curSearchResult.Segment.EndOffset));
                            window.textEditor.Text = res.ToString();
                            textSearcher.Searcher.Text = window.textEditor.Text;
                            textSearcher.Searcher.StartOffset += ComboboxQuickReplace.Text.Length;
                        }
                    }
                }
                else
                    break;
            }
        }

        private void SearchSource1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSearchType1();
        }        

        private void SearchSource2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSearchType2();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSearchType1();
        }

        private void UpdateSearchType1()
        {
            SearchType searchType = SearchType.CurrentDocument;
            switch (SearchSource1.SelectedIndex)
            {
                case 0: searchType = SearchType.CurrentDocument; break;
                case 1: searchType = SearchType.AllOpenedDocuments; break;
                case 2: searchType = SearchType.CurrentProject; break;
                case 3: searchType = SearchType.EntireSolution; break;
            }
            if (textSearcher != null)
                textSearcher.Reset(searchType);
        }

        private void UpdateSearchType2()
        {
            SearchType searchType = SearchType.CurrentDocument;
            switch (SearchSource2.SelectedIndex)
            {
                case 0: searchType = SearchType.CurrentDocument; break;
                case 1: searchType = SearchType.AllOpenedDocuments; break;
                case 2: searchType = SearchType.CurrentProject; break;
                case 3: searchType = SearchType.EntireSolution; break;
            }
            if (textSearcher != null)
                textSearcher.Reset(searchType);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        internal void Refresh()
        {
            TabItem ti = MainTabControl.SelectedItem as TabItem;

            if (ti != null)
            {
                if (ti == TabSearch)
                {
                    UpdateSearchType1();
                }
                else
                {
                    UpdateSearchType2();
                }
            }
        }
    }
}
