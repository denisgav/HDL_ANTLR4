using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Schematix.Core.Compiler
{
    public class GHDLLibraryParser
    {
        GHDLParsedHead parseHead(string input)
        {
            int i;
            GHDLParsedHead tmp = new GHDLParsedHead("", "", "");
            for (i = 8; i < input.Length; i++)
                if (input[i] != '"')
                    tmp.fileName += input[i];
                else
                    break;
            i += 3;
            for (; i < input.Length; i++)
                if (input[i] != '"')
                    tmp.timeAdd += input[i];
                else
                    break;
            i += 3;
            for (; i < input.Length; i++)
                if (input[i] != '"')
                    tmp.timeAdd += input[i];
                else
                    break;
            return tmp;
        }

        string parseEntity(string input)
        {
            int i;
            string tmp = "";
            for (i = 9; i < input.Length; i++)
                if (input[i] != ' ')
                    tmp += input[i];
                else
                    break;
            return tmp;
        }

        GHDLParsedArchitecture parseArchitecture(string input)
        {
            GHDLParsedArchitecture tmp = new GHDLParsedArchitecture("", "");
            int i;
            for (i = 15; i < input.Length; i++)
                if (input[i] != ' ')
                    tmp.architectureName += input[i];
                else
                    break;
            i += 4;
            for (; i < input.Length; i++)
                if (input[i] != ' ')
                    tmp.entityName += input[i];
                else
                    break;
            return tmp;
        }

        public SortedList<string, GHDLCompiledFile> Reparse(string libraryFile)
        {
            if (!File.Exists(libraryFile))
                return null;

            string libDir = Path.GetDirectoryName(libraryFile);

            SortedList<string, GHDLCompiledFile> library = new SortedList<string, GHDLCompiledFile>();
            TextReader tr = File.OpenText(libraryFile);
            try
            {
                tr.ReadLine();
                GHDLParsedHead head;
                int headIndex = 0;
                string buf;
                for (; ; )
                {
                    buf = tr.ReadLine();
                    switch (buf[2])
                    {
                        case 'l':
                            head = parseHead(buf);
                            if (head.fileName.Length > 1)
                                if (head.fileName[0] == '.' && head.fileName[1] == '.')
                                    head.fileName = Path.GetFullPath(libDir + '\\' + head.fileName);
                            if (!library.ContainsKey(head.fileName))
                                library.Add(head.fileName,
                                    new GHDLCompiledFile(head.timeAdd, head.timeCompile,
                                        new SortedList<string, List<string>>()));
                            headIndex = library.IndexOfKey(head.fileName);
                            break;
                        case 'e':
                            {
                                string tmp = parseEntity(buf);
                                if (!library.Values[headIndex].vhdlStruct.ContainsKey(tmp))
                                {
                                    library.Values[headIndex].vhdlStruct.Add(tmp, new List<string>());
                                }
                            }
                            break;
                        case 'a':
                            {
                                GHDLParsedArchitecture tmp = parseArchitecture(buf);
                                if (!library.Values[headIndex].vhdlStruct[tmp.entityName].Contains(tmp.architectureName))
                                    library.Values[headIndex].vhdlStruct[tmp.entityName].Add(tmp.architectureName);
                            }
                            break;
                    }
                }
            }
            catch
            {
            }
            return library;
        }
    }
}