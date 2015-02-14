using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ELW.Library.Math;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;
using ELW.Library.Math.Tools;
using System.IO;

namespace Parser
{
    class Globals
    {
        public static List<Module_Type> Types = new List<Module_Type>();
        public static List<KeyValuePair<string, int>> KnownGenerics = new List<KeyValuePair<string, int>>();
        public static void   InitTypes()
        {
            Globals.Types.Add(new Module_Type("INTEGER", new TypeInfo()));
            Globals.Types.Add(new Module_Type("BIT", new TypeInfo()));
            Globals.Types.Add(new Module_Type("BIT_VECTOR", new TypeInfo()));
            Globals.Types.Add(new Module_Type("STD_LOGIC", new TypeInfo()));
            Globals.Types.Add(new Module_Type("STD_LOGIC_VECTOR", new TypeInfo()));
            Globals.Types.Add(new Module_Type("BOOLEAN", new TypeInfo()));
        }
        public static bool   isParen(string str)
        {
            if (str == ")" || (str == "(")) return true;
            return false;
        }
        public static bool   isSpaceOrEmpty(string str)
        {
            if (str == " " || (str == "")) return true;
            return false;
        }
        public static void   RemoveAllSpaces(ref string str)
        {
            for (int i = 0; i < str.Length; )
            {
                if (str.ElementAt(i) == ' ')
                    str.Remove(i, 1);
                else i++;
            }
        }
        public static void   DeleteBeforeWord(string word, ref List<string> list)
        {
            int index_of_double_point = list.IndexOf(word);
            if (index_of_double_point > 0) ;
            list.RemoveRange(0, index_of_double_point + 1);
        }
        public static void   RemoveComments(ref string text)
        {
            bool commentsPresents = true ;
            while (commentsPresents)
            {
                try
                {
                    if (text.IndexOf("--").Equals(-1)) { commentsPresents = false; return;}
                    text = text.Remove(text.IndexOf("--"), text.IndexOf("\r\n", text.IndexOf("--")) - text.IndexOf("--"));
                    if (text.IndexOf("--").Equals(-1)) { commentsPresents = false;  return;}
                }
                catch(ArgumentOutOfRangeException e )
                {
                    if (text.IndexOf("\r\n", text.IndexOf("--")).Equals(-1))
                    {
                        text = text.Remove(text.IndexOf("--"));
                    }
                }
            }
        }
        public static double Calculate(ref List<string> expression)
        {
            for (int i = 0; i < expression.Count; ++i)
            {
                string atom = expression[i];
                if (!(atom.Contains('+') ||
                    atom.Contains('-') ||
                    atom.Contains('*') ||
                    atom.Contains('/') ||
                    atom.Contains('(') ||
                    atom.Contains(')')))
                {
                    try 
                    { 
                        expression[i] = AtomToValue(atom).ToString();
                    }
                    catch (NoValueForAtom e) 
                    { 
                        expression[i] = "";
                        expression = new List<string>();
                        expression.Add("0");
                    }
                }

            }
            string sExpression = MakeStringFromList(expression);
            PreparedExpression preparedExpression = ToolsHelper.Parser.Parse(sExpression);
            CompiledExpression compiledExpression = ToolsHelper.Compiler.Compile(preparedExpression);
            List<VariableValue> variables = new List<VariableValue>();
            return ToolsHelper.Calculator.Calculate(compiledExpression, variables);

        }
        public static int    AtomToValue(string atom)
        {
            int Value;
            try
            {
                Value = Convert.ToInt32(atom);
            }
            catch (FormatException e)
            {
                try
                {
                    Value = FindKnownValues(atom);
                }
                catch (NoValueForAtom ex)
                { throw ex; }
            }
            return Value;
        }
        public static int FindKnownValues(string id)
        {
            foreach (KeyValuePair<string, int> pair_id_value in Globals.KnownGenerics)
                if (pair_id_value.Key.Equals(id))
                    return pair_id_value.Value;

            int indexOf_ns = id.IndexOf("ns");
            string temp = id;
            if (indexOf_ns != -1 &&
                indexOf_ns != 0  &&
                Char.IsDigit(id.ElementAt(indexOf_ns - 1)))
            {
                temp = id.Remove(indexOf_ns, "ns".Length);
                return AtomToValue(temp);
            }
            else
            {
                throw new NoValueForAtom();//ERROR: нету такого значения
            }
            
        }
        public static string MakeStringFromList(List<string> list)
        {
            string all = "";
            foreach (string item in list)
                all += item;
            return all;
        }
        public static void   FormatText(string str, out List<string> Formated)
        {
            Formated = new List<string>();
            StringBuilder cur_word = new StringBuilder();
            int current_symbol_index = 0;

            List<char> Divided_Symbols = new List<char>();
            Divided_Symbols.Add(',');
            Divided_Symbols.Add('*');
            Divided_Symbols.Add('/');
            Divided_Symbols.Add('+');
            Divided_Symbols.Add('-');
            Divided_Symbols.Add(':');
            Divided_Symbols.Add('(');
            Divided_Symbols.Add(')');
            Divided_Symbols.Add('&');
            Divided_Symbols.Add('%');
            Divided_Symbols.Add('.');
            Divided_Symbols.Add(';');

            List<char> Hidden_Symbols = new List<char>();
            Hidden_Symbols.Add(' ');
            Hidden_Symbols.Add('\n');
            Hidden_Symbols.Add('\r');
            Hidden_Symbols.Add('\t');

            while (current_symbol_index < str.Length)
            {
                char current_symb = str.ElementAt(current_symbol_index);
                if (Char.IsLetterOrDigit(current_symb) ||
                        current_symb.Equals('_'))
                    cur_word.Append(current_symb);

                else
                {
                    if (current_symb.Equals(':') && str.ElementAt(current_symbol_index + 1).Equals('='))
                    {
                        Formated.Add(cur_word.ToString());
                        Formated.Add(" ");
                        cur_word.Clear();
                        cur_word.Append(current_symb);
                        current_symbol_index++;
                        continue;
                    }

                    if (current_symb.Equals('=') &&
                            str.ElementAt(current_symbol_index - 1).Equals(':'))
                    {
                        cur_word.Append(current_symb);
                        Formated.Add(cur_word.ToString());
                        Formated.Add(" ");
                        cur_word.Clear();
                        current_symbol_index++;
                        continue;
                    }

                    if (Hidden_Symbols.Contains(current_symb))

                        if (!cur_word.Length.Equals(0))
                        {
                            Formated.Add(cur_word.ToString());
                            Formated.Add(" ");
                            cur_word.Clear();
                        }
                    if (Divided_Symbols.Contains(current_symb))
                    {
                        Formated.Add(cur_word.ToString());
                        cur_word.Clear();
                        Formated.Add(current_symb.ToString());
                        Formated.Add(" ");
                    }
                }
                current_symbol_index++;
            }
            Formated.Add(cur_word.ToString());
        }
        public static void   FormatText(string str, out string FormatedText)
        {
            List<string> Formated = new List<string>();
            StringBuilder FormatedText_ = new StringBuilder();
            StringBuilder cur_word = new StringBuilder();
            int current_symbol_index = 0;

            List<char> Divided_Symbols = new List<char>();
            Divided_Symbols.Add(',');
            Divided_Symbols.Add('*');
            Divided_Symbols.Add('/');
            Divided_Symbols.Add('+');
            Divided_Symbols.Add('-');
            Divided_Symbols.Add(':');
            Divided_Symbols.Add('(');
            Divided_Symbols.Add(')');
            Divided_Symbols.Add('&');
            Divided_Symbols.Add('%');
            Divided_Symbols.Add('.');
            Divided_Symbols.Add(';');


            List<char> Hidden_Symbols = new List<char>();
            Hidden_Symbols.Add(' ');
            Hidden_Symbols.Add('\n');
            Hidden_Symbols.Add('\r');
            Hidden_Symbols.Add('\t');
            while (current_symbol_index < str.Length)
            {
                char current_symb = str.ElementAt(current_symbol_index);
                if (Char.IsLetterOrDigit(current_symb) ||
                        current_symb.Equals('_'))
                    cur_word.Append(current_symb);

                else
                {
                    if (current_symb.Equals(':') && str.ElementAt(current_symbol_index + 1).Equals('='))
                    {
                        Formated.Add(cur_word.ToString());
                        Formated.Add(" ");
                        cur_word.Clear();
                        cur_word.Append(current_symb);
                        current_symbol_index++;
                        continue;
                    }

                    if (current_symb.Equals('=') &&
                            str.ElementAt(current_symbol_index - 1).Equals(':'))
                    {
                        cur_word.Append(current_symb);
                        Formated.Add(cur_word.ToString());
                        Formated.Add(" ");
                        cur_word.Clear();
                        current_symbol_index++;
                        continue;
                    }

                    if (Hidden_Symbols.Contains(current_symb))

                        if (!cur_word.Length.Equals(0))
                        {
                            Formated.Add(cur_word.ToString());
                            Formated.Add(" ");
                            cur_word.Clear();
                        }
                    if (Divided_Symbols.Contains(current_symb))
                    {
                        Formated.Add(cur_word.ToString());
                        cur_word.Clear();
                        Formated.Add(" ");
                        Formated.Add(current_symb.ToString());
                        Formated.Add(" ");
                    }
                }
                current_symbol_index++;
            }
            Formated.Add(cur_word.ToString());
            foreach (string st in Formated)
                FormatedText_.Append(st);
            FormatedText = FormatedText_.ToString();
        }
        public static string FormatText(ref string PrimaryModule)
        {
            string FormatedModule;
            Globals.RemoveComments(ref PrimaryModule);
            Globals.FormatText(PrimaryModule, out FormatedModule);
            FormatedModule = FormatedModule.ToLower();
            return FormatedModule;
        }
        public static string GetFirstWord(List<string> str, out int index_of_first_word)
        {
            index_of_first_word = 0;
            while (str.ElementAt(index_of_first_word).Contains(' ') ||
                    str.ElementAt(index_of_first_word).Contains('(') ||
                    str.ElementAt(index_of_first_word).Contains(')') ||
                    str.ElementAt(index_of_first_word).Contains(';') ||
                    str.ElementAt(index_of_first_word).Contains('.') ||
                    str.ElementAt(index_of_first_word).Contains(':') ||
                    str.ElementAt(index_of_first_word).Contains(":=") ||
                    str.ElementAt(index_of_first_word).Contains("+") ||
                    str.ElementAt(index_of_first_word).Contains('-') ||
                    str.ElementAt(index_of_first_word).Contains('/') ||
                    str.ElementAt(index_of_first_word).Contains('*'))
                index_of_first_word++;
            return str.ElementAt(index_of_first_word);
        }
        public static void   AddType(Module_Type type)
        {
            Globals.Types.Add(type);
        }
        public static void   SelectFragments(String Text, String SbeginFragment, String SendFragment, out List<String> fragments)
        {
            fragments = new List<string>();
            int fragmentBeginPosit = 0, fragmentEndPosit = 0;

            while (fragmentEndPosit < Text.Length)
            {
                fragmentBeginPosit = Text.IndexOf(SbeginFragment, fragmentEndPosit);
                if (fragmentBeginPosit == -1) break;
                fragmentEndPosit = Text.IndexOf(SendFragment, fragmentBeginPosit);
                if (fragmentEndPosit == -1) break;
                try
                {
                    fragments.Add(new String(Text.ToCharArray(), fragmentBeginPosit + SbeginFragment.Length, fragmentEndPosit - fragmentBeginPosit - SbeginFragment.Length));
                    fragmentEndPosit = Text.IndexOf(';', fragmentEndPosit);
                }
                catch (ArgumentOutOfRangeException e)
                { }
            }
        }
        public static void   SelectFragmentsWithLast(String Text, String SbeginFragment, String SendFragment, out List<String> fragments)
        {
            fragments = new List<string>();
            int fragmentBeginPosit = 0, fragmentEndPosit = 0;

            while (fragmentEndPosit < Text.Length)
            {
                fragmentBeginPosit = Text.IndexOf(SbeginFragment, fragmentEndPosit);
                if (fragmentBeginPosit == -1) break;
                fragmentEndPosit = Text.IndexOf(SendFragment, fragmentBeginPosit);
                if (fragmentEndPosit == -1) break;
                try
                {
                    fragments.Add(new String(Text.ToCharArray(), fragmentBeginPosit + SbeginFragment.Length, fragmentEndPosit - fragmentBeginPosit - SbeginFragment.Length + SendFragment.Length));
                }
                catch (ArgumentOutOfRangeException e)
                { }
            }    
        }
        public static string SelectFragments(String Text, String SbeginFragment, String SendFragment)
        {
            string fragment = "";
            int fragmentBeginPosit = 0, fragmentEndPosit = 0;

            fragmentBeginPosit = Text.IndexOf(SbeginFragment, fragmentEndPosit);
            fragmentEndPosit = Text.IndexOf(SendFragment, fragmentBeginPosit);
            try
            {
                fragment = new String(Text.ToCharArray(), fragmentBeginPosit + SbeginFragment.Length, fragmentEndPosit - fragmentBeginPosit - SbeginFragment.Length);
            }
            catch (ArgumentOutOfRangeException e)
            { }
            return fragment;
        }

        public static void   FormatedSelectFragmentsWithBlocks(string Text, string SbeginFragment, char SendFragment,
            out List<String> fragments)
        {
            fragments = new List<string>();

            int LPAREN = 0, RPAREN = 0;
            int fragmentEndPosit = 0;
            while (fragmentEndPosit < Text.Length)
            {
                if (Text.IndexOf(SbeginFragment, fragmentEndPosit) < 0) break;
                int fragmentBeginPosit = Text.IndexOf(SbeginFragment, fragmentEndPosit) + SbeginFragment.Length;
                for (int i = fragmentBeginPosit; i < Text.Length; ++i)
                {
                    if (Text.ElementAt(i).Equals('('))
                        LPAREN++;
                    if (Text.ElementAt(i).Equals(')'))
                        RPAREN++;
                    if (LPAREN.Equals(RPAREN) && Text.ElementAt(i).Equals(SendFragment))
                    {
                        char[] buf = new char[i - fragmentBeginPosit];
                        Text.CopyTo(fragmentBeginPosit, buf, 0, i - fragmentBeginPosit);
                        fragments.Add(new String(buf));
                        fragmentEndPosit = fragmentBeginPosit + i;
                        break;
                    }
                }
            }
        }
        
    }
}
