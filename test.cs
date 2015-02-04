using System;

public partial class Scanner {
	private string __testResults;

	public bool TestDriver() {
		this.__testResults = "";
		TestDigitFSA();
		Console.WriteLine(__testResults);
		return false;
	}

	public bool TestDigitFSA(){
		Token testToken;
		string validInteger = "1234567890";
		string validFixed = "1234567890.1234567890";
		string validFloat1 = "1234567890.1234567890e1234567890";
		string validFloat2 = "1234567890.1234567890E1234567890";
		string validFloat3 = "1234567890.1234567890e+1234567890";
		string validFloat4 = "1234567890.1234567890e-1234567890";
		string validFloat5 = "1234567890.1234567890E+1234567890";
		string validFloat6 = "1234567890.1234567890E-1234567890";

		Console.WriteLine("Testing digitFSA... ");
		Console.WriteLine(validInteger);

		// validInteger Test
		__bytes = validInteger.ToCharArray();
		__curByte = 0;
		testToken = fsaDigit();
		Console.WriteLine("flag");
		__testResults += testToken.Lexeme == validInteger ? true : false;
		Console.WriteLine("flag");
		__testResults += testToken.Type == TOKENS.INTEGER_LIT ? true : false;
		Console.WriteLine("flag");

		// validFixed Test
		//__bytes = validFixed.ToCharArray();
		//testToken = fsaDigit();
		//__testResults += testToken.Lexeme == validFixed ? true : false;
		//__testResults += testToken.Type == TOKENS.FIXED_LIT ? true : false;

		// validFloat1 Test


		return false;
	}
}