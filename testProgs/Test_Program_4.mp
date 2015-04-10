program test4;
  var x, y : integer;
  z : float;
begin
  for x := 10 downto 0 do
    begin
      for y := 10 downto 0 do
        begin
          writeln(x);
        end;
      x := x + 1
    end
end.
