/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;
using System.Collections.Generic;

/*
 *  Driver : initializes the program and either displays a usage message, runs unit tests, or begins
 *         : to run the µPascal Compiler.
 */
public class Driver {
    static public void Main(string[] args) {
        Scanner scanner = new Scanner();

        // User entered incorrect usage for our program
        if(args.Length == 0 || args.Length > 2) { // incorrect number of arguments
            Console.WriteLine(Constants.USAGE_HELP);
        } else if(args[0].Equals("-t")) { // run unit tests
            scanner.TestDriver();
        } else { // Run the compiler
            // Create token list for mp file
            List<Token> tokens = scanner.initializeScanner(args[0]);
            // Formatting scanner output
            Console.WriteLine(Constants.TOKEN_TABLE_HEADER_FORMAT,
                "TOKEN", "LINE", "COLUMN", "LEXEME"
            );
            // Display full tokens list
            foreach(Token token in tokens) {
                Console.WriteLine(
                    "{0,-12}{1,8}{2,8}\t{3}",
                    token.Type.ToString(),
                    token.Line,
                    token.Column,
                    token.Lexeme
                );
            }

            Parser parser = new Parser(tokens);
            try {
                parser.Parse();
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}
