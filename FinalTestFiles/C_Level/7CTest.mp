program test;

var 	aString 	: strIng;
	int1, int2	: InteGer;
	aBool		: booleaN;
	f1, f2		: FLOat;

Begin
	writeln('This is test 7 of level C');
	writeln('This will further test reading and math');
	writeln('');

	aString := 'Please enter 2 integers';
	writeln('',aString,'','','','');
	read(int1, int2);

	writeln(int1, ' * ', int2, ' = ', int1*int2);
	writeln('');

	aString := 'Please enter 2 floats';
	writeln('','','','','','',aString);
	read(f1,f2);

	writeln(f1, ' * ', f2, ' = ', f1*f2);
	writeln('');

	writeln('Now to double your first flaot and integer');
	
	f1 := f1 * f1;
	int1 := int1 * int1;
	
	writeln(f1, ' and ', int1);
	writeln('');

	write('Now to increase your second integer by your second float and vice versa');
	writeln(' (should come out the same value)');

	int1 := int2 * f2;
	f2 := int2 * f2;
	int2 := int1;

	write(f2);
	write(' and ', int2);
	writeln('');
	

END.
