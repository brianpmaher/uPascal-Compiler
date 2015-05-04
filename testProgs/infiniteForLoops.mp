program infiniteLoops;
  var otherThing : integer;
begin
  for otherThing := -5 to (otherThing + 20) do
    begin
      writeln('otherThing: ', otherThing);
    end
end.
