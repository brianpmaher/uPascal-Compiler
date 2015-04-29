{ Simple procedure that just prints "hello" and a number to the screen. }
program procedureProgram;
	procedure printHello(x : integer);
	begin
		writeln('hello');
		writeln(x);
	end;
begin
	printHello(3);
end.
