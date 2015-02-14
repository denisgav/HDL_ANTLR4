using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ELW.Library.Math;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;
using ELW.Library.Math.Tools;

namespace Parser
{
    public class Parser :IDisposable
    {
        private FileStream fs;
        private StreamReader sr;
        public List<Entity> EntityList;
        public List<Architecture> ArchitectureList;
        public List<ArchitectureBlockDeclarativeItem> architecture_items;
        
   
        public Parser(String fileName)
        {
            Initializing(fileName);
        }

        public void Parse()
        {
            List<string> entities = new List<string>();
            List<string> generics = new List<string>();
            List<string> ports = new List<string>();
            List<string> architectures = new List<string>();

            SelecEntityAndArchitectureBlocks(ref entities, ref architectures);//выбираем строковые фрагменты, содержащие entity и architecture
            //ParseArchitectures(architectures);//парсим область декларации архитектуры
            ParseEntities(entities, ref generics, ref ports);//парсим энитити (парсим generics и ports)
        }
        
        private void Initializing(String FileName)
        {
            fs = new FileStream(FileName, FileMode.Open);
            sr = new StreamReader(fs);
            Globals.InitTypes();
            EntityList = new List<Entity>();
            ArchitectureList = new List<Architecture>();
            architecture_items = new List<ArchitectureBlockDeclarativeItem>();
        }

        private void ParseEntities(List<string> entities, ref List<string> generics, ref List<string> ports)
        {
            foreach (string FormnatedEntity in entities)
            {
                ParseEntity(ref generics, ref ports, FormnatedEntity);
            }
        }

        private void ParseEntity(ref List<string> generics, ref List<string> ports, string FormnatedEntity)
        {
            Entity Entity_temp = EntityInit(FormnatedEntity);
            generics = ParseGenerics(generics, FormnatedEntity, Entity_temp);
            ports = ParsePorts(ports, FormnatedEntity, Entity_temp); 
            EntityList.Add(Entity_temp);
        }

        private static List<string> ParsePorts(List<string> ports, string FormnatedEntity, Entity Entity_temp)
        {
            Globals.FormatedSelectFragmentsWithBlocks(FormnatedEntity, "port", ';', out ports);
            ParsePortDeclarations(ports, Entity_temp);
            if (ports.Count != 0)
            {
                StringBuilder strPorts = new StringBuilder().Append("port ").Append(Globals.MakeStringFromList(ports)).Append(";");
                Entity_temp.entityDeclarativePart += strPorts.ToString() + "\n";
            }
            return ports;
        }

        private static List<string> ParseGenerics(List<string> generics, string FormnatedEntity, Entity Entity_temp)
        {
            Globals.FormatedSelectFragmentsWithBlocks(FormnatedEntity, "generic", ';', out generics);
            ParseGenericItems(generics, Entity_temp);
            if (generics.Count != 0)
            {
                StringBuilder strGenerics = new StringBuilder().Append("generic ").Append(Globals.MakeStringFromList(generics)).Append(";");
                Entity_temp.entityDeclarativePart = strGenerics.ToString() + "\n";
            }
            return generics;
        }

        private static Entity EntityInit(string FormnatedEntity)
        {
            Entity Entity_temp = new Entity(FormnatedEntity.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(0));

            return Entity_temp;
        }

        private static void ParsePortDeclarations(List<string> portDeclarations, Entity Entity_temp)
        {
            foreach (string portDeclarationString in portDeclarations)
            {
                ParsePortDeclarartion(Entity_temp, portDeclarationString);
            }
        }

        private static void ParsePortDeclarartion(Entity Entity_temp, string portItemString)
        {
            portItemString = RemoveMainParens(portItemString);
            List<string> interfaceItems = new List<string>(portItemString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            foreach (string interfaceItemString in interfaceItems)
            {
                List<String> FormatedCode;
                Globals.FormatText(interfaceItemString, out FormatedCode);
                Entity_temp.AddPortItem(new PortItem(FormatedCode));
            }
        }

        private static string RemoveMainParens(string port_item_string)
        {
            StringBuilder temp = new StringBuilder(port_item_string);
            temp.Remove(0, 3);
            temp.Remove(temp.Length - 3, 3);
            string formatedPortItemString = temp.ToString();
            return formatedPortItemString;
        }

        private static void ParseGenericItems(List<string> generics, Entity Entity_temp)
        {
            foreach (string generic_item_string in generics)
            {
                StringBuilder temp = new StringBuilder(generic_item_string);
                temp.Remove(0, 3);
                temp.Remove(temp.Length - 3, 3);
                string formatedGenericItemString = temp.ToString();
                List<string> interface_items = new List<string>(formatedGenericItemString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (string interface_item_string in interface_items)
                {
                    List<String> FormatedCode;
                    Globals.FormatText(interface_item_string, out FormatedCode);
                    Entity_temp.AddGenericItem(new GenericItem(FormatedCode));
                }
            }
        }

        private void ParseArchitectures(List<string> architectures)
        {
            foreach (string Full_architecture in architectures)
            {
                Architecture Architecture_temp = new Architecture(Full_architecture.Split(new string[] { " ", "of" }, StringSplitOptions.RemoveEmptyEntries).ElementAt(0),
                    Full_architecture.Split(new string[] { " ", "of" }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1));

                string architecture = Globals.SelectFragments(Full_architecture, "is", "begin");
                List<string> interface_items = new List<string>(architecture.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (string interface_item_string in interface_items)
                {
                    if (interface_item_string == " ") continue;
                    List<String> FormatedCode;
                    Globals.FormatText(interface_item_string, out FormatedCode);
                    Architecture_temp.AddArchitectureItem(new ArchitectureBlockDeclarativeItem(FormatedCode));// Add(new architecture_block_declarative_item(FormatedCode));
                }
                ArchitectureList.Add(Architecture_temp);
            }
        }

        private void SelecEntityAndArchitectureBlocks(ref List<string> entities, ref List<string> architectures)
        {
            string FormatedModule;
            string PrimaryModule = sr.ReadToEnd();

            FormatedModule = Globals.FormatText(ref PrimaryModule);

            string sEntityBegin = "entity ";
            string sEntityEnd = " end ";
            Globals.SelectFragments(FormatedModule, sEntityBegin, sEntityEnd,
                out entities);

            string sArchitectureBegin = "architecture";
            string sArchitectureEnd = "begin";
            Globals.SelectFragmentsWithLast(FormatedModule, sArchitectureBegin, sArchitectureEnd, out architectures);
        }

        ~Parser()
        {
            sr.Close();
            fs.Close();
        }


        #region IDisposable Members

        public void Dispose()
        {
            sr.Close();
            fs.Close();
        }

        #endregion
    }
}

