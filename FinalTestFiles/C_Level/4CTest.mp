program mathTest;
var a, b, c, d 	: integer;
	  e, f 	: float;


begin
	writeln('This is test 4 of C Level');
	writeln('Output should be as indicated');
	writeln('');

	a := 10;
	b := 5;
	c := 100;
	d := 30;
	e := 1.1111;
	f := 8;

	writeln('a is ', a);
	writeln('b is ', b);
	writeln('c is ', c);
	writeln('d is ', d);
	writeln('e is ', e);
	writeln('f is ', f);

	writeln('');
	writeln('a mod b = ', a mod b);
	writeln('b * b * b = ', b*b*b);

	writeln('b * c + d = ', b * c + d);
	writeln('b * (c + d) = ', b * (c + d));

	writeln('e * e = ', e * e);



end.

