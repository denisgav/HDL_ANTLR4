using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Schematix.ProjectExplorer;
using Schematix.DesignBrowser;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for ToolBoxPanel.xaml
    /// </summary>
    public partial class ToolBoxPanel : SchematixPanelBase
    {
        private Schematix.Windows.SchematixBaseWindow ActiveForm;

        public ToolBoxPanel()
        {
            InitializeComponent();
            this.IconSource = new BitmapImage(new Uri("Images/ToolBox.png", UriKind.Relative));
            toolBox.ToggleButtonClick += new ToolBoxWindow.ToolBox.ToggleButtonClickDelegate(toolBox_ToggleButtonClick);
        }

        void toolBox_ToggleButtonClick(object sender, EventArgs e)
        {
            if (ActiveForm is Schematix.Windows.FSM.FSM)
            {
                (ActiveForm as Schematix.Windows.FSM.FSM).SetMode(toolBox.selectedToolBoxItem.Command);
                return;
            }
            if (ActiveForm is Schematix.Windows.EntityDrawning.EntityDrawning)
            {
                (ActiveForm as Schematix.Windows.EntityDrawning.EntityDrawning).SetMode(toolBox.selectedToolBoxItem.Command);
                return;
            }
            if (ActiveForm is Schematix.Windows.Schema.Schema)
            {
                (ActiveForm as Schematix.Windows.Schema.Schema).schemaUserControl.SetMode(toolBox.selectedToolBoxItem.Command);
            }
        }

        public override void OnActivateChild(Schematix.Windows.SchematixBaseWindow form)
        {
            //if (form == ActiveForm)
            //    return;

            ActiveForm = form;
            if (form == null)
            {
                toolBox.SelectContent("null");
                return;
            }

            if (form is Schematix.Windows.Code.Code)
            {
                Schematix.Windows.Code.Code c = form as Schematix.Windows.Code.Code;
                if ((c.textEditor.SyntaxHighlighting != null) && (c.textEditor.SyntaxHighlighting.Name.Equals("VHDL")))
                    toolBox.SelectContent("VHDLTextTemplates");
                if ((c.textEditor.SyntaxHighlighting != null) && (c.textEditor.SyntaxHighlighting.Name.Equals("Verilog")))
                    toolBox.SelectContent("VerilogTextTemplates");
                return;
            }

            if (form is Schematix.Windows.EntityDrawning.EntityDrawning)
            {
                toolBox.SelectContent("EntityDrawning");
                return;
            }

            if (form is Schematix.Windows.FSM.FSM)
            {
                toolBox.SelectContent("FSM");
                return;
            }

            if (form is Schematix.Windows.Schema.Schema)
            {
                if((SchematixCore.Core == null) || (SchematixCore.Core.CurrentSelectedProject == null))
                {
                    toolBox.SelectContent("none");
                    return;
                }

                List<Schematix.ProjectExplorer.EDR_File> Files = SchematixCore.Core.CurrentSelectedProject.GetProjectElements<Schematix.ProjectExplorer.EDR_File>();
                List<string> SchemaFiles = new List<string>();
                foreach (Schematix.ProjectExplorer.EDR_File f in Files)
                    SchemaFiles.Add(f.Path);

                toolBox.SelectedType = "Schema";
                ToolBoxWindow.ExpanderData exp = new ToolBoxWindow.ExpanderData();
                exp.IconPath = "EntityDrawning.jpg";
                exp.TypeOfElements = "EntityDrawning";
                exp.Description = "Entity Drawning elements avaliable in this project";
                exp.Caption = "EntityDrawning Items";
                foreach (string file in SchemaFiles)
                {
                    ToolBoxWindow.ToolBoxItem item = new ToolBoxWindow.ToolBoxItem(exp);
                    item.Caption = System.IO.Path.GetFileName(file);
                    item.Command = file;
                    item.Description = "Insert " + file + " entity drawning file";
                    item.IconPath = "EntityDrawning.jpg";
                    exp.Items.Add(item);
                }
                exp.CreateExpander();
                List<ToolBoxWindow.ExpanderData> expanders = new List<ToolBoxWindow.ExpanderData>();
                expanders.Add(exp);
                toolBox.LoadExpanders(expanders);
                return;
            }
            toolBox.SelectContent("none");
        }
    }
}
