program procedureParameters;
	var someInt, someInt2 : integer;
	someFloat, someFloat2 : float;

	function printHello(x : integer): integer;
	var hi, hey : integer;
	begin
		if x = 0 then
    		write('DONE')

  		else
  			printHello := 7;
	end;
begin
	someInt := 3;
	someInt2 := printHello(someInt) + 5;
	write(someInt2);
end.
