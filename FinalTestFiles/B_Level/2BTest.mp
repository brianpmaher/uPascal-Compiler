program test;
var 	a, startA		:inteGer;
	b1	 		:boolean;

begin

	writeln('This is test 3 of B level');
	writeln('Please enter a positive integer');
	read(a);

	startA := a;

	while a > 0 do
	begin
		write(a, ', ');
		a := a - 1;
	end;

	writeln('\nShould print from ', startA ,' to 0 exclusive');

	writeln('Please enter a negative integer');
	read(a);

	startA := a;

	while a < 0 do
	begin
		write(a, ', ');
		a := a + 1;
	end;

	writeln('\nShould print from ', startA , ' to 0 exclusive');

	while a = 1 	do writeln('This should not print');
	while a <> 0	do writeln('This should not print');

end.
