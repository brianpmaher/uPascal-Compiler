using System;
using System.IO;

public class Scanner {
	private StreamReader __reader;

	public void initializeScanner(string fileName) {
		try {
			if(!fileName.EndsWith(".mp")) {
				throw new Exception(Constants.ERROR_FILE_FORMAT);
			} else {
				this.__reader = new StreamReader(fileName);
				getNextToken();
			}
		} catch(Exception ex) {
			Console.WriteLine(ex);
		}
	}

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

	private int fsaDigit() {
		return 0; // change this
	}

	private int fsaPunct() { // doesn't include quotes
		return 0; // change this
	}

	private int fsaString() { // surrounded by quotes
		return 0; // change this
	}
}
