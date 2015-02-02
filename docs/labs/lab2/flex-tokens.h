#ifndef TOKENS_H
#define TOKENS_H

/* Definition for uPascal token types                                       */
/*--------------------------------------------------------------------------*/
typedef enum tagTokenType {
  MP_FTYPE = -1,

  /* RESERVED WORDS                                                         */
  /*------------------------------------------------------------------------*/
  MP_AND, MP_BEGIN,     MP_DIV,      MP_DO,    MP_DOWNTO, MP_ELSE,
  MP_END, MP_FOR,       MP_FUNCTION, MP_IF,    MP_MOD,    MP_NOT,
  MP_OR,  MP_PROCEDURE, MP_PROGRAM,  MP_READ,  MP_REPEAT, MP_THEN,
  MP_TO,  MP_UNTIL,     MP_VAR,      MP_WHILE, MP_WRITE,

  /* IDENTIFIERS AND LITERALS                                               */
  /*------------------------------------------------------------------------*/
  MP_IDENTIFIER, MP_INTLIT, MP_FIXLIT, MP_FLTLIT, MP_STRLIT,

  /* SYMBOLS                                                                */
  /*------------------------------------------------------------------------*/
  MP_PERIOD, MP_COMMA, MP_SCOLON, MP_LPAREN, MP_RPAREN,
  MP_EQUAL,  MP_LTHAN, MP_GTHAN,  MP_LEQUAL, MP_GEQUAL, MP_NEQUAL,
  MP_PLUS,   MP_MINUS, MP_TIMES,
  MP_ASSIGN,

  /* END-OF-FILE                                                            */
  /*------------------------------------------------------------------------*/
  MP_EOFILE,

  /* ERRORS                                                                 */
  /*------------------------------------------------------------------------*/
  MP_RUNCOM, MP_RUNSTR, MP_ERROR,

  MP_LTYPE
} TokenType;

#endif /* #ifndef TOKENS_H */
