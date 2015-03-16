using System;
using System.IO;
using System.Collections.Generic;

public class Parser {
    private List<Token> __tokens;
    private Token __lookahead;
    private List<Token>.Enumerator __e;

    public Parser(List<Token> tokens){
        this.__tokens = tokens;
        this.__e = __tokens.GetEnumerator();
        this.__e.MoveNext();
        this.__lookahead = __e.Current;
    }

    public void Parse(){
        systemGoal();
    }

    // Error
    private void error(List<TOKENS> expected){
        string exception; // Message to be displayed

        // First check if a scan error token was found and throw an exception accordingly
        if(__lookahead.Type == TOKENS.ERROR) {
            exception = "SCAN ERROR: \"" +__lookahead.Lexeme +
                "\" at column " + __lookahead.Column +
                " and line " + __lookahead.Line;
                throw new Exception(exception);
        } else {
            exception = "PARSE ERROR: expected (";
        }

        int count = expected.Count; // Size of expected list

        // There will always be at least one expected to pass in, doing this in order to make
        // displaying the expected tokens simpler (so we can separate by commas)
        exception += expected[0];

        // Iterate over every expected token after the first (this will not enter if there is only
        // one expected token)
        for(int i = 1; i < count; i++) {
            exception += ", " + expected[i].ToString();
        }

        // Build our exception message with details about our current __lookahead to see what could
        // have gone wrong with as much information as possible.
        exception += "), but saw \"" + __lookahead.Lexeme +
            "\" of type " + __lookahead.Type.ToString() +
            " at column " + __lookahead.Column +
            " and line " + __lookahead.Line;



        throw new Exception(exception);
    }

    // Match
    private void match(TOKENS token){
        // Simple output
        Console.WriteLine("Matching " + __lookahead.Lexeme);
        if(token == TOKENS.EOF){
            return;
        } else if(__e.Current.Type == token){
            if(__e.MoveNext()){
                __lookahead = __e.Current;
            } else {
                throw new Exception(
                    "Expected token, got null!"
                );
            }
        } else {
            error(new List<TOKENS>{token});
        }
    }

    // Non-terminals
    private void systemGoal(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                // Rule 1
                program();
                match(TOKENS.EOF);
                Console.WriteLine("The input program parses!");
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void program(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                // Rule 2
                programHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.PERIOD);
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void programHeading(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                // Rule 3
                match(TOKENS.PROGRAM);
                programIdentifier();
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void block(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
            case TOKENS.VAR:
                // Rule 4
                variableDeclarationPart();
                procedureAndFunctionDeclarationPart();
                statementPart();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN, TOKENS.FUNCTION, TOKENS.PROCEDURE,
                    TOKENS.VAR});
                break;
        }
    }

    private void variableDeclarationPart(){
        switch(__lookahead.Type){
            case TOKENS.VAR:
                // Rule 5
                match(TOKENS.VAR);
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
                // Rule 6
                break;
            default:
                error(new List<TOKENS>{TOKENS.VAR, TOKENS.BEGIN, TOKENS.FUNCTION,
                    TOKENS.PROCEDURE});
                break;
        }
    }

    private void variableDeclarationTail(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 7
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.PROCEDURE:
                // Rule 8
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER, TOKENS.PROCEDURE});
                break;
        }
    }

    private void variableDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 9
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void type(){
        switch(__lookahead.Type){
            case TOKENS.INTEGER:
                // Rule 10
                match(TOKENS.INTEGER);
                break;
            case TOKENS.FLOAT:
                // Rule 11
                match(TOKENS.FLOAT);
                break;
            case TOKENS.STRING:
                // Rule 12
                match(TOKENS.STRING);
                break;
            case TOKENS.BOOLEAN:
                // Rule 13
                match(TOKENS.BOOLEAN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.INTEGER, TOKENS.FLOAT, TOKENS.STRING,
                    TOKENS.BOOLEAN});
                break;
        }
    }

    private void procedureAndFunctionDeclarationPart(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                // Rule 14
                procedureDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.FUNCTION:
                // Rule 15
                functionDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.BEGIN:
                // Rule 16
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE, TOKENS.FUNCTION, TOKENS.BEGIN});
                break;
        }
    }

    private void procedureDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                // Rule 17
                procedureHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE});
                break;
        }
    }

    private void functionDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.FUNCTION:
                // Rule 18
                functionHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FUNCTION});
                break;
        }
    }

    private void procedureHeading(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                // Rule 19
                match(TOKENS.PROCEDURE);
                procedureIdentifier();
                optionalFormalParameterList();
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE});
                break;
        }
    }

    private void functionHeading(){
        switch(__lookahead.Type){
            case TOKENS.FUNCTION:
                // Rule 20
                match(TOKENS.FUNCTION);
                functionIdentifier();
                optionalFormalParameterList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FUNCTION});
                break;
        }
    }

    private void optionalFormalParameterList(){
        switch(__lookahead.Type){
            case TOKENS.LPAREN:
                // Rule 21
                match(TOKENS.LPAREN);
                formalParameterSection();
                formalParameterSectionTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.COLON:
            case TOKENS.SCOLON:
                // Rule 22
                break;
            default:
                error(new List<TOKENS>{TOKENS.LPAREN, TOKENS.COLON, TOKENS.SCOLON});
                break;
        }
    }

    private void formalParameterSectionTail(){
        switch(__lookahead.Type){
            case TOKENS.SCOLON:
                // Rule 23
                match(TOKENS.SCOLON);
                formalParameterSection();
                formalParameterSectionTail();
                break;
            case TOKENS.RPAREN:
                // Rule 24
                break;
            default:
                error(new List<TOKENS>{TOKENS.SCOLON, TOKENS.RPAREN});
                break;
        }
    }

    private void formalParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 25
                valueParameterSection();
                break;
            case TOKENS.VAR:
                // Rule 26
                variableParameterSection();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER, TOKENS.VAR});
                break;
        }
    }

    private void valueParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 27
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void variableParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.VAR:
                // Rule 28
                match(TOKENS.VAR);
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error(new List<TOKENS>{TOKENS.VAR});
                break;
        }
    }

    private void statementPart(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
                // Rule 29
                compoundStatement();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN});
                break;
        }
    }

    private void compoundStatement(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
                // Rule 30
                match(TOKENS.BEGIN);
                statementSequence();
                match(TOKENS.END);
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN});
                break;
        }
    }

    private void statementSequence(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
            case TOKENS.END:
            case TOKENS.FOR:
            case TOKENS.IF:
            case TOKENS.READ:
            case TOKENS.REPEAT:
            case TOKENS.WHILE:
            case TOKENS.WRITE:
            case TOKENS.WRITELN:
            case TOKENS.IDENTIFIER:
            case TOKENS.SCOLON:
                // Rule 31
                statement();
                statementTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN, TOKENS.END, TOKENS.FOR, TOKENS.IF, TOKENS.READ,
                    TOKENS.REPEAT, TOKENS.WHILE, TOKENS.WRITE, TOKENS.WRITELN, TOKENS.IDENTIFIER,
                    TOKENS.SCOLON});
                break;
        }
    }

    private void statementTail(){
        switch(__lookahead.Type) {
            case TOKENS.END:
                // Rule 33
                break;
            case TOKENS.SCOLON:
                // Rule 32
                match(TOKENS.SCOLON);
                statement();
                statementTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void statement(){
        switch(__lookahead.Type) {
            case TOKENS.BEGIN:
                // Rule 35
                compoundStatement();
                break;
            case TOKENS.FOR:
                // Rule 42
                forStatement();
                break;
            case TOKENS.IF:
                // Rule 39
                ifStatement();
                break;
            case TOKENS.READ:
                // Rule 36
                readStatement();
                break;
            case TOKENS.REPEAT:
                // Rule 41
                repeatStatement();
                break;
            case TOKENS.WHILE:
                // Rule 40
                whileStatement();
                break;
            case TOKENS.WRITE:
            case TOKENS.WRITELN:
                // Rule 37
                writeStatement();
                break;
            case TOKENS.IDENTIFIER:
                // Rule 38
                assignmentStatement();
                // Rule 43
                // OR procedureStatement();
                break;
            case TOKENS.END:
            case TOKENS.SCOLON:
                // Rule 34
                emptyStatement();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN, TOKENS.FOR, TOKENS.IF, TOKENS.READ,
                    TOKENS.REPEAT, TOKENS.WHILE, TOKENS.WRITE, TOKENS.WRITELN, TOKENS.IDENTIFIER,
                    TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void emptyStatement(){
        switch(__lookahead.Type) {
            case TOKENS.END:
            case TOKENS.SCOLON:
                // Rule 44
                break;
            default:
                error(new List<TOKENS>{TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void readStatement(){
        switch(__lookahead.Type) {
            case TOKENS.READ:
                // Rule 45
                match(TOKENS.READ);
                match(TOKENS.LPAREN);
                readParameter();
                readParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.READ});
                break;
        }
    }

    private void readParameterTail(){
        switch(__lookahead.Type) {
            case TOKENS.COMMA:
                // Rule 46
                match(TOKENS.COMMA);
                readParameter();
                readParameterTail();
                break;
            case TOKENS.RPAREN:
                // Rule 47
                break;
            default:
                error(new List<TOKENS>{TOKENS.COMMA, TOKENS.RPAREN});
                break;
        }
    }

    private void readParameter(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                // Rule 48
                variableIdentifier();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void writeStatement(){
        switch(__lookahead.Type) {
            case TOKENS.WRITE:
                // Rule 49
                match(TOKENS.WRITE);
                match(TOKENS.LPAREN);
                writeParameter();
                writeParameterTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.WRITELN:
                // Rule 50
                match(TOKENS.WRITELN);
                match(TOKENS.LPAREN);
                writeParameter();
                writeParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.WRITE, TOKENS.WRITELN});
                break;
        }
    }

    private void writeParameterTail(){
        switch(__lookahead.Type) {
            case TOKENS.COMMA:
                // Rule 51
                match(TOKENS.COMMA);
                writeParameter();
                writeParameterTail();
                break;
            case TOKENS.RPAREN:
                // Rule 52
                break;
            default:
                error(new List<TOKENS>{TOKENS.COMMA, TOKENS.RPAREN});
                break;
        }
    }

    private void writeParameter(){
        switch(__lookahead.Type) {
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 53
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void assignmentStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                // Rule 54
                variableIdentifier();
                // Rule 55
                // OR functionIdentifier();
                match(TOKENS.ASSIGN);
                expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void ifStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IF:
                // Rule 56
                match(TOKENS.IF);
                booleanExpression();
                match(TOKENS.THEN);
                statement();
                optionalElsePart();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IF});
                break;
        }
    }

    private void optionalElsePart(){
        switch(__lookahead.Type) {
            case TOKENS.ELSE:
                // Rule 57
                match(TOKENS.ELSE);
                statement();
                break;
            case TOKENS.END:
            case TOKENS.SCOLON:
                // Rule 58
                break;
            default:
                error(new List<TOKENS>{TOKENS.ELSE, TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void repeatStatement(){
        switch(__lookahead.Type) {
            case TOKENS.REPEAT:
                // Rule 59
                match(TOKENS.REPEAT);
                statementSequence();
                match(TOKENS.UNTIL);
                booleanExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.REPEAT});
                break;
        }
    }

    private void whileStatement(){
        switch(__lookahead.Type) {
            case TOKENS.WHILE:
                // Rule 60
                match(TOKENS.WHILE);
                booleanExpression();
                match(TOKENS.DO);
                statement();
                break;
            default:
                error(new List<TOKENS>{TOKENS.WHILE});
                break;
        }
    }

    private void forStatement(){
        switch(__lookahead.Type) {
            case TOKENS.FOR:
                // Rule 61
                match(TOKENS.FOR);
                controlVariable();
                match(TOKENS.ASSIGN);
                initialValue();
                stepValue();
                finalValue();
                match(TOKENS.DO);
                statement();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FOR});
                break;
        }
    }

    private void controlVariable(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                // Rule 62
                variableIdentifier();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void initialValue(){
        switch(__lookahead.Type) {
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 63
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void stepValue(){
        switch(__lookahead.Type) {
            case TOKENS.DOWNTO:
                // Rule 65
                match(TOKENS.DOWNTO);
                break;
            case TOKENS.TO:
                // Rule 64
                match(TOKENS.TO);
                break;
            default:
                error(new List<TOKENS>{TOKENS.DOWNTO, TOKENS.TO});
                break;
        }
    }

    private void finalValue(){
        switch(__lookahead.Type) {
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 66
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void procedureStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                // Rule 67
                procedureIdentifier();
                optionalActualParameterList();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void optionalActualParameterList(){
        switch(__lookahead.Type) {
            case TOKENS.AND:
            case TOKENS.DIV:
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.ELSE:
            case TOKENS.END:
            case TOKENS.MOD:
            case TOKENS.OR:
            case TOKENS.THEN:
            case TOKENS.TO:
            case TOKENS.COMMA:
            case TOKENS.EQUAL:
            case TOKENS.FLOAT_DIVIDE:
            case TOKENS.GEQUAL:
            case TOKENS.GTHAN:
            case TOKENS.LEQUAL:
            case TOKENS.LTHAN:
            case TOKENS.MINUS:
            case TOKENS.NEQUAL:
            case TOKENS.PLUS:
            case TOKENS.RPAREN:
            case TOKENS.SCOLON:
            case TOKENS.TIMES:
                // Rule 69
                break;
            case TOKENS.LPAREN:
                // Rule 68
                match(TOKENS.LPAREN);
                actualParameter();
                actualParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.AND, TOKENS.DIV, TOKENS.DO, TOKENS.DOWNTO,
                    TOKENS.ELSE, TOKENS.END, TOKENS.MOD, TOKENS.OR, TOKENS.THEN, TOKENS.TO,
                    TOKENS.COMMA, TOKENS.EQUAL, TOKENS.FLOAT_DIVIDE, TOKENS.GEQUAL, TOKENS.GTHAN,
                    TOKENS.LEQUAL, TOKENS.LTHAN, TOKENS.MINUS, TOKENS.NEQUAL, TOKENS.PLUS,
                    TOKENS.RPAREN, TOKENS.SCOLON, TOKENS.TIMES});
                break;
        }
    }

    private void actualParameterTail(){
        switch(__lookahead.Type){
            case TOKENS.COMMA:
                // Rule 70
                match(TOKENS.COMMA);
                actualParameter();
                actualParameterTail();
                break;
            case TOKENS.RPAREN:
                // Rule 71
                break;
            default:
                error(new List<TOKENS>{TOKENS.COMMA, TOKENS.RPAREN});
                break;
        }
    }

    private void actualParameter(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 72
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void expression(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 73
                simpleExpression();
                optionalRelationalPart();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void optionalRelationalPart(){
        switch(__lookahead.Type){
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.END:
            case TOKENS.THEN:
            case TOKENS.TO:
            case TOKENS.COMMA:
            case TOKENS.RPAREN:
            case TOKENS.SCOLON:
                // Rule 75
                break;
            case TOKENS.EQUAL:
            case TOKENS.GEQUAL:
            case TOKENS.GTHAN:
            case TOKENS.LEQUAL:
            case TOKENS.LTHAN:
            case TOKENS.NEQUAL:
                // Rule 74
                relationalOperator();
                simpleExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.DO, TOKENS.DOWNTO, TOKENS.END, TOKENS.THEN,
                    TOKENS.TO, TOKENS.COMMA, TOKENS.RPAREN, TOKENS.SCOLON, TOKENS.EQUAL,
                    TOKENS.GEQUAL, TOKENS.GTHAN, TOKENS.LEQUAL, TOKENS.LTHAN, TOKENS.NEQUAL});
                break;
        }
    }

    private void relationalOperator(){
        switch(__lookahead.Type){
            case TOKENS.EQUAL:
                // Rule 76
                match(TOKENS.EQUAL);
                break;
            case TOKENS.LTHAN:
                // Rule 77
                match(TOKENS.LTHAN);
                break;
            case TOKENS.GTHAN:
                // Rule 78
                match(TOKENS.GTHAN);
                break;
            case TOKENS.LEQUAL:
                // Rule 79
                match(TOKENS.LEQUAL);
                break;
            case TOKENS.GEQUAL:
                // Rule 80
                match(TOKENS.GEQUAL);
                break;
            case TOKENS.NEQUAL:
                // Rule 81
                match(TOKENS.NEQUAL);
                break;
            default:
                error(new List<TOKENS>{TOKENS.EQUAL, TOKENS.LTHAN, TOKENS.GTHAN, TOKENS.LEQUAL,
                    TOKENS.GEQUAL, TOKENS.NEQUAL});
                break;
        }
    }

    private void simpleExpression(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 82
                optionalSign();
                term();
                termTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void termTail(){
        switch(__lookahead.Type){
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.END:
            case TOKENS.THEN:
            case TOKENS.TO:
            case TOKENS.COMMA:
            case TOKENS.EQUAL:
            case TOKENS.GEQUAL:
            case TOKENS.GTHAN:
            case TOKENS.LEQUAL:
            case TOKENS.LTHAN:
            case TOKENS.NEQUAL:
            case TOKENS.RPAREN:
            case TOKENS.SCOLON:
                // Rule 84
                break;
            case TOKENS.OR:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                // Rule 83
                addingOperator();
                term();
                termTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.DO, TOKENS.DOWNTO, TOKENS.END, TOKENS.THEN, TOKENS.TO,
                    TOKENS.COMMA, TOKENS.EQUAL, TOKENS.GEQUAL, TOKENS.GTHAN, TOKENS.LEQUAL,
                    TOKENS.LTHAN, TOKENS.NEQUAL, TOKENS.RPAREN, TOKENS.SCOLON, TOKENS.OR,
                    TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void optionalSign(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                // Rule 87
                break;
            case TOKENS.MINUS:
                // Rule 86
                match(TOKENS.MINUS);
                break;
            case TOKENS.PLUS:
                // Rule 85
                match(TOKENS.PLUS);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private void addingOperator(){
        switch(__lookahead.Type){
            case TOKENS.OR:
                // Rule 90
                match(TOKENS.OR);
                break;
            case TOKENS.MINUS:
                // Rule 89
                match(TOKENS.MINUS);
                break;
            case TOKENS.PLUS:
                // Rule 88
                match(TOKENS.PLUS);
                break;
            default:
                error(new List<TOKENS>{TOKENS.OR, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void term(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                // Rule 91
                factor();
                factorTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private void factorTail(){
        switch(__lookahead.Type){
            case TOKENS.AND:
            case TOKENS.DIV:
            case TOKENS.MOD:
            case TOKENS.FLOAT_DIVIDE:
            case TOKENS.TIMES:
                // Rule 92
                multiplyingOperator();
                factor();
                factorTail();
                break;
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.END:
            case TOKENS.OR:
            case TOKENS.THEN:
            case TOKENS.TO:
            case TOKENS.COMMA:
            case TOKENS.EQUAL:
            case TOKENS.GEQUAL:
            case TOKENS.GTHAN:
            case TOKENS.LEQUAL:
            case TOKENS.LTHAN:
            case TOKENS.MINUS:
            case TOKENS.NEQUAL:
            case TOKENS.PLUS:
            case TOKENS.RPAREN:
            case TOKENS.SCOLON:
                // Rule 93
                break;
            default:
                error(new List<TOKENS>{TOKENS.AND, TOKENS.DIV, TOKENS.MOD, TOKENS.FLOAT_DIVIDE,
                    TOKENS.TIMES, TOKENS.DO, TOKENS.DOWNTO, TOKENS.END, TOKENS.OR, TOKENS.THEN,
                    TOKENS.TO, TOKENS.COMMA, TOKENS.EQUAL, TOKENS.GEQUAL, TOKENS.LEQUAL,
                    TOKENS.LTHAN, TOKENS.MINUS, TOKENS.NEQUAL, TOKENS.PLUS, TOKENS.RPAREN,
                    TOKENS.SCOLON});
                break;
        }
    }

    private void multiplyingOperator(){
        switch(__lookahead.Type){
            case TOKENS.AND:
                // Rule 98
                match(TOKENS.AND);
                break;
            case TOKENS.DIV:
                // Rule 96
                match(TOKENS.DIV);
                break;
            case TOKENS.MOD:
                //Rule 97
                match(TOKENS.MOD);
                break;
            case TOKENS.FLOAT_DIVIDE:
                // Rule 95
                match(TOKENS.FLOAT_DIVIDE);
                break;
            case TOKENS.TIMES:
                // Rule 94
                match(TOKENS.TIMES);
                break;
            default:
                error(new List<TOKENS>{TOKENS.AND, TOKENS.DIV, TOKENS.MOD, TOKENS.FLOAT_DIVIDE,
                    TOKENS.TIMES});
                break;
        }
    }

    private void factor(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
                // Rule 103
                match(TOKENS.FALSE);
                break;
            case TOKENS.NOT:
                // Rule 104
                match(TOKENS.NOT);
                factor();
                break;
            case TOKENS.TRUE:
                // Rule 102
                match(TOKENS.TRUE);
                break;
            case TOKENS.IDENTIFIER:
                // Rule 106
                functionIdentifier();
                optionalActualParameterList();
                break;
            case TOKENS.INTEGER_LIT:
                // Rule 99
                match(TOKENS.INTEGER_LIT);
                break;
            case TOKENS.FIXED_LIT:
                // Rule 100?
                match(TOKENS.FIXED_LIT);
                break;
            case TOKENS.FLOAT_LIT:
                // Rule 100
                match(TOKENS.FLOAT_LIT);
                break;
            case TOKENS.STRING_LIT:
                // Rule 101
                match(TOKENS.STRING_LIT);
                break;
            case TOKENS.LPAREN:
                // Rule 105
                match(TOKENS.LPAREN);
                expression();
                match(TOKENS.RPAREN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private void programIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 107
                match(TOKENS.IDENTIFIER);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void variableIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 108
                match(TOKENS.IDENTIFIER);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void procedureIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 109
                match(TOKENS.IDENTIFIER);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void functionIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 110
                match(TOKENS.IDENTIFIER);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void booleanExpression(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                // Rule 111
                expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT, TOKENS.LPAREN});
                break;
        }
    }

    private void ordinalExpression(){
        switch(__lookahead.Type){
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FIXED_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                // Rule 112
                expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FIXED_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private void identifierList(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                // Rule 113
                match(TOKENS.IDENTIFIER);
                identifierTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void identifierTail(){
        switch(__lookahead.Type){
            case TOKENS.COLON:
                // Rule 115
                break;
            case TOKENS.COMMA:
                // Rule 114
                match(TOKENS.COMMA);
                match(TOKENS.IDENTIFIER);
                identifierTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.COLON, TOKENS.COMMA});
                break;
        }
    }
}
