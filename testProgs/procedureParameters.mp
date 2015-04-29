program procedureParameters;
	var someInt : integer;
	procedure printHello(x : integer);
	begin 
		writeln('hello');
		writeln(x);
	end;
begin
	someInt := 1;
	printHello(3);
end.
