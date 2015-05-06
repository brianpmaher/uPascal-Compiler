program test;

begin

	writeln('This is test 1 of B level');
	writeln('This test will simply test your boolean logic');
	writeln('');

	if true 			then	writeln('This line should be 1st');

	if(false)			then 	writeln('This should not print')
	else 					writeln('This line should be 2nd');

	if true and true		then	writeln('This line should be 3rd');

	if true or false		then 	writeln('This line should be 4th');

	if false and true		then 	writeln('This should not print')
	else					writeln('This line should be 5th');

	if false or false		then 	writeln('This should not print')
	else					writeln('This line should be 6th');

	if (true or false) and true 	then	writeln('This line should be 7th')
	else					writeln('This should not print');

	if (false and true) or true	then 	writeln('This line should be 8th')
	else					writeln('This should not print');

	if (false)			then 	writeln('This should not print')
	else if true			then 	writeln('This line should be 9th');

	if false			then 	writeln('This should not print')
	else if false			then 	writeln('This should not print')
	else					writeln('This line should be 10th');

	if true				then 	writeln('This line should be last')
	else if	true			then 	writeln('This should not print')
	else					writeln('This should not print');

end.
