using System;

// Contains all constants for our program. Put any large and/or repeated strings here.
public static class Constants {
	public const string USAGE_HELP = "Usage: mono YµP.exe <µPascal filename> <output filename>";

	// All token IDs (add more as we come across them)
	public enum TOKENS {
		// Reserved Words
		AND,			// "and"
		BEGIN,			// "begin"
		BOOLEAN,		// "boolean"
		DIV,			// "div"
		DO, 			// "do"
		DOWNTO,			// "downto"
		ELSE,			// "else"
		END,			// "end"
		FALSE,			// "False"
		FIXED,			// "fixed"
		FLOAT,			// "float"
		FOR,			// "for"
		FUNCTION,		// "function"
		IF,				// "if"
		INTEGER,		// "integer"
		MOD,			// "mod"
		NOT,			// "not"
		OR,				// "or"
		PROCEDURE,		// "procedure"
		PROGRAM,		// "program"
		READ,			// "read"
		REPEAT,			// "repeat"
		STRING,			// "string"
		THEN,			// "then"
		TRUE,			// "true"
		TO,				// "to"
		TYPE,			// "type"
		UNTIL,			// "until"
		VAR,			// "var"
		WHILE,			// "while"
		WRITE,			// "write"
		WRITELN,		// "writeln"

		// Identifiers and Literals
		IDENTIFIER,		// (letter | "_"(letter | digit)){["_"](letter | digit)}
		INTEGER_LIT,	// digit{digit}
		FIXED_LIT,		// digit{digit} "." digit{digit}
		FLOAT_LIT,		// (digit{digit} | digit{digit} "." digit{digit}) ("e"|"E")["+"|"-"]digit{digit}
		STRING_LIT,		// "'" {"''" | AnyCharacterExceptApostropheOrEOL} "'"

		// Single String Tokens
		ASSIGN,			// ":="
		COLON,			// ":"
		COMMA,			// ","
		EQUAL,			// "="
		FLOAT_DIVIDE,	// "/"
		GEQUAL,			// ">="
		GTHAN,			// ">"
		LEQUAL,			// "<="
		LPAREN,			// "("
		LTHAN,			// "<"
		MINUS,			// "-"
		NEQUAL,			// "<>"
		PERIOD,			// "."
		PLUS,			// "+"
		RPAREN,			// ")"
		SCOLON,			// ";"
		TIMES,			// "*"

		// Special Tokens
		EOF,			// end-of-file character
		RUN_COMMENT,	// run-on comment error
		RUN_STRING,		// run-on string error
		ERROR,			// token for other scan errors
	};
}
