{
{
{
{
{
{These files should just highlight errors}
}
}
}
}

program test;
var a,b,c	:float;
d, e		:                         boolean;

begin
	a := 1.0e2;
	b := 1.0E-3;
	c := (a * ((a * a) * b) / a - b  * ((((((b)))))));

	d := true;
	e := false;
	e := d or e;
	
	write('What's wrong?');
	
end.
