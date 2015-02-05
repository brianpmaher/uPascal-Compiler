using System;

public partial class Scanner {
	private string __testResults;

	public void TestDriver() {
		this.__testResults = "";
		TestDigitFSA();
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

		Console.WriteLine("\nTesting digitFSA... If all true then good to go! :D");

		// validInteger Test
		__testResults += validInteger + "\nValid Integer? Valid integer Token?: ";
		__bytes = (validInteger + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validInteger ? true : false;
		__testResults += testToken.Type == TOKENS.INTEGER_LIT ? true : false;
		__testResults += "\n";

		// validFixed Test
		__testResults += validFixed + "\nValid Fixed? Valid fixed Token?: ";
		__bytes = (validFixed + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFixed ? true : false;
		__testResults += testToken.Type == TOKENS.FIXED_LIT ? true : false;
		__testResults += "\n";

		// validFloat1 Test
		__testResults += validFloat1 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat1 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat1 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";


		// validFloat2 Test
		__testResults += validFloat2 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat2 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat2 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";

		// validFloat3 Test
		__testResults += validFloat3 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat3 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat3 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";

		// validFloat4 Test
		__testResults += validFloat4 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat4 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat4 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";

		// validFloat5 Test
		__testResults += validFloat5 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat5 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat5 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";

		// validFloat6 Test
		__testResults += validFloat6 + "\nValid Float? Valid float Token?: ";
		__bytes = (validFloat6 + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == validFloat6 ? true : false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? true : false;
		__testResults += "\n";

		// invalidFloat Test
		__testResults += invalidFloat + "\nInvalid Float? Invalid float Token?: ";
		__bytes = (invalidFloat + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == invalidFloat ? !true : !false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? !true : !false;
		__testResults += "\n";

		// invalidInteger Test
		__testResults += invalidInteger + "\nInvalid Integer? Invalid integer Token?: ";
		__bytes = (invalidInteger + "\n").ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		__testResults += testToken.Lexeme == invalidInteger ? !true : !false;
		__testResults += testToken.Type == TOKENS.FLOAT_LIT ? !true : !false;
		__testResults += "\n";
	}

	public void TestLetterFSA(){
		string validIdentifier1 = "worksGood";
		string validIdentifier2 = "thisWorksExcellentRight";
		string validIdentifier3 = "does_this_work";
		string validIdentifier4 = "th1s_sh0u1d_w0rk_too";
		string validIdentifier5 = "1_";
		string validReserveWord1 = "int";
	}

	public void TestStringFSA(){

	}

	public void TestPunctFSA(){

	}
}