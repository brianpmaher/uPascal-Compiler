using System;
using System.IO;
using System.Collections.Generic;

public class Scanner {
	private int curByte = 0;
	private byte[] __bytes;
	private List<Tuple<string, TOKENS>> __tokens;

	// Initializes the scanner and checks for file format
	public void initializeScanner(string fileName) {
		try {
			if(!fileName.EndsWith(".mp")) {
				// File format exception
				throw new Exception(Constants.ERROR_FILE_FORMAT);
			} else {
				// Grab all bytes from the file
				this.__bytes = File.ReadAllBytes(fileName);

				// Caching variables to save memory
				int i = 0,
					length = __bytes.Length;
				string ws = Constants.WHITESPACE;

				// Loop through, ignoring whitespace
				for(i=0; i<length; i++) {
					if(ws.Contains(""+__bytes[i])) {
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
