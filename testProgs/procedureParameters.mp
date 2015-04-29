program procedureParameters;
	var someInt, someInt2 : integer;
	someFloat, someFloat2 : float;
	procedure printHello(x, y : integer);
	begin 
		writeln(y);
		writeln(x);
	end;
begin
	someInt := 1;
	someInt2 := 2;
	printHello(someInt, someInt2);
end.
