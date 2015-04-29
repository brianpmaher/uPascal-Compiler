program Program4;
  var I: Integer;
      N: Integer;

  procedure Fred(A: Integer; Var B: Integer);
    var I: Integer;

    procedure Doit;
      var X: Integer;
      begin
        X := 0;
        I := -5;
        N := -10;
        Write(X, I, N);
      end;

    begin
      I := 5;
      A := 10;
      B := 20;
      Write(I, A, B);
      Doit;
      Write(I, A, B);
    end;

  function Why: Integer;
    begin
      Why := 1
    end;

  begin
    I := 0;
    N := 1;
    Write(I, N);
    Fred(I, N);
    Write(I, N);
    Write(Why);
  end.
