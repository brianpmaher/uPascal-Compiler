program Program2;
  var I: Integer;
      N: Integer;

  procedure Fred(A: Integer; Var B: Integer);
    var I: Integer;
    begin
      I := 5;
      A := 10;
      B := 20;
      Write(I, A, B);
    end;

  begin
    I := 0;
    N := 1;
    Write(I, N);
    Fred(I, N);
    Write(I, N)
  end.
