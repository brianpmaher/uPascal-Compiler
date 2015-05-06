{This is more of a scanner test than anything.
Here's a bunch of strange symbols that shouldn't matter:

~!@#$%^&&*())^%$@#`1335<>M[[]]]][;;'"""//??><|||
}


program test;

var 	aString : string;
	anInt	: integer;
	aFloat 	: float;
	{aBool	: boolean;}

begin

	writeln('This is test 6 of C level');
	writeln('This is the same as 5, without the string');
	writeln('');

	writeln('Enter in any integer');
	read(anInt);
	writeln('You entered: ', anInt);

	writeln('Enter in any float');
	read(aFloat);
	writeln('You entered: ', aFloat);

	{writeln('Enter in any string');
	read(aString);
	writeln('You entered: ', aString');}

	{writeln('Enter in any boolean value');
	read(aBool);
	writeln('You entered: ', aBool);}

	

end.
