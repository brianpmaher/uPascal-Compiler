program intmath;
  var a, b: integer;
      c: boolean;
  begin
    b := 7.0;
    a := 5;
    c := True and False;
    writeln('This is 5: ', a);
    write('Enter a integer: ');
    read(a);
    writeln('This is what you entered: ', a);
    a := -b + b * b + a;
    writeln(a);
  end.
