using System;
using System.IO;
using System.Collections.Generic;

public class Scanner {
    private int __curByte = 0;
    private int __column = 1;
    private int __line = 1;
    private char[] __bytes;
    private List<Tuple<string, TOKENS>> __tokens;

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
                int i = 0,
                    length = __bytes.Length;
                string ws = Constants.WHITESPACE;

                // Loop through, ignoring whitespace
                for(i = 0; i < length; i++) {
                    if(ws.Contains("" + __bytes[i])) {
                        if(__bytes[i] == '\n'){
                            __line++;
                        }
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
    private int fsaLetter() {
        return 0; // change this
    }

    private Tuple<string, TOKENS> fsaDigit() {
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
                return new Tuple<string, TOKENS>(lexeme, token);
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
                return new Tuple<string, TOKENS>(lexeme, token);
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
                return new Tuple<string, TOKENS>(lexeme, token);
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
                return new Tuple<string, TOKENS>(lexeme, token);
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
                return new Tuple<string, TOKENS>(lexeme, token);
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
                return new Tuple<string, TOKENS>(lexeme, token);
            }
    }

    private int fsaPunct() { // doesn't include quotes
        return 0; // change this
    }

    private int fsaString() { // surrounded by quotes
        return 0; // change this
    }
}
