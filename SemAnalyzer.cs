using System;
using System.Collections.Generic;
using System.IO;

public class SemAnalyzer{
    public Stack<SymbolTable> SymbolTableStack{get; private set;}
    public String File{get; private set;}
    public TYPES topStackType{get; private set;}

    public SemAnalyzer(Stack<SymbolTable> symbolTableStack, String progname){
        this.SymbolTableStack = symbolTableStack;
        this.File = progname + ".exe";
        using(StreamWriter writer = new StreamWriter(File)){

        }
    }

    // Initializes the register on the stack
    public void genInit(){
        SymbolTable top = SymbolTableStack.Peek();
        genLabel();
        output(
            "PUSH D" + top.NestingLevel,
            "MOV SP D" + top.NestingLevel
        );
    }

    public void genSymSize(){
        SymbolTable top = SymbolTableStack.Peek();
        output("ADD SP #" + top.Size + " SP");
    }

    //Pops the table off the stack
    public void genEnd(){
        SymbolTable top = SymbolTableStack.Peek();
        output(
            "SUB SP #" + top.Size + " SP",
            "POP D" + top.NestingLevel,
            "HLT"
        );
    }

    public void genPushVar(SemRecord toPush){
        SymbolTable top = SymbolTableStack.Peek();
        Entry idEntry = top.GetEntry(toPush.Lexeme);
        output("PUSH " + idEntry.Offset + "(D" + top.NestingLevel + ")");
        topStackType = toPush.Type;
    }

    public void genPushLit(SemRecord toPush){
        output("PUSH #" + toPush.Lexeme);
        topStackType = toPush.Type;
    }

    // Do we really need a label? Let's use the Name as the label
    public void genLabel(){
        String label = SymbolTableStack.Peek().Name;
        output("L" + label + ":");
    }

    public void genNot(){
        if(topStackType == TYPES.BOOLEAN){
            output("NOTS");
        } else {
            // TODO: Professionalize exception message
            throw new Exception("Oh shit, you tried to negate a non-boolean");
        }
    }

    public void genWrite(){
        output("WRTS");
    }

    public void genRead(String identifier){
        SymbolTable top = SymbolTableStack.Peek();
        Entry idEntry = top.GetEntry(identifier);
        if(idEntry.Kind != KINDS.VAR){
            throw new Exception("Tried to write to a non-variable");
        } else{
            String outputString = "RD";
            switch(idEntry.Type){
                case TYPES.INTEGER:
                    outputString += " ";
                    break;
                case TYPES.FLOAT:
                    outputString += "F ";
                    break;
                case TYPES.STRING:
                    outputString += "S ";
                    break;
                default:
                    throw new Exception("Tried to write to an invalid type");
            }
            output(outputString + idEntry.Offset + "(D" + top.NestingLevel + ")");
        }
    }

    public void genWriteLine(){
        output(
            "PUSH #\"\"",
            "WRTLNS"
        );
    }

    public void genAssign(SemRecord assignee, SemRecord expression){
        if(assignee.Type == expression.Type){} //Do nothing
        else if(assignee.Type == TYPES.INTEGER && expression.Type == TYPES.FLOAT){
            output("CASTSI");
        } else if(assignee.Type == TYPES.FLOAT && expression.Type == TYPES.INTEGER){
            output("CASTSF");
        } else{
            throw new Exception("Incompatible types found");
        }
        Entry assigneeSymRec = SymbolTableStack.Peek().GetEntry(assignee.Lexeme);
        output("POP " + assigneeSymRec.Offset + "(D" + SymbolTableStack.Peek().NestingLevel + ")");
    }

    private void output(params String[] outputStrings){
        using(StreamWriter writer = new StreamWriter(File, true)){
            foreach(String command in outputStrings){
                writer.WriteLine(command);
            }
        }
    }
}

public class SemRecord{
    public TYPES Type {get; private set;}
    public String Lexeme {get; private set;}

    public SemRecord(TYPES type, String lexeme){
        this.Type = type;
        this.Lexeme = lexeme;
    }
}
