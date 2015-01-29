using System;

public class Driver {
    static public void Main(string[] args) {
        // User entered incorrect usage for our program
        if(args.Length == 0 || args.Length > 2) {
            Console.WriteLine(Constants.USAGE_HELP);
        } else { // Begin compiling
            Scanner scanner = new Scanner();
            scanner.initializeScanner(args[0]);
        }
    }
}
