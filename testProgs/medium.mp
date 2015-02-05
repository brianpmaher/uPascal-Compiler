{*  Author:   Sean Rogers
    Date:     Jan 27, 2015
    Description: Finds the average of five predetermined numbers.
    Version: 1.0 - the original
*}

program Medium;

{* Constant Declarations are first *}
const

  first = 45;
  second = 7;
  third = 68;
  fourth = 2;
  fifth = 34;

{* Then the variable/subprogram declarations/definitions *}

var
  average : real;
  sum : integer;

begin
  writeln ('This is the medium difficulty program. Good luck. ');
  writeln ('');

  sum := first + second + third + fourth + fifth;
  writeln ('Number 1 = ', first);
  writeln ('Number 2 = ', second);
  writeln ('Number 3 = ', third);
  writeln ('Number 4 = ', fourth);
  writeln ('Number 5 = ', fifth);

  writeln ('Sum = ', sum);

  average := sum / 5;
  writeln ('Average = ', average);
end.
