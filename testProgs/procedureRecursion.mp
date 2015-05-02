program procedureParameters;
	var someInt, someInt2 : integer;
	someFloat, someFloat2 : float;

	procedure printHello(x : integer);
	var hi, hey : integer;
	begin 
		hi := 4;
		if x > 3 then
    		write('DONE')
  		else
  			printHello(x + 1);
  		write(hi);
	end;

	procedure print;
	begin
		write('Hi');
	end;
begin
	someInt := 0;
	someInt2 := 2;
	printHello(someInt);
	print;
end.
