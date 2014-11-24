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
namespace VHDL.expression
{
    /// <summary>
    /// Expression kind.
    /// </summary>
    public enum ExpressionKind
    {
        ABS,
        AND,
        MOD,
        NAND,
        NOR,
        NOT,
        OR,
        REM,
        ROL,
        ROR,
        SLA,
        SLL,
        SRA,
        SRL,
        XNOR,
        XOR,
        EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        LESS_EQUALS,
        GREATER_THAN,
        GREATER_EQUALS,
        PLUS,
        MINUS,
        CONCATENATE,
        MULTIPLY,
        DIVIDE,
        POW
    }
}