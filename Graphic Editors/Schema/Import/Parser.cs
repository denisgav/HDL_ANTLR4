using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace csx
{
    class Parser
    {
        private bool lastTure = false;
        public SortedList<string, vhdEntity> entities;
        public List<string> libraries;

        public Parser()
        {
            entities = new SortedList<string, vhdEntity>();
            libraries = new List<string>();
        }

        private int StrToInt(string str)
        {
            int i;
            int sum = 0;
            for (i = 0; i < str.Length; i++)
                switch (str[i])
                {
                    case '0':
                        sum = sum * 10;
                        break;
                    case '1':
                        sum = sum * 10 + 1;
                        break;
                    case '2':
                        sum = sum * 10 + 2;
                        break;
                    case '3':
                        sum = sum * 10 + 3;
                        break;
                    case '4':
                        sum = sum * 10 + 4;
                        break;
                    case '5':
                        sum = sum * 10 + 5;
                        break;
                    case '6':
                        sum = sum * 10 + 6;
                        break;
                    case '7':
                        sum = sum * 10 + 7;
                        break;
                    case '8':
                        sum = sum * 10 + 8;
                        break;
                    case '9':
                        sum = sum * 10 + 9;
                        break;
                    default:
                        return sum;
                }
            return sum;
        }

        private string nextWord(FileStream fs, StreamReader sr, char[] razdel)
        {
            string s = "";
            char[] buf = new char[1];
            int i;
            bool find = false; ;
            int cnt_ = 0;
            if (lastTure)
                cnt_++;
            while (!sr.EndOfStream)
            {
                sr.Read(buf, 0, 1);
                if (buf[0] == '-')
                {
                    cnt_++;
                    if (cnt_ == 2)
                    {
                        sr.ReadLine();
                        cnt_ = 0;
                    }
                }
                else
                {
                    cnt_ = 0;
                    find = false;
                    for (i = 0; i < razdel.Length; i++)
                        if (razdel[i] == buf[0])
                        {
                            find = true;
                            break;
                        }
                    if (!find)
                        break;
                }
            }
            if (!find)
                s = buf[0].ToString();
            while (!sr.EndOfStream)
            {
                sr.Read(buf, 0, 1);
                find = false;
                for (i = 0; i < razdel.Length; i++)
                    if (razdel[i] == buf[0])
                    {
                        find = true;
                        break;
                    }
                if (find)
                {
                    if (buf[0] == '-')
                        lastTure = true;
                    else
                        lastTure = false;
                    return s.ToLower();
                }
                s += buf[0].ToString();
            }
            return s.ToLower();
        }

        public void Parsing(string FileName)
        {
            char[] razd = { ' ', ';', ',', '(', ')', '-', '+', '=', ':', '\t', '\n', '\r' };
            FileStream fs = new FileStream(FileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string str;
            string str2;
            vhdEntity ent;
            vhdPort port;
            string type = "";
            portInOut inout;
            bool bus = false;
            int leftBound = 0;
            int rightBound = 0;
            List<string> names = new List<string>();
            while (!(sr.EndOfStream))
            {
                str = nextWord(fs, sr, razd);
                if (str.CompareTo("entity") == 0)
                {
                    ent = new vhdEntity();
                    ent.name = nextWord(fs, sr, razd);
                    if (nextWord(fs, sr, razd).CompareTo("is") == 0)
                    {
                        while (nextWord(fs, sr, razd).CompareTo("port") != 0) ;
                        str = nextWord(fs, sr, razd);
                        str2 = nextWord(fs, sr, razd);
                        while (str.CompareTo("end") != 0)
                        {
                            names.Clear();
                            names.Add(str);
                            while (!(str2.CompareTo("in") == 0 ||
                                    str2.CompareTo("out") == 0 ||
                                    str2.CompareTo("inout") == 0 ||
                                    str2.CompareTo("buffer") == 0))
                            {
                                names.Add(str2);
                                str2 = nextWord(fs, sr, razd);
                            }
                            if (str2.CompareTo("in") == 0)
                                inout = portInOut.In;
                            else
                                if (str2.CompareTo("out") == 0)
                                    inout = portInOut.Out;
                                else
                                    inout = portInOut.InOut;
                            type = nextWord(fs, sr, razd);
                            bus = false;
                            leftBound = 0;
                            rightBound = 0;
                            str = nextWord(fs, sr, razd);
                            str2 = nextWord(fs, sr, razd);
                            if (str2.CompareTo("to") == 0 || str2.CompareTo("downto") == 0)
                            {
                                bus = true;
                                leftBound = StrToInt(str);
                                rightBound = StrToInt(nextWord(fs, sr, razd));
                                str = nextWord(fs, sr, razd);
                                str2 = nextWord(fs, sr, razd);
                            }
                            int i;
                            for (i = 0; i < names.Count; i++)
                            {
                                port = new vhdPort();
                                port.name = names[i];
                                port.inout = inout;
                                port.type = type;
                                port.bus = bus;
                                port.leftBound = leftBound;
                                port.rightBound = rightBound;
                                ent.ports.Add(port);
                            }
                        }
                        entities.Add(ent.name, ent);
                    }
                }
                else if (str.CompareTo("library") == 0 || str.CompareTo("use") == 0)
                {
                    string libName = nextWord(fs, sr, razd);
                    libraries.Add(str + " " + libName + ";");
                }
            }
            sr.Close();
        }
    }
}