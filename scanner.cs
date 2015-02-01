using System;
using System.IO;
using System.Collections.Generic;

public class Scanner {
    private int __curByte = 0;
    private int __column = 1;
    private int __line = 1;
    private char[] __bytes;
    private List<Token> __tokens;

    // Initializes the scanner and checks for file format
    public void initializeScanner(string fileName) {
        try {
            if(!fileName.EndsWith(".mp")) {
                // File format exception
                throw new Exception(Constants.ERROR_FILE_FORMAT);
            } else {
                // Grab all bytes from the file
                StreamReader reader = new StreamReader(fileName);
                string temp = reader.ReadToEnd();
                __bytes = temp.ToCharArray();

                // Caching variables to save memory
                int length = __bytes.Length;
                string ws = Constants.WHITESPACE;

                // Loop until EOF, ignoring whitespace
                while(__curByte < length){
                    if(ws.Contains("" + __bytes[__curByte])) {
                        if(__bytes[__curByte] == '\n'){
                            __line++;
                            __column = 0;
                        } else if(__bytes[__curByte] == ' '){
                            __column++;
                        }
                        __curByte++;
                        continue;
                    } else {
                        getNextToken();
                    }
                }
            }
        } catch(Exception ex) {
            Console.WriteLine(ex);
        }
    }

    // This is the dispatcher
    private int getNextToken() {
        return 0; // change this
    }

    private int getLineNumber() {
        return 0; // change this
    }

    private int getColumnNumber() {
        return 0; // change this
    }

    // Finite State Automatons
<<<<<<< HEAD
    private Token fsaLetter() {
=======
    private Tuple<string, TOKENS> fsaLetter() {
>>>>>>> 22bf12013bce6f8040b6b17f2d16776212ac2def
        string lexeme = "",
        LETTERS = Constants.LETTERS,
        DIGITS = Constants.DIGITS;
        char next;
        goto S0;
        S0: //start state
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '_') {
                lexeme += next;
                goto S1;
            } else if (LETTERS.Contains("" + next)) {
                lexeme += next;
                goto S2;
            } else {
                __column--;
                __curByte--;
                throw new Exception(String.Format(Constants.ERROR_DISPATCHER_LETTERS, next));
            }
        S1: // underscore route
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if (LETTERS.Contains("" + next) || DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S2;
            } else {
                __column--;
                __curByte--;
                throw new Exception(String.Format(Constants.ERROR_DISPATCHER_LETTERS, next));
            }
        S2: // letter/digit
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if (LETTERS.Contains("" + next) || DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S2;
            } else if (next == '_') {
                lexeme += next;
                goto S1;
            } else {
                __column--;
                __curByte--;
<<<<<<< HEAD
                if (Constants.DICTIONARY.ContainsKey(lexeme)){
                    switch (lexeme)
                    {
                        case "and": return new Token (lexeme, TOKENS.AND, __column, __line);
                        case "begin": return new Token (lexeme, TOKENS.BEGIN, __column, __line);
                        case "Boolean": 
                            return new Token (lexeme, TOKENS.BOOLEAN, __column, __line);
                        case "div": return new Token (lexeme, TOKENS.DIV, __column, __line);
                        case "do": return new Token (lexeme, TOKENS.DO, __column, __line);
                        case "downto": return new Token (lexeme, TOKENS.DOWNTO, __column, __line);
                        case "else": return new Token (lexeme, TOKENS.ELSE, __column, __line);
                        case "end": return new Token (lexeme, TOKENS.END, __column, __line);
                        case "false": return new Token (lexeme, TOKENS.FALSE, __column, __line);
                        case "fixed": return new Token (lexeme, TOKENS.FIXED, __column, __line);
                        case "float": return new Token (lexeme, TOKENS.FLOAT, __column, __line);
                        case "for": return new Token (lexeme, TOKENS.FOR, __column, __line);
                        case "function": 
                            return new Token (lexeme, TOKENS.FUNCTION, __column, __line);
                        case "if": return new Token (lexeme, TOKENS.IF, __column, __line);
                        case "integer": 
                            return new Token (lexeme, TOKENS.INTEGER, __column, __line);
                        case "mod": return new Token (lexeme, TOKENS.MOD, __column, __line);
                        case "not": return new Token (lexeme, TOKENS.NOT, __column, __line);
                        case "or": return new Token (lexeme, TOKENS.OR, __column, __line);
                        case "procedure": 
                            return new Token (lexeme, TOKENS.PROCEDURE, __column, __line);
                        case "program": 
                            return new Token (lexeme, TOKENS.PROGRAM, __column, __line);
                        case "read": return new Token (lexeme, TOKENS.READ, __column, __line);
                        case "repeat": return new Token (lexeme, TOKENS.REPEAT, __column, __line);
                        case "string": return new Token (lexeme, TOKENS.STRING, __column, __line);
                        case "then": return new Token (lexeme, TOKENS.THEN, __column, __line);
                        case "true": return new Token (lexeme, TOKENS.TRUE, __column, __line);
                        case "to": return new Token (lexeme, TOKENS.TO, __column, __line);
                        case "type": return new Token (lexeme, TOKENS.TYPE, __column, __line);
                        case "until": return new Token (lexeme, TOKENS.UNTIL, __column, __line);
                        case "var": return new Token (lexeme, TOKENS.VAR, __column, __line);
                        case "while": return new Token (lexeme, TOKENS.WHILE, __column, __line);
                        case "write": return new Token (lexeme, TOKENS.WRITE, __column, __line);
                        case "writeln": 
                            return new Token (lexeme, TOKENS.WRITELN, __column, __line);
                    }
                } else {
                    return new Token (lexeme, TOKENS.IDENTIFIER, __column, __line);
                } 
                throw new Exception(String.Format(Constants.ERROR_DISPATCHER_LETTERS, next));
=======
                return new Tuple<string, TOKENS> (lexeme, TOKENS.IDENTIFIER);
>>>>>>> 22bf12013bce6f8040b6b17f2d16776212ac2def
            }
    }

    private Token fsaDigit() {
        string lexeme = "";
        string DIGITS = Constants.DIGITS;
        TOKENS token;
        char next;
        goto S0; // Smothers warning about S0 label not used
        S0: // Start state
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S1;
            } else {
                __curByte--;
                __column--;
                throw new Exception(
                    String.Format(Constants.ERROR_DISPATCHER_DIGIT, next)
                );
            }
        S1: // One or more digits have been read
            token = TOKENS.INTEGER_LIT;
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S1;
            } else if(next == '.'){
                lexeme += next;
                goto S2;
            } else if(next == 'e' || next == 'E'){
                lexeme += next;
                goto S4;
            } else { // This is a success state, so reset the fp and return the lexeme
                __curByte--; // reset the fp
                __column--;
                return new Token(lexeme, token, __column, __line);
            }
        S2: // A '.' has been read
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S3;
            } else {
                // Must remove the last character (.) from lexeme
                lexeme.Remove(lexeme.Length - 1);
                __curByte -= 2;
                __column -=2;
                return new Token(lexeme, token, __column, __line);
            }
        S3: // Digits have followed a valid '.'
            token = TOKENS.FIXED_LIT;
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S3;
            } else if(next == 'e' || next == 'E'){
                lexeme += next;
                goto S4;
            } else {
                __curByte--;
                __column--;
                return new Token(lexeme, token, __column, __line);
            }
        S4: // An e or E has been read
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '+' || next == '-'){
                lexeme += next;
                goto S5;
            } else if (DIGITS.Contains("" + next)){
                lexeme += next;
                goto S6;
            } else {
                // Must remove the last character (e or E) from lexeme
                lexeme = lexeme.Remove(lexeme.Length - 1);
                __curByte -= 2;
                __column -= 2;
                return new Token(lexeme, token, __column, __line);
            }
        S5: // A + or - has followed a valid 'e' or 'E'
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S6;
            } else {
                // Must remove the last two characters ((e or E) and (- or +))
                lexeme = lexeme.Remove(lexeme.Length - 2);
                __curByte -= 3;
                __column -= 3;
                return new Token(lexeme, token, __column, __line);
            }
        S6: // A float has been found, keep parsing digits
            token = TOKENS.FLOAT_LIT;
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)){
                lexeme += next;
                goto S6;
            } else {
                __curByte--;
                __column--;
                return new Token(lexeme, token, __column, __line);
            }
    }

    private Token fsaPunct() { // doesn't include quotes
        string  lexeme = "",
                PUNCTUATION = Constants.PUNCTUATION;
        char    next;
        goto S0; // Smothers warning about S0 label not used
        S0: // Start state
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(PUNCTUATION.Contains("" + next)){
                lexeme += next;
                switch(next) {
                    case ':':
                        goto S1;
                    case ',':
                        return new Token(lexeme, TOKENS.COMMA, __column, __line);
                    case '=':
                        return new Token(lexeme, TOKENS.EQUAL, __column, __line);
                    case '/':
                        return new Token(lexeme, TOKENS.FLOAT_DIVIDE, __column, __line);
                    case '>':
                        goto S3;
                    case '<':
                        goto S2;
                    case '(':
                        return new Token(lexeme, TOKENS.LPAREN, __column, __line);
                    case '-':
                        return new Token(lexeme, TOKENS.MINUS, __column, __line);
                    case '.':
                        return new Token(lexeme, TOKENS.PERIOD, __column, __line);
                    case '+':
                        return new Token(lexeme, TOKENS.PLUS, __column, __line);
                    case ')':
                        return new Token(lexeme, TOKENS.RPAREN, __column, __line);
                    case ';':
                        return new Token(lexeme, TOKENS.SCOLON, __column, __line);
                    case '*':
                        return new Token(lexeme, TOKENS.TIMES, __column, __line);
                }
            } else {
                __curByte--;
                throw new Exception(
                    String.Format(Constants.ERROR_DISPATCHER_PUNCTUATION, next)
                );
            }
        S1: // ":"
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '=') {
                lexeme += next;
                return new Token(lexeme, TOKENS.ASSIGN, __column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.COLON, __column, __line);
            }
        S2: // "<"
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '=') {
                lexeme += next;
                return new Token(lexeme, TOKENS.LEQUAL, __column, __line);
            } else if(next == '>') {
                lexeme += next;
                return new Token(lexeme, TOKENS.NEQUAL, __column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.LTHAN, __column, __line);
            }
        S3: // ">"
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '=') {
                lexeme += next;
                return new Token(lexeme, TOKENS.GEQUAL, __column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.GTHAN, __column, __line);
            }
    }

    private int fsaString() { // surrounded by quotes
        return 0; // change this
    }
}
