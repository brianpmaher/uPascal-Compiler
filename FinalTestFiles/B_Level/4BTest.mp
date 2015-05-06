program test;

begin

	while (1 <> 3) and (4 = 4)
	begin:
		writeln('This should not compile');
	end;

end.
