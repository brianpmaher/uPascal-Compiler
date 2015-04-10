program Repeater;

  var Sum, I, N : Integer;

  begin
    Read(N);
    I := 1;
    repeat
      Sum := 0;
      repeat
        Sum := Sum + I;
      until Sum > N;
      I := I + 1;
    until I > 5
  end.
