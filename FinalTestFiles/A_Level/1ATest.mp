program test;
var a 	: integer;

procedure testProc( b,c:integer);

	begin
		writeln(b,' times ', c ,' is ', b * c);
	end;

begin
	writeln('This is test 2 of A level');
	writeln('This well test parameter use in procedures');
	writeln('Please enter an integer');
	read(a);

	testProc(a, 5);
	testProc(a, 6);
	testProc(a,a);
	testProc(a + a, a mod a);
	testProc(a, -a);
end.
