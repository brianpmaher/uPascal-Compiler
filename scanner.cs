/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;
using System.IO;
using System.Collections.Generic;

/*
 *  Scanner : Iterates through every character in the µPascal file generating a list of tokens for
 *          : each matched token type.
 */
public partial class Scanner {
    // Current byte pointer in the file µPascal file
    private int     __curByte = 0,
    // Current column number in the µPascal file
                    __column = 1,
    // Current line number in the µPascal file
                    __line = 1;
    // Array of all bytes in the µPascal file
    private char[]  __bytes;
    // Array of tokens as they are set by the scanner during iteration
    private List<Token> __tokens = new List<Token>();

    // Initializes the scanner and checks for file format
    public List<Token> initializeScanner(string fileName) {
        try {
            if(!fileName.EndsWith(".mp")) {
                // File format exception
                throw new Exception(Constants.ERROR_FILE_FORMAT);
            } else {
                // Grab all bytes from the file and store them in the __bytes character array
                StreamReader reader = new StreamReader(Path.GetFullPath(fileName));
                string temp = reader.ReadToEnd();
                __bytes = temp.ToCharArray();

                // Caching length to save memory
                int length = __bytes.Length;
                if(__bytes[length - 1] != '\n'){
                    throw new Exception(Constants.ERROR_NO_NEWLINE);
                }

                // String of whitespace characters
                string ws = Constants.WHITESPACE;
                // Flag for if the current byte being read is contained within a comment
                bool commentFlag = false;

                // Loop until EOF, ignoring whitespace
                while(__curByte < length){
                    // In comment mode, ignore everything
                    if(commentFlag) {
                        if(__bytes[__curByte] == '\n') {
                            __line++;
                            __column = 1;
                        }
                        // Look for end of comments
                        else if (__bytes[__curByte] == '}') {
                            commentFlag = false;
                            __column++;
                        }
                        // Handle run on comment error
                        else if (__curByte == __bytes.Length - 1) {
                            __tokens.Add(new Token("{", TOKENS.RUN_COMMENT, __column, __line));
                        } else {
                            __column++;
                        }
                        __curByte++;
                        continue;
                    }
                    // Look for comments
                    else if(__bytes[__curByte] == '{') {
                        commentFlag = true;
                        __curByte++;
                        continue;
                    }
                    // Handle whitespace
                    else if(ws.Contains("" + __bytes[__curByte])) {
                        if(__bytes[__curByte] == '\n'){
                            __line++;
                            __column = 1;
                        } else if(__bytes[__curByte] == ' ') {
                            __column++;
                        }
                        __curByte++;
                        continue;
                    }
                    // Handle tokens
                    else {
                        __tokens.Add(getNextToken());
                    }
                }
                __tokens.Add(new Token("EOF", TOKENS.EOF, __column, __line));
                reader.Close();
                reader.Dispose();
            }
        } catch(Exception ex) {
            Console.WriteLine(ex);
        }

        return __tokens;
    }

    // Dispatches the current character to the appropriate FSA
    private Token getNextToken() {
        // Current/Next byte to look at from the array of bytes
        char next = __bytes[__curByte];

        // Dispatch to appropriate FSA
        if(Constants.DIGITS.Contains("" + next)) {
            return fsaDigit();
        } else if(Constants.PUNCTUATION.ContainsKey("" + next)) {
            return fsaPunct();
        } else if(Constants.LETTERS.Contains("" + next) || next == '_') {
            return fsaLetter();
        } else if(next == '\''){
            return fsaString();
        } else {
            __curByte++;
            __column++;
            return new Token("" + next, TOKENS.ERROR, __column - 1, __line);
        }
    }

    // ===== Finite State Automatons =====
    // Letter FSA to handle identifiers and keywords and return appropriate tokens for each
    private Token fsaLetter() {
        int     column = __column;
        string  lexeme = "",
                LETTERS = Constants.LETTERS,
                DIGITS = Constants.DIGITS;
        char    next;

        goto S0;
        S0: // Start state
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
        S1: // Underscore route
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
        S2: // Letter/digit
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
                String lexeme_lower = lexeme.ToLower(); // Cached for ease of use
                if (Constants.RESERVE_WORDS.ContainsKey(lexeme_lower)) {
                    return new Token (lexeme, Constants.RESERVE_WORDS[lexeme_lower], column, __line);
                } else {
                    return new Token (lexeme, TOKENS.IDENTIFIER, column, __line);
                }
            }
    }

    // Digit FSA to handle tokens beginning with digits and returns appropriate tokens for each
    private Token fsaDigit() {
        int     column = __column;
        string  lexeme = "",
                DIGITS = Constants.DIGITS;
        char    next;
        TOKENS  token;

        goto S0;
        S0: // Start state
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)) {
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
            if(DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S1;
            } else if(next == '.'){
                lexeme += next;
                goto S2;
            } else if(next == 'e' || next == 'E') {
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
            if(DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S3;
            } else {
                // Must remove the last character (.) from lexeme
                lexeme.Remove(lexeme.Length - 1);
                __curByte -= 2;
                __column -= 2;
                return new Token(lexeme, token, column, __line);
            }
        S3: // Digits have followed a valid '.'
            token = TOKENS.FLOAT_LIT;
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S3;
            } else if(next == 'e' || next == 'E') {
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
            if(next == '+' || next == '-') {
                lexeme += next;
                goto S5;
            } else if (DIGITS.Contains("" + next)) {
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
            if(DIGITS.Contains("" + next)) {
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
            if(DIGITS.Contains("" + next)) {
                lexeme += next;
                goto S6;
            } else {
                __curByte--;
                __column--;
                return new Token(lexeme, token, column, __line);
            }
    }

    // Punctuation FSA handles punctuation and returns appropriate tokens for each
    private Token fsaPunct() { // doesn't include quotes
        int     column = __column;
        string  lexeme = "";
        char    next;

        goto S0; // Smothers warning about S0 label not used
        S0: // Start state
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(Constants.PUNCTUATION.ContainsKey("" + next)) {
                lexeme += next;
                if(next == ':') {
                    goto S1;
                } else if(next == '<') {
                    goto S2;
                } else if(next == '>') {
                    goto S3;
                } else {
                    return new Token(lexeme, Constants.PUNCTUATION[lexeme], column, __line);
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

    // String FSA handles constant strings that begin and end with single quotes
    private Token fsaString() { // Surrounded by quotes
        string  lexeme = "";
        char    next;
        int     column = __column;

        goto S0;
        S0:
            next = __bytes[__curByte];
            __column++;
            __curByte++;
            if(next == '\'') {
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
            } else if(next == '\n') {
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
            if(next == '\'') {
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
            if(next == '\'') {
                // Potentially closing apostrophe, don't include
                goto S2;
            } else if( next == '\n') {
                __column--;
                __curByte--;
                return new Token(lexeme, TOKENS.RUN_STRING, column, __line);
            } else {
                lexeme += next;
                goto S3;
            }
    }
}
