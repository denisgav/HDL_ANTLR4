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
using System.Collections.Generic;


namespace VHDL
{
    /// <summary>
    /// Choice constants.
    /// </summary>
    [Serializable]
    public class Choices : VhdlElement
    {
        [Serializable]
        public class OTHERSChoice : Choice
        {
            public override Choice copy()
            {
                //OTHERS is immutable and doesn't need to be copied.
                return this;
            }
        }

        /// <summary>
        /// OTHERS choice.
        /// </summary>
        public static Choice OTHERS = new OTHERSChoice();

        private List<Choice> choices;

        public List<Choice> InnerChoices
        {
            get { return choices; }
        }
        
        public Choices()
        {
            choices = new List<Choice>();
        }

        public Choices(List<Choice> choices)
        {
            this.choices = choices;
        }
    }

}