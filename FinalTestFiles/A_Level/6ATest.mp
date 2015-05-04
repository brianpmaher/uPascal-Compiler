program test;
var num :integer;

function recursion(x:integer):integer;
	
	begin
		if x = 0 then 	recursion := 1
		else		recursion := x * recursion(x-1);
	end;

procedure notRecursion;

	begin end;


begin
	writeln('This is test 7 of level A');
	writeln('This should not compile');

	writeln('Enter an integer to find the factorial of (hint: factorial of 6 is 720)');
	read(num);

	writeln('The factorial of ', num, ' is ', recursion(notRecursion));
	
end.
