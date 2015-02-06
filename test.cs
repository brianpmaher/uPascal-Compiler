using System;

public partial class Scanner {
	private string __testResults;

	public void TestDriver() {
		this.__testResults = "";
		TestDigitFSA();
		TestLetterFSA();
		TestStringFSA();
		TestPunctFSA();
		Console.WriteLine(__testResults);
	}

	public void TestDigitFSA(){
		Token testToken;
		string validInteger = "1234567890";
		string validFixed = "1234567890.1234567890";
		string validFloat1 = "1234567890.1234567890e1234567890";
		string validFloat2 = "1234567890.1234567890E1234567890";
		string validFloat3 = "1234567890.1234567890e+1234567890";
		string validFloat4 = "1234567890.1234567890e-1234567890";
		string validFloat5 = "1234567890.1234567890E+1234567890";
		string validFloat6 = "1234567890.1234567890E-1234567890";
		string invalidFloat = "1234567890.1234567890eeeeeeee";
		string invalidInteger = "123dt4d3";

		Console.WriteLine("\nTesting fsaDigit... If all true then good to go! :D");

		// validInteger Test
		__bytes = (validInteger + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validInteger ? true : false;
		__testResults += testToken.Type == TOKENS.INTEGER_LIT ? true : false;
		//__testResults += " " + validInteger + " Valid Integer? Valid integer Token?";
		__testResults += "\n";

		// validFixed Test
		__bytes = (validFixed + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFixed ? true : false;
		__testResults += testToken.Type == TOKENS.FIXED_LIT ? true : false;
		//__testResults += " " + validFixed + " Valid Fixed? Valid fixed Token?";
		__testResults += "\n";

		// validFloat1 Test
		__bytes = (validFloat1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat1 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat1 + " Valid Float? Valid float Token?";
		__testResults += "\n";


		// validFloat2 Test
		__bytes = (validFloat2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat2 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat2 + " Valid Float? Valid float Token?";
		__testResults += "\n";

		// validFloat3 Test
		__bytes = (validFloat3 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat3 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat3 + " Valid Float? Valid float Token?";
		__testResults += "\n";

		// validFloat4 Test
		__bytes = (validFloat4 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat4 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat4 + " Valid Float? Valid float Token?";
		__testResults += "\n";

		// validFloat5 Test
		__bytes = (validFloat5 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat5 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat5 + " Valid Float? Valid float Token?";
		__testResults += "\n";

		// validFloat6 Test
		__bytes = (validFloat6 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat6 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		//__testResults += " " + validFloat6 + " Valid Float? Valid float Token?";
		__testResults += "\n";

		// invalidFloat Test
		__bytes = (invalidFloat + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == invalidFloat ? !true : !false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? !true : !false;
		//__testResults += " " + invalidFloat + " Invalid Float? Invalid float Token?";
		__testResults += "\n";

		// invalidInteger Test
		__bytes = (invalidInteger + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == invalidInteger ? !true : !false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? !true : !false;
		//__testResults += " " + invalidInteger + " Invalid Integer? Invalid integer Token?";
		__testResults += "\n";
	}

	public void TestLetterFSA(){
		Token testToken;
		string validIdentifier1 = "worksGood";
		string validIdentifier2 = "thisWorksExcellentRight";
		string validIdentifier3 = "does_this_work";
		string validIdentifier4 = "th1s_sh0u1d_w0rk_too";
		string validIdentifier5 = "x1_m";
		string validReserveWord1 = "integer";
		string validReserveWord2 = "begin";
		string validReserveWord3 = "end";


		__testResults += "\nTesting fsaLetter...\n";
		// validIdentifier1 Test
		__bytes = (validIdentifier1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validIdentifier1 ? true : false;
		__testResults += testToken.Type == TOKENS.IDENTIFIER ? true : false;
		//__testResults += " " + validIdentifier1 + " Valid identifier? Valid identifier Token?";
		__testResults += "\n";

		// validIdentifier2 Test
		__bytes = (validIdentifier2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validIdentifier2 ? true : false;
		__testResults += testToken.Type == TOKENS.IDENTIFIER ? true : false;
		//__testResults += " " + validIdentifier2 + " Valid identifier? Valid identifier Token?";
		__testResults += "\n";

		// validIdentifier3 Test
		__bytes = (validIdentifier3 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validIdentifier3 ? true : false;
		__testResults += testToken.Type == TOKENS.IDENTIFIER ? true : false;
		//__testResults += " " + validIdentifier3 + " Valid identifier? Valid identifier Token?";
		__testResults += "\n";

		// validIdentifier4 Test
		__bytes = (validIdentifier4 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validIdentifier4 ? true : false;
		__testResults += testToken.Type == TOKENS.IDENTIFIER ? true : false;
		//__testResults += " " + validIdentifier4 + " Valid identifier? Valid identifier Token?";
		__testResults += "\n";

		// validIdentifier5 Test
		__bytes = (validIdentifier5 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validIdentifier5 ? true : false;
		__testResults += testToken.Type == TOKENS.IDENTIFIER ? true : false;
		//__testResults += " " + validIdentifier5 + " Valid identifier? Valid identifier Token?";
		__testResults += "\n";

		// validReserveWord1 Test
		__bytes = (validReserveWord1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validReserveWord1 ? true : false;
		__testResults += testToken.Type == TOKENS.INTEGER ? true : false;
		__testResults += 
			Constants.RESERVE_WORDS.ContainsKey(testToken.Lexeme.ToLower()) ? true : false;
		//__testResults += " " + validReserveWord1 + 
		//	" Valid reserve word? Valid reserve word Token? In RESERVE_WORDS dictionary?";
		__testResults += "\n";

		// validReserveWord2 Test
		__bytes = (validReserveWord2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validReserveWord2 ? true : false;
		__testResults += testToken.Type == TOKENS.BEGIN ? true : false;
		__testResults += 
			Constants.RESERVE_WORDS.ContainsKey(testToken.Lexeme.ToLower()) ? true : false;
		//__testResults += " " + validReserveWord2 + 
		//	" Valid reserve word? Valid reserve word Token? In RESERVE_WORDS dictionary?";
		__testResults += "\n";

		// validReserveWord3 Test
		__bytes = (validReserveWord3 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaLetter();
		__testResults += testToken.Lexeme == validReserveWord3 ? true : false;
		__testResults += testToken.Type == TOKENS.END ? true : false;
		__testResults += 
			Constants.RESERVE_WORDS.ContainsKey(testToken.Lexeme.ToLower()) ? true : false;
		//__testResults += " " + validReserveWord3 + 
		//	" Valid reserve word? Valid reserve word Token? In RESERVE_WORDS dictionary?";
		__testResults += "\n";
	}

	public void TestStringFSA(){
		Token testToken;
		string validString1 = "'Sean''s string should look good.'";
		string validString2 = "'Stephens'' string has many apostrophes'";

		__testResults += "\nTesting fsaString...\n";
		// validString1 Test
		__bytes = (validString1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaString();
		__testResults += testToken.Lexeme == "Sean's string should look good." ? true : false;
		__testResults += testToken.Type == TOKENS.STRING_LIT ? true : false;
		//__testResults += " " + 
		// 	"Sean's string should look good." + " Valid string? Valid string token?";
		__testResults += "\n";

		// validString2 Test
		__bytes = (validString2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaString();
		__testResults += testToken.Lexeme == "Stephens' string has many apostrophes" ? true : false;
		__testResults += testToken.Type == TOKENS.STRING_LIT ? true : false;
		//__testResults += " " + 
		//	"Stephens' string has many apostrophes" + " Valid string? Valid string token? ";
		__testResults += "\n";
	}

	public void TestPunctFSA(){
		Token testToken;
		string validPunct1 = "<";
		string validPunct2 = "<=";
		string validPunct3 = ">";
		string validPunct4 = "-";
		string validPunct5 = "/";
		string validPunct6 = "*";
		string validPunct7 = ";";
		string validPunct8 = ">=";

		__testResults += "\nTesting fsaPunct...\n";
		// validPunct1 Test
		__bytes = (validPunct1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct1 ? true : false;
		__testResults += testToken.Type == TOKENS.LTHAN ? true : false;
		//__testResults += " " + validPunct1 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct2 Test
		__bytes = (validPunct2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct2 ? true : false;
		__testResults += testToken.Type == TOKENS.LEQUAL ? true : false;
		//__testResults += " " + validPunct2 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct3 Test
		__bytes = (validPunct3 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct3 ? true : false;
		__testResults += testToken.Type == TOKENS.GTHAN ? true : false;
		//__testResults += " " + validPunct3 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct4 Test
		__bytes = (validPunct4 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct4 ? true : false;
		__testResults += testToken.Type == TOKENS.MINUS ? true : false;
		//__testResults += " " + validPunct4 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct5 Test
		__bytes = (validPunct5 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct5 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_DIVIDE ? true : false;
		//__testResults += " " + validPunct5 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct6 Test
		__bytes = (validPunct6 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct6 ? true : false;
		__testResults += testToken.Type == TOKENS.TIMES ? true : false;
		//__testResults += " " + validPunct6 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct7 Test
		__bytes = (validPunct7 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct7 ? true : false;
		__testResults += testToken.Type == TOKENS.SCOLON ? true : false;
		//__testResults += " " + validPunct7 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";

		// validPunct8 Test
		__bytes = (validPunct8 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaPunct();
		__testResults += testToken.Lexeme == validPunct8 ? true : false;
		__testResults += testToken.Type == TOKENS.GEQUAL ? true : false;
		//__testResults += " " + validPunct8 + " Valid punctuation? Valid punctuation token?";
		__testResults += "\n";
	}
}