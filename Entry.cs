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
    public string Label {get; private set;}
    public TYPES Type {get; private set;}
    public KINDS Kind {get; private set;}
    public int Size {get; private set;}
    public int Offset {get; set;}
    public List<Parameter> Parameters {get; private set;}
    public bool Modifiable {get; set;}
    public bool VarParameter {get; private set;}

    public Entry(string lexeme, TYPES type, KINDS kind, int size, int offset, List<Parameter> parameters, bool varparameter = false) {
        Lexeme     = lexeme;
        Type       = type;
        Kind       = kind;
        Size       = size;
        Offset     = offset;
        Parameters = parameters;
        Modifiable = true;
        VarParameter = varparameter;
    }

    public Entry(string lexeme, string label, TYPES type, KINDS kind, int size, int offset, List<Parameter> parameters, bool varparameter = false) {
        Lexeme     = lexeme;
        Label      = label;
        Type       = type;
        Kind       = kind;
        Size       = size;
        Offset     = offset;
        Parameters = parameters;
        Modifiable = true;
        VarParameter = varparameter;
    }
}
