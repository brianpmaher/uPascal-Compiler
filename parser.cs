/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;
using System.IO;
using System.Collections.Generic;

public class Parser {
    private List<Token> __tokens;
    private Token __lookahead;
    private List<Token>.Enumerator __e;
    private Stack<SymbolTable> __symbolTableStack;
    private SemAnalyzer __analyzer;

    public Parser(List<Token> tokens, String progname) {
        this.__tokens = tokens;
        this.__e = __tokens.GetEnumerator();
        this.__e.MoveNext();
        this.__lookahead = __e.Current;
        this.__symbolTableStack = new Stack<SymbolTable>();
        this.__analyzer = new SemAnalyzer(__symbolTableStack, progname);
    }

    public void Parse() {
        systemGoal();
    }

    // Error
    private void error(List<TOKENS> expected) {
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
    private String match(TOKENS token) {
        // Simple output
        String matchedLexeme = __lookahead.Lexeme;
        Console.WriteLine("\nMatching " + matchedLexeme);
        if(token == TOKENS.EOF) {
            return null;
        } else if(__e.Current.Type == token) {
            if(__e.MoveNext()) {
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
        return null;
    }

    // Non-terminals
    private void systemGoal() {
        switch(__lookahead.Type) {
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

    private void program() {
        switch(__lookahead.Type) {
            case TOKENS.PROGRAM:
                Console.Write(2 + " ");
                programHeading();
                match(TOKENS.SCOLON);

                // generate label and branch to it
                string label = LabelMaker.genLabel();
                __analyzer.genBr(label);

                block(label);
                match(TOKENS.PERIOD);
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void programHeading() {
        switch(__lookahead.Type) {
            case TOKENS.PROGRAM:
                Console.Write(3 + " ");
                match(TOKENS.PROGRAM);
                String programName = programIdentifier();
                Console.WriteLine("Program name is: " + programName);
                __symbolTableStack.Push(new SymbolTable(
                    programName, 0, 0, 0, new List<Entry>(),
                    null));
                __analyzer.genInit();
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROGRAM});
                break;
        }
    }

    private void block(string label) {
        switch(__lookahead.Type) {
            case TOKENS.BEGIN:
            case TOKENS.FUNCTION:
            case TOKENS.PROCEDURE:
            case TOKENS.VAR:
                Console.Write(4 + " ");
                variableDeclarationPart();
                procedureAndFunctionDeclarationPart();
                __analyzer.genOut(label + ":");
                __analyzer.genSymSize();
                statementPart();
                __analyzer.genEnd();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN, TOKENS.FUNCTION, TOKENS.PROCEDURE,
                    TOKENS.VAR});
                break;
        }
    }

    private void variableDeclarationPart() {
        switch(__lookahead.Type) {
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

    private void variableDeclarationTail() {
        switch(__lookahead.Type) {
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

    private void variableDeclaration() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(9 + " ");
                List<String> identifiers = identifierList();
                match(TOKENS.COLON);
                TYPES typeRec = type();
                SymbolTable top = __symbolTableStack.Peek();
                if(typeRec != TYPES.NONE) {
                    foreach(String identifier in identifiers) {
                        top.AddEntry(identifier, typeRec, KINDS.VAR, 1, null);
                    }
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private TYPES type() {
        switch(__lookahead.Type) {
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
        return TYPES.NONE;
    }

    private void procedureAndFunctionDeclarationPart() {
        switch(__lookahead.Type) {
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

    private void procedureDeclaration() {
        switch(__lookahead.Type) {
            case TOKENS.PROCEDURE:
                Console.Write(17 + " ");
                procedureHeading();
                match(TOKENS.SCOLON);
                string label = LabelMaker.genLabel();
                block(label);
                match(TOKENS.SCOLON);
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE});
                break;
        }
    }

    private void functionDeclaration() {
        switch(__lookahead.Type) {
            case TOKENS.FUNCTION:
                Console.Write(18 + " ");
                functionHeading();
                match(TOKENS.SCOLON);
                string label = LabelMaker.genLabel();
                block(label);
                match(TOKENS.SCOLON);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FUNCTION});
                break;
        }
    }

    private void procedureHeading() {
        switch(__lookahead.Type) {
            case TOKENS.PROCEDURE:
                Console.Write(19 + " ");
                match(TOKENS.PROCEDURE);
                String identifier = procedureIdentifier();
                List<Entry> entries = optionalFormalParameterList();
                // Make procedure symbol table entry and table
                List<String> paras = new List<String>();
                foreach(Entry entry in entries) {
                    paras.Add(entry.Lexeme);
                }
                //Add the entry for the procedure
                __symbolTableStack.Peek().AddEntry(
                    identifier,
                    TYPES.NONE,
                    KINDS.PROCEDURE,
                    0,
                    paras
                );
                __symbolTableStack.Push(
                    new SymbolTable(
                        identifier,
                        __symbolTableStack.Peek().NestingLevel +1,
                        0,
                        __symbolTableStack.Peek().NestingLevel +1,
                        new List<Entry>(),
                        __symbolTableStack.Peek()
                    )
                );
                foreach(Entry entry in entries) {
                    __symbolTableStack.Peek().AddEntry(entry);
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.PROCEDURE});
                break;
        }
    }

    private void functionHeading() {
        switch(__lookahead.Type) {
            case TOKENS.FUNCTION:
                Console.Write(20 + " ");
                match(TOKENS.FUNCTION);
                String identifier = functionIdentifier();
                List<Entry> entries = optionalFormalParameterList();
                match(TOKENS.COLON);
                TYPES funcRetType = type();
                // Make function symbol table entry and table
                List<String> paras = new List<String>();
                foreach(Entry entry in entries) {
                    if(entry != null) {
                        paras.Add(entry.Lexeme);
                    }
                }
                // Add the entry for the function
                __symbolTableStack.Peek().AddEntry(
                    identifier,
                    funcRetType,
                    KINDS.FUNCTION,
                    0,
                    paras
                );
                //Push it on, then add all the parameters as symbols
                __symbolTableStack.Push(
                    new SymbolTable(
                        identifier,
                        __symbolTableStack.Peek().NestingLevel + 1,
                        0,
                        //TODO: Change this to labeling
                        __symbolTableStack.Peek().NestingLevel + 1,
                        new List<Entry>(),
                        __symbolTableStack.Peek()
                    )
                );
                foreach(Entry entry in entries) {
                    __symbolTableStack.Peek().AddEntry(entry);
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.FUNCTION});
                break;
        }
    }

    private List<Entry> optionalFormalParameterList() {
        List<Entry> entries = new List<Entry>();
        switch(__lookahead.Type) {
            case TOKENS.LPAREN:
                Console.Write(21 + " ");
                match(TOKENS.LPAREN);
                entries.AddRange(formalParameterSection());
                entries.AddRange(formalParameterSectionTail());
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
        return entries;
    }

    private List<Entry> formalParameterSectionTail() {
        List<Entry> entries = new List<Entry>();
        switch(__lookahead.Type) {
            case TOKENS.SCOLON:
                Console.Write(23 + " ");
                match(TOKENS.SCOLON);
                entries.AddRange(formalParameterSection());
                entries.AddRange(formalParameterSectionTail());
                break;
            case TOKENS.RPAREN:
                Console.Write(24 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.SCOLON, TOKENS.RPAREN});
                break;
        }
        return entries;
    }

    private List<Entry> formalParameterSection() {
        List<Entry> entries = new List<Entry>();
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(25 + " ");
                entries.AddRange(valueParameterSection());
                break;
            case TOKENS.VAR:
                Console.Write(26 + " ");
                entries.AddRange(variableParameterSection());
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER, TOKENS.VAR});
                break;
        }
        return entries;
    }

    private List<Entry> valueParameterSection() {
        List<Entry> entries = new List<Entry>();
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(27 + " ");
                List<String> idList = identifierList();
                match(TOKENS.COLON);
                TYPES varType = type();
                foreach(String id in idList) {
                    entries.Add(new Entry(id, varType, KINDS.PARAMETER, 1, 0, null));
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
        return entries;
    }

    private List<Entry> variableParameterSection() {
        List<Entry> entries = new List<Entry>();
        switch(__lookahead.Type) {
            case TOKENS.VAR:
                Console.Write(28 + " ");
                match(TOKENS.VAR);
                List<String> idList = identifierList();
                match(TOKENS.COLON);
                TYPES varType = type();
                foreach(String id in idList) {
                    entries.Add(new Entry(id, varType, KINDS.PARAMETER, 1, 0, null));
                }
                break;
            default:
                error(new List<TOKENS>{TOKENS.VAR});
                break;
        }
        return entries;
    }

    private void statementPart() {
        switch(__lookahead.Type) {
            case TOKENS.BEGIN:
                Console.Write(29 + " ");
                compoundStatement();
                break;
            default:
                error(new List<TOKENS>{TOKENS.BEGIN});
                break;
        }
    }

    private void compoundStatement() {
        switch(__lookahead.Type) {
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

    private void statementSequence() {
        switch(__lookahead.Type) {
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

    private void statementTail() {
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
                error(new List<TOKENS>{TOKENS.END, TOKENS.UNTIL, TOKENS.SCOLON});
                break;
        }
    }

    private void statement() {
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
                if(identifierKind == KINDS.VAR || identifierKind == KINDS.PARAMETER || identifierKind == KINDS.FUNCTION) {
                    Console.Write(38 + " ");
                    assignmentStatement();
                } else if(identifierKind == KINDS.PROCEDURE) {
                    Console.Write(43 + " ");
                    procedureStatement();
                } else {
                    Console.WriteLine("Error processing " + identifier + " of Kind " + identifierKind);
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

    private void emptyStatement() {
        switch(__lookahead.Type) {
            case TOKENS.END:
            case TOKENS.ELSE:
            case TOKENS.SCOLON:
            case TOKENS.UNTIL:
                Console.Write(44 + " ");
                break;
            default:
                error(new List<TOKENS>{TOKENS.END, TOKENS.SCOLON});
                break;
        }
    }

    private void readStatement() {
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

    private void readParameterTail() {
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

    private void readParameter() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(48 + " ");
                String id = variableIdentifier();
                __analyzer.genRead(id);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void writeStatement() {
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
                __analyzer.genWriteLine();
                break;
            default:
                error(new List<TOKENS>{TOKENS.WRITE, TOKENS.WRITELN});
                break;
        }
    }

    private void writeParameterTail() {
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

    private void writeParameter() {
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
                __analyzer.genWrite();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
    }

    private void assignmentStatement() {
        SemRecord assigneeRec, expressionRec;
        assigneeRec = null;
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                String identifier = __lookahead.Lexeme;
                Entry identifierEntry = __symbolTableStack.Peek().GetEntry(identifier);
                if(identifierEntry.Kind == KINDS.VAR || identifierEntry.Kind == KINDS.PARAMETER) {
                    Console.Write(54 + " ");
                    variableIdentifier();
                    assigneeRec = new SemRecord(identifierEntry.Type, identifierEntry.Lexeme);
                } else if(identifierEntry.Kind == KINDS.FUNCTION) {
                    Console.Write(55 + " ");
                    functionIdentifier();
                    assigneeRec = new SemRecord(identifierEntry.Type, identifierEntry.Lexeme);
                } else {
                    throw new Exception("Expected a function or variable, got " + identifierEntry.Kind);
                }
                match(TOKENS.ASSIGN);
                expressionRec = expression();
                __analyzer.genAssign(assigneeRec, expressionRec);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
    }

    private void ifStatement() {
        switch(__lookahead.Type) {
            case TOKENS.IF:
                Console.Write(56 + " ");
                match(TOKENS.IF);
                string elseLabel = "L" + LabelMaker.genLabel();
                string outLabel = "L" + LabelMaker.genLabel();
                booleanExpression();
                match(TOKENS.THEN);
                __analyzer.genBrfs(elseLabel);
                statement();
                __analyzer.genBr(outLabel);
                __analyzer.genOut(elseLabel + ":");
                optionalElsePart();
                __analyzer.genOut(outLabel + ":");
                break;
            default:
                error(new List<TOKENS>{TOKENS.IF});
                break;
        }
    }

    private void optionalElsePart() {
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

    private void repeatStatement() {
        switch(__lookahead.Type) {
            case TOKENS.REPEAT:
                Console.Write(59 + " ");
                match(TOKENS.REPEAT);
                String lableRec = __analyzer.genLabel();
                statementSequence();
                match(TOKENS.UNTIL);
                booleanExpression();
                __analyzer.genBrfs(lableRec);
                break;
            default:
                error(new List<TOKENS>{TOKENS.REPEAT});
                break;
        }
    }

    private void whileStatement() {
        switch(__lookahead.Type) {
            case TOKENS.WHILE:
                Console.Write(60 + " ");
                string conditionLabel = "L" + LabelMaker.genLabel();
                string elseLabel = "L" + LabelMaker.genLabel();
                match(TOKENS.WHILE);
                __analyzer.genOut(conditionLabel + ":");
                booleanExpression();
                __analyzer.genBrfs(elseLabel);
                match(TOKENS.DO);
                statement();
                __analyzer.genBr(conditionLabel);
                __analyzer.genOut(elseLabel + ":");
                break;
            default:
                error(new List<TOKENS>{TOKENS.WHILE});
                break;
        }
    }

    private void forStatement() {
        switch(__lookahead.Type) {
            case TOKENS.FOR:
                Console.Write(61 + " ");
                match(TOKENS.FOR);

                // Store the labels for start and end of loop.
                string start = "L" + LabelMaker.genLabel();
                string end = "L" + LabelMaker.genLabel();

                // Get semrecord for control and initial
                SemRecord controlVar = controlVariable();
                match(TOKENS.ASSIGN);
                SemRecord initialVal = initialValue();

                // Generate the assign for control and initial value.
                __analyzer.genAssign(controlVar, initialVal);

                Entry variable = __symbolTableStack.Peek().GetEntry(controlVar.Lexeme);
                variable.Modifiable = false;

                // Generate the starting label
                __analyzer.genOut(start + ":");

                // Retrieve the semrecord for what type of for loop
                // (to or downto). Note: this will return either a 1 or -1
                // for adding later.
                SemRecord step = stepValue();

                // Retrieve finalValue semrec and generate the push the initial
                // (now controlVariable) and the finalValue on the stack for it.
                SemRecord finalVal = finalValue();

                Entry finalValEntry = __symbolTableStack.Peek().GetEntry(finalVal.Lexeme);

                if(finalValEntry != null) {
                    finalValEntry.Modifiable = false;
                }

                __analyzer.genPushVar(controlVar);

                // Generate either CMPGES or CMPLES depending on what was chosen
                // (either to or downto).
                if(step.Lexeme.Equals("1")) {
                    // to was chosen
                    __analyzer.genGte(controlVar, finalVal);
                } else if(step.Lexeme.Equals("-1")){
                    // downto was chosen
                    __analyzer.genLte(controlVar, finalVal);
                } else {
                    throw new Exception("Something went wrong with to or downto");
                }

                // Generate the branch false on stack
                __analyzer.genBrfs(end);

                match(TOKENS.DO);
                statement();

                // Generate for increment or decrement of controlVariable
                __analyzer.genPushLit(step);
                __analyzer.genPushVar(controlVar);
                SemRecord result = new SemRecord(__analyzer.genAdd(step, controlVar), "");
                variable.Modifiable = true;
                __analyzer.genAssign(controlVar, result);
                variable.Modifiable = false;

                // Generate branch to beginning of loop and end label
                __analyzer.genBr(start);
                variable.Modifiable = true;
                if (finalValEntry != null) {
                    finalValEntry.Modifiable = true;
                }
                __analyzer.genOut(end + ":");
                break;
            default:
                error(new List<TOKENS>{TOKENS.FOR});
                break;
        }
    }

    private SemRecord controlVariable() {
        SemRecord controlRec = null;
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(62 + " ");
                string id = variableIdentifier();
                Entry variable = __symbolTableStack.Peek().GetEntry(id);
                controlRec = new SemRecord(variable.Type, variable.Lexeme);
                break;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                break;
        }
        return controlRec;
    }

    private SemRecord initialValue() {
        SemRecord initialRec = null;
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
                initialRec = ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return initialRec;
    }

    private SemRecord stepValue() {
        SemRecord stepValRec = null;
        switch(__lookahead.Type) {
            case TOKENS.DOWNTO:
                Console.Write(65 + " ");
                match(TOKENS.DOWNTO);
                stepValRec = new SemRecord(TYPES.INTEGER, "-1");
                break;
            case TOKENS.TO:
                Console.Write(64 + " ");
                match(TOKENS.TO);
                stepValRec = new SemRecord(TYPES.INTEGER, "1");
                break;
            default:
                error(new List<TOKENS>{TOKENS.DOWNTO, TOKENS.TO});
                break;
        }
        return stepValRec;
    }

    private SemRecord finalValue() {
        SemRecord finalValRec = null;
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
                finalValRec = ordinalExpression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return finalValRec;
    }

    private void procedureStatement() {
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

    private void optionalActualParameterList() {
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

    private void actualParameterTail() {
        switch(__lookahead.Type) {
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

    private void actualParameter() {
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

    private SemRecord expression() {
        SemRecord expRec = null;
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
                Console.Write(73 + " ");
                expRec = simpleExpression();
                expRec = optionalRelationalPart(expRec);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return expRec;
    }

    private SemRecord optionalRelationalPart(SemRecord left) {
        SemRecord relRec = left;
        switch(__lookahead.Type) {
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
                Func<SemRecord, SemRecord, TYPES> relOp = relationalOperator();
                SemRecord right = simpleExpression();
                relRec = new SemRecord(relOp(left, right), "");
                break;
            default:
                error(new List<TOKENS>{TOKENS.DO, TOKENS.DOWNTO, TOKENS.END, TOKENS.THEN,
                    TOKENS.TO, TOKENS.COMMA, TOKENS.RPAREN, TOKENS.SCOLON, TOKENS.EQUAL,
                    TOKENS.GEQUAL, TOKENS.GTHAN, TOKENS.LEQUAL, TOKENS.LTHAN, TOKENS.NEQUAL});
                break;
        }
        return relRec;
    }

    private Func<SemRecord, SemRecord, TYPES> relationalOperator() {
        Func<SemRecord, SemRecord, TYPES> function = null;
        switch(__lookahead.Type) {
            case TOKENS.EQUAL:
                Console.Write(76 + " ");
                match(TOKENS.EQUAL);
                function = __analyzer.genEq;
                return function;
            case TOKENS.LTHAN:
                Console.Write(77 + " ");
                match(TOKENS.LTHAN);
                function = __analyzer.genLt;
                return function;
            case TOKENS.GTHAN:
                Console.Write(78 + " ");
                match(TOKENS.GTHAN);
                function = __analyzer.genGt;
                return function;
            case TOKENS.LEQUAL:
                Console.Write(79 + " ");
                match(TOKENS.LEQUAL);
                function = __analyzer.genLte;
                return function;
            case TOKENS.GEQUAL:
                Console.Write(80 + " ");
                match(TOKENS.GEQUAL);
                function = __analyzer.genGte;
                return function;
            case TOKENS.NEQUAL:
                Console.Write(81 + " ");
                match(TOKENS.NEQUAL);
                function = __analyzer.genNeq;
                return function;
            default:
                error(new List<TOKENS>{TOKENS.EQUAL, TOKENS.LTHAN, TOKENS.GTHAN, TOKENS.LEQUAL,
                    TOKENS.GEQUAL, TOKENS.NEQUAL});
                break;
        }
        return function;
    }

    private SemRecord simpleExpression() {
        SemRecord expRec = null;
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
                Console.Write(82 + " ");
                bool minus = optionalSign();
                expRec = term();
                if(minus) {
                    __analyzer.genNeg();
                }
                expRec = termTail(expRec);
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return expRec;
    }

    private SemRecord termTail(SemRecord left) {
        switch(__lookahead.Type) {
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
                Func<SemRecord, SemRecord, TYPES> addOp = addingOperator();
                SemRecord right = term();
                SemRecord addRec = new SemRecord(addOp(left, right), "");
                return termTail(addRec);
            default:
                error(new List<TOKENS>{TOKENS.DO, TOKENS.DOWNTO, TOKENS.END, TOKENS.THEN, TOKENS.TO,
                    TOKENS.COMMA, TOKENS.EQUAL, TOKENS.GEQUAL, TOKENS.GTHAN, TOKENS.LEQUAL,
                    TOKENS.LTHAN, TOKENS.NEQUAL, TOKENS.RPAREN, TOKENS.SCOLON, TOKENS.OR,
                    TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return left;
    }

    // Return true if optional sign exists and is minus
    // Otherwise, we don't have to do anything
    private bool optionalSign() {
        switch(__lookahead.Type) {
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
                return true;
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
        return false;
    }

    private Func<SemRecord, SemRecord, TYPES> addingOperator() {
        Func<SemRecord, SemRecord, TYPES> function = null;
        switch(__lookahead.Type) {
            case TOKENS.OR:
                Console.Write(90 + " ");
                match(TOKENS.OR);
                function = __analyzer.genOr;
                return function;
            case TOKENS.MINUS:
                Console.Write(89 + " ");
                match(TOKENS.MINUS);
                function = __analyzer.genSub;
                return function;
            case TOKENS.PLUS:
                Console.Write(88 + " ");
                match(TOKENS.PLUS);
                function = __analyzer.genAdd;
                return function;
            default:
                error(new List<TOKENS>{TOKENS.OR, TOKENS.MINUS, TOKENS.PLUS});
                break;
        }
        return function;
    }

    private SemRecord term() {
        SemRecord termRec = null;
        switch(__lookahead.Type) {
            case TOKENS.FALSE:
            case TOKENS.NOT:
            case TOKENS.TRUE:
            case TOKENS.IDENTIFIER:
            case TOKENS.INTEGER_LIT:
            case TOKENS.FLOAT_LIT:
            case TOKENS.STRING_LIT:
            case TOKENS.LPAREN:
                Console.Write(91 + " ");
                termRec = factor();
                SemRecord tailRec = factorTail(termRec);
                return tailRec;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
        return termRec;
    }

    private SemRecord factorTail(SemRecord left) {
        switch(__lookahead.Type) {
            case TOKENS.AND:
            case TOKENS.DIV:
            case TOKENS.MOD:
            case TOKENS.FLOAT_DIVIDE:
            case TOKENS.TIMES:
                Console.Write(92 + " ");
                Func<SemRecord, SemRecord, TYPES> mulOp = multiplyingOperator();
                SemRecord right = factor();
                Console.WriteLine(right.Lexeme + " of Type " + right.Type);
                SemRecord mulRec = new SemRecord(mulOp(left, right), "");
                SemRecord tailRec = factorTail(mulRec);
                return tailRec;
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
        return left;
    }

    private Func<SemRecord, SemRecord, TYPES> multiplyingOperator() {
        Func<SemRecord, SemRecord, TYPES> function = null;
        switch(__lookahead.Type) {
            case TOKENS.AND:
                Console.Write(98 + " ");
                match(TOKENS.AND);
                function = __analyzer.genAnd;
                return function;
            case TOKENS.DIV:
                Console.Write(96 + " ");
                match(TOKENS.DIV);
                function = __analyzer.genIntDiv;
                return function;
            case TOKENS.MOD:
                Console.Write(97 + " ");
                match(TOKENS.MOD);
                function = __analyzer.genMod;
                return function;
            case TOKENS.FLOAT_DIVIDE:
                Console.Write(95 + " ");
                match(TOKENS.FLOAT_DIVIDE);
                function = __analyzer.genDiv;
                return function;
            case TOKENS.TIMES:
                Console.Write(94 + " ");
                match(TOKENS.TIMES);
                function = __analyzer.genMul;
                return function;
            default:
                error(new List<TOKENS>{TOKENS.AND, TOKENS.DIV, TOKENS.MOD, TOKENS.FLOAT_DIVIDE,
                    TOKENS.TIMES});
                break;
        }
        return function;
    }

    // At the end of factor, whatever the factor is should be at the top of the stack
    // But we need to return the SemRecord for type knowledge and checking
    private SemRecord factor() {
        SemRecord factorRec = null;
        switch(__lookahead.Type) {
            case TOKENS.FALSE:
                Console.Write(103 + " ");
                match(TOKENS.FALSE);
                factorRec = new SemRecord(TYPES.BOOLEAN, "0");
                __analyzer.genPushLit(factorRec);
                return factorRec;
            case TOKENS.NOT:
                Console.Write(104 + " ");
                match(TOKENS.NOT);
                factorRec = factor();
                __analyzer.genNot();
                return factorRec;
            case TOKENS.TRUE:
                Console.Write(102 + " ");
                match(TOKENS.TRUE);
                factorRec = new SemRecord(TYPES.BOOLEAN, "1");
                __analyzer.genPushLit(factorRec);
                return factorRec;
            case TOKENS.IDENTIFIER:
                String identifier = __lookahead.Lexeme;
                KINDS identifierKind = __symbolTableStack.Peek().GetKind(identifier);
                if(identifierKind == KINDS.VAR || identifierKind == KINDS.PARAMETER) {
                    Console.Write(116 + " ");
                    String factorId = variableIdentifier();
                    TYPES factorType = __symbolTableStack.Peek().GetType(factorId);
                    factorRec = new SemRecord(factorType, factorId);
                    __analyzer.genPushVar(factorRec);
                    return factorRec;
                } else if(identifierKind == KINDS.FUNCTION) {
                    Console.Write(106 + " ");
                    String funcId = functionIdentifier();
                    optionalActualParameterList();
                    TYPES funcType = __symbolTableStack.Peek().GetType(funcId);
                    return new SemRecord(funcType, funcId); // Just a placeholder until we implement functions
                } else {
                    throw new Exception("Expected variable or function identifier");
                }
            case TOKENS.INTEGER_LIT:
                Console.Write(99 + " ");
                factorRec = new SemRecord(TYPES.INTEGER, match(TOKENS.INTEGER_LIT));
                __analyzer.genPushLit(factorRec);
                return factorRec;
            case TOKENS.FLOAT_LIT:
                Console.Write(100 + " ");
                factorRec = new SemRecord(TYPES.FLOAT, match(TOKENS.FLOAT_LIT));
                __analyzer.genPushLit(factorRec);
                return factorRec;
            case TOKENS.STRING_LIT:
                Console.Write(101 + " ");
                factorRec = new SemRecord(TYPES.STRING, match(TOKENS.STRING_LIT));
                __analyzer.genPushLit(factorRec);
                return factorRec;
            case TOKENS.LPAREN:
                Console.Write(105 + " ");
                match(TOKENS.LPAREN);
                factorRec = expression();
                match(TOKENS.RPAREN);
                Console.WriteLine("factorRec is of type " + factorRec.Type);
                return factorRec;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
        return factorRec;
    }

    private String programIdentifier() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(107 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                return null;
        }
    }

    private String variableIdentifier() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(108 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                return null;
        }
    }

    private String procedureIdentifier() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(109 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                return null;
        }
    }

    private String functionIdentifier() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(110 + " ");
                return match(TOKENS.IDENTIFIER);
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                return null;
        }
    }

    private void booleanExpression() {
        switch(__lookahead.Type) {
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

    private SemRecord ordinalExpression() {
        SemRecord ordExpRec = null;
        switch(__lookahead.Type) {
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
                ordExpRec = expression();
                break;
            default:
                error(new List<TOKENS>{TOKENS.FALSE, TOKENS.NOT, TOKENS.TRUE, TOKENS.IDENTIFIER,
                    TOKENS.INTEGER_LIT, TOKENS.FLOAT_LIT, TOKENS.STRING_LIT,
                    TOKENS.LPAREN});
                break;
        }
        return ordExpRec;
    }

    private List<String> identifierList() {
        switch(__lookahead.Type) {
            case TOKENS.IDENTIFIER:
                Console.Write(113 + " ");
                List<String> idList = new List<String>();
                idList.Add(match(TOKENS.IDENTIFIER));
                List<String> idTailList = identifierTail();
                if(idTailList != null) {
                    idList.AddRange(idTailList);
                }
                return idList;
            default:
                error(new List<TOKENS>{TOKENS.IDENTIFIER});
                return null;
        }
    }

    private List<String> identifierTail() {
        switch(__lookahead.Type) {
            case TOKENS.COLON:
                Console.Write(115 + " ");
                break;
            case TOKENS.COMMA:
                Console.Write(114 + " ");
                List<String> idList = new List<String>();
                match(TOKENS.COMMA);
                idList.Add(match(TOKENS.IDENTIFIER));
                List<String> idTailList = identifierTail();
                if(idTailList != null) {
                    idList.AddRange(idTailList);
                }
                return idList;
            default:
                error(new List<TOKENS>{TOKENS.COLON, TOKENS.COMMA});
                break;
        }
        return null;
    }
}
