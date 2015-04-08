program intmath;
  var a, b: integer;
      c, d: boolean;
  begin
    b := 7.0;
    a := 5;
    c := True and False;
    d := True = False;
    writeln(d);
    writeln('This is 5: ', a);
    write('Enter a integer: ');
    read(a);
    writeln('This is what you entered: ', a);
    a := -b + b * b + a;
    writeln(a);
  end.
