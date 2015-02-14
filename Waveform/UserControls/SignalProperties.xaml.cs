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
using System.Windows.Shapes;
using Schematix.Waveform.Value_Dump;
using DataContainer;
using DataContainer.ValueDump;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for SignalProperties.xaml
    /// </summary>
    public partial class SignalProperties : Window
    {
        private My_Variable variable;
        public SignalProperties(My_Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (variable.DataRepresentation is VectorDataRepresentation)
                ApplyVector();
            ApplySimple();
            variable.Update();
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (variable.DataRepresentation is VectorDataRepresentation)
                LoadVector();
            else
                TabControlMain.Items.Remove(TabItemVectorProperties);
            LoadSimple();
        }

        private void LoadVector()
        {
            switch ((variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem)
            {
                case EnumerationSystem.Bin:
                    RadioButtonBin.IsChecked = true;
                    break;
                case EnumerationSystem.Oct:
                    RadioButtonOct.IsChecked = true;
                    break;
                case EnumerationSystem.Dec:
                    RadioButtonDec.IsChecked = true;
                    break;
                case EnumerationSystem.Hex:
                    RadioButtonHex.IsChecked = true;
                    break;
                case EnumerationSystem.ASCII:
                    RadioButtonASCII.IsChecked = true;
                    break;
            }

            switch ((variable.DataRepresentation as VectorDataRepresentation).DecimalDataPresentation)
            {
                case DecimalDataPresentation.Unsigned:
                    ComboBoxDecimalDataPresentation.SelectedIndex = 0;
                    break;
                case DecimalDataPresentation.Complement:
                    ComboBoxDecimalDataPresentation.SelectedIndex = 1;
                    break;
                case DecimalDataPresentation.Twos_Сomplement:
                    ComboBoxDecimalDataPresentation.SelectedIndex = 2;
                    break;
                default:
                    break;
            }

            CheckBoxIsReorderBits.IsChecked = (variable.DataRepresentation as VectorDataRepresentation).IsReorderedBits;
            CheckBoxAnalog.IsChecked = (variable.DataRepresentation as VectorDataRepresentation).IsAnalog;
        }

        private void LoadSimple()
        {
            CheckBoxIsInverted.IsChecked = variable.DataRepresentation.IsInverted;


            CheckBoxShowSize.IsChecked = variable.ShowSize == System.Windows.Visibility.Visible;
            CheckBoxShowTypeName.IsChecked = variable.ShowTypeName == System.Windows.Visibility.Visible;

            CheckBoxAnalog.IsChecked = variable.DataRepresentation.IsAnalog == true;
        }

        private void ApplyVector()
        {
            if (RadioButtonBin.IsChecked == true)
                (variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem = EnumerationSystem.Bin;
            if (RadioButtonOct.IsChecked == true)
                (variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem = EnumerationSystem.Oct;
            if (RadioButtonDec.IsChecked == true)
                (variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem = EnumerationSystem.Dec;
            if (RadioButtonHex.IsChecked == true)
                (variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem = EnumerationSystem.Hex;
            if (RadioButtonASCII.IsChecked == true)
                (variable.DataRepresentation as VectorDataRepresentation).EnumerationSystem = EnumerationSystem.ASCII;

            switch ((ComboBoxDecimalDataPresentation.SelectedItem as ComboBoxItem).Content as string)
            {
                case "Unsigned":
                    (variable.DataRepresentation as VectorDataRepresentation).DecimalDataPresentation = DecimalDataPresentation.Unsigned;
                    break;
                case "Complement":
                    (variable.DataRepresentation as VectorDataRepresentation).DecimalDataPresentation = DecimalDataPresentation.Complement;
                    break;
                case "Two's complement":
                    (variable.DataRepresentation as VectorDataRepresentation).DecimalDataPresentation = DecimalDataPresentation.Twos_Сomplement;
                    break;
                default: break;
            }

            (variable.DataRepresentation as VectorDataRepresentation).IsReorderedBits = CheckBoxIsReorderBits.IsChecked == true;
        }

        private void ApplySimple()
        {
            variable.DataRepresentation.IsInverted = CheckBoxIsInverted.IsChecked == true;

            variable.ShowSize = (CheckBoxShowSize.IsChecked == true) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            variable.ShowTypeName = (CheckBoxShowTypeName.IsChecked == true) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            variable.DataRepresentation.IsAnalog = CheckBoxAnalog.IsChecked == true;
        }
    }
}
