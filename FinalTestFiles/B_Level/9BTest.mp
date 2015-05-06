program test;
var i, n 	: integer;

begin
	writeln('This is test 10 for B level');
	writeln('This will test the immutability of i as well as whether for loops');
	writeln('Calculate their conditions once, then run that number of times\n');

	n := 5;
	for i := 0 to n do
	begin
		{i := 5; This should be illegal}
		writeln('i is ', i , ' (i should not be changed, and this should show up 5 times)');
	end; 

	writeln('');

	for i := 0 to n do
	begin
		n := 1;
		writeln('i is ', i);
	end;

	writeln('\ni should have made it to 5');
end.
