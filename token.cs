/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;

/*
 *  Token : Token object to contain detailed information about each token found during scanning in
 *        : order to output useful information in the event a scanner error is found.
 */
public class Token {
    public string   Lexeme  { get; private set; }
    public TOKENS   Type    { get; private set; }
    public int      Column  { get; private set; }
    public int      Line    { get; private set; }

    public Token(string lexeme, TOKENS type, int column, int line) {
        Lexeme =    lexeme;
        Type =      type;
        Column =    column;
        Line =      line;
    }
}
