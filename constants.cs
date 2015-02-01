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
    FALSE,          // "False"
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
    public static readonly Dictionary<string, int> DICTIONARY = new Dictionary<string, int>() 
    {
        {"and", 1}, {"begin", 2}, {"Boolean", 3}, {"div", 4}, {"do", 5}, {"downto", 6},
        {"else", 7}, {"end", 8}, {"false", 9}, {"fixed", 10}, {"float", 11}, {"for", 12},
        {"function", 13}, {"if", 14}, {"integer", 15}, {"mod", 16}, {"not", 17}, 
        {"or", 18}, {"procedure", 19}, {"program", 20}, {"read", 21}, {"repeat", 22},
        {"string", 23}, {"then", 24}, {"true", 25}, {"to", 26}, {"type", 27}, {"until", 28}, 
        {"var", 29}, {"while", 30}, {"write", 31}, {"writeln", 32}
    };
}
