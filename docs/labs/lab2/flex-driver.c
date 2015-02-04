#include <stdio.h>
#include <stdlib.h>

#include "flex-tokens.h"
#include "lex.yy.c"

/* Definitions for echoing scanned tokens                                */
/*-----------------------------------------------------------------------*/
static char *TokenNames[] = {
  "AND" , "BEGIN" , "DIV"  , "DO"       , "DOWNTO" ,
  "ELSE", "END"   , "FOR"  , "FUNCTION ", "IF"     ,
  "MOD" , "NOT"   , "OR"   , "PROCEDURE", "PROGRAM",
  "READ", "REPEAT", "THEN" , "TO"       , "UNTIL  ",
  "VAR" , "WHILE" , "WRITE",

  "IDENTIFIER", "INTLIT", "FIXLIT", "FLTLIT", "STRLIT",

  "PERIOD", "COMMA", "SCOLON", "LPAREN", "RPAREN",
  "EQUAL" , "LTHAN", "GTHAN" , "LEQUAL", "GEQUAL", "NEQUAL",
  "PLUS"  , "MINUS", "TIMES" ,
  "ASSIGN",

  "EOFILE",

  "RUNCOM", "RUNSTR", "ERROR "
};

#define TOKENNAME(Token) (TokenNames[(Token)-(int)MP_AND])

/* LEX/FLEX interface                                                    */
/*-----------------------------------------------------------------------*/
extern FILE *yyin;
extern char *yytext;

/*=======================================================================*/
int main(int argc, char *argv[])
{
  TokenType token;

  /* Command line argument processing                                    */
  /*---------------------------------------------------------------------*/
  if(argc != 2) {
    printf("USAGE:  mp <filename>\n\n");
    exit(1);
  }

  else {
    yyin = fopen(argv[1], "r");

    if(!yyin) {
      perror(argv[1]);
      exit(1);
    }
  }

  /* Scanner testing loop (echo scanned tokens and lexemes)             */
  /*--------------------------------------------------------------------*/
  printf("Scanning %s for tokens...\n", argv[1]);

  do {
    token = yylex();

    printf("  %-10s scanned" , TOKENNAME(token));
    printf(" from [%s]\n" , yytext            );

    if((token == MP_RUNCOM) || (token == MP_RUNSTR) || (token == MP_ERROR)) {
      printf("Scanning terminated by error.\n");
      exit(1);
    }
  } while(token != MP_EOFILE);

  printf("Scanning terminated by end-of-file.\n");
}


