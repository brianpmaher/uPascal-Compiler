/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;
using System.Collections.Generic;

/*
 *  Entry : Entry object to contain a single entry into the SymbolTable. This entry will represent
 *        : an identifier, a procedure, or a function. It will contain information about the lexeme,
 *        : token type, token kind, size, and a list of parameters.
 */
public class Entry {
    public string Lexeme {get; private set;}
    public TYPES Type {get; private set;}
    public KINDS Kind {get; private set;}
    public int Size {get; private set;}
    public int Offset {get; set;}
    public List<Parameter> Parameters {get; private set;}
    public bool Modifiable {get; set;}

    public Entry(string lexeme, TYPES type, KINDS kind, int size, int offset, List<Parameter> parameters) {
        Lexeme     = lexeme;
        Type       = type;
        Kind       = kind;
        Size       = size;
        Offset     = offset;
        Parameters = parameters;
        Modifiable = true;
    }

    public ParamType(string lexeme){
        foreach(Parameter para in Parameters){
            if(lexeme == para.Lexeme){
                return para.InOut;
            }
        }
        return null; // Parameter not found
    }
}

public class Parameter {
    public string Lexeme {get; private set;}
    public boolean InOut {get; private set;}

    public Parameter(string lexeme, inOut=True){
        Lexeme = lexeme;
        InOut  = inOut;
    }
}
