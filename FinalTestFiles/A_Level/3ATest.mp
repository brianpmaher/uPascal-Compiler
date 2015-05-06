program test;

var a,b,c : float;

function testFunc(x:integer; y:float):integer;
	var a : integer;
	begin
		writeln('Please enter an integer');
		read(a);
		writeln('The value ', (a mod x) * y,' should be returned');
		testFunc := (a mod x) * y;
	end;

function otherFunc(x:integer):integer;
	var a,b,c,d,e :integer;
		thisIsAReallyLongVariableNameForNoReasonOtherThanToTestLongVariableNames: string;

	begin
		otherFunc := x * 2;
	end;

begin
	writeln('This is test 4 of A level');
	writeln('This will test function calling and return values');
	writeln('Please enter two floats');
	read(a,b);

	c := testFunc(123,b);

	writeln('The value ', c, ' was returned\n');

	writeln('The value ', testFunc(123,a),' was returned\n');

	writeln('The value ', testFunc(otherFunc(11), 5.0), ' was returned\n');

end.
