using System;
using System.IO;
using System.Collections.Generic;

public class Scanner {
    private int __curByte = 0;
    private int __column = 1;
    private int __line = 1;
    private char[] __bytes;
    private List<Token> __tokens = new List<Token>();

    // Initializes the scanner and checks for file format
    public List<Token> initializeScanner(string fileName) {
        try {
            if(!fileName.EndsWith(".mp")) {
                // File format exception
                throw new Exception(Constants.ERROR_FILE_FORMAT);
            } else {
                // Grab all bytes from the file
                StreamReader reader = new StreamReader(
                    Path.GetFullPath(fileName));
                string temp = reader.ReadToEnd();
                __bytes = temp.ToCharArray();
                // Caching variables to save memory
                int length = __bytes.Length;
                string ws = Constants.WHITESPACE;
                // Loop until EOF, ignoring whitespace
                while(__curByte < length){
                    // TODO: Handle comments like we handle whitespace
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
                        __tokens.Add(getNextToken());
                    }
                }
            }
        } catch(Exception ex) {
            Console.WriteLine(ex);
        }
        return __tokens;
    }

    private Token getNextToken() {
        char next = __bytes[__curByte];
        if(Constants.DIGITS.Contains("" + next)){
            return fsaDigit();
        } else if(Constants.PUNCTUATION.Contains("" + next)){
            return fsaPunct();
        } else if(Constants.LETTERS.Contains("" + next) || next == '_'){
            return fsaLetter();
        } else if(next == '\''){
            return fsaString();
        } else {
            __curByte++;
            __column++;
            return new Token("" + next, TOKENS.ERROR, __column - 1, __line);
        }
    }

    // Finite State Automatons
    private Token fsaLetter() {
        int column = __column;
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
                lexeme += next;
                __column--;
                __curByte--;
                return new Token (lexeme, TOKENS.ERROR, column, __line);
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
                return new Token (lexeme, TOKENS.ERROR, column, __line);
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
                if (Constants.RESERVE_WORDS.ContainsKey(lexeme)){
                    return new Token (lexeme, Constants.RESERVE_WORDS[lexeme], column, __line);
                } else {
                    return new Token (lexeme, TOKENS.IDENTIFIER, column, __line);
                }
            }
    }

    private Token fsaDigit() {
        int column = __column;
        string lexeme = "";
        string DIGITS = Constants.DIGITS;
        char next;
        TOKENS token;
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
                return new Token(lexeme, token, column, __line);
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
                return new Token(lexeme, token, column, __line);
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
                return new Token(lexeme, token, column, __line);
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
                return new Token(lexeme, token, column, __line);
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
                return new Token(lexeme, token, column, __line);
            }
    }

    private Token fsaPunct() { // doesn't include quotes
        int column = __column;
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
                        return new Token(lexeme, TOKENS.COMMA, column, __line);
                    case '=':
                        return new Token(lexeme, TOKENS.EQUAL, column, __line);
                    case '/':
                        return new Token(lexeme, TOKENS.FLOAT_DIVIDE, column, __line);
                    case '>':
                        goto S3;
                    case '<':
                        goto S2;
                    case '(':
                        return new Token(lexeme, TOKENS.LPAREN, column, __line);
                    case '-':
                        return new Token(lexeme, TOKENS.MINUS, column, __line);
                    case '.':
                        return new Token(lexeme, TOKENS.PERIOD, column, __line);
                    case '+':
                        return new Token(lexeme, TOKENS.PLUS, column, __line);
                    case ')':
                        return new Token(lexeme, TOKENS.RPAREN, column, __line);
                    case ';':
                        return new Token(lexeme, TOKENS.SCOLON, column, __line);
                    case '*':
                        return new Token(lexeme, TOKENS.TIMES, column, __line);
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
                return new Token(lexeme, TOKENS.ASSIGN, column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.COLON, column, __line);
            }
        S2: // "<"
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '=') {
                lexeme += next;
                return new Token(lexeme, TOKENS.LEQUAL, column, __line);
            } else if(next == '>') {
                lexeme += next;
                return new Token(lexeme, TOKENS.NEQUAL, column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.LTHAN, column, __line);
            }
        S3: // ">"
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '=') {
                lexeme += next;
                return new Token(lexeme, TOKENS.GEQUAL, column, __line);
            } else {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.GTHAN, column, __line);
            }
    }

    private Token fsaString() { // surrounded by quotes
        string lexeme = "";
        char next;
        int column = __column;
        goto S0;
        S0:
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '\''){
                // We don't include the opening apostrophe
                goto S1;
            } else {
                return new Token(lexeme, TOKENS.ERROR, column, __line);
            }
        S1:
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '\''){
                // Closing apostrophe, don't include
                goto S2;
            } else if(next == '\n'){
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.RUN_STRING, column, __line);
            } else {
                lexeme += next;
                goto S3;
            }
        S2:
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '\''){
                // This apostrophe we include
                lexeme += next;
                goto S1;
            } else {
                // Success, reset fp
                // We don't include the closing apostrophe
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.STRING_LIT, column, __line);
            }
        S3:
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '\''){
                // Potentially closing apostrophe, don't include
                goto S2;
            } else if( next == '\n'){
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.RUN_STRING, column, __line);
            } else {
                lexeme += next;
                goto S3;
            }
    }
}
