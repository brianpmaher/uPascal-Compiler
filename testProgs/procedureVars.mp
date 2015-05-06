program procedureParameters;
	var someInt, someInt2 : integer;

	procedure printHello(x : integer; var y : integer);
	begin 
		y := 0;
		writeln(y);
	end;
begin
	someInt := 1;
	someInt2 := 5;
	printHello(someInt, someInt2);
end.
