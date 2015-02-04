using System;
using System.Collections.Generic;

public class Driver {
    static public void Main(string[] args) {
        // User entered incorrect usage for our program
        if(args.Length == 0 || args.Length > 2) {
            Console.WriteLine(Constants.USAGE_HELP);
            Environment.Exit(0);
        }
        Scanner scanner = new Scanner();
        List<Token> tokens = scanner.initializeScanner(args[0]);
        Console.WriteLine(Constants.TOKEN_TABLE_HEADER);
        foreach(Token token in tokens){
            Console.WriteLine(
                token.Type.ToString() + "\t" +
                token.Lexeme + "\t" +
                token.Line + "\t" +
                token.Column);
        }
    }
}
