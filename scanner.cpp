using System;
using System.IO;

public class Scanner {
	public List<Tuple<Constants.TOKEN, string>> initializeScanner(string file_name) {
		var token_lexemes = new List<Tuple<Constants.TOKEN, string>>;
		using (StreamReader fp = new StreamReader(file_name))
		{
			while(fp.Peek() != null){
				token_lexemes.Add(getNextToken(fp));
			}
		}
		return token_lexemes;
	}

	private Tuple<Constants.TOKEN, string> getNextToken(StreamReader fp) {
		
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

	private Tuple<Constants.TOKEN, string> fsaDigit(StreamReader fp) {
		if(!Constants.DIGITS.Contains(fp.Peek())){
			Console.WriteLine("You done goofed!");
			System.exit(1);
		} else{
			int file_pos = fp.Position();
			bool integer_not_fixed = true; // This should be true until Fixed_Success is reached
			string lexeme = "" + fp.Read();
			goto Integer_Success;
		Integer_Success:
			while(Constants.DIGITS.Contains(fp.Peek())){
				lexeme += fp.Read();
			}
			// Broken out of loop, something new has appeared!
			if(fp.Peek() == '.'){
				file_pos = fp.Position();
				lexeme += fp.Read();
				goto Fixed_Transition;
			}
			else if("Ee".Contains(fp.Peek())){
				file_pos = fp.Position();
				lexeme += fp.Read();
				goto Float_Transition_eE;
			}
			else {
				// We are no longer parsing a number, fp is on the next item, return the lexeme and token
				return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_INTEGER_LIT, lexeme);
			}
		Fixed_Transition:
			// Entering this means we had some digits, but now we have a dot. If the next is not a digit, back this train up
			if(Constants.DIGITS.Contains(fp.Peek())){
				lexeme += fp.Read();
				goto Fixed_Success;
			}
			else{
				lexeme = lexeme.Remove(strgroupids.Length - 1);
				fp.BaseStream.Seek(file_pos, SeekOrigin.Begin);
				// It's an Integer after all
				assert(fp.Peek() == '.');
				return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_INTEGER_LIT, lexeme);
			}
		Fixed_Success:
			integer_not_fixed = false;
			// Entering this means we have read a digit, and shall keep reading digits until transition on E/e or something else occurs
			while(Constants.DIGITS.Contains(fp.Peek())){
				lexeme += fp.Read();
			}
			if("Ee".Contains(fp.Peek())){
				file_pos = fp.Position();
				lexeme += fp.Read();
				goto Float_Transition_eE;
			} else {
				return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_FIXED_LIT, lexeme);
			}

		Float_Transition_eE:
			// Entering this state means we have read an e. A sign may follow, or a digit may follow. Anything else, back this train up.
			if(fp.Peek() == '+' || fp.Peek == '-'){
				lexeme += fp.Read();
				goto Float_Transition_Sign;
			} else if (Constants.DIGITS.Contains(fp.Peek())){
				lexeme += fp.Read();
				goto Float_Success;
			} else {
				lexeme = lexeme.Remove(strgroupids.Length - 1);
				fp.BaseStream.Seek(file_pos, SeekOrigin.Begin);
				assert("eE".Contains(fp.Peek()));
				if(integer_not_fixed){
					return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_INTEGER_LIT, lexeme);
				} else {
					return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_FIXED_LIT, lexeme);
				}
			}
		Float_Transition_Sign:
			// Don't bother reading if there ain't no digit next
			if(Constants.DIGITS.Contains(fp.Peek())){
				lexeme = fp.Read();
				goto Float_Success;
			} else {
				lexeme = lexeme.Remove(strgroupids.Length - 2); // We read e|E then +|- neither of which can be included in this token
				fp.BaseStream.Seek(file_pos, SeekOrigin.Begin);
				assert("Ee".Contains(fp.Peek()));
				if(integer_not_fixed){
					return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_INTEGER_LIT, lexeme);
				} else {
					return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_FIXED_LIT, lexeme);
				}
			}
		Float_Success:
			while(Constants.DIGITS.Contains(fp.Peek())){
				lexeme += fp.Read();
			}
			return new Tuple<Constants.TOKEN, string>(Constants.TOKEN.MP_FLOAT_LIT, lexeme);
		}
	}

	private int fsaPunct() { // doesn't include quotes
		return 0; // change this
	}

	private int fsaString() { // surrounded by quotes
		return 0; // change this
	}
}
