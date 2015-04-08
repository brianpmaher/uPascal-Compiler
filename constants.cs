/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;
using System.Collections.Generic;

// All token IDs
public enum TOKENS {
    // Reserved Words
    AND = 0,        // "and"
    BEGIN,          // "begin"
    BOOLEAN,        // "boolean"
    DIV,            // "div"
    DO,             // "do"
    DOWNTO,         // "downto"
    ELSE,           // "else"
    END,            // "end"
    FALSE,          // "false"
    FIXED,          // "fixed"
    FLOAT,          // "float"
    FOR,            // "for"
    FUNCTION,       // "function"
    IF,             // "if"
    INTEGER,        // "integer"
    MOD,            // "mod"
    NOT,            // "not"
    OR,             // "or"
    PROCEDURE,      // "procedure"
    PROGRAM,        // "program"
    READ,           // "read"
    REPEAT,         // "repeat"
    STRING,         // "string"
    THEN,           // "then"
    TRUE,           // "true"
    TO,             // "to"
    TYPE,           // "type"
    UNTIL,          // "until"
    VAR,            // "var"
    WHILE,          // "while"
    WRITE,          // "write"
    WRITELN,        // "writeln"

    // Identifiers and Literals
    IDENTIFIER,     // (letter | "_"(letter | digit)){["_"](letter | digit)}
    INTEGER_LIT,    // digit{digit}
    FIXED_LIT,      // digit{digit} "." digit{digit}
    FLOAT_LIT,      // (digit{digit} | digit{digit} "." digit{digit}) ("e"|"E")["+"|"-"]digit{digit}
    STRING_LIT,     // "'" {"''" | AnyCharacterExceptApostropheOrEOL} "'"

    // Single String Tokens
    ASSIGN,         // ":="
    COLON,          // ":"
    COMMA,          // ","
    EQUAL,          // "="
    FLOAT_DIVIDE,   // "/"
    GEQUAL,         // ">="
    GTHAN,          // ">"
    LEQUAL,         // "<="
    LPAREN,         // "("
    LTHAN,          // "<"
    MINUS,          // "-"
    NEQUAL,         // "<>"
    PERIOD,         // "."
    PLUS,           // "+"
    RPAREN,         // ")"
    SCOLON,         // ";"
    TIMES,          // "*"

    // Special Tokens
    EOF,            // end-of-file character
    RUN_COMMENT,    // run-on comment error
    RUN_STRING,     // run-on string error
    ERROR,          // token for other scan errors
};

public enum TYPES {
    BOOLEAN   =     TOKENS.BOOLEAN,
    FIXED     =     TOKENS.FIXED,
    FLOAT     =     TOKENS.FLOAT,
    INTEGER   =     TOKENS.INTEGER,
    STRING    =     TOKENS.STRING,
    NONE
};

public enum KINDS {
    VAR       =     TOKENS.VAR,
    PROCEDURE =     TOKENS.PROCEDURE,
    FUNCTION  =     TOKENS.FUNCTION,
    PARAMETER,
    NONE
};

// Contains all constants for our program. Put any large and/or repeated strings here.
public static class Constants {
    // Generic string messages
    public const string USAGE_HELP = "Usage: mono YµP.exe <µPascal filename> <output filename>";
    public const string TOKEN_TABLE_HEADER_FORMAT = "{0,-12}{1,8}{2,8}\t{3}";

    // Error messages
    public const string ERROR_FILE_FORMAT =
        "ERROR: File format does not conform to format: <filename>.mp; ";
    public const string ERROR_NO_NEWLINE = "ERROR: File must end in a newline character;";
    public const string ERROR_DISPATCHER_DIGIT =
        "ERROR: Dispatcher done goofed. Passed {0} to the digit FSA, but {0} is not a digit.";
    public const string ERROR_DISPATCHER_PUNCTUATION =
        "ERROR: Dispatcher done goofed. Passed {0} to the punctuation FSA, but {0} is not " +
        "punctuation.";
    public const string ERROR_DISPATCHER_LETTERS =
        "ERROR: Dispatcher done goofed. Passed {0} to the letters FSA, but {0} is not a letter";
    public const string ERROR_PARSER =
        "ERROR: Expected {0}, got {1}";
    public const string ERROR_SYNTAX =
        "Syntax error found on line {0}, column {1} -> ";

    // List of whitespace characters
    public const string WHITESPACE = " \r\n\t";
    // List of digit characters
    public const string DIGITS = "0123456789";
    // List of letters
    public const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    // Dictionary of punctuation
    public static readonly Dictionary<string, TOKENS> PUNCTUATION =
        new Dictionary<string, TOKENS>() {
        {":=", TOKENS.ASSIGN},          {":", TOKENS.COLON},
        {",", TOKENS.COMMA},            {"=", TOKENS.EQUAL},
        {"/", TOKENS.FLOAT_DIVIDE},     {">=", TOKENS.GEQUAL},
        {">", TOKENS.GTHAN},            {"<=", TOKENS.LEQUAL},
        {"(", TOKENS.LPAREN},           {"<", TOKENS.LTHAN},
        {"-", TOKENS.MINUS},            {"<>", TOKENS.NEQUAL},
        {".", TOKENS.PERIOD},           {"+", TOKENS.PLUS},
        {")", TOKENS.RPAREN},           {";", TOKENS.SCOLON},
        {"*", TOKENS.TIMES}
    };
    // Dictionary of reserved words
    public static readonly Dictionary<string, TOKENS> RESERVE_WORDS =
        new Dictionary<string, TOKENS>() {
        {"and", TOKENS.AND},            {"begin", TOKENS.BEGIN},
        {"boolean", TOKENS.BOOLEAN},    {"downto", TOKENS.DOWNTO},
        {"div", TOKENS.DIV},            {"do", TOKENS.DO},
        {"else", TOKENS.ELSE},          {"end", TOKENS.END},
        {"false", TOKENS.FALSE},        {"for", TOKENS.FOR},
        {"fixed", TOKENS.FIXED},        {"float", TOKENS.FLOAT},
        {"function", TOKENS.FUNCTION},  {"if", TOKENS.IF},
        {"integer", TOKENS.INTEGER},    {"mod", TOKENS.MOD},
        {"not", TOKENS.NOT},            {"then", TOKENS.THEN},
        {"or", TOKENS.OR},              {"procedure", TOKENS.PROCEDURE},
        {"program", TOKENS.PROGRAM},    {"read", TOKENS.READ},
        {"repeat", TOKENS.REPEAT},      {"string", TOKENS.STRING},
        {"true", TOKENS.TRUE},          {"to", TOKENS.TO},
        {"type", TOKENS.TYPE},          {"while", TOKENS.WHILE},
        {"until", TOKENS.UNTIL},        {"var", TOKENS.VAR},
        {"write", TOKENS.WRITE},        {"writeln", TOKENS.WRITELN}
    };
}
