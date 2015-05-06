program test;
var  	loop1,loop2,temp	: integer;
	
begin
	writeln('Test 4 of the B level');
	writeln('This will test more while looping\n');

	writeln('Please enter an integer');
	read(loop1);

	loop2 := loop1 - 5;

	while loop1 > loop2 do
	begin
		temp := loop2;

		while loop2 <> loop1 do
		begin
			write(loop2,', ');
			loop2 := loop2 + 1;
		end;
		writeln('');
		loop2 := temp;
		loop1 := loop1 - 2.0;
	end;

	writeln('\nShould print the numbers from ', loop2, ' to ', loop2 + 4,' then the first 3 of that list, then just ', loop2);
	
end.
