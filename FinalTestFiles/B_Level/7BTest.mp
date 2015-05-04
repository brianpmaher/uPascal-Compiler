program test;
var	a,b	:integer;

begin
	writeLN('This is test 8 for B level');
	writeln('This will test repeat until loops');

	repeat
		writeln('\nThis line should print once\n')
	until 1<>2 ;

	b := 5;

	repeat
	begin
		writeln('Please enter an integer');
		read(a);
	end;
	until b = 5;



	repeat
	begin
		write(a,', ');
		a := a + 1;
	end;
	until (a mod b) = 0;

	writeln('\nShould have stopped one below the first multiple of 5 above your number');
	
	
end.
