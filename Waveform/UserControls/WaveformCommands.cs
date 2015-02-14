using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Комманды, используемые в WaveformUserControl
    /// </summary>
    public static class WaveformCommands
    {
        /// <summary>
        /// Command for zoom in.
        /// </summary>
        public static RoutedCommand ZoomIn =
          new RoutedCommand("ZoomIn", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Add, ModifierKeys.Control) }));

        /// <summary>
        /// Command for zoom out.
        /// </summary>
        public static RoutedCommand ZoomOut =
          new RoutedCommand("ZoomOut", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Subtract, ModifierKeys.Control) }));

        /// <summary>
        /// Command for zoom to fit.
        /// </summary>
        public static RoutedCommand ZoomToFit =
          new RoutedCommand("ZoomToFit", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F, ModifierKeys.Control) }));

        /// <summary>
        /// Command for zoom to selection.
        /// </summary>
        public static RoutedCommand ZoomToSelection =
          new RoutedCommand("ZoomToSelection", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.H, ModifierKeys.Alt) }));

        /// <summary>
        /// Command set one generator.
        /// </summary>
        public static RoutedCommand SetOneGenerator =
          new RoutedCommand("SetOneGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.D1, ModifierKeys.Alt) }));

        /// <summary>
        /// Command set zero generator.
        /// </summary>
        public static RoutedCommand SetZeroGenerator =
          new RoutedCommand("SetZeroGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.D0, ModifierKeys.Alt) }));

        /// <summary>
        /// Command constant generator.
        /// </summary>
        public static RoutedCommand SetConstantGenerator =
          new RoutedCommand("SetConstantGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Alt) }));

        /// <summary>
        /// Command constant generator.
        /// </summary>
        public static RoutedCommand SetClockGenerator =
          new RoutedCommand("SetClockGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.T, ModifierKeys.Alt) }));

        /// <summary>
        /// Command random generator.
        /// </summary>
        public static RoutedCommand SetRandomGenerator =
          new RoutedCommand("SetRandomGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.R, ModifierKeys.Alt) }));

        /// <summary>
        /// Command counter generator.
        /// </summary>
        public static RoutedCommand SetCounterGenerator =
          new RoutedCommand("SetCounterGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.C, ModifierKeys.Alt) }));

        /// <summary>
        /// Command generator.
        /// </summary>
        public static RoutedCommand SetGenerator =
          new RoutedCommand("SetGenerator", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.G, ModifierKeys.Alt) }));

        /// <summary>
        /// Command correct selection inteval.
        /// </summary>
        public static RoutedCommand CorrectSelectionInterval =
          new RoutedCommand("CorrectSelectionInterval", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.I, ModifierKeys.Control) }));

        /// <summary>
        /// Command start simulation.
        /// </summary>
        public static RoutedCommand StartSimulation =
          new RoutedCommand("StartSimulation", typeof(WaveformUserControl), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F5, ModifierKeys.Control) }));
    }
}
