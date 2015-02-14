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


using DataContainer;
using DataContainer.Generator;
using Schematix.Waveform.Value_Dump;
using DataContainer.ValueDump;
using DataContainer.Generator.Random.Continuous;
using DataContainer.Generator.Random.Discrete;
using DataContainer.Generator.Counters;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for GeneratorDialog.xaml
    /// </summary>
    public partial class GeneratorDialog : Window
    {
        /// <summary>
        /// Настройки генератора
        /// </summary>
        private GeneratorSettings genSettings;
        public GeneratorSettings GenSettings
        {
            get { return genSettings; }
            set 
            {
                genSettings = value;
                UpdateGeneratorSettings();
            }
        }

        /// <summary>
        /// Генератор, который является выходними данными для данного диалогового окна
        /// </summary>
        private BaseGenerator generator;
        public BaseGenerator Generator
        {
            get { return generator; }
            set { generator = value; }
        }

        public GeneratorDialog() 
        {
            InitializeComponent(); 
        }

        public GeneratorDialog(GeneratorSettings GenSettings)
        {
            InitializeComponent();
            this.GenSettings = GenSettings;
        }

        public GeneratorDialog(My_Variable variable)
        {
            InitializeComponent();
            this.GenSettings = ModellingType.GetGeneratorSettings(variable.Signal.Type);
        }

        public GeneratorDialog(My_Variable variable, GeneratorType genType)
        {
            InitializeComponent();
            this.GenSettings = ModellingType.GetGeneratorSettings(variable.Signal.Type);
            switch (genType)
            {
                case GeneratorType.Constant:
                    TabControlGenerator.SelectedItem = TabItemConstantGenerator;
                    break;
                case GeneratorType.Clock:
                    TabControlGenerator.SelectedItem = TabItemClockGenerator;
                    break;
                case GeneratorType.Counter:
                    TabControlGenerator.SelectedItem = TabItemCounterGenerator;
                    break;
                case GeneratorType.DiscretteRandom:
                    TabControlGenerator.SelectedItem = TabItemRandomGeneratorDiscrette;
                    break;
                case GeneratorType.ContinousRandom:
                    TabControlGenerator.SelectedItem = TabItemRandomGeneratorContinuous;
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Эта функция вызывается при обновлении настроек генератора
        /// </summary>
        private void UpdateGeneratorSettings()
        {

            //Отображение/скрытие всех вкладок
            TabControlGenerator.Items.Clear();

            if (
                ((genSettings.GenValue & GeneratorSettings.GeneratedValue.IntegerValue) != 0) ||
                (genSettings.GenValue == GeneratorSettings.GeneratedValue.IntegerValue) ||
                ((genSettings.GenValue & GeneratorSettings.GeneratedValue.DoubleValue) != 0) ||
                (genSettings.GenValue == GeneratorSettings.GeneratedValue.DoubleValue) ||
                ((genSettings.GenValue & GeneratorSettings.GeneratedValue.BoolArray) != 0) ||
                (genSettings.GenValue == GeneratorSettings.GeneratedValue.BoolArray)
                )
            {
                TabControlGenerator.Items.Add(TabItemConstantGenerator);
                TabControlGenerator.Items.Add(TabItemCounterGenerator);
                TabControlGenerator.Items.Add(TabItemRandomGeneratorContinuous);
                TabControlGenerator.Items.Add(TabItemRandomGeneratorDiscrette);
            }


            if (((genSettings.GenValue & GeneratorSettings.GeneratedValue.EnumerableValue) != 0) || (genSettings.GenValue == GeneratorSettings.GeneratedValue.EnumerableValue))
            {
                TabControlGenerator.Items.Add(TabItemClockGenerator);
                TabControlGenerator.Items.Add(TabItemConstantGenerator);
            }
            ////////////////////////////////////////////////////

            //Настройка вкладки TabItemConstantGenerator
            switch (genSettings.PrivilegeValue)
            {
                case GeneratorSettings.GeneratedValue.EnumerableValue:
                    {
                        GroupBoxConstantEnumerativeValue.Visibility = System.Windows.Visibility.Visible;
                        GroupBoxConstantIntegerValue.Visibility = System.Windows.Visibility.Collapsed;
                        GroupBoxConstantRealValue.Visibility = System.Windows.Visibility.Collapsed;

                        ComboboxConstantenumerativeValue.Items.Clear();
                        foreach (object obj in genSettings.EnumerableValues)
                        {
                            ComboboxConstantenumerativeValue.Items.Add(obj);
                        }
                        ComboboxConstantenumerativeValue.SelectedIndex = 0;
                    }
                    break;
                case GeneratorSettings.GeneratedValue.IntegerValue:
                    {
                        GroupBoxConstantEnumerativeValue.Visibility = System.Windows.Visibility.Collapsed;
                        GroupBoxConstantIntegerValue.Visibility = System.Windows.Visibility.Visible;
                        GroupBoxConstantRealValue.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                case GeneratorSettings.GeneratedValue.DoubleValue:
                    {
                        GroupBoxConstantEnumerativeValue.Visibility = System.Windows.Visibility.Collapsed;
                        GroupBoxConstantIntegerValue.Visibility = System.Windows.Visibility.Collapsed;
                        GroupBoxConstantRealValue.Visibility = System.Windows.Visibility.Visible;
                    }
                    break;
                case GeneratorSettings.GeneratedValue.BoolArray:
                    {
                        GroupBoxConstantEnumerativeValue.Visibility = System.Windows.Visibility.Collapsed;
                        GroupBoxConstantIntegerValue.Visibility = System.Windows.Visibility.Visible;
                        GroupBoxConstantRealValue.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timeUnitUserControlClock.TimeInterval = new TimeInterval(4, TimeUnit.fs);
            randomParametersContinuousRandom.SetContinuousParameters();
            randomParametersDiscretteRandom.SetDiscreteParameters();
            //UpdateGeneratorSettings();
            RadiobuttonConstantBin.IsChecked = true;
            RadiobuttonCounterBin.IsChecked = true;
            RadiobuttonCounterBinary.IsChecked = true;
            RadiobuttonCounterUp.IsChecked = true;
            RadiobuttonContinuousSimple.IsChecked = true;
            RadiobuttonDiscretteSimple.IsChecked = true;
        }

        private void RadiobuttonConstantBin_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxConstantInteger.Bus = EnumerationSystem.Bin;
        }

        private void RadiobuttonConstantOct_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxConstantInteger.Bus = EnumerationSystem.Oct;
        }

        private void RadiobuttonConstantDec_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxConstantInteger.Bus = EnumerationSystem.Dec;
        }

        private void RadiobuttonConstantHex_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxConstantInteger.Bus = EnumerationSystem.Hex;
        }

        private void RadiobuttonCounterBin_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxCounterStartValue.Bus = EnumerationSystem.Bin;
        }

        private void RadiobuttonCounterOct_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxCounterStartValue.Bus = EnumerationSystem.Oct;
        }

        private void RadiobuttonCounterDec_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxCounterStartValue.Bus = EnumerationSystem.Dec;
        }

        private void RadiobuttonCounterHex_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxCounterStartValue.Bus = EnumerationSystem.Hex;
        }

        private void RadiobuttonContinuousChisquare_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Chisquare.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParametern);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousErlang_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Erlang.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParametern);
            panels.Add(randomParametersContinuousRandom.GridParameterb);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousExponential_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Exponential.Description;
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParameterm);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousLognormal_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Lognormal.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParametera);
            panels.Add(randomParametersContinuousRandom.GridParameterb);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousNormal_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Normal.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParameterm);
            panels.Add(randomParametersContinuousRandom.GridParameters);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousStudent_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = Student.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParametern);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonContinuousSimple_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockContinuousDescription.Text = SimpleContinuous.Description;
            randomParametersContinuousRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersContinuousRandom.GridParameterDiapasone);
            randomParametersContinuousRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscretteBernoulli_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Bernoulli.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParameterp);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscretteBinomial_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Binomial.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParametern);
            panels.Add(randomParametersDiscretteRandom.GridParameterp);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscretteEquilikely_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Equilikely.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParametera);
            panels.Add(randomParametersDiscretteRandom.GridParameterb);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscretteGeometric_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Geometric.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParameterp);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscrettePascal_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Pascal.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParametern);
            panels.Add(randomParametersDiscretteRandom.GridParameterp);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscrettePoisson_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = Poisson.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParameterm);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void RadiobuttonDiscretteSimple_Checked(object sender, RoutedEventArgs e)
        {
            TextBlockDiscretteDescription.Text = SimpleDiscrete.Description;
            randomParametersDiscretteRandom.HideAllPanels();
            List<Grid> panels = new List<Grid>();
            panels.Add(randomParametersDiscretteRandom.GridParameterDiapasone);
            randomParametersDiscretteRandom.ShowPanels(panels);
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (TabControlGenerator.SelectedItem == TabItemClockGenerator)
            {
                if (GenerateClock() == true)
                {
                    DialogResult = true;
                    Close();
                    return;
                }
            }

            if (TabControlGenerator.SelectedItem == TabItemConstantGenerator)
            {
                if (GenerateConstant() == true)
                {
                    DialogResult = true;
                    Close();
                    return;
                }
            }
            if(TabControlGenerator.SelectedItem == TabItemCounterGenerator)
            {
                if (GenerateCounter() == true)
                {
                    DialogResult = true;
                    Close();
                    return;
                }
            }
            if (TabControlGenerator.SelectedItem == TabItemRandomGeneratorContinuous)
            {
                if (GenerateContinuousRandom() == true)
                {
                    DialogResult = true;
                    Close();
                    return;
                }
            }
            if (TabControlGenerator.SelectedItem == TabItemRandomGeneratorDiscrette)
            {
                if (GenerateDiscreteRandom() == true)
                {
                    DialogResult = true;
                    Close();
                    return;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
            return;
        }

        private bool GenerateClock()
        {
            Clock clock = new Clock();
            clock.DutyCycle = sliderClockDutyCycle.Value;
            if (RadioButtonClockOne.IsChecked == true)
                clock.StartValue = true;
            else
                clock.StartValue = false;

            if (timeUnitUserControlClock.TimeInterval.GetTimeUnitInFS() == 0)
            {
                MessageBox.Show("Time must be non zero", "Error!", MessageBoxButton.OK);
                return false;
            }

            clock.TimeStep = timeUnitUserControlClock.TimeInterval;            
            generator = clock;
            return true;
        }

        private bool GenerateConstant()
        {
            Constant constant = new Constant();
            constant.GenValue = genSettings.PrivilegeValue;
            constant.IntegerValue = TextBoxConstantInteger.GetIntegerValue();
            constant.DoubleValue = TextBoxConstantInteger.GetDoubleValue();
            constant.EnumerableValue = ComboboxConstantenumerativeValue.SelectedItem;
            constant.SizeVector = genSettings.Size;
            generator = constant;
            return true;
        }

        private bool GenerateCounter()
        {
            Counter counter = null;

            if (RadiobuttonCounterBinary.IsChecked == true)
                counter = new BinaryCounter();
            if (RadiobuttonCounterGray.IsChecked == true)
                counter = new GrayCounter();
            if (RadiobuttonCounterJohnson.IsChecked == true)
                counter = new JohnsonCounter();
            if (RadiobuttonCounterOneHot.IsChecked == true)
                counter = new Circular1();
            if (RadiobuttonCounterOneZero.IsChecked == true)
                counter = new Circular0();

            counter.IsUpDirection = RadiobuttonCounterUp.IsChecked == true;
            counter.StepCount = (uint)TextBoxCounterChangeBy.GetIntegerValue();
            counter.CurrentValue = DataConvertorUtils.ToBitArray(TextBoxCounterStartValue.GetIntegerValue(), (int)genSettings.Size);
            if (counter.CurrentValue == null)
            {
                MessageBox.Show(string.Format("You have errors, when enter StartValue for {0}. It must be one \'0\' and other \'1\'(Circular0) or one \'1\' and other \'0\'(Circular1)",counter.ToString()),"Error!");
                return false;
            }

            if (timeUnitUserControlClock.TimeInterval.GetTimeUnitInFS() == 0)
            {
                MessageBox.Show("Time must be non zero", "Error!", MessageBoxButton.OK);
                return false;
            }

            counter.TimeStep = timeUnitUserControlCounter.TimeInterval;

            generator = counter;
            return true;
        }

        private bool GenerateContinuousRandom()
        {
            My_Random_Continuous_Base rand = null;
            if (RadiobuttonContinuousChisquare.IsChecked == true)
            {
                long n = randomParametersContinuousRandom.TextBoxParametern.GetIntegerValue();
                rand = new Chisquare(n);
            }
            if (RadiobuttonContinuousErlang.IsChecked == true)
            {
                long n = randomParametersContinuousRandom.TextBoxParametern.GetIntegerValue();
                double b = randomParametersContinuousRandom.TextBoxParameterb.GetDoubleValue();
                rand = new Erlang(n, b);
            }
            if (RadiobuttonContinuousExponential.IsChecked == true)
            {
                double m = randomParametersContinuousRandom.TextBoxParameterm.GetDoubleValue();
                rand = new Exponential(m);
            }
            if (RadiobuttonContinuousLognormal.IsChecked == true)
            {
                double a = randomParametersContinuousRandom.TextBoxParametera.GetDoubleValue();
                double b = randomParametersContinuousRandom.TextBoxParameterb.GetDoubleValue();
                rand = new Lognormal(a, b);
            }
            if (RadiobuttonContinuousNormal.IsChecked == true)
            {
                double m = randomParametersContinuousRandom.TextBoxParameterm.GetDoubleValue();
                double s = randomParametersContinuousRandom.TextBoxParameters.GetDoubleValue();
                rand = new Normal(m, s); 
            }
            if (RadiobuttonContinuousSimple.IsChecked == true)
            {
                double d = randomParametersContinuousRandom.TextBoxParameterRange.GetDoubleValue();
                rand = new SimpleContinuous(d);
            }
            if (RadiobuttonContinuousStudent.IsChecked == true)
            {
                long n = randomParametersContinuousRandom.TextBoxParametern.GetIntegerValue();
                rand = new Student(n);
            }
            rand.SizeVector = genSettings.Size;

            if (timeUnitUserControlClock.TimeInterval.GetTimeUnitInFS() == 0)
            {
                MessageBox.Show("Time must be non zero", "Error!", MessageBoxButton.OK);
                return false;
            }

            rand.TimeStep = timeUnitUserControlContinuousRandom.TimeInterval;

            generator = rand;
            return true;
        }

        private bool GenerateDiscreteRandom()
        {
            My_Random_Discrete_Base rand = null;
            if (RadiobuttonDiscretteBernoulli.IsChecked == true)
            {
                double p = randomParametersDiscretteRandom.TextBoxParameterp.GetDoubleValue();
                rand = new Bernoulli(p);
            }
            if (RadiobuttonDiscretteBinomial.IsChecked == true)
            {
                long n = randomParametersDiscretteRandom.TextBoxParametern.GetIntegerValue();
                double p = randomParametersDiscretteRandom.TextBoxParameterp.GetDoubleValue();
                rand = new Binomial(n, p);
            }
            if (RadiobuttonDiscretteEquilikely.IsChecked == true)
            {
                long a = randomParametersDiscretteRandom.TextBoxParametera.GetIntegerValue();
                long b = randomParametersDiscretteRandom.TextBoxParameterb.GetIntegerValue();
                rand = new Equilikely(a, b);
            }
            if (RadiobuttonDiscretteGeometric.IsChecked == true)
            {
                double p = randomParametersDiscretteRandom.TextBoxParameterp.GetDoubleValue();
                rand = new Geometric(p);
            }
            if (RadiobuttonDiscrettePascal.IsChecked == true)
            {
                long n = randomParametersDiscretteRandom.TextBoxParametern.GetIntegerValue();
                double p = randomParametersDiscretteRandom.TextBoxParameterp.GetDoubleValue();
                rand = new Pascal(n, p);
            }
            if (RadiobuttonDiscrettePoisson.IsChecked == true)
            {
                double m = randomParametersDiscretteRandom.TextBoxParameterm.GetDoubleValue();
                rand = new Poisson(m);
            }
            if (RadiobuttonDiscretteSimple.IsChecked == true)
            {
                int d = randomParametersDiscretteRandom.TextBoxParameterRange.GetIntegerValue();
                rand = new SimpleDiscrete(d);
            }
            rand.SizeVector = genSettings.Size;

            if (timeUnitUserControlClock.TimeInterval.GetTimeUnitInFS() == 0)
            {
                MessageBox.Show("Time must be non zero", "Error!", MessageBoxButton.OK);
                return false;
            }

            rand.TimeStep = timeUnitUserControlDiscretteRandom.TimeInterval;

            generator = rand;
            return true;
        }
    }
}
