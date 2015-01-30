using System;

public class Token {
    public string Lexeme { get; private set; }
    public TOKENS Type { get; private set; }
    public int Column { get; private set; }
    public int Line { get; private set; }

    public Token(string lexeme, TOKENS type, int column, int line){
        Lexeme = lexeme;
        Type = type;
        Column = column;
        Line = line;
    }
}
