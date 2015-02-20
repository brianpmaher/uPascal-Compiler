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
            case TOKENS.INTEGER_LIT:
                match(TOKENS.INTEGER_LIT);
                break;
            case TOKENS.FLOAT:
                match(TOKENS.FLOAT_LIT);
                break;
            case TOKENS.STRING:
                match(TOKENS.STRING_LIT);
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
        switch(__lookahead.Type) {
            case TOKENS.END:
                break;
            case TOKENS.SCOLON:
                match(TOKENS.SCOLON);
                statement();
                statementTail();
                break;
            default:
                error("end of line or ';'");
                break;
        }
    }

    private void statement(){
        switch(__lookahead.Type) {
            case TOKENS.BEGIN:
                compoundStatement();
                break;
            case TOKENS.FOR:
                forStatement();
                break;
            case TOKENS.IF:
                ifStatement();
                break;
            case TOKENS.READ:
                readStatement();
                break;
            case TOKENS.REPEAT:
                repeatStatement();
                break;
            case TOKENS.WHILE:
                whileStatement();
                break;
            case TOKENS.WRITE:
            case TOKENS.WRITELN:
                writeStatement();
                break;
            case TOKENS.IDENTIFIER:
                assignmentStatement();
                // OR procedureStatement();
                break;
            case TOKENS.END:
            case TOKENS.SCOLON:
                emptyStatement();
                break;
            default:
                error("an identifier or one of: " +
                    "'begin', 'end', 'for', 'if', 'read', 'repeat', " +
                    "'while', 'write', 'writeln', ':'"
                );
                break;
        }
    }

    private void emptyStatement(){
        switch(__lookahead.Type) {
            case TOKENS.SCOLON:
                break;
            default:
                error("';'");
                break;
        }
    }

    private void readStatement(){
        switch(__lookahead.Type) {
            case TOKENS.READ:
                match(TOKENS.READ);
                match(TOKENS.LPAREN);
                readParameter();
                readParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error("'read'");
                break;
        }
    }

    private void readParameterTail(){
        switch(__lookahead.Type) {
            case TOKENS.COMMA:
                match(TOKENS.COMMA);
                readParameter();
                readParameterTail();
                break;
            case TOKENS.RPAREN:
                break;
            default:
                error("',',')'");
                break;
        }
    }

    private void readParameter(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                variableIdentifier();
                break;
            default:
                error("An identifier");
                break;
        }
    }

    private void writeStatement(){
        switch(__lookahead.Type) {
            case TOKENS.WRITE:
                match(TOKENS.WRITE);
                match(TOKENS.LPAREN);
                writeParameter();
                writeParameterTail();
                match(TOKENS.RPAREN);
                break;
            case TOKENS.WRITELN:
                match(TOKENS.WRITELN);
                match(TOKENS.LPAREN);
                writeParameter();
                writeParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error("'write','writeln'");
                break;
        }
    }

    private void writeParameterTail(){
        switch(__lookahead.Type) {
            case TOKENS.COMMA:
                match(TOKENS.COMMA);
                writeParameter();
                writeParameterTail();
                break;
            case TOKENS.RPAREN:
                break;
            default:
                error("',',')'");
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
                ordinalExpression();
                break;
            default:
                error(
                    "An identifier, integer, float, fixed, string, or any of the following: " +
                    "'false','not','true','(','-','+'"
                );
                break;
        }
    }

    private void assignmentStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                variableIdentifier();
                // OR functionIdentifier();
                match(TOKENS.ASSIGN);
                expression();
                break;
            default:
                error("An identifier");
                break;
        }
    }

    private void ifStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IF:
                match(TOKENS.IF);
                booleanExpression();
                match(TOKENS.THEN);
                statement();
                optionalElsePart();
                break;
            default:
                error("'if'");
                break;
        }
    }

    private void optionalElsePart(){
        switch(__lookahead.Type) {
            case TOKENS.ELSE:
                match(TOKENS.ELSE);
                statement();
                break;
            case TOKENS.END:
            case TOKENS.SCOLON:
                break;
            default:
                error("end of line or 'else',';'");
                break;
        }
    }

    private void repeatStatement(){
        switch(__lookahead.Type) {
            case TOKENS.REPEAT:
                match(TOKENS.REPEAT);
                statementSequence();
                match(TOKENS.UNTIL);
                booleanExpression();
                break;
            default:
                error("'repeat'");
                break;
        }
    }

    private void whileStatement(){
        switch(__lookahead.Type) {
            case TOKENS.WHILE:
                match(TOKENS.WHILE);
                booleanExpression();
                match(TOKENS.DO);
                statement();
                break;
            default:
                error("'while'");
                break;
        }
    }

    private void forStatement(){
        switch(__lookahead.Type) {
            case TOKENS.FOR:
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
                error("'for'");
                break;
        }
    }

    private void controlVariable(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                variableIdentifier();
                break;
            default:
                error("An identifier");
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
                ordinalExpression();
                break;
            default:
                error(
                    "An identifier, integer, fixed, float, string or any of the following: " +
                    "'false','not','true','(','-','+'"
                );
                break;
        }
    }

    private void stepValue(){
        switch(__lookahead.Type) {
            case TOKENS.DOWNTO:
                match(TOKENS.DOWNTO);
                break;
            case TOKENS.TO:
                match(TOKENS.TO);
                break;
            default:
                error("'downto','to'");
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
                ordinalExpression();
                break;
            default:
                error(
                    "An identifier, integer, fixed, float, string or any of the following: " +
                    "'false','not','true','(','-','+'"
                );
                break;
        }
    }

    private void procedureStatement(){
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                procedureIdentifier();
                optionalActualParameterList();
                break;
            default:
                error("An identifier");
                break;
        }
    }

    private void optionalActualParameterList(){
        switch(__lookahead.Type) {
            case TOKENS.END:
            case TOKENS.SCOLON:
                break;
            case TOKENS.LPAREN:
                match(TOKENS.LPAREN);
                actualParameter();
                actualParameterTail();
                match(TOKENS.RPAREN);
                break;
            default:
                error("End of line or ';','('");
                break;
        }
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
