program simpleRecursion;

  procedure Inc(N: Integer)
    begin
      if N < 10 then
        Inc(N+1);
      writeln(N);
    end;

  begin
    Inc(0);
  end.
