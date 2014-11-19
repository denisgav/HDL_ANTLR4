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


grammar VhdlAntlr;

    //-----------------------------------------------------------------
    // keywords
    //-----------------------------------------------------------------

    ABS           : 'abs';
    ACCESS        : 'access';
    AFTER         : 'after';
    ALIAS         : 'alias';
    ALL           : 'all';
    AND           : 'and';
    ARCHITECTURE  : 'architecture';
    ARRAY         : 'array';
    ASSERT        : 'assert';
    ATTRIBUTE     : 'attribute';
    BEGIN         : 'begin';
    BLOCK         : 'block';
    BODY          : 'body';
    BUFFER        : 'buffer';
    BUS           : 'bus';
    CASE          : 'case';
    COMPONENT     : 'component';
    CONFIGURATION : 'configuration';
    CONSTANT      : 'constant';
    DISCONNECT    : 'disconnect';
    DOWNTO        : 'downto';
    ELSE          : 'else';
    ELSIF         : 'elsif';
    END           : 'end';
    ENTITY        : 'entity';
    EXIT          : 'exit';
    FILE          : 'file';
    FOR           : 'for';
    FUNCTION      : 'function';
    GENERATE      : 'generate';
    GENERIC       : 'generic';
    GROUP         : 'group';
    GUARDED       : 'guarded';
    IF            : 'if';
    IMPURE        : 'impure';
    INERTIAL      : 'inertial';
    IN            : 'in';
    INOUT         : 'inout';
    IS            : 'is';
    LABEL         : 'label';
    LIBRARY       : 'library';
    LINKAGE       : 'linkage';
    LITERAL       : 'literal';
    LOOP          : 'loop';
    MAP           : 'map';
    MOD           : 'mod';
    NAND          : 'nand';
    NEW           : 'new';
    NEXT          : 'next';
    NOR           : 'nor';
    NOT           : 'not';
    NULLTOK       : 'null';
    OF            : 'of';
    ON            : 'on';
    OPEN          : 'open';
    OR            : 'or';
    OTHERS        : 'others';
    OUT           : 'out';
    PACKAGE       : 'package';
    PORT          : 'port';
    POSTPONED     : 'postponed';
    PROCEDURE     : 'procedure';
    PROCESS       : 'process';
    PURE          : 'pure';
    RANGETOK      : 'range';
    RECORD        : 'record';
    REGISTER      : 'register';
    REJECT        : 'reject';
    REM           : 'rem';
    REPORT        : 'report';
    RETURN        : 'return';
    ROL           : 'rol';
    ROR           : 'ror';
    SELECT        : 'select';
    SEVERITY      : 'severity';
    SHARED        : 'shared';
    SIGNAL        : 'signal';
    SLA           : 'sla';
    SLL           : 'sll';
    SRA           : 'sra';
    SRL           : 'srl';
    SUBTYPE       : 'subtype';
    THEN          : 'then';
    TO            : 'to';
    TRANSPORT     : 'transport';
    TYPE          : 'type';
    UNAFFECTED    : 'unaffected';
    UNITS         : 'units';
    UNTIL         : 'until';
    USE           : 'use';
    VARIABLE      : 'variable';
    WAIT          : 'wait';
    WHEN          : 'when';
    WHILE         : 'while';
    WITH          : 'with';
    XNOR          : 'xnor';
    XOR           : 'xor';

    //-----------------------------------------------------------------
    // symbols
    //-----------------------------------------------------------------

    DOUBLESTAR    : '**';
    LE            : '<=';
    GE            : '>=';
    ARROW         : '=>';
    NEQ           : '/=';
    VARASGN       : ':=';
    BOX           : '<>';
    DBLQUOTE      : '\"';
    SEMI          : ';';
    COMMA         : ',';
    AMPERSAND     : '&';
    LPAREN        : '(';
    RPAREN        : ')';
    LBRACKET      : '[';
    RBRACKET      : ']';
    COLON         : ':';
    MUL           : '*';
    DIV           : '/';
    PLUS          : '+';
    MINUS         : '-';
    LT            : '<';
    GT            : '>';
    EQ            : '=';
    BAR           : '|';
    EXCLAMATION   : '!';
    DOT           : '.';
    BACKSLASH     : '\\';
    
    APOSTROPHE    : '\'';
    
//-----------------------------------------------------------------------------------------
//        Operators
//-----------------------------------------------------------------------------------------
adding_operator
    :   PLUS
    |   MINUS
    |   AMPERSAND
    ;
    
logical_operator
    :   AND
    |   OR
    |   NAND
    |   NOR
    |   XOR
    |   XNOR
    ;
    
multiplying_operator
    :   MUL
    |   DIV
    |   MOD
    |   REM
    ;
    
relational_operator
    :   EQ
    |   NEQ
    |   LT
    |   LE
    |   GT
    |   GE
    ;
    
shift_operator
    :   SLL
    |   SRL
    |   SLA
    |   SRA
    |   ROL
    |   ROR
    ;

sign
    :   PLUS
    |   MINUS
    ;
    
    
//-----------------------------------------------------------------------------------------
//      Primitive groups
//-----------------------------------------------------------------------------------------
direction
    :   TO
    |   DOWNTO
    ;
    
    
signal_mode
    :   IN
    |   OUT
    |   INOUT
    |   BUFFER
    |   LINKAGE
    ;

signal_kind
    :   REGISTER
    |   BUS
    ;

//-----------------------------------------------------------------------------------------
//         Digit and characters
//-----------------------------------------------------------------------------------------

UPPER_CASE_LETTER
    :   'A'..'Z' | '\u00c0'..'\u00d6' | '\u00d8' .. '\u00de'
    ;
    
LOWER_CASE_LETTER
    :   'a'..'z' | '\u00df'..'\u00f6' | '\u00f8'.. '\u00ff'
    ;

DIGIT
    :   '0'..'9'
    ;
    
    
//removed " from SPECIAL_CHARACTER to handle separately in different contexts
SPECIAL_CHARACTER
    :   '#' | '&' | '\'' | '(' | ')' | '*' | '+' | ',' | '-'
    |   '.' | '/' | ':' | ';' | '<' | '=' | '>' | '[' | ']' | '_' | '|'
    ;

SPACE_CHARACTER
    :   ' ' | '\u00a0'
    ;

//removed \ and % from OTHER_SPECIAL_CHARACTER to handle separately in different contexts
OTHER_SPECIAL_CHARACTER
    :   '!' | '$' | '@' | '?' | '^' | '`' | '{' | '}' | '~'
    |   '\u00a1'..'\u00bf' | '\u00d7' | '\u00f7'
    ;
    
LETTER_OR_DIGIT
    :   LETTER
    |   DIGIT
    ;

LETTER
    :   UPPER_CASE_LETTER
    |   LOWER_CASE_LETTER
    //|	HEXA_INTEGER_LITERAL
    ;

GRAPHIC_CHARACTER
    :   UPPER_CASE_LETTER
    |   DIGIT
    |   SPECIAL_CHARACTER
    |   SPACE_CHARACTER
    |   LOWER_CASE_LETTER
    |   OTHER_SPECIAL_CHARACTER
    ;
    
//-----------------------------------------------------------------------------------------
//         Identifiers
//-----------------------------------------------------------------------------------------
BASIC_IDENTIFIER
    :   LETTER ( LETTER_OR_DIGIT | '_' )*
    ;

//extended identifiers can't contain a single backslash
EXTENDED_IDENTIFIER
    :   '\\' ( '\"' | '\\\\' | '%'  | GRAPHIC_CHARACTER )+ '\\'
    ;

identifier
    :   BASIC_IDENTIFIER
    |   EXTENDED_IDENTIFIER
    ;
    
end_identifier
 	:	identifier;

identifier_list
    :   identifier ( COMMA identifier )*
    ;

null_statement
    :   NULLTOK SEMI
    ;

//-----------------------------------------------------------------------------------------
//         Numeric Literals
//-----------------------------------------------------------------------------------------
BINANRY_BASED_INTEGER
:
	'2' '#' ('1' | '0' | '_')+ '#' (DEC_BASED_INTEGER)?
	;

OCTAL_BASED_INTEGER
:
	'8' '#' ('7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '#' (DEC_BASED_INTEGER)?
	;
	
HEXA_BASED_INTEGER
:
	'16' '#' ( 'f' |'e' |'d' |'c' |'b' |'a' | 'F' |'E' |'D' |'C' |'B' |'A' | '9' | '8' | '7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '#' (DEC_BASED_INTEGER)?
	;

DEC_BASED_INTEGER
	:   DIGIT
	|   ('1'..'9') (DIGIT | '_')+ 
	;

numeric_literal
    :   BINANRY_BASED_INTEGER
    |   OCTAL_BASED_INTEGER
    |   HEXA_BASED_INTEGER
    |   DEC_BASED_INTEGER
    ;
    
//-----------------------------------------------------------------------------------------
//         Float point Literal
//-----------------------------------------------------------------------------------------
FLOAT_POINT_LITERAL
	: DEC_BASED_INTEGER ('.' (DIGIT)+) (('e'|'E') ('-'|'+')? DEC_BASED_INTEGER)?
	;
    
//-----------------------------------------------------------------------------------------
//         String Literals
//-----------------------------------------------------------------------------------------
STRING_LITERAL
    :   '\"' ( '\"\"' | '\\' | '%' | GRAPHIC_CHARACTER )* '\"'
    |   '%'  ( '%%' | '\\' | GRAPHIC_CHARACTER )* '%'
    ;

BIT_STRING_LITERAL_BINARY
    :   'b' '\"' ('1' | '0' | '_')+ '\"'
    |   'b' '%'  ('1' | '0' | '_')+ '%'
    ;

BIT_STRING_LITERAL_OCTAL
    :   'o' '\"' ('7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '\"'
    |   'o' '%'  ('7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '%'
    ;

BIT_STRING_LITERAL_HEX
    :   'x' '\"' ( 'f' |'e' |'d' |'c' |'b' |'a' | 'F' |'E' |'D' |'C' |'B' |'A' | '9' | '8' | '7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '\"'
    |   'x' '%'  ( 'f' |'e' |'d' |'c' |'b' |'a' | 'F' |'E' |'D' |'C' |'B' |'A' | '9' | '8' | '7' |'6' |'5' |'4' |'3' |'2' |'1' | '0' | '_')+ '%'
    ;
 
//-----------------------------------------------------------------------------------------
//         Comment
//-----------------------------------------------------------------------------------------
COMMENT
    :   '--' ( ~( '\n' | '\r' ) )* -> skip;
    
//-----------------------------------------------------------------------------------------
//         Whitespace
//-----------------------------------------------------------------------------------------
WHITESPACE
    :   [ \n\t\r]+ -> skip;
    
//-----------------------------------------------------------------------------------------
//        Attribute designator
//-----------------------------------------------------------------------------------------
attribute_designator
    :   identifier
    |   RANGETOK
    ;
    
//-----------------------------------------------------------------------------------------
//    Association
//-----------------------------------------------------------------------------------------
formal_part
    :   name
    ;
    
actual_part
    :   expression
    |   OPEN
    ;
    
association_element
    :   ( formal_part ARROW )? actual_part
    ;

association_list
    :   association_element ( COMMA association_element )*
    ;
    
//-----------------------------------------------------------------------------------------
//         Name
//-----------------------------------------------------------------------------------------
name_prefix
    :   identifier
    |   STRING_LITERAL
    ;
    
name_part
    :   name_selected_part
    |   LPAREN name (constraint)? RPAREN
    |   LPAREN expression
        (
                direction expression
            |   ( COMMA expression )*
        )
        RPAREN
    |   name_attribute_part
;

name_with_association_part
    :   name_selected_part
    |   LPAREN name ( constraint )? RPAREN
    |   LPAREN
        (
			    expression direction expression
            |   association_list
        )
        RPAREN
    |   name_attribute_part
    ;
    
    
name
    :   name_prefix ( name_part )*
    ;

name_with_association
    :   name_prefix name_with_association_part*
    ;

name_without_parens
    :   name_prefix name_without_parens_part*
    ;

name_without_parens_part
    :   name_selected_part
    |   name_attribute_part
    ;

name_selected_part
    :   DOT suffix
    ;

name_attribute_part
    :   signature? APOSTROPHE attribute_designator
        ( LPAREN expression RPAREN )?
    ;


//-------------------------------------------------------------------
//      Suffix
//-------------------------------------------------------------------
suffix
    :   identifier
    |   CHARACTER_LITERAL
    |   STRING_LITERAL
    |   ALL
    ;
//-------------------------------------------------------------------
//   Signature
//-------------------------------------------------------------------
signature_type_marks
    :   name ( COMMA name )*
    ;
signature
    :   LBRACKET signature_type_marks? ( RETURN return_type=name )? RBRACKET
    ;
    
//-------------------------------------------------------------------
//     Constraint
//-------------------------------------------------------------------
constraint
    :   range_constraint
    |   index_constraint
    ;
    
range_constraint
    :   RANGETOK range
    ;
    
index_constraint
    :   LPAREN discrete_range ( COMMA discrete_range )* RPAREN
    ;
    
    
//--------------------------------------------------------------------
//          Range
//--------------------------------------------------------------------
range
    :   simple_expression direction simple_expression
    |   name
    ;
    
discrete_range
    :   simple_expression direction simple_expression
    |   name_without_parens constraint?
    ;
    
//-------------------------------------------------------------------
// Choice
//-------------------------------------------------------------------
choice
    :   simple_expression ( direction simple_expression)?
    |   OTHERS
    ;
    
choices
    :   choice ( ( BAR | EXCLAMATION ) choice )*
    ;


//--------------------------------------------------------------------
//    Element association
//--------------------------------------------------------------------
element_association
    :   ( choices ARROW )? expression
    ;


//--------------------------------------------------------------------
//   Aggregate
//--------------------------------------------------------------------
aggregate
    :   LPAREN element_association ( COMMA element_association )* RPAREN
    ;
    
//--------------------------------------------------------------------
//   Primary
//--------------------------------------------------------------------
   
primary
    :   numeric_literal (identifier)?
    |	FLOAT_POINT_LITERAL
    |   CHARACTER_LITERAL    
    |   BIT_STRING_LITERAL_BINARY
    |   BIT_STRING_LITERAL_OCTAL
    |   BIT_STRING_LITERAL_HEX
    |   NULLTOK
    |   aggregate
    |   allocator
    |   name_with_association ( qualified_expression)?
    ;
    
//--------------------------------------------------------------------
//     Expression
//--------------------------------------------------------------------
term
    :   factor ( multiplying_operator factor )*
    ;
factor
    :   primary ( DOUBLESTAR primary )?
    |   ABS primary
    |   NOT primary
    ;
    
simple_expression
    :   sign? term ( adding_operator term )*
    ;

allocator
    :   NEW name_without_parens
        (
                name_without_parens? index_constraint?
            |   qualified_expression
        )
    ;
    
qualified_expression
    :   APOSTROPHE aggregate
    ;
    
expression
    :   relation ( logical_operator relation )*
    ;
    
relation
    :   shift_expression ( relational_operator shift_expression )?
    ;

shift_expression
    :   simple_expression ( shift_operator simple_expression )?
    ;
    
    
//--------------------------------------------------------------------------------
//        Alias
//--------------------------------------------------------------------------------
alias_declaration
    :   ALIAS (identifier | CHARACTER_LITERAL | STRING_LITERAL) ( COLON subtype_indication )? IS name signature? SEMI
    ;

//---------------------------------------------------------------------------------
//   Types
//---------------------------------------------------------------------------------
subtype_indication
    :   name_without_parens name_without_parens? constraint?
    ;
    
subtype_declaration
    :   SUBTYPE identifier IS subtype_indication SEMI
    ;
    
full_type_declaration
    :   TYPE identifier IS type_definition SEMI
    ;

incomplete_type_declaration
    :   TYPE identifier SEMI
    ;
    
type_declaration
    :   full_type_declaration
    |   incomplete_type_declaration
    ;

enumeration_literal
    :   CHARACTER_LITERAL
    |   identifier
    ;
    
enumeration_type_definition
    :   LPAREN enumeration_literal ( COMMA enumeration_literal )* RPAREN
    ;

secondary_unit_declaration
    :   identifier EQ numeric_literal? name SEMI
    ;
    
//TODO: check repeated label
physical_type_definition
    :   range_constraint UNITS
        identifier SEMI
        secondary_unit_declaration*
        END UNITS end_identifier?
    ;

integer_or_floating_type_definition
    :   range_constraint
    ;
    
scalar_type_definition
    :   enumeration_type_definition
    |   physical_type_definition
    |   integer_or_floating_type_definition
    ;

constrained_array_definition
    :   index_constraint OF subtype_indication
    ;
    
array_type_definition
    :   ARRAY 
        (
                unconstrained_array_definition
            |   constrained_array_definition
        )
    ;

element_declaration
    :   identifier_list COLON subtype_indication SEMI
    ;
    
//TODO: check repeated label
record_type_definition
    :   RECORD
        element_declaration+
        END RECORD identifier?
    ;
    
composite_type_definition
    :   array_type_definition
    |   record_type_definition
    ;

access_type_definition
    :   ACCESS subtype_indication
    ;

file_type_definition
    :   FILE OF name
    ;
    
type_definition
    :   scalar_type_definition
    |   composite_type_definition
    |   access_type_definition
    |   file_type_definition
    ;

index_subtype_definition
    :   type_mark=name RANGETOK BOX
    ;
    
unconstrained_array_definition
    :   LPAREN index_subtype_definition ( COMMA index_subtype_definition )* RPAREN
        OF element_subtype_indication=subtype_indication
    ;
    
//---------------------------------------------------------------------------------------
// Declarations
//---------------------------------------------------------------------------------------
constant_declaration
    :   CONSTANT identifier_list COLON subtype_indication ( VARASGN expression )? SEMI
    ;

variable_declaration
    :   SHARED? VARIABLE identifier_list COLON subtype_indication ( VARASGN expression )? SEMI
    ;

file_declaration
    :   FILE identifier_list COLON subtype_indication (( OPEN expression )? IS expression)? SEMI
    ;

attribute_declaration
    :   ATTRIBUTE identifier COLON name SEMI
    ;

attribute_specification
    :   ATTRIBUTE attribute_designator OF entity_specification IS expression SEMI
    ;
    
group_template_declaration
    :   GROUP identifier IS LPAREN entity_class_entry_list RPAREN SEMI
    ;

group_constituent
    :   name
    |   CHARACTER_LITERAL
    ;

group_constituent_list
    :   group_constituent ( COMMA group_constituent )*
    ;
    
group_declaration
    :   GROUP identifier COLON name_without_parens
        LPAREN group_constituent_list RPAREN SEMI
    ;

signal_declaration
    :   SIGNAL identifier_list COLON subtype_indication signal_kind? ( VARASGN expression )? SEMI
    ;

signal_list
    :   name ( COMMA name )*
    |   OTHERS
    |   ALL
    ;
    
guarded_signal_specification
    :   signal_list COLON name
    ;
    
disconnection_specification
    :   DISCONNECT guarded_signal_specification AFTER time_expression=expression SEMI
    ;

//---------------------------------------------------------------------------------------
// Assertion statements
//---------------------------------------------------------------------------------------

condition
    :   expression
    ;

assertion
    :   ASSERT condition ( REPORT expression )? ( SEVERITY expression )?
    ;

//---------------------------------------------------------------------------------------
// If statements
//---------------------------------------------------------------------------------------
if_statement
    :   IF condition THEN
        sequence_of_statements
        if_statement_elsif_part*
        if_statement_else_part?
        END IF end_identifier? SEMI
    ;

if_statement_elsif_part
    :   ELSIF condition THEN sequence_of_statements
    ;

if_statement_else_part
    :   ELSE sequence_of_statements
    ;

//---------------------------------------------------------------------------------------
// Case statements
//---------------------------------------------------------------------------------------
case_statement
    :   CASE expression IS
        case_statement_alternative+
        END CASE end_identifier? SEMI
    ;

case_statement_alternative
    :   WHEN choices ARROW sequence_of_statements
    ;
//---------------------------------------------------------------------------------------
// Loop statements
//---------------------------------------------------------------------------------------
parameter_specification
    :   identifier IN discrete_range
    ;
    
iteration_scheme
    :   WHILE condition
    |   FOR parameter_specification
    ;
    
loop_statement
    :   iteration_scheme?
        LOOP
        sequence_of_statements
        END LOOP end_identifier? SEMI
    ;

next_statement
    :   NEXT identifier? ( WHEN condition )? SEMI
    ;
    
 exit_statement
    :   EXIT identifier? ( WHEN condition )? SEMI
    ;

//---------------------------------------------------------------------------------------
// Wait statements
//---------------------------------------------------------------------------------------
sensitivity_clause
    :   ON sensitivity_list
    ;

sensitivity_list
    :   name ( COMMA name )*
    ;
    
condition_clause
    :   UNTIL condition
    ;
    
timeout_clause
    :   FOR expression
    ;
    
wait_statement
    :   WAIT sensitivity_clause? condition_clause? timeout_clause? SEMI
    ;

//---------------------------------------------------------------------------------------
// Waveform
//---------------------------------------------------------------------------------------
waveform
    :   waveform_element ( COMMA waveform_element )*
    |   UNAFFECTED
    ;

waveform_element
    :   expression ( AFTER expression )?
    ;

selected_waveform
    :   waveform WHEN choices
    ;

selected_waveforms
    :   selected_waveform ( COMMA selected_waveform )*
    ;
    
//---------------------------------------------------------------------------------------
// Signal assignment statements
//---------------------------------------------------------------------------------------
target
    :   name
    |   aggregate
    ;
    
delay_mechanism
    :   TRANSPORT
    |   ( REJECT expression )? INERTIAL
    ;
    
signal_assignment_statement
    :   target LE delay_mechanism? waveform SEMI
    ;
    
signal_assignment_options
    :   GUARDED? delay_mechanism?
    ;

selected_signal_assignment
    :   WITH expression SELECT target LE signal_assignment_options selected_waveforms SEMI
    ;
    
conditional_signal_assignment
    :   target LE signal_assignment_options conditional_waveforms SEMI
    ;

conditional_waveforms
    :   waveform ( WHEN condition ( ELSE conditional_waveforms2 )? )?
    ;

conditional_waveforms2
    :   waveform ( WHEN condition ( ELSE conditional_waveforms2 )? )?
    ;
//---------------------------------------------------------------------------------------
// Sequential statements
//---------------------------------------------------------------------------------------   
assertion_statement
    :   assertion SEMI
    ;

report_statement
    :   REPORT expression ( SEVERITY expression )? SEMI
    ;

return_statement
    :   RETURN expression? SEMI
    ;
    
sequential_statement
    :   identifier COLON sequential_statement_2
    |   sequential_statement_2
    ;
        
variable_assignment_statement
    :   target VARASGN expression SEMI
    ;

procedure_call
    :   name_without_parens ( LPAREN association_list RPAREN )?
    ;
    
procedure_call_statement
    :   procedure_call SEMI
    ;

sequential_statement_2
    :   wait_statement
    |   assertion_statement
    |   report_statement
    |   signal_assignment_statement
    |   variable_assignment_statement
    |   procedure_call_statement
    |   if_statement
    |   case_statement
    |   loop_statement
    |   next_statement
    |   exit_statement
    |   return_statement
    |   null_statement
    ;

sequence_of_statements
    :   sequential_statement*
    ;
    
    
//---------------------------------------------------------------------------------------
// Use clause
//---------------------------------------------------------------------------------------
use_clause
    :   USE name ( COMMA name )* SEMI
    ;
    
//---------------------------------------------------------------------------------------
// Library clause
//---------------------------------------------------------------------------------------
library_clause
    :   LIBRARY identifier ( COMMA identifier )* SEMI
    ;

//---------------------------------------------------------------------------------------
// Interface
//---------------------------------------------------------------------------------------
interface_ambigous_declaration_procedure
    :   identifier_list COLON
        (
                IN? subtype_indication ( VARASGN expression )?
            |   ( OUT | INOUT ) subtype_indication ( VARASGN expression )?
        )
    ;

//made contant not optional. ambigous case is handled separately.
interface_constant_declaration
    :   CONSTANT identifier_list COLON IN? subtype_indication
        ( VARASGN expression )?
    ;

interface_constant_declaration_optional_class
    :   CONSTANT? identifier_list COLON IN? subtype_indication
        ( VARASGN expression )?
    ;

interface_element_function
    :   interface_constant_declaration_optional_class
    |   interface_signal_declaration
    |   interface_variable_declaration
    |   interface_file_declaration
    ;

interface_element_procedure
    :   interface_constant_declaration
    |   interface_signal_declaration
    |   interface_variable_declaration
    |   interface_file_declaration
    |   interface_ambigous_declaration_procedure
    ;

interface_file_declaration
    :   FILE identifier_list COLON subtype_indication
    ;

//made signal not optional. ambigous case is handled separately.
interface_signal_declaration
    :   SIGNAL identifier_list COLON signal_mode? subtype_indication BUS?
        ( VARASGN expression )?
    ;

interface_signal_declaration_for_port
    :   SIGNAL? identifier_list COLON signal_mode? subtype_indication BUS?
        ( VARASGN expression )?
    ;

//made variable not optional. ambigous case is handled separately.
interface_variable_declaration
    :   VARIABLE identifier_list COLON signal_mode? subtype_indication
        ( VARASGN expression )?
    ;

//---------------------------------------------------------------------------------------
// Subprogram
//---------------------------------------------------------------------------------------
designator
    :   identifier
    |   STRING_LITERAL
    ;
    
subprogram_body_part
    :   IS
        subprogram_declarative_part?
        BEGIN
        subprogram_statement_part?
        END subprogram_kind? designator?
    ;
    
subprogram_declaration
    :   subprogram_specification SEMI
    ;
    
subprogram_body_or_declaration
    :   subprogram_specification (subprogram_body_part)?
        SEMI
    ;

subprogram_declarative_item
    :   subprogram_body_or_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   variable_declaration
    |   file_declaration
    |   alias_declaration
    |   attribute_declaration
    |   attribute_specification
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

subprogram_declarative_part
    :   subprogram_declarative_item+
    ;

subprogram_kind
    :   PROCEDURE
    |   FUNCTION
    ;

subprogram_specification
    :   PROCEDURE designator ( LPAREN parameter_interface_list_procedure RPAREN )?
    |   ( PURE | IMPURE )? FUNCTION designator
        ( LPAREN parameter_interface_list_function RPAREN )? RETURN name
    ;
    
parameter_interface_list_function
    :   interface_element_function ( SEMI interface_element_function )*
    ;

parameter_interface_list_procedure
    :   interface_element_procedure ( SEMI interface_element_procedure )*
    ;

subprogram_statement_part
    :   sequential_statement+
    ;

//---------------------------------------------------------------------------------------
// Component
//---------------------------------------------------------------------------------------
component_configuration
    :   FOR cs=component_specification
        block_configuration?
        END FOR SEMI
    ;

//TODO: check repeated label
component_declaration
    :   COMPONENT identifier IS?
        generic_clause?
        port_clause?
        END COMPONENT end_identifier? SEMI
    ;
    
instantiated_unit
    :   COMPONENT? name_without_parens
    |   ENTITY name_without_parens ( LPAREN identifier RPAREN )?
    |   CONFIGURATION name_without_parens
    ;

instantiation_list
    :   instantiation_label1=identifier ( COMMA identifier )*
    |   OTHERS
    |   ALL
    ;

generic_map_aspect
    :   GENERIC MAP LPAREN association_list RPAREN
    ;
    
port_map_aspect
    :   PORT MAP LPAREN association_list RPAREN
    ;
    
binding_indication
    :   ( USE entity_aspect )? generic_map_aspect? port_map_aspect?
    ;
    
component_instantiation_statement
    :   instantiated_unit
        generic_map_aspect?
        port_map_aspect? SEMI
    ;

component_specification
    :   instantiation_list COLON name
    ;


//---------------------------------------------------------------------------------------
// Configuration
//---------------------------------------------------------------------------------------
//TODO: check repeated label
configuration_declaration
    :   CONFIGURATION identifier OF name IS
        configuration_declarative_part?
        block_configuration
        END CONFIGURATION? end_identifier? SEMI
    ;

configuration_declarative_item
    :   use_clause
    |   attribute_specification
    |   group_declaration
    ;

configuration_declarative_part
    :   configuration_declarative_item+
    ;

configuration_item
    :   block_configuration
    |   component_configuration
    ;

configuration_specification
    :   FOR component_specification binding_indication SEMI
    ;

//---------------------------------------------------------------------------------------
// Concurrent statements
//---------------------------------------------------------------------------------------
block_configuration
    :   FOR block_specification
        use_clause*
        configuration_item*
        END FOR SEMI
    ;

block_declarative_item
    :   subprogram_body_or_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   signal_declaration
    |   variable_declaration
    |   file_declaration
    |   alias_declaration
    |   component_declaration
    |   attribute_declaration
    |   attribute_specification
    |   configuration_specification
    |   disconnection_specification
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

block_declarative_part
    :   block_declarative_item+
    ;

block_header
    :   ( generic_clause ( generic_map_aspect SEMI )? )?
        ( port_clause ( port_map_aspect SEMI )? )?
    ;

block_specification
    :   name
    ;
    
//TODO: check repeated label
block_statement
    :   BLOCK ( LPAREN guard_expression=expression RPAREN )? IS?
        block_header
        block_declarative_part?
        BEGIN
        block_statement_part?
        END BLOCK end_identifier? SEMI
    ;

block_statement_part
    :   concurrent_statement+
    ;
    
concurrent_assertion_statement
    :   assertion SEMI
    ;

concurrent_procedure_call_statement
    :   procedure_call SEMI
    ;

//a procedure call statement might be a component instantiation
concurrent_statement
    :   (
            identifier COLON
            (
                    concurrent_statement_optional_label
                |   concurrent_statement_with_label
            )
        )
    |   concurrent_statement_optional_label
    ;

concurrent_statement_with_label
    :   block_statement
    |   component_instantiation_statement
    |   generate_statement
    ;

concurrent_statement_optional_label
    :   POSTPONED concurrent_statement_optional_label_2
    |   concurrent_statement_optional_label_2
    ;

concurrent_statement_optional_label_2
    :   process_statement
    |   conditional_signal_assignment
    |   concurrent_procedure_call_statement
    |   concurrent_assertion_statement
    |   selected_signal_assignment
    ;

//TODO: check repeated label
generate_statement
    :   generation_scheme
        GENERATE
        ( block_declarative_item* BEGIN )?
        concurrent_statement*
        END GENERATE end_identifier? SEMI
    ;

generation_scheme
    :   FOR parameter_specification
    |   IF condition
    ;
    
//---------------------------------------------------------------------------------------
// Entity
//---------------------------------------------------------------------------------------

entity_aspect
    :   ENTITY name_without_parens ( LPAREN identifier RPAREN )?
    |   CONFIGURATION name
    |   OPEN
    ;

entity_class
    :   ENTITY
    |   ARCHITECTURE
    |   CONFIGURATION
    |   PROCEDURE
    |   FUNCTION
    |   PACKAGE
    |   TYPE
    |   SUBTYPE
    |   CONSTANT
    |   SIGNAL
    |   VARIABLE
    |   COMPONENT
    |   LABEL
    |   LITERAL
    |   UNITS
    |   GROUP
    |   FILE
    ;

entity_class_entry
    :   entity_class BOX?
    ;

entity_class_entry_list
    :   entity_class_entry ( COMMA entity_class_entry )*
    ;
    
entity_declaration
    :   ENTITY identifier IS
        entity_header
        entity_declarative_part?
        ( BEGIN entity_statement_part? )?
        END ENTITY? end_identifier? SEMI
    ;

entity_declarative_item
    :   subprogram_body_or_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   signal_declaration
    |   variable_declaration
    |   file_declaration
    |   alias_declaration
    |   attribute_declaration
    |   attribute_specification
    |   disconnection_specification
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

entity_declarative_part
    :   entity_declarative_item+
    ;

entity_designator
    :   entity_tag signature?
    ;

entity_header
    :   generic_clause?
        port_clause?
    ;

entity_name_list
    :   entity_designator ( COMMA entity_designator )*
    |   OTHERS
    |   ALL
    ;

entity_specification
    :   entity_name_list COLON entity_class
    ;

entity_statement
    :   ( identifier COLON )? POSTPONED? entity_statement2
    ;

entity_statement2
    :   concurrent_assertion_statement
    |   concurrent_procedure_call_statement
    |   process_statement
    ;

entity_statement_part
    :   entity_statement+
    ;

entity_tag
    :   identifier
    |   CHARACTER_LITERAL
    |   STRING_LITERAL
    ;

generic_clause
    :   GENERIC LPAREN generic_interface_list RPAREN SEMI
    ;
    
generic_interface_list
    :   interface_constant_declaration_optional_class
        ( SEMI interface_constant_declaration_optional_class )*
    ;
    
port_clause
    :   PORT LPAREN port_interface_list RPAREN SEMI
    ;

port_interface_list
    :   interface_signal_declaration_for_port ( SEMI interface_signal_declaration_for_port )*
    ;

//---------------------------------------------------------------------------------------
//     Process
//---------------------------------------------------------------------------------------
process_declarative_item
    :   subprogram_body_or_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   variable_declaration
    |   file_declaration
    |   alias_declaration
    |   attribute_declaration
    |   attribute_specification
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

process_declarative_part
    :   process_declarative_item+
    ;

//TODO: check repeated label
process_statement
    :   PROCESS ( LPAREN sensitivity_list RPAREN )? IS?
        process_declarative_part?
        BEGIN
        process_statement_part?
        END POSTPONED? PROCESS end_identifier? SEMI
    ;

process_statement_part
    :   sequential_statement+
    ;


//---------------------------------------------------------------------------------------
//     Package
//---------------------------------------------------------------------------------------
//TODO: check repeated label
package_body
    :   PACKAGE BODY identifier IS
        package_body_declarative_part?
        END ( PACKAGE BODY )? end_identifier? SEMI
    ;

package_body_declarative_item
    :   subprogram_body_or_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   shared_variable_declaration=variable_declaration
    |   file_declaration
    |   alias_declaration
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

package_body_declarative_part
    :   package_body_declarative_item+
    ;

//TODO: check repeated label
package_declaration
    :   PACKAGE identifier IS
        package_declarative_part?
        END PACKAGE? end_identifier? SEMI
    ;

package_declarative_item
    :   subprogram_declaration
    |   type_declaration
    |   subtype_declaration
    |   constant_declaration
    |   signal_declaration
    |   shared_variable_declaration=variable_declaration
    |   file_declaration
    |   alias_declaration
    |   component_declaration
    |   attribute_declaration
    |   attribute_specification
    |   disconnection_specification
    |   use_clause
    |   group_template_declaration
    |   group_declaration
    ;

package_declarative_part
    :   package_declarative_item+
    ;


//---------------------------------------------------------------------------------------
//     Architecture
//---------------------------------------------------------------------------------------
architecture_body
    :   ARCHITECTURE identifier OF entity=name IS
        architecture_declarative_part?
        BEGIN
        architecture_statement_part?
        END ARCHITECTURE? end_identifier? SEMI
    ;
architecture_declarative_part
	:   block_declarative_item+
	;

architecture_statement_part
	:   concurrent_statement+
	;


//---------------------------------------------------------------------------------------
//     Design File
//---------------------------------------------------------------------------------------
design_file
    :   design_unit* EOF
    ;

design_unit
    :   context_clause (primary_unit|secondary_unit)
    ;
    
context_clause
    :   (library_clause | use_clause)*
    ;

//---------------------------------------------------------------------------------------
// Primary unit
//---------------------------------------------------------------------------------------
primary_unit
    :
    entity_declaration
    |   configuration_declaration
    |   package_declaration
    ;
    
//---------------------------------------------------------------------------------------
// Secondary unit
//---------------------------------------------------------------------------------------
secondary_unit
    :   architecture_body
    |   package_body
    ;