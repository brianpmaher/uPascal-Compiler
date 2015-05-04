program test;

{Lots of variables that won't be used
but will hopefully make big and interesting symbol tables}

var 	a,b,c,d,e,f,g,h,i,j,k 	: float;
	asdf,jkl,lkj,fda  	: string;
	chicken,notchicken	: boolean;
	lolireallyliketheselongnamesfornootherreasonthanthattheyareunreadableandunusableandtheymessuppeoplesniceprinting,anInt	:integer;

function firstFunc(var x: integer):string;
	
	procedure secondProc(x:integer);
		var anotherlongnamesimplybecauseIlikethem :integer;
		
		begin
			a := 15.0 * x;
			x := 10;
			writeln('This should put 45 into variable a');
		end;


	begin
		x := 3;
		secondProc(x);
		firstFunc := 'x in the function is 3';
	end;	

begin
	writeln('This is test 5 of A level');
	writeln('This will test pass by value and pass by reference');
	
	a := 1.0;
	anInt := 1;

	fda := firstFunc(anInt);

	writeln('The reference variable is ', anInt);
	writeln('If the reference variable is not 3, pass by reference is done incorrectly');
	writeln('a is ', a, '. a should be 45');
	
end.
