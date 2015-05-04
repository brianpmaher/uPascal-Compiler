program test;
var num :integer;

function recursion(x:integer):integer;
	
	begin
		if x = 0 then 	recursion := 1
		else		recursion := x * recursion(x-1);
	end;


begin
	writeln('This is test 6 of level A');
	writeln('This will test simple recursion');
	writeln('Enter an integer to find the factorial of (hint: factorial of 6 is 720)');
	read(num);
	writeln('The factorial of ', num, ' is ', recursion(num));
	
end.
