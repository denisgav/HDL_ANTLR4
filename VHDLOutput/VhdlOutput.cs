//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;

namespace VHDL.output
{
    using VhdlElement = VHDL.VhdlElement;

    /// <summary>
    /// Collection of functions to generate VHDL output.
    /// This class provides methods to convert an instance of a meta class to VHDL code. The generated
    /// VHDL code can be converted to a <code>String</code>, printed to <code>System.out</code> or
    /// can be written directly to a file or a <code>Writer</code>.
    /// </summary>
    public class VhdlOutput
    {
        private const int INITIAL_STRING_CAPACITY = 8192;

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private VhdlOutput()
        {
        }

        /// <summary>
        /// Converts a <code>VhdlElement</code> to a string using the default code format.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <returns>the converted string</returns>
        public static string toVhdlString(VhdlElement element)
        {
            return toVhdlString(element, DEFAULTVhdlCodeFormat.DEFAULT);

        }

        /// <summary>
        /// Converts a <code>VhdlElement</code> to a string using a custom code format.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="format">the custom code format</param>
        /// <returns>the converted string</returns>
        public static string toVhdlString(VhdlElement element, IVhdlCodeFormat format)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter stringWriter = new StreamWriter(stream);

            try
            {
                toWriter(element, format, stringWriter);
            }
            catch (IOException ex)
            {
                //shouldn't happen for a string writer
                throw new ArgumentException();
            }

            stringWriter.Close();
            StreamReader reader = new StreamReader(stream);
            string res = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return res;
        }

        /// <summary>
        /// Writes the VHDL representation of a <code>VhdlElement</code> to a file.
        /// The default code format is used to convert the <code>VhdlElement</code> to VHDL code.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="fileName">name of the file</param>
        public static void toFile(VhdlElement element, string fileName)
        {
            toFile(element, DEFAULTVhdlCodeFormat.DEFAULT, fileName);
        }

        /// <summary>
        /// Writes the VHDL representation of a <code>VhdlElement</code> to a file using a custom code format.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="format">the custom code format</param>
        /// <param name="fileName">the name of the file</param>
        public static void toFile(VhdlElement element, IVhdlCodeFormat format, string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            toWriter(element, format, writer);
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Outputs the VHDL representation of a <code>VhdlElement</code> to a <code>Writer</code>.
        /// The default code format is used to generate the VHDL output.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="writer">the <code>Writer</code></param>
        public static void toWriter(VhdlElement element, StreamWriter writer)
        {
            toWriter(element, DEFAULTVhdlCodeFormat.DEFAULT, writer);
        }

        /// <summary>
        /// Outputs the VHDL representation of a <code>VhdlElement</code> to a <code>Writer</code> using a custom code format.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="format">the custom code format</param>
        /// <param name="writer">the <code>Writer</code></param>
        public static void toWriter(VhdlElement element, IVhdlCodeFormat format, StreamWriter writer)
        {
            VhdlWriter vhdlWriter = new VhdlWriter(writer, format);
            OutputModule output = new VhdlOutputModule(vhdlWriter);
            output.writeVhdlElement(element);
        }

        /// <summary>
        /// Prints the VHDL representation of a <code>VhdlElement</code> to <code>System.out</code>.
        /// The default code format is used to convert the <code>VhdlElement</code> to the string that
        /// is printed.
        /// </summary>
        /// <param name="element"></param>
        public static void print(VhdlElement element)
        {
            Console.WriteLine(toVhdlString(element));
        }

        /// <summary>
        /// Prints the VHDL representation of a <code>VhdlElement</code> to <code>System.out</code> using
        /// a custom code format.
        /// </summary>
        /// <param name="element">the VHDL element</param>
        /// <param name="format">the custom code format</param>
        public static void print(VhdlElement element, IVhdlCodeFormat format)
        {
            Console.WriteLine(toVhdlString(element, format));
        }
    }

}