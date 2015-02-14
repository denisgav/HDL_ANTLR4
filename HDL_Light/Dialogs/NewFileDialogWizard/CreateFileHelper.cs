using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Schematix.FSM;
using Schematix.Core;

namespace Schematix.Dialogs.NewFileDialogWizard
{
    internal class CreateFileHelper
    {

        internal static ProjectExplorer.VHDL_Code_File CreateEmptyVHDLCodeFile(string p, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            ProjectExplorer.VHDL_Code_File newFile = ProjectExplorer.VHDL_Code_File.CreateFile(p, string.Empty, projectFolder);
            core.SaveSolution();
            core.UpdateExplorerPanel();
            return newFile;
        }

        internal static ProjectExplorer.Verilog_Code_File CreateEmptyVerilogCodeFile(string p, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            ProjectExplorer.Verilog_Code_File newFile = ProjectExplorer.Verilog_Code_File.CreateFile(p, string.Empty, projectFolder);
            core.SaveSolution();
            core.UpdateExplorerPanel();
            return newFile;
        }

        internal static ProjectExplorer.EDR_File CreateEmptyEDRFile(string p, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, string.Concat(p, ".edr"));

            EntityDrawning.EntityDrawningCore edr_core = new EntityDrawning.EntityDrawningCore(null);
            edr_core.Picture.SaveProject(path);

            ProjectExplorer.EDR_File newFile = new ProjectExplorer.EDR_File(path, projectFolder);
            projectFolder.AddElement(newFile);
            core.SaveSolution();
            core.UpdateExplorerPanel();

            return newFile;
        }

        internal static Schematix.ProjectExplorer.FSM_File CreateEmptyFSM(string p, FSM_Language fSM_Language, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, string.Concat(p, ".fsm"));
            string name = p;
            Constructor_Core cc = new Constructor_Core(null);
            My_Graph graph = cc.Graph;
            if (fSM_Language == FSM_Language.VHDL)
                graph.VHDLModule = new VHDL_Module() { ArchitectureName = name, EntityName = name };
            if (fSM_Language == FSM_Language.Verilog)
                graph.VerilogModule = new Verilog_Module() { ModuleName = name };
            graph.Language = fSM_Language;
            cc.SaveToFile(path);

            Schematix.ProjectExplorer.FSM_File fsm = new ProjectExplorer.FSM_File(path, projectFolder);
            projectFolder.AddElement(fsm);

            core.SaveSolution();
            core.UpdateExplorerPanel();
            return fsm;
        }

        public static ProjectExplorer.Schema_File CreateEmptyScheme(string p, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, string.Concat(p, ".csx"));
            string name = p;

            ProjectExplorer.Schema_File newFile = new ProjectExplorer.Schema_File(path, projectFolder);
            Schematix.Windows.Schema.Schema schema = new Windows.Schema.Schema(newFile);
            schema.Save();
            projectFolder.AddElement(newFile);
            core.SaveSolution();
            core.UpdateExplorerPanel();
            return newFile;
        }

        internal static ProjectExplorer.VHDL_Code_File CreateWizardVHDLCodeFile(string p, SchematixCore schematixCore, string text, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            ProjectExplorer.VHDL_Code_File newFile = ProjectExplorer.VHDL_Code_File.CreateFile(p, text, projectFolder);
            core.SaveSolution();
            core.UpdateExplorerPanel();
            return newFile;
        }

        internal static ProjectExplorer.Verilog_Code_File CreateWizardVerilogCodeFile(string p, string text, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            ProjectExplorer.Verilog_Code_File newFile = ProjectExplorer.Verilog_Code_File.CreateFile(p, text, projectFolder);
            core.SaveSolution();
            core.UpdateExplorerPanel();
            return newFile;
        }

        internal static Schematix.ProjectExplorer.FSM_File CreateWizardFSM(string p, VHDL_Module vHDL_Module, FSM_OptionsHelper fSM_OptionsHelper, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, string.Concat(p, ".fsm"));
            Constructor_Core cc = new Constructor_Core(null);
            Schematix.Windows.FSM.FSM_Utils.InitVHDLData(fSM_OptionsHelper, vHDL_Module, cc);
            cc.SaveToFile(path);

            Schematix.ProjectExplorer.FSM_File fsm = new ProjectExplorer.FSM_File(path, projectFolder);
            projectFolder.AddElement(fsm);

            core.SaveSolution();
            core.UpdateExplorerPanel();
            return fsm;
        }

        internal static Schematix.ProjectExplorer.FSM_File CreateWizardFSM(string p, SchematixCore schematixCore, Verilog_Module verilog_Module, FSM_OptionsHelper fSM_OptionsHelper, SchematixCore core, ProjectExplorer.ProjectFolder projectFolder)
        {
            string path = System.IO.Path.Combine(projectFolder.Path, string.Concat(p, ".fsm"));
            Constructor_Core cc = new Constructor_Core(null);
            Schematix.Windows.FSM.FSM_Utils.InitVerilogData(fSM_OptionsHelper, verilog_Module, cc);
            cc.SaveToFile(path);

            Schematix.ProjectExplorer.FSM_File fsm = new ProjectExplorer.FSM_File(path, projectFolder);
            projectFolder.AddElement(fsm);

            core.SaveSolution();
            core.UpdateExplorerPanel();
            return fsm;
        }
    }
}
