using System;

// Contains all constants for our program. Put any large and/or repeated strings here.
public static class Constants {
	public const string USAGE_HELP = "Usage: mono YµP.exe <µPascal filename> <output filename>";
	public const string DIGITS = "0123456789";
	public const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

	// All token IDs (add more as we come across them)
	public enum TOKEN 
	{
		// Digit tokens
		MP_INTEGER_LIT,
		MP_FIXED_LIT,
		MP_FLOAT_LIT
	};
}
