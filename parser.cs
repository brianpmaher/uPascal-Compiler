using System;
using System.IO;
using System.Collections.Generic;

public class Parser {
    // Dunders because derp
    private List<Token> __tokens;
    private Token __lookahead;
    private List<Token>.Enumerator __e;

    public Parser(List<Token> tokens){
        this.__tokens = tokens;
        this.__e = __tokens.GetEnumerator();
        this.__lookahead = __e.Current;
    }

    // Error
    private void error(string expected){
        throw new Exception(
            String.Format(
                Constants.ERROR_SYNTAX,
                __lookahead.Line,
                __lookahead.Column
            ) +
            String.Format(
                Constants.ERROR_PARSER,
                expected,
                __lookahead
            )
        );
    }

    // Match
    private void match(TOKENS token){
        if(__e.Current.Type == token){
            if(__e.MoveNext()){
                __lookahead = __e.Current;
            }
            else{
                throw new Exception(
                    "Expected token, got null!"
                );
            }
        } else{
            throw new Exception("Match called on nonmatching token!");
        }
    }

    // Non-terminals
    private void systemGoal(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                program();
                match(TOKENS.EOF);
                Console.WriteLine("The input program parses!");
                break;
            default:
                error("'program'");
                break;
        }
    }

    private void program(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                programHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.PERIOD);
                break;
            default:
                error("'program'");
                break;
        }
    }

    private void programHeading(){
        switch(__lookahead.Type){
            case TOKENS.PROGRAM:
                match(TOKENS.PROGRAM);
                programIdentifier();
                break;
            default:
                error("'program'");
                break;
        }
    }

    private void block(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
            case TOKENS.VAR:
                variableDeclarationPart();
                procedureAndFunctionDeclarationPart();
                statementPart();
                break;
            default:
                error("one of: 'begin', 'function', 'procedure', 'var'");
                break;
        }
    }

    private void variableDeclarationPart(){
        switch(__lookahead.Type){
            case TOKENS.VAR:
                match(TOKENS.VAR);
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
                break;
            default:
                error("one of: 'var', 'begin', 'function', 'procedure'");
                break;
        }
    }

    private void variableDeclarationTail(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                variableDeclaration();
                match(TOKENS.SCOLON);
                variableDeclarationTail();
                break;
            case TOKENS.PROCEDURE:
                break;
            default:
                error("an identifier or 'procedure'");
                break;
        }
    }

    private void variableDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error("an identifier");
                break;
        }
    }

    private void type(){
        switch(__lookahead.Type){
            case TOKENS.INTEGER:
                match(TOKENS.INTEGER);
                break;
            case TOKENS.FLOAT:
                match(TOKENS.FLOAT);
                break;
            case TOKENS.STRING:
                match(TOKENS.STRING);
                break;
            case TOKENS.BOOLEAN:
                match(TOKENS.BOOLEAN);
                break;
            default:
                error("a type");
                break;
        }
    }

    private void procedureAndFunctionDeclarationPart(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                procedureDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.FUNCTION:
                functionDeclaration();
                procedureAndFunctionDeclarationPart();
                break;
            case TOKENS.BEGIN:
                break;
            default:
                error("one of: 'procedure', 'function', 'begin'");
                break;
        }
    }

    private void procedureDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                procedureHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                break;
            default:
                error("'procedure'");
                break;
        }
    }

    private void functionDeclaration(){
        switch(__lookahead.Type){
            case TOKENS.FUNCTION:
                functionHeading();
                match(TOKENS.SCOLON);
                block();
                match(TOKENS.SCOLON);
                break;
            default:
                error("'function'");
                break;
        }
    }

    private void procedureHeading(){
        switch(__lookahead.Type){
            case TOKENS.PROCEDURE:
                match(TOKENS.PROCEDURE);
                procedureIdentifier();
                optionalFormalParameterList();
                break;
            default:
                error("'procedure'");
                break;
        }
    }

    private void functionHeading(){
        switch(__lookahead.Type){
            case TOKENS.FUNCTION:
                match(TOKENS.FUNCTION);
                functionIdentifier();
                optionalFormalParameterList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error("'function'");
                break;
        }
    }

    private void optionalFormalParameterList(){
        switch(__lookahead.Type){
            case TOKENS.LPAREN:
                match(TOKENS.LPAREN);
                formalParameterSection();
                formalParameterSectionTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.COLON:
            case TOKENS.SCOLON:
                break;
            default:
                error("one of: '(', ':', ';'");
                break;
        }
    }

    private void formalParameterSectionTail(){
        switch(__lookahead.Type){
            case TOKENS.SCOLON:
                match(TOKENS.SCOLON);
                formalParameterSection();
                formalParameterSectionTail();
                break;
            case TOKENS.RPAREN:
                break;
            default:
                error("one of: ';', ')'");
                break;
        }
    }

    private void formalParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                valueParameterSection();
                break;
            case TOKENS.VAR:
                variableParameterSection();
                break;
            default:
                error("an identifier or 'var'");
                break;
        }
    }

    private void valueParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.IDENTIFIER:
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error("an identifier");
                break;
        }
    }

    private void variableParameterSection(){
        switch(__lookahead.Type){
            case TOKENS.VAR:
                match(TOKENS.VAR);
                identifierList();
                match(TOKENS.COLON);
                type();
                break;
            default:
                error("'var'");
                break;
        }
    }

    private void statementPart(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
                compoundStatement();
                break;
            default:
                error("'begin'");
                break;
        }
    }

    private void compoundStatement(){
        switch(__lookahead.Type){
            case TOKENS.BEGIN:
                match(TOKENS.BEGIN);
                statementSequence();
                match(TOKENS.END);
                break;
            default:
                error("'begin'");
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
                statement();
                statementTail();
                break;
            default:
                error("an identifier or one of: " +
                    "'begin', 'end', 'for', 'if', 'read', 'repeat', " +
                    "'while', 'write', 'writeln', ':'"
                );
                break;
        }
    }

    private void statementTail(){

    }

    private void statement(){

    }

    private void emptyStatement(){

    }

    private void readStatement(){

    }

    private void readParameterTail(){

    }

    private void readParameter(){

    }

    private void writeStatement(){

    }

    private void writeParameterTail(){

    }

    private void writeParameter(){

    }

    private void assignmentStatement(){

    }

    private void ifStatement(){

    }

    private void optionalElsePart(){

    }

    private void repeatStatement(){

    }

    private void whileStatement(){

    }

    private void forStatement(){

    }

    private void controlVariable(){

    }

    private void initialValue(){

    }

    private void stepValue(){

    }

    private void finalValue(){

    }

    private void procedureStatement(){

    }

    private void optionalActualParameterList(){

    }

    private void actualParameterTail(){

    }

    private void actualParameter(){

    }

    private void expression(){

    }

    private void optionalRelationalPart(){

    }

    private void relationalOperator(){

    }

    private void simpleExpression(){

    }

    private void termTail(){

    }

    private void optionalSign(){

    }

    private void addingOperator(){

    }

    private void term(){

    }

    private void factorTail(){

    }

    private void multiplyingOperator(){

    }

    private void factor(){

    }

    private void programIdentifier(){

    }

    private void variableIdentifier(){

    }

    private void procedureIdentifier(){

    }

    private void functionIdentifier(){

    }

    private void booleanExpression(){

    }

    private void ordinalExpression(){

    }

    private void identifierList(){

    }

    private void identifierTail(){

    }
}
