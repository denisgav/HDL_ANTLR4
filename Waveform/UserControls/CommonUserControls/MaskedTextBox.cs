using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using DataContainer.ValueDump;

namespace Schematix.Waveform.UserControls
{
    class MaskedTextBox : TextBox
    {
        /// <summary>
        /// Тип маски
        /// </summary>
        public enum MaskType
        {
            Mask,
            IntegerNumber,
            DoubleNumber
        }

        private System.ComponentModel.MaskedTextProvider _mprovider = null;
        /// <summary>
        /// Gets/Sets the desired mask
        /// </summary>
        public string Mask
        {
            get
            {
                if (_mprovider != null) return _mprovider.Mask;
                else return "";
            }
            set
            {
                _mprovider = new System.ComponentModel.MaskedTextProvider(value);
                this.Text = _mprovider.ToDisplayString();
            }
        }

        private MaskType _maskType = MaskType.Mask;
        public MaskType Mask_Type 
        {
            get { return _maskType; }
            set { _maskType = value; }
        }

        /// <summary>
        /// Система счисления
        /// </summary>
        private EnumerationSystem bus = EnumerationSystem.Dec;
        public EnumerationSystem Bus
        {
            get { return bus; }
            set 
            {
                if ((Mask_Type == MaskType.IntegerNumber) || (bus != value))
                {
                    ConvertBus(bus, value);
                    bus = value; 
                }
            }
        }

        private bool PreviousInsertState = false;

        private bool _InsertIsON = true;
        private bool _stayInFocusUntilValid = false;

        /// <summary>
        /// Sets whether the focus should stay on the control until the contents are valid
        /// </summary>
        public bool StayInFocusUntilValid
        {
            get { return _stayInFocusUntilValid; }
            set { _stayInFocusUntilValid = value; }
        }

        private bool _NewTextIsOk = false;
        /// <summary>
        /// Defines whether the next entered input text is ok according to the mask
        /// </summary>
        public bool NewTextIsOk
        {
            get { return _NewTextIsOk; }
            set { _NewTextIsOk = value; }
        }

        private bool _ignoreSpace = true;
        /// <summary>
        /// Sets whether space should be ignored
        /// </summary>
        public bool IgnoreSpace
        {
            get { return _ignoreSpace; }
            set { _ignoreSpace = value; }
        }

        private bool _iDoubleType = false;
        public bool IsDoubleType
        {
            get { return _iDoubleType; }
            set { _iDoubleType = value; }
        }

        private bool _iIntType = false;
        public bool IsIntType
        {
            get { return _iIntType; }
            set { _iIntType = value; }
        }

        /// <summary>
        /// Stops the effect of some common keys
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            //if (this.SelectionLength > 1)
            //{
            //    this.SelectionLength = 0;
            //    e.Handled = true;
            //}
            //if (/*e.Key == Key.Insert || e.Key == Key.Delete || e.Key == Key.Back ||*/ (e.Key == Key.Space && _ignoreSpace))
            //{
            //    e.Handled = true;
            //}
            base.OnPreviewKeyDown(e);

        }

        /// <summary>
        /// We check whether we are ok
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            switch (_maskType)
            {
                case MaskType.Mask:
                    {
                        System.ComponentModel.MaskedTextResultHint hint;
                        int TestPosition;
                        if (e.Text.Length == 1)
                            this._NewTextIsOk = _mprovider.VerifyChar(e.Text[0], this.CaretIndex, out hint);
                        else
                            this._NewTextIsOk = _mprovider.VerifyString(e.Text, out TestPosition, out hint);
                    }
                    break;
                case MaskType.DoubleNumber:
                    {
                        double num = 0;
                        if ((Text == string.Empty) || (Text == "-") || (e.Text == "."))
                            this._NewTextIsOk = true;
                        else
                        {
                            this._NewTextIsOk = double.TryParse(Text + "0", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out num);
                        }
                    }
                    break;

                case MaskType.IntegerNumber:
                    {
                        int num = 0;
                        if ((Text == string.Empty) || ((bus == EnumerationSystem.Dec) && (Text == "-")))
                            this._NewTextIsOk = true;
                        else
                        {
                            switch (bus)
                            {
                                case EnumerationSystem.Dec:
                                    this._NewTextIsOk = int.TryParse(Text + "0", System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out num);
                                    break;
                                case EnumerationSystem.Bin:
                                    {
                                        this._NewTextIsOk = true;
                                        foreach (char ch in Text + "0")
                                        {
                                            if (!((ch == '0') || (ch == '1')))
                                            {
                                                this._NewTextIsOk = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case EnumerationSystem.Oct:
                                    {
                                        this._NewTextIsOk = true;
                                        foreach (char ch in Text + "0")
                                        {
                                            if (!((ch == '0') || (ch == '1') || (ch == '2') || (ch == '3') || (ch == '4') || (ch == '5') || (ch == '6') || (ch == '7')))
                                            {
                                                this._NewTextIsOk = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case EnumerationSystem.Hex:
                                    this._NewTextIsOk = int.TryParse(Text+"0", System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out num);
                                    break;
                            }

                        }
                    }
                    break;

                default:
                    break;
            }

           base.OnPreviewTextInput(e);

        }

        /// <summary>
        /// When text is received by the TextBox we check whether to accept it or not
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            string PreviousText = this.Text;
            if (NewTextIsOk)
            {
                base.OnTextInput(e);
                switch (_maskType)
                {
                    case MaskType.Mask:
                        {
                            if (_mprovider.VerifyString(this.Text) == false)
                                this.Text = PreviousText;
                            while (!_mprovider.IsEditPosition(this.CaretIndex) && _mprovider.Length > this.CaretIndex)
                                this.CaretIndex++;
                        }
                        break;
                    case MaskType.DoubleNumber:
                        {
                            double num = 0;
                            if (double.TryParse(Text + "0", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out num) == false)
                            {
                                this.Text = PreviousText;
                            }
                        }
                        break;

                    case MaskType.IntegerNumber:
                        {
                            int num = 0;
                            bool res = true;
                            switch (bus)
                            {
                                case EnumerationSystem.Dec:
                                    res = int.TryParse(Text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out num);
                                    break;
                                case EnumerationSystem.Bin:
                                    {
                                        foreach (char ch in Text)
                                        {
                                            if (!((ch == '0') || (ch == '1')))
                                            {
                                                res = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case EnumerationSystem.Oct:
                                    {
                                        this._NewTextIsOk = true;
                                        foreach (char ch in Text)
                                        {
                                            if (!((ch == '0') || (ch == '1') || (ch == '2') || (ch == '3') || (ch == '4') || (ch == '5') || (ch == '6') || (ch == '7')))
                                            {
                                                res = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case EnumerationSystem.Hex:
                                    res = int.TryParse(Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out num);
                                    break;
                            }
                            if(res == false)
                            {
                                this.Text = PreviousText;
                            }
                        }
                        break;
                }

            }
            else
                e.Handled = true;
        }


        /// <summary>
        /// возвращает данные в целочисленном виде с учетом системы счисления
        /// </summary>
        /// <returns></returns>
        public int GetIntegerValue()
        {
            if (string.IsNullOrEmpty(Text))
                return 0;
            if (Mask_Type != MaskType.IntegerNumber)
                return 0;
            switch (bus)
            {
                case EnumerationSystem.Bin:
                    return Convert.ToInt32(Text, 2);
                case EnumerationSystem.Dec:
                    return Convert.ToInt32(Text, 10);
                case EnumerationSystem.Hex:
                    return Convert.ToInt32(Text, 16);
                case EnumerationSystem.Oct:
                    return Convert.ToInt32(Text, 8);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// возвращает данные в виде числа с плавающей точкой
        /// </summary>
        /// <returns></returns>
        public double GetDoubleValue()
        {
            if (string.IsNullOrEmpty(Text))
                return -1;
            if (Mask_Type != MaskType.DoubleNumber)
                return -1;
            double num = 0;
            if (double.TryParse(Text + "0", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out num) == false)
            {
                return -1;
            }
            return num;
        }

        /// <summary>
        /// When the TextBox takes the focus we make sure that the Insert is set to Replace
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (!_InsertIsON)
            {
                PressKey(Key.Insert);
                _InsertIsON = true;
            }
        }

        /// <summary>
        /// When the textbox looses the keyboard focus we may want to verify (based on the StayInFocusUntilValid) whether
        /// the control has a valid value (fully complete)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (StayInFocusUntilValid)
            {
                _mprovider.Clear();
                _mprovider.Add(this.Text);
                if (!_mprovider.MaskFull) e.Handled = true;
            }

            base.OnPreviewLostKeyboardFocus(e);
        }

        /// <summary>
        /// When the textbox looses its focus we need to return the Insert Key state to its previous state
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (PreviousInsertState != System.Windows.Input.Keyboard.PrimaryDevice.IsKeyToggled(System.Windows.Input.Key.Insert))
                PressKey(Key.Insert);
        }

        /// <summary>
        /// Simulates pressing a key
        /// </summary>
        /// <param name="key">The key to be pressed</param>
        private void PressKey(Key key)
        {
            KeyEventArgs eInsertBack = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                        Keyboard.PrimaryDevice.ActiveSource,
                                                        0, key);
            eInsertBack.RoutedEvent = KeyDownEvent;
            InputManager.Current.ProcessInput(eInsertBack);
        }

        private void ConvertBus(EnumerationSystem _old, EnumerationSystem _new)
        {
            if (Text.Length == 0)
                return;

            int num = 0;
            switch (_old)
            {
                case EnumerationSystem.Bin:
                    num = Convert.ToInt32(Text, 2);
                    break;
                case EnumerationSystem.Dec:
                    num = Convert.ToInt32(Text, 10);
                    break;
                case EnumerationSystem.Hex:
                    num = Convert.ToInt32(Text, 16);
                    break;
                case EnumerationSystem.Oct:
                    num = Convert.ToInt32(Text, 8);
                    break;
                default:
                    break;
            }

            switch (_new)
            {
                case EnumerationSystem.Bin:
                    Text = Convert.ToString(num, 2);
                    break;
                case EnumerationSystem.Dec:
                    Text = Convert.ToString(num, 10);
                    break;
                case EnumerationSystem.Hex:
                    Text = Convert.ToString(num, 16);
                    break;
                case EnumerationSystem.Oct:
                    Text = Convert.ToString(num, 8);
                    break;
                default:
                    break;
            }
            
        }
    }
}
