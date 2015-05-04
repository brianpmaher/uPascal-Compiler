program test;
var	ref		: float;
	pass		: boolean;

procedure thirdRef(var x: float);

	begin
		if pass then begin pass := false; thirdRef(x); end else x := 9000 + x;
	end;

procedure secondRef(var x: float);

	begin
		thirdRef(x);
	end;

procedure firstRef(var x: float);

	begin
		secondRef(x);
	end;

begin
	writeln('This is test 9 for level A');
	writeln('This will be a deep test of pass by reference');
	
	pass := 1<>2;
	ref := 1;

	firstRef(ref);

	writeln('x is ', ref);
	writeln('If x is over 9000, you have succeeded');
end.
