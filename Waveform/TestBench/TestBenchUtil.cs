using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser;
using Schematix.Waveform;
using System.IO;
using System.Windows.Forms;
using DataContainer;
using DataContainer.Generator.Random;

namespace Schematix.Waveform.TestBench
{
    internal class TestBenchUtil
    {
        private static string LibraryHeader()
        {
            StringBuilder res = new StringBuilder();
            res.Append("library ieee;").Append("\n");
            res.Append("use ieee.std_logic_1164.all;").Append("\n");
            res.Append("use ieee.std_logic_unsigned.all;").Append("\n\n");
            return res.ToString();
        }
        private static string EntityDeclarate()
        {
            StringBuilder res = new StringBuilder();
            res.Append("entity test1 is").Append("\n");
            res.Append("end test1;").Append("\n");
            return res.ToString();
        }
        private static string EndTime(TimeInterval time)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\nconstant END_TIME: TIME := ").Append(time.TimeNumber).Append(" ").Append(time.Unit).Append(" ;").Append("\n");
            return res.ToString(); 
        }
        private static string ArchitectureDeclarate(List<PortGenerator> lpg, string arch ,TimeInterval time)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\n\narchitecture test1 of test1 is ").Append("\n");
            res.Append(EndTime(time)).Append("\n");
            res.Append("component ").Append(lpg[0].Entity.name).Append(" is ").Append("\n");
            res.Append(lpg[0].Entity).Append("\n");
            res.Append("end component;").Append("\n");
            res.Append(EntityArchCom(lpg,arch));
            foreach (PortGenerator pg in lpg)
            {
                res.Append(SignalDeclarate(pg));
            }
            res.Append("\n").Append("begin");
            return res.ToString(); 
        }
        private static string SignalDeclarate(PortGenerator pg)
        {
            StringBuilder res = new StringBuilder();
            
            res.Append("\n").Append("signal ").Append(pg.Name).Append(" : ").Append(pg.Port.subtype.sub_type_indication_string.ToLower());
            if (pg.Port.subtype.hasRange)
            {
                if (pg.Port.subtype.range.isReverseRange)
                    res.Append(" ( " + pg.Port.subtype.range.leftRange + " downto " + pg.Port.subtype.range.rightRange + " ) ");    
                else
                    res.Append(" ( " + pg.Port.subtype.range.leftRange + " to " + pg.Port.subtype.range.rightRange + " ) ");
            }
            if(pg.Generator != null && !(pg.Generator is My_Random_Base))
            {
                res.Append(" := ").Append(pg.Generator.GetStringStartValue()).Append(" ;");
            }
            else
                res.Append(";");
            return res.ToString();
        }

        private static string EntityArchCom(List<PortGenerator> lpg,string arch)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\r\n").Append("for all: ").Append(lpg[0].Entity.name).Append(" use entity ").Append(lpg[0].Entity.name).Append("(").Append(arch).Append(") ; ").Append("\r\n");
            return res.ToString();
        }

        private static string PortMapDeclarate(List<PortGenerator> lpg)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\n").Append("UUT: ").Append(lpg[0].Entity.name).Append(" port map ( ").Append("\n");
            int count = 0;
            foreach (PortGenerator pg in lpg)
            {
                if(count>lpg.Count-2)
                    res.Append("\n").Append(pg.Name).Append(" => ").Append(pg.Name).Append("\n").Append(");").Append("\n");
                else
                    res.Append("\n").Append(pg.Name).Append(" => ").Append(pg.Name).Append(",").Append("\n");
                count++;
            }
            res.Append("\n");
            return res.ToString();
        }
        private static string ArchitectureBodyDeclarate(List<PortGenerator> slpg, TimeInterval time)
        {
            StringBuilder res = new StringBuilder();
            foreach (PortGenerator pg in slpg)
            {
                if(pg.Generator != null)
                    res.Append(pg.Generator.StringVhdlRealization(new KeyValuePair<string,TimeInterval>(pg.Name,time)).ToString());
            }
            res.Append("\n").Append("end test1;");
            return res.ToString();
        }
        public static string GenerateTestbenchAsString(List<PortGenerator> lpg, List<PortGenerator> slpg,string arch,TimeInterval time)
        {
            StringBuilder res = new StringBuilder();
            
            res.Append(LibraryHeader());
            res.Append(EntityDeclarate());
            res.Append(ArchitectureDeclarate(lpg,arch,time));
            res.Append(PortMapDeclarate(lpg));
            res.Append(ArchitectureBodyDeclarate(slpg,time));
            return res.ToString();
        }

        //--------------------------------------------------------------------------------------------------------------------
        private static void LibraryHeader(StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            res.Append("library ieee;").Append("\n");
            res.Append("use ieee.std_logic_1164.all;").Append("\n");
            res.Append("use ieee.std_logic_unsigned.all;").Append("\n\n");
            sw.Write(res);
        }
        private static void EntityDeclarate(StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            res.Append("entity test1 is").Append("\n");
            res.Append("end test1;").Append("\n");
            sw.Write(res);
        }
        private static void EndTime(TimeInterval time, StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\nconstant END_TIME: TIME := ").Append(time.TimeNumber).Append(" ").Append(time.Unit).Append(" ;").Append("\n");
            sw.Write(res);
        }

        private static void EntityArchCom(List<PortGenerator> lpg, string arch, StreamWriter sw)
        {
            sw.Write(EntityArchCom(lpg, arch));
        }

        private static void ArchitectureDeclarate(List<PortGenerator> lpg, TimeInterval time,string arch,StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\n\narchitecture test1 of test1 is ").Append("\n");
            res.Append(EndTime(time)).Append("\n");
            res.Append("component ").Append(lpg[0].Entity.name).Append(" is ").Append("\n");
            res.Append(lpg[0].Entity).Append("\n");
            res.Append("end component;").Append("\n");
            res.Append(EntityArchCom(lpg, arch));
            foreach (PortGenerator pg in lpg)
            {
                res.Append(SignalDeclarate(pg));
            }
            res.Append("\n").Append("begin");
            sw.Write(res);
        }

        private static void PortMapDeclarate(List<PortGenerator> lpg, StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            res.Append("\n").Append("UUT: ").Append(lpg[0].Entity.name).Append(" port map ( ").Append("\n");
            int count = 0;
            foreach (PortGenerator pg in lpg)
            {
                if (count > lpg.Count - 2)
                    res.Append("\n").Append(pg.Name).Append(" => ").Append(pg.Name).Append("\n").Append(");").Append("\n");
                else
                    res.Append("\n").Append(pg.Name).Append(" => ").Append(pg.Name).Append(",").Append("\n");
                count++;
            }
            res.Append("\n");
            sw.Write(res);
        }
        private static void ArchitectureBodyDeclarate(List<PortGenerator> slpg, TimeInterval time, StreamWriter sw)
        {
            StringBuilder res = new StringBuilder();
            foreach (PortGenerator pg in slpg)
            {
                if (pg.Generator != null)
                    pg.Generator.StreamVhdlRealization(new KeyValuePair<string, TimeInterval>(pg.Name, time), sw);
            }
            res.Append("\n").Append("end test1;");
            sw.Write(res);
        }
           
        public static void GenerateTestbenchAsStream(List<PortGenerator> lpg, List<PortGenerator> slpg, string arch,TimeInterval time, StreamWriter sw)
        {
            sw.AutoFlush = true;
            LibraryHeader(sw);
            EntityDeclarate(sw);
            ArchitectureDeclarate(lpg,time,arch, sw);
            PortMapDeclarate(lpg, sw);
            ArchitectureBodyDeclarate(slpg, time, sw);
            sw.Close();
        }
    }
}
