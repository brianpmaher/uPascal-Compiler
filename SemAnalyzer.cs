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
        // If the file exists, remove all existing code
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
            throw new Exception("You can't not a non-boolean");
        }
    }

    public void genNeg(){
        if(topStackType == TYPES.INTEGER){
            output("PUSH #-1", "MULS");
        } else if(topStackType == TYPES.FLOAT){
            output("PUSH #-1.0", "MULSF");
        } else {
            throw new Exception("You can't negate a non-numeric with '-'");
        }
    }

    /**
    Multiplying Operators
    */
    public TYPES genAnd(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.BOOLEAN){
            output("ANDS");
        } else {
            throw new Exception("Can't AND non-booleans, sucka");
        }
        topStackType = TYPES.BOOLEAN;
        return TYPES.BOOLEAN;
    }

    public TYPES genMul(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("MULS");
            topStackType = TYPES.INTEGER;
            return TYPES.INTEGER;
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("MULSF");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "MULSF");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("CASTSF", "MULSF");
        } else{
            throw new Exception("Can't multiply non-numbers");
        }
        topStackType = TYPES.FLOAT;
        return TYPES.FLOAT;
    }

    public TYPES genIntDiv(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("DIVS");
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("DIVSF", "CASTSI");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "DIVSF", "CASTSI");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("CASTSF", "DIVSF", "CASTSI");
        } else{
            throw new Exception("Can't divide non-numbers, what were you thinking?");
        }
        topStackType = TYPES.INTEGER;
        return TYPES.INTEGER;
    }

    public TYPES genDiv(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "CASTSF", "DIVSF");
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("DIVSF");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("CASTSF", "DIVSF");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "DIVSF");
        } else {
            throw new Exception("Divide by 0! Just kidding. Although maybe we should check for that");
        }
        topStackType = TYPES.FLOAT;
        return TYPES.FLOAT;
    }

    public TYPES genMod(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("MODS");
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("CASTSI", "SUB SP #1 SP", "CASTSI", "ADD SP #1 SP", "MODS");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("CASTSI", "MODS");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("SUB SP #1 SP", "CASTSI", "ADD SP #1 SP");
        } else{
            throw new Exception("Can't modulus non-numeric types");
        }
        topStackType = TYPES.INTEGER;
        return TYPES.INTEGER;
    }

    /**
    Adding Operators
    */
    public TYPES genOr(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.BOOLEAN){
            output("ORS");
        } else {
            throw new Exception("Can't OR non-booleans");
        }
        topStackType = TYPES.BOOLEAN;
        return TYPES.BOOLEAN;
    }

    public TYPES genSub(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("SUBS");
            topStackType = TYPES.INTEGER;
            return TYPES.INTEGER;
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("SUBSF");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "SUBSF");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("CASTSF", "SUBSF");
        } else {
            throw new Exception("Cannot subtract non-numeric types");
        }
        topStackType = TYPES.FLOAT;
        return TYPES.FLOAT;
    }

    public TYPES genAdd(SemRecord left, SemRecord right){
        if(left.Type == right.Type && left.Type == TYPES.INTEGER){
            output("ADDS");
            topStackType = TYPES.INTEGER;
            return TYPES.INTEGER;
        } else if(left.Type == right.Type && left.Type == TYPES.FLOAT){
            output("ADDSF");
        } else if(left.Type == TYPES.INTEGER && right.Type == TYPES.FLOAT){
            output("SUB SP #1 SP", "CASTSF", "ADD SP #1 SP", "ADDSF");
        } else if(left.Type == TYPES.FLOAT && right.Type == TYPES.INTEGER){
            output("CASTSF", "ADDSF");
        } else {
            throw new Exception("Cannot add non-numeric types");
        }
        topStackType = TYPES.FLOAT;
        return TYPES.FLOAT;
    }

    /**
    Relational Operators
    */


    /**
    Write and Read statements
    */
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
