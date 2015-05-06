program Program3;
  var N : Integer;

  function factorial(N: Integer): Integer;
    begin
      if N = 0 then
        factorial := 1
      else
        factorial := N * factorial(N-1)
      end;

  begin
    Read(N);
    If N >= 0 then
      write(factorial(N))
    else
      write(-1);
  end.
