using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Schematix.Dialogs.Search_Replace
{

    public enum SearchMode
    {
        Instant,
        Regular
    }

    public enum SearchBoxState
    {
        Prompt,
        Input,
        Text,
        Search
    }

    [TemplatePart(Name = "PART_SearchText", Type = typeof(TextBoxBase))]
    [TemplatePart(Name = "PART_SearchButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_DropDownArrow", Type = typeof(ToggleButton))]
    public class SearchBox : Control, ICommandSource
    {
        #region Text Dependency Property

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(SearchBox),
                    new FrameworkPropertyMetadata(
                        String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(OnTextChanged)));

        #endregion

        #region Prompt Dependency Property

        public string Prompt
        {
            get { return (string)GetValue(PromptProperty); }
            set { SetValue(PromptProperty, value); }
        }

        public static readonly DependencyProperty PromptProperty =
                DependencyProperty.Register("Prompt", typeof(string), typeof(SearchBox), new UIPropertyMetadata(String.Empty));

        #endregion

        #region Command Dependency Property

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
                DependencyProperty.Register("Command", typeof(ICommand), typeof(SearchBox), new UIPropertyMetadata(null));

        #endregion

        #region CommandParameter Dependency Property

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
                DependencyProperty.Register("CommandParameter", typeof(object), typeof(SearchBox), new UIPropertyMetadata(null));

        #endregion

        #region CommandTarget Dependency Property

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
                DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SearchBox),
                    new FrameworkPropertyMetadata(null));

        #endregion

        #region CommandDelay Dependency Property

        public Duration CommandDelay
        {
            get { return (Duration)GetValue(CommandDelayProperty); }
            set { SetValue(CommandDelayProperty, value); }
        }

        public static readonly DependencyProperty CommandDelayProperty =
                DependencyProperty.Register("CommandDelay", typeof(Duration), typeof(SearchBox),
                    new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(500d)),
                                        new PropertyChangedCallback(OnSearchCommandDelayChanged)));

        #endregion

        #region Mode Dependency Property

        public SearchMode Mode
        {
            get { return (SearchMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
                DependencyProperty.Register("Mode", typeof(SearchMode), typeof(SearchBox),
                    new UIPropertyMetadata(SearchMode.Instant));

        #endregion

        #region State Dependency Property

        public SearchBoxState State
        {
            get { return (SearchBoxState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
                DependencyProperty.Register("State", typeof(SearchBoxState), typeof(SearchBox),
                    new UIPropertyMetadata(SearchBoxState.Prompt));

        #endregion

        private DispatcherTimer delayTimer;

        static SearchBox()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox))
            );
        }

        public SearchBox()
        {
            delayTimer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
            delayTimer.Interval = CommandDelay.TimeSpan;
            delayTimer.Tick += OnCommandDelayTimerTick;
            Focusable = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var searchText = GetTemplateChild("PART_SearchText") as TextBoxBase;
            if (searchText != null)
            {
                searchText.IsKeyboardFocusWithinChanged += OnTextBoxIsKeyboardFocusedChanged;
                searchText.PreviewKeyDown += OnTextBoxKeyDown;
            }
            var searchButton = GetTemplateChild("PART_SearchButton") as ButtonBase;
            if (searchButton != null)
            {
                searchButton.Click += OnSearchButtonClick;
            }
            var dropDownArrow = GetTemplateChild("PART_DropDownArrow") as ToggleButton;
            if (dropDownArrow != null)
            {
                dropDownArrow.Click += OnDropDownArrowClick;
            }
        }

        private void OnTextBoxIsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var focused = (bool)(e.NewValue ?? false);
            if (focused)
            {
                OnTextBoxGotFocus(sender);
            }
            else
            {
                OnTextBoxLostFocus();
            }
        }

        private void OnTextBoxLostFocus()
        {
            if (State == SearchBoxState.Input)
                State = SearchBoxState.Prompt;
        }

        private void OnTextBoxGotFocus(object sender)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                var searchText = (TextBoxBase)sender;
                searchText.SelectAll();

                State = SearchBoxState.Text;
            }
            else
            {
                State = SearchBoxState.Input;
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var searchBox = (SearchBox)d;
            var text = (string)e.NewValue;

            searchBox.State = String.IsNullOrEmpty(text) ? SearchBoxState.Input : SearchBoxState.Text;
            /*
            if (searchBox.Mode == SearchMode.Instant)
            {
                searchBox.delayTimer.Stop();
                searchBox.delayTimer.Start();
            }
            */
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && Mode == SearchMode.Instant)
            {
                ClearText(SearchBoxState.Input);
            }
            else if ((e.Key == Key.Return || e.Key == Key.Enter) /*&& Mode == SearchMode.Regular*/)
            {
                if (State == SearchBoxState.Search)
                    ClearText(SearchBoxState.Input);
                else
                    ExecuteCommand();
                e.Handled = true;
            }
        }

        private void ClearText(SearchBoxState state)
        {
            if (!String.IsNullOrEmpty(Text))
                Text = String.Empty;
            State = state;
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (ContextMenu != null)
                ContextMenu.IsOpen = false;
            e.Handled = true;
        }

        private void OnDropDownArrowClick(object sender, RoutedEventArgs e)
        {
            if (ContextMenu == null || !ContextMenu.HasItems)
                return;

            var dropDownArrow = (ToggleButton)sender;

            if (dropDownArrow.IsChecked ?? false)
            {
                ContextMenu.PlacementTarget = dropDownArrow;
                ContextMenu.Placement = PlacementMode.Bottom;
                ContextMenuService.SetPlacement(dropDownArrow, PlacementMode.Bottom);
            }
        }

        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                if (Mode == SearchMode.Instant || State == SearchBoxState.Search)
                    ClearText(SearchBoxState.Prompt);
                else
                    ExecuteCommand();
            }
        }

        private static void OnSearchCommandDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var searchBox = (SearchBox)d;
            if (searchBox != null)
            {
                searchBox.delayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                searchBox.delayTimer.Stop();
            }
        }

        private void OnCommandDelayTimerTick(object sender, EventArgs e)
        {
            delayTimer.Stop();
            ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            ExecuteCommand(this);
            if (Mode == SearchMode.Regular)
                State = SearchBoxState.Search;
        }

        [SecurityCritical]
        private static void ExecuteCommand(ICommandSource commandSource)
        {
            var command = commandSource.Command;
            if (command != null)
            {
                var commandParameter = commandSource.CommandParameter;
                var commandTarget = commandSource.CommandTarget;
                var routedCommand = command as RoutedCommand;
                if (routedCommand != null)
                {
                    if (commandTarget == null)
                    {
                        commandTarget = commandSource as IInputElement;
                    }
                    if (routedCommand.CanExecute(commandParameter, commandTarget))
                    {
                        routedCommand.Execute(commandParameter, commandTarget);
                    }
                }
                else if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }
    }
}
