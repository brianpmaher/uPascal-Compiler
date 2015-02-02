%option noyywrap
MP_LETTER 			[a-zA-Z]
MP_DIGIT 			[0-9]
MP_INTEGER 			MP_DIGIT(MP_DIGIT)*
MP_FIXED			MP_INTEGER.MP_INTEGER
MP_FLOAT			MP_INTEGER.MP_INTEGER[eE][+-]?MP_INTEGER
MP_IDENTIFIER		(_?MP_LETTER)*|(_?MP_LETTER*[MP_LETTER|MP_DIGIT]+)
MP_AND				[AND]
MP_BEGIN			[BEGIN]
MP_END				[END]
MP_SCOLON			[;]
MP_ASSIGN			[:=]
MP_EQUAL			[=]
MP_GTHAN			[>]
MP_LTHAN			[<]
MP_GEQUAL			[>=]
MP_LEQUAL			[<=]
MP_NEQUAL			[<>]
MP_EOF				<<EOF>>
%%