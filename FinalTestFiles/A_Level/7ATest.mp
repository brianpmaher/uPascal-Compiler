program test;
var num :integer;

function recursion(x:integer):integer;
	var num : float;
	begin
		if x = 0 then 	recursion := 1
		else		recursion := x * recursion(x-1);
	end;

procedure notRecursion;
	var num : String;
	begin end;

procedure stuff;
	var stuff:float;
	begin end;

function stuff:integer;
	var stuff:float;
	begin end;

begin
	writeln('This is test 8 of level A');
	writeln('This should not compile');

	writeln('Enter an integer to find the factorial of (hint: factorial of 6 is 720)');
	read(num);

	writeln('The factorial of ', num, ' is ', recursion(num));
	
end.
