using System;
using System.IO;
using System.Collections.Generic;

public class Parser {
    private List<Token> __tokens;
    private Token __lookahead;
    private List<Token>.Enumerator __e;
    private Stack<SymbolTable> __symbolTableStack;
    private int __forCount = 0; // Used for labeling purposes

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
    private String match(TOKENS token){
        // Simple output
        String matchedLexeme = __lookahead.Lexeme;
        Console.WriteLine("\nMatching " + matchedLexeme);
        if(token == TOKENS.EOF){
            return;
        } else if(__e.Current.Type == token){
            if(__e.MoveNext()){
                __lookahead = __e.Current;
                return matchedLexeme;
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
                Console.Write(1 + " ");
                program();
                match(TOKENS.EOF);
                Console.WriteLine("\nThe input program parses!");
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void program(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                Console.Write(2 + " ");
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
                Console.Write(3 + " ");
                match(TOKENS.PROGRAM);
                String programName = programIdentifier();
                __symbolTableStack.Push(new SymbolTable(programName, 0, programName, null));
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
                Console.Write(4 + " ");
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
                Console.Write(5 + " ");
                match(TOKENS.VAR);
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
                Console.Write(6 + " ");
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
                Console.Write(7 + " ");
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
                Console.Write(8 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER, TOKENS.PROCEDURE});
                break;
        }
    }

    private void variableDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(9 + " ");
                List<String> identifiers = identifierList();
                match(TOKENS.COLON);
                TYPES type = type();
                SymbolTable top = __symbolTableStack.Peek();
                if(type != null){
                    for(String identifier in identifiers){
                        top.AddEntry(new Entry(identifier, type, KINDS.VAR, 1, null));
                    }
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private TYPES type(){
        switch(__lookahead.Type){
            case TOKENS.INTEGER:
                Console.Write(10 + " ");
                match(TOKENS.INTEGER);
                return TYPES.INTEGER;
            case TOKENS.FLOAT:
                Console.Write(11 + " ");
                match(TOKENS.FLOAT);
                return TYPES.FLOAT;
            case TOKENS.STRING:
                Console.Write(12 + " ");
                match(TOKENS.STRING);
                return TYPES.STRING;
            case TOKENS.BOOLEAN:
                Console.Write(13 + " ");
                match(TOKENS.BOOLEAN);
                return TYPES.BOOLEAN;
            default:
                error(new List<TOKENS>{TOKENS.INTEGER, TOKENS.FLOAT, TOKENS.STRING,
                    TOKENS.BOOLEAN});
                break;
        }
        return null;
    }

    private void procedureAndFunctionDeclarationPart(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                Console.Write(14 + " ");
                procedureDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.FUNCTION:
                Console.Write(15 + " ");
                functionDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.BEGIN:
                Console.Write(16 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE, TOKENS.FUNCTION, TOKENS.BEGIN});
                break;
        }
    }

    private void procedureDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                Console.Write(17 + " ");
                procedureHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                __symbolTableStack.Pop();
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE});
                break;
        }
    }

    private void functionDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.FUNCTION:
                Console.Write(18 + " ");
                functionHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                __symbolTableStack.Pop();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FUNCTION});
                break;
        }
    }

    private void procedureHeading(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                Console.Write(19 + " ");
                match(TOKENS.PROCEDURE);
                String procName = procedureIdentifier();
                __symbolTableStack.Peek().AddEntry(
                    new Entry(
                        procName,
                        null,
                        KINDS.PROCEDURE,
                        0,
                        new List<String>()));
                __symbolTableStack.Push(
                    new SymbolTable(
                        procName,
                        __symbolTableStack.Peek().NestingLevel + 1,
                        procName,
                        new List<Entry>()));
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
                Console.Write(20 + " ");
                match(TOKENS.FUNCTION);
                String funcName = functionIdentifier();
                __symbolTableStack.Peek().AddEntry(
                    new Entry(
                        procName,
                        null,
                        KINDS.FUNCTION,
                        0,
                        new List<String>()));
                __symbolTableStack.Push(
                    new SymbolTable(
                        procName,
                        __symbolTableStack.Peek().NestingLevel + 1,
                        procName,
                        new List<Entry>()));
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
                Console.Write(21 + " ");
                match(TOKENS.LPAREN);
                formalParameterSection();
                formalParameterSectionTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.COLON:
            case TOKENS.SCOLON:
                Console.Write(22 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.LPAREN, TOKENS.COLON, TOKENS.SCOLON});
                break;
        }
    }

    private void formalParameterSectionTail(){
        switch(__lookahead.Type){
            case TOKENS.SCOLON:
                Console.Write(23 + " ");
                match(TOKENS.SCOLON);
                formalParameterSection();
                formalParameterSectionTail();
                break;
            case TOKENS.RPAREN:
                Console.Write(24 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.SCOLON, TOKENS.RPAREN});
                break;
        }
    }

    private void formalParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(25 + " ");
                valueParameterSection();
                break;
            case TOKENS.VAR:
                Console.Write(26 + " ");
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
                Console.Write(27 + " ");
                List<String> idList = identifierList();
                match(TOKENS.COLON);
                TYPES type = type();
                for(String identifier in idList){
                    __symbolTableStack.Peek().AddEntry(
                        new Entry(
                            identifier,
                            type,
                            KINDS.PARAMETER,
                            1,
                            null
                        )
                    );
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void variableParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.VAR:
                Console.Write(28 + " ");
                match(TOKENS.VAR);
                List<String> idList = identifierList();
                match(TOKENS.COLON);
                TYPES type = type();
                for(String identifier in idList){
                    __symbolTableStack.Peek().AddEntry(
                        new Entry(
                            identifier,
                            type,
                            KINDS.PARAMETER,
                            1,
                            null
                        )
                    );
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.VAR});
                break;
        }
    }

    private void statementPart(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
                Console.Write(29 + " ");
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
                Console.Write(30 + " ");
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
            case TOKENS.UNTIL:
            case TOKENS.WHILE:
            case TOKENS.WRITE:
            case TOKENS.WRITELN:
            case TOKENS.IDENTIFIER:
            case TOKENS.SCOLON:
                Console.Write(31 + " ");
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
            case TOKENS.UNTIL:
                Console.Write(33 + " ");
                break;
            case TOKENS.SCOLON:
                Console.Write(32 + " ");
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
                Console.Write(35 + " ");
                compoundStatement();
                break;
            case TOKENS.FOR:
                Console.Write(42 + " ");
                forStatement();
                break;
            case TOKENS.IF:
                Console.Write(39 + " ");
                ifStatement();
                break;
            case TOKENS.READ:
                Console.Write(36 + " ");
                readStatement();
                break;
            case TOKENS.REPEAT:
                Console.Write(41 + " ");
                repeatStatement();
                break;
            case TOKENS.WHILE:
                Console.Write(40 + " ");
                whileStatement();
                break;
            case TOKENS.WRITE:
            case TOKENS.WRITELN:
                Console.Write(37 + " ");
                writeStatement();
                break;
            case TOKENS.IDENTIFIER:
                String identifier = __lookahead.Lexeme;
                KINDS identifierKind = __symbolTableStack.Peek().GetKind(identifier);
                if(identifierKIND == KINDS.VAR){
                    Console.Write(38 + " ");
                    assignmentStatement();
                } else if(identifierKIND == KINDS.PROCEDURE){
                    Console.Write(43 + " ");
                    procedureStatement();
                } else {
                    // New Kind of Error?
                }
                break;
            case TOKENS.ELSE:
            case TOKENS.END:
            case TOKENS.SCOLON:
            case TOKENS.UNTIL:
                Console.Write(34 + " ");
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
            // case TOKENS.ELSE: - Commenting this may resolve the else conflict
            case TOKENS.SCOLON:
            case TOKENS.UNTIL:
                Console.Write(44 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void readStatement(){
        switch(__lookahead.Type) {
            case TOKENS.READ:
                Console.Write(45 + " ");
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
                Console.Write(46 + " ");
                match(TOKENS.COMMA);
                readParameter();
                readParameterTail();
                break;
            case TOKENS.RPAREN:
                Console.Write(47 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.COMMA, TOKENS.RPAREN});
                break;
        }
    }

    private void readParameter(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(48 + " ");
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
                Console.Write(49 + " ");
                match(TOKENS.WRITE);
                match(TOKENS.LPAREN);
                writeParameter();
                writeParameterTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.WRITELN:
                Console.Write(50 + " ");
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
                Console.Write(51 + " ");
                match(TOKENS.COMMA);
                writeParameter();
                writeParameterTail();
                break;
            case TOKENS.RPAREN:
                Console.Write(52 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(53 + " ");
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void assignmentStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                String identifer = __lookahead.Lexeme;
                KINDS identifierKind = __symbolTableStack.Peek().getKind(identifier);
                if(identifierKind == KINDS.VAR){
                    Console.Write(54 + " ");
                    variableIdentifier();
                } else if(identiferKind == KINDS.FUNCTION){
                    Console.Write(55 + " ");
                    functionIdentifier();
                } else {
                    throw new Exception("Expected a function or variable, got " + identifierKind);
                }
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
                Console.Write(56 + " ");
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
                Console.Write(57 + " ");
                match(TOKENS.ELSE);
                statement();
                break;
            case TOKENS.END:
            case TOKENS.SCOLON:
            case TOKENS.UNTIL:
                Console.Write(58 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.ELSE, TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void repeatStatement(){
        switch(__lookahead.Type) {
            case TOKENS.REPEAT:
                Console.Write(59 + " ");
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
                Console.Write(60 + " ");
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
                Console.Write(61 + " ");
                match(TOKENS.FOR);
                __forCount++;
                __symbolTableStack.Push(new SymbolTable(
                    "For_" + __forCount,
                    __symbolTableStack.Peek().NestingLevel + 1,
                    "For_" + __forCount,
                    null
                ));
                String identifier = controlVariable();
                match(TOKENS.ASSIGN);
                TYPES typeInitial = initialValue();
                __symbolTableStack.Peek().AddEntry(
                    new Entry(
                        identifier,
                        typeInitial,
                        KINDS.VAR,
                        1,
                        null
                    )
                );
                stepValue();
                finalValue();
                match(TOKENS.DO);
                statement();
                __symbolTableStack.Pop();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FOR});
                break;
        }
    }

    private String controlVariable(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(62 + " ");
                String identifier = variableIdentifier();
                return identifier;
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(63 + " ");
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void stepValue(){
        switch(__lookahead.Type) {
            case TOKENS.DOWNTO:
                Console.Write(65 + " ");
                match(TOKENS.DOWNTO);
                break;
            case TOKENS.TO:
                Console.Write(64 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(66 + " ");
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void procedureStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(67 + " ");
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
            case TOKENS.UNTIL:
                Console.Write(69 + " ");
                break;
            case TOKENS.LPAREN:
                Console.Write(68 + " ");
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
                Console.Write(70 + " ");
                match(TOKENS.COMMA);
                actualParameter();
                actualParameterTail();
                break;
            case TOKENS.RPAREN:
                Console.Write(71 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(72 + " ");
                ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(73 + " ");
                simpleExpression();
                optionalRelationalPart();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void optionalRelationalPart(){
        switch(__lookahead.Type){
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.ELSE:
            case TOKENS.END:
            case TOKENS.THEN:
            case TOKENS.TO:
            case TOKENS.COMMA:
            case TOKENS.RPAREN:
            case TOKENS.SCOLON:
            case TOKENS.UNTIL:
                Console.Write(75 + " ");
                break;
            case TOKENS.EQUAL:
            case TOKENS.GEQUAL:
            case TOKENS.GTHAN:
            case TOKENS.LEQUAL:
            case TOKENS.LTHAN:
            case TOKENS.NEQUAL:
                Console.Write(74 + " ");
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
                Console.Write(76 + " ");
                match(TOKENS.EQUAL);
                break;
            case TOKENS.LTHAN:
                Console.Write(77 + " ");
                match(TOKENS.LTHAN);
                break;
            case TOKENS.GTHAN:
                Console.Write(78 + " ");
                match(TOKENS.GTHAN);
                break;
            case TOKENS.LEQUAL:
                Console.Write(79 + " ");
                match(TOKENS.LEQUAL);
                break;
            case TOKENS.GEQUAL:
                Console.Write(80 + " ");
                match(TOKENS.GEQUAL);
                break;
            case TOKENS.NEQUAL:
                Console.Write(81 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(82 + " ");
                optionalSign();
                term();
                termTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void termTail(){
        switch(__lookahead.Type){
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.ELSE:
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
            case TOKENS.UNTIL:
                Console.Write(84 + " ");
                break;
            case TOKENS.OR:
            case TOKENS.MINUS:
            case TOKENS.PLUS:
                Console.Write(83 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                Console.Write(87 + " ");
                break;
            case TOKENS.MINUS:
                Console.Write(86 + " ");
                match(TOKENS.MINUS);
                break;
            case TOKENS.PLUS:
                Console.Write(85 + " ");
                match(TOKENS.PLUS);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private void addingOperator(){
        switch(__lookahead.Type){
            case TOKENS.OR:
                Console.Write(90 + " ");
                match(TOKENS.OR);
                break;
            case TOKENS.MINUS:
                Console.Write(89 + " ");
                match(TOKENS.MINUS);
                break;
            case TOKENS.PLUS:
                Console.Write(88 + " ");
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                Console.Write(91 + " ");
                factor();
                factorTail();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
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
                Console.Write(92 + " ");
                multiplyingOperator();
                factor();
                factorTail();
                break;
            case TOKENS.DO:
            case TOKENS.DOWNTO:
            case TOKENS.ELSE:
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
            case TOKENS.UNTIL:
                Console.Write(93 + " ");
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
                Console.Write(98 + " ");
                match(TOKENS.AND);
                break;
            case TOKENS.DIV:
                Console.Write(96 + " ");
                match(TOKENS.DIV);
                break;
            case TOKENS.MOD:
                Console.Write(97 + " ");
                match(TOKENS.MOD);
                break;
            case TOKENS.FLOAT_DIVIDE:
                Console.Write(95 + " ");
                match(TOKENS.FLOAT_DIVIDE);
                break;
            case TOKENS.TIMES:
                Console.Write(94 + " ");
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
                Console.Write(103 + " ");
                match(TOKENS.FALSE);
                break;
            case TOKENS.NOT:
                Console.Write(104 + " ");
                match(TOKENS.NOT);
                factor();
                break;
            case TOKENS.TRUE:
                Console.Write(102 + " ");
                match(TOKENS.TRUE);
                break;
            case TOKENS.IDENTIFIER:
                String identifier = __lookahead.Lexeme;
                KINDS identifierKind = __symbolTableStack.Peek().getKind(identifier);
                if(identifierKind == KINDS.VAR){
                    Console.Write(116 + " ");
                    variableIdentifier();
                } else if(identifierKind == KINDS.FUNCTION){
                    Console.Write(106 + " ");
                    functionIdentifier();
                    optionalActualParameterList();
                } else {
                    throw new Exception("Expected variable or function identifier");
                }
                break;
            case TOKENS.INTEGER_LIT:
                Console.Write(99 + " ");
                match(TOKENS.INTEGER_LIT);
                break;
            case TOKENS.FLOAT_LIT:
                Console.Write(100 + " ");
                match(TOKENS.FLOAT_LIT);
                break;
            case TOKENS.STRING_LIT:
                Console.Write(101 + " ");
                match(TOKENS.STRING_LIT);
                break;
            case TOKENS.LPAREN:
                Console.Write(105 + " ");
                match(TOKENS.LPAREN);
                expression();
                match(TOKENS.RPAREN);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private String programIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(107 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private String variableIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(108 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private String procedureIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(109 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private String functionIdentifier(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(110 + " ");
                return match(TOKENS.IDENTIFIER);
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
            case TOKENS.INTEGER_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.PLUS:
            case TOKENS.MINUS:
                Console.Write(111 + " ");
                expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.FLOAT_LIT, TOKENS.STRING_LIT, TOKENS.LPAREN});
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
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
            case TOKENS.PLUS:
            case TOKENS.MINUS:
                Console.Write(112 + " ");
                expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
    }

    private List<String> identifierList(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                Console.Write(113 + " ");
                List<String> idList = new List<String>();
                idList.Add(match(TOKENS.IDENTIFIER));
                idList.AddRange(identifierTail());
                return idList;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void identifierTail(){
        switch(__lookahead.Type){
            case TOKENS.COLON:
                Console.Write(115 + " ");
                break;
            case TOKENS.COMMA:
                Console.Write(114 + " ");
                List<String> idList = new List<String>();
                match(TOKENS.COMMA);
                idList.Add(match(TOKENS.IDENTIFIER));
                idList.AddRange(identifierTail());
                return idList;
            default:
                error(new List<TOKENS>{TOKENS.COLON, TOKENS.COMMA});
                break;
        }
        return null;
    }
}
