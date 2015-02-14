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

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for RandomParametersUserControl.xaml
    /// </summary>
    public partial class RandomParametersUserControl : UserControl
    {
        public RandomParametersUserControl()
        {
            InitializeComponent();
        }

        public void SetContinuousParameters()
        {
            TextBoxParametera.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParameterb.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParameterRange.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParameterm.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParametern.Mask_Type = MaskedTextBox.MaskType.IntegerNumber;
            TextBoxParameterp.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParameters.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
        }

        public void SetDiscreteParameters()
        {
            TextBoxParametera.Mask_Type = MaskedTextBox.MaskType.IntegerNumber;
            TextBoxParameterb.Mask_Type = MaskedTextBox.MaskType.IntegerNumber;
            TextBoxParameterRange.Mask_Type = MaskedTextBox.MaskType.IntegerNumber;
            TextBoxParameterm.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParametern.Mask_Type = MaskedTextBox.MaskType.IntegerNumber;
            TextBoxParameterp.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
            TextBoxParameters.Mask_Type = MaskedTextBox.MaskType.DoubleNumber;
        }

        public void ShowPanels(List<Grid> panels)
        {
            MainStackPanel.Children.Clear();
            foreach (Panel p in panels)
            {
                p.Visibility = System.Windows.Visibility.Visible;
                MainStackPanel.Children.Add(p);
            }
        }

        public void HideAllPanels()
        {
            GridParametera.Visibility = System.Windows.Visibility.Collapsed;
            GridParameterb.Visibility = System.Windows.Visibility.Collapsed;
            GridParameterDiapasone.Visibility = System.Windows.Visibility.Collapsed;
            GridParameterm.Visibility = System.Windows.Visibility.Collapsed;
            GridParametern.Visibility = System.Windows.Visibility.Collapsed;
            GridParameterp.Visibility = System.Windows.Visibility.Collapsed;
            GridParameters.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
