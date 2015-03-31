using System.Collections.Generic;
using Systems.IO;

public class SemAnalyzer{
    public Stack<SymbolTable> SymbolTableStack{get; private set;}
    public StreamWriter File{get; private set;}
    public TYPES topStackType{get; private set;}

    SemAnalyzer(SymbolTable symbolTable, String progname){
        this.SymbolTableStack = symbolTableStack;
        this.StreamWriter = new StreamWriter(progname);
    }

    // Destructor for ensuring file writer object is destroyed
    ~SemAnalyzer(){
        this.File.Close();
        this.File.Dispose();
    }

    // Initializes the register on the stack
    public void genInit(){
        SymbolTable top = SymbolTableStack.Peek();
        genLabel();
        output(
            "PUSH D" + top.NestingLevel,
            "MOV SP D" + top.NestingLevel,
            "ADD SP #" + top.Size + " SP");
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
        output("PUSH " + toPush.Lexeme);
        topStackType = toPush.Type;
    }

    public void genPushLit(SemRecord toPush){
        output("PUSH #" + toPush.Lexeme);
        topStackType = toPush.Type;
    }

    public void genLabel(){
        String label = SymbolTableStack.Peek().Label;
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

    public void genWriteLine(){
        output(
            "PUSH #\"\"",
            "WRTLNS"
        );
    }

    public void genAssign(SemRecord assignee, SemRecord expression){
        if(assignee.Type == expression.Type); //Do nothing
        else if(assignee.Type == TYPES.INTEGER && expression.Type == TYPES.FLOAT){
            output("CASTSI");
        } else if(assigneee.Type == TYPES.FLOAT && expression.Type == TYPES.INTEGER){
            output("CASTSF");
        } else{
            throw new Exception("Incompatible types found");
        }
        Entry assigneeSymRec = symbolStack.GetEntry(assignee.Lexeme);
        output("POP " + symbolTableStack.Peek().NestingLevel + "(D" + assigneeSymRec.Offset + ")");
    }

    private void output(params String[] outputStrings){
        for(String command in outputStrings){
            this.File.WriteLine(command);
        }
    }
}

public class SemRecord{
    public TYPES Type {get; private set;}
    public String Lexeme {get; private set;}

    SemRecord(TYPES type, String lexeme){
        this.Type = type;
        this.Lexeme = lexeme;
    }
}
