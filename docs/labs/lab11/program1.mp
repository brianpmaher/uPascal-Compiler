program Program1;
  var N : Integer;

  procedure Fred;
    begin
      N := N + 1
    end;

  begin
    Read(N);
    Writeln(N);
    Fred;
    Write(N);
  end.
