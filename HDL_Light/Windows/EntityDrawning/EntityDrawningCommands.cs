using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Schematix.Windows.EntityDrawning
{
    public static class EntityDrawningCommands
    {
        /// <summary>
        /// Bring To Front Command
        /// </summary>
        public static RoutedCommand BringToFront =
          new RoutedCommand("BringToFront", typeof(EntityDrawning), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F, ModifierKeys.Control) }));

        /// <summary>
        /// Bring SendTo Back
        /// </summary>
        public static RoutedCommand SendToBack =
          new RoutedCommand("SendToBack", typeof(EntityDrawning), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.B, ModifierKeys.Control) }));

    }
}
