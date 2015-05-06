program test;
var a 	: integer;

	procedure testProc;
	var a	: integer;

	begin
		a := 2;
		writeln('If procedures are working, this will print');
	end;

begin
	writeln('This is test 1 of the A level');
	writeln('This will test scope and procedure calling');

	a := 1;

	testProc;
	writeln('\na is ', a);
	writeln('If a is 1, scope is kept. If a is 2, variable scope is implemented incorrectly');
end.
