program Program3;

  var Sum: Integer;
        I: Integer;
        N: Integer;

  begin
    Read(N);
    I   := 1;
    Sum := 0;
    while (I <= N) or (I > 100) do begin
      Sum := Sum + I;
      I := I + 1;
      Write(I, Sum)
    end
  end.
