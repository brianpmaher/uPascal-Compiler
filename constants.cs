using System;
using System.Collections.Generic;

// All token IDs (add more as we come across them)
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

// Contains all constants for our program. Put any large and/or repeated strings here.
public static class Constants {
    // Generic string messages
    public const string USAGE_HELP = "Usage: mono YµP.exe <µPascal filename> <output filename>";
    public const string TOKEN_TABLE_HEADER = "token\t\tlexeme\tline\tcolumn";

    // Error messages
    public const string ERROR_FILE_FORMAT = "ERROR: File format does not conform to format: <filename>.mp; ";

    public const string ERROR_DISPATCHER_DIGIT =
        "ERROR: Dispatcher done goofed. Passed {0} to the digit FSA, but {0} is not a digit.";
    public const string ERROR_DISPATCHER_PUNCTUATION =
        "ERROR: Dispatcher done goofed. Passed {0} to the punctuation FSA, but {0} is not punctuation.";
    public const string ERROR_DISPATCHER_LETTERS =
        "ERROR: Dispatcher done goofed. Passed {0} to the letters FSA, but {0} is not a letter";

    // List of whitespace characters
    public const string WHITESPACE = " \r\n\t";
    // List of digit characters
    public const string DIGITS = "0123456789";

    // List of punctuation characters
    public const string PUNCTUATION = ":,=/><(-.+);*";
    // List of letters
    public const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    // Dictionary of reserved words
    public static readonly Dictionary<string, TOKENS> RESERVE_WORDS = new Dictionary<string, TOKENS>()
    {
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
