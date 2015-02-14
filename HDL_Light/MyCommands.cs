using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Schematix
{
    public static class MyCommands
    {
        #region File Commands
        /// <summary>
        /// Command for creating new project.
        /// </summary>
        public static RoutedCommand NewProject =
          new RoutedCommand("NewProject", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.N, ModifierKeys.Control) }));

        /// <summary>
        /// Command for creating new project.
        /// </summary>
        public static RoutedCommand OpenProject =
          new RoutedCommand("OpenProject", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.O, ModifierKeys.Control) }));

        /// <summary>
        /// Command for Save.
        /// </summary>
        public static RoutedCommand Save =
          new RoutedCommand("Save", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Control) }));

        /// <summary>
        /// Command for Save As...
        /// </summary>
        public static RoutedCommand SaveAs =
          new RoutedCommand("SaveAs", typeof(MainWindow));

        /// <summary>
        /// Command for Save All.
        /// </summary>
        public static RoutedCommand SaveAll =
          new RoutedCommand("SaveAll", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift) }));

        /// <summary>
        /// Command for Close.
        /// </summary>
        public static RoutedCommand Close =
          new RoutedCommand("Close", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Control) }));

        /// <summary>
        /// Command for Close Project.
        /// </summary>
        public static RoutedCommand CloseProject =
          new RoutedCommand("CloseProject", typeof(MainWindow));

        /// <summary>
        /// Command for Exit.
        /// </summary>
        public static RoutedCommand Exit =
          new RoutedCommand("Exit", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F4, ModifierKeys.Alt) }));
        #endregion

        #region Edit Commands
        /// <summary>
        /// Command for Undo operation
        /// </summary>
        public static RoutedCommand Undo =
          new RoutedCommand("Undo", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Z, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Redo operation
        /// </summary>
        public static RoutedCommand Redo =
          new RoutedCommand("Redo", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Y, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Cut operation
        /// </summary>
        public static RoutedCommand Cut =
          new RoutedCommand("Cut", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.X, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Copy operation
        /// </summary>
        public static RoutedCommand Copy =
          new RoutedCommand("Copy", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.C, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Paste operation
        /// </summary>
        public static RoutedCommand Paste =
          new RoutedCommand("Paste", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.V, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Delete operation
        /// </summary>
        public static RoutedCommand Delete =
          new RoutedCommand("Delete", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Delete, ModifierKeys.None) }));
        /// <summary>
        /// Command for Select All operation
        /// </summary>
        public static RoutedCommand SelectAll =
          new RoutedCommand("SelectAll", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.A, ModifierKeys.Control) }));
        /// <summary>
        /// Command for Select All operation
        /// </summary>
        public static RoutedCommand Search =
          new RoutedCommand("Search", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.R, ModifierKeys.Control) }));
        #endregion

        #region View Commands
        /// <summary>
        /// Command for Open toolbox
        /// </summary>
        public static RoutedCommand Toolbox =
          new RoutedCommand("Toolbox", typeof(MainWindow));

        /// <summary>
        /// Command for Open project explorer
        /// </summary>
        public static RoutedCommand ProjectExplorer =
          new RoutedCommand("ProjectExplorer", typeof(MainWindow));

        /// <summary>
        /// Command for Open design browser
        /// </summary>
        public static RoutedCommand DesignBrowser =
          new RoutedCommand("DesignBrowser", typeof(MainWindow));

        /// <summary>
        /// Command for Open message window
        /// </summary>
        public static RoutedCommand MessageWindow =
          new RoutedCommand("MessageWindow", typeof(MainWindow));

        /// <summary>
        /// Command for Open console window
        /// </summary>
        public static RoutedCommand ConsoleWindow =
          new RoutedCommand("ConsoleWindow", typeof(MainWindow));

        /// <summary>
        /// Command for new console window
        /// </summary>
        public static RoutedCommand NewConsoleWindow =
          new RoutedCommand("NewConsoleWindow", typeof(MainWindow));
        #endregion

        #region Window  Commands
        /// <summary>
        /// Command for Open project explorer
        /// </summary>
        public static RoutedCommand CloseAll =
          new RoutedCommand("CloseAll", typeof(MainWindow));
        /// <summary>
        /// Command for Open project explorer
        /// </summary>
        public static RoutedCommand CloseAllButThis =
          new RoutedCommand("CloseAllButThis", typeof(MainWindow));
        /// <summary>
        /// Command for Open project explorer
        /// </summary>
        public static RoutedCommand ResetLayout =
          new RoutedCommand("ResetLayout", typeof(MainWindow));
        #endregion

        #region Help  Commands
        /// <summary>
        /// Command "Contents"
        /// </summary>
        public static RoutedCommand Contents =
          new RoutedCommand("Contents", typeof(MainWindow), new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.F1, ModifierKeys.None) }));
        /// <summary>
        /// Command "About"
        /// </summary>
        public static RoutedCommand About =
          new RoutedCommand("About", typeof(MainWindow));
        #endregion

        #region Compiler commands
        /// <summary>
        /// Command "CheckSyntax"
        /// </summary>
        public static RoutedCommand CheckSyntax =
          new RoutedCommand("CheckSyntax", typeof(MainWindow));
        /// <summary>
        /// Command "Compile"
        /// </summary>
        public static RoutedCommand Compile =
          new RoutedCommand("Compile", typeof(MainWindow));
        /// <summary>
        /// Command "Clear"
        /// </summary>
        public static RoutedCommand Clear =
          new RoutedCommand("Clear", typeof(MainWindow));
        /// <summary>
        /// Command "Rebuild Library"
        /// </summary>
        public static RoutedCommand RebuildLibrary =
          new RoutedCommand("Clear", typeof(MainWindow));
        #endregion

    }
}
