program Program4;

  var Sum, I, N : Integer;

  begin
    Read(N);
    I   := 0;
    Sum := 0;
    repeat
      Sum := Sum + I;
      I   := I + 1;
      Write(I, Sum);
    until (I > N) and (I <= 100)
  end.
