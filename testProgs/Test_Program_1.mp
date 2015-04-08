

{ak;djfa;slkjfd;alskjd}

program lab1;
    var  otherThing, choice: integer;
		floatMathTest: Float;
		booleanValue: boolean;
		float_MathTest: string;
		bob, fred: boolean;

	function factorial( b: integer):integer;
	var x: integer;
	begin
		if b = 0 then
			factorial := 1
		else
			factorial := b * factorial(b-1);
	end;

	procedure listNum( start: integer);
	var finish: integer;
	begin
		finish := -50;
		while start > finish do
		begin
			write(start, ' ');
			start := start - 1;
		end;
		writeln('');
	end;

	{Testing scope, as otherthing was already declared at the top
	This should only use the internal variable}
	procedure scopeTest;
	var otherthing: float;
	begin
		otherthing := 1 * 2.0 / 4 + 2;
		writeln('From procedure otherthing is: ', otherthing);
	end;

	function retString( x,y:float; b:string):string;
		var intConverter: integer;
		procedure imbedded(b:integer);

			procedure imbeddedAgain(b:integer);

				procedure somemoreImbedding(b:integer);

				begin
					writeln('I am super imbedded and awesome. B is ', b);
				end;

			begin
				somemoreImbedding(b);
			end;
		begin
			imbeddedAgain(b);
		end;
	begin
		writeln('trying the whole imbedded thing', b);
		intConverter := x + y;
		imbedded(intConverter);

		retString := 'This string was returned as a value';
	end;

	procedure refCheck(var x: integer);

	begin
		x := 5 + x;
		writeln('Variable x should be changed to ', x);
	end;

	procedure refCheck2(z: integer; var x, y: float);

		procedure refCheck3(var chicken: float);

		begin
			writeln('In refCheck 3 value is: ', chicken);
			writeln('');
		end;

	begin
		x := x + y;
		writeln('X in refcheck2 is ', x);
		refCheck3(y);
	end;

  begin
		writeln('Welcome to my program (^_^ )');
		writeln('Please enter an integer to find the factorial of:');
		read(choice);
		if not(false) then
			writeln(factorial(choice))
		else
			writeln('-1');

		listNum(100 div 2);
		listNum(factorial(9 div 3));

		otherthing := 5 mod 2;
		writeln(-otherthing);


		bob := true;
		fred := false;


		writeln(bob,fred);

		fred := fred and bob;
		writeln(fred);
		fred := fred or bob;
		writeln(fred);

		writeln(-(1 + 2));

		otherthing := 15 * 15 div 5 + 1 - 2 * 2;
		writeln('From *main* otherthing is: ', otherthing);
		scopeTest;
		writeln('From *main* otherthing still is: ', otherthing);
		floatMathTest := (7.0 *2.1 - 0.6 / 13) * (factorial(13 - 11));
		writeln(floatMathTest);

		{scopeTest(scopeTest); This has been fixed and no longer compiles.}
		{intersting because scopeTest has no input parameters, and it's ok because scopeTest doesn't have a value return}

		writeln(retString(0.001, 1.23, 'String as input argument'));

		writeln('X starts at ', otherthing);
		refCheck(otherthing);
		writeln('X is now ', otherthing, ' in main');

		writeln('floatMathTest start: ', floatMathTest);

		refCheck2(10 div 3, floatMathTest, floatMathTest);

		writeln('floatMathTest is now ', floatMathTest, ' in main');

		otherthing := 5;
		repeat
			begin
			write(otherthing, ' ');
			otherthing := otherthing - 3;
			end;
		until (otherthing < 0);

		for otherthing := (-5) to (otherthing + 20) do

			writeln('otherthing is ', otherthing);

		booleanValue := true;

		writeln(5 + 6.5);

		writeln('Thank you for using this!');
  end.
