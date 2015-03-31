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
 *  SymbolTable : SymbolTable object to contain Entries. In addition to the information stored
 *              : the Entry object. SymbolTable will also contain information such as a name,
 *              : nesting level, and a label.
 */
public class SymbolTable {
    public string Name {get; private set;}
    public int NestingLevel {get; private set;}
    public int Label {get; private set;}
    public int Size {get; private set;}
    public List<Entry> Entries {get; private set;}

    public SymbolTable(string name, int nestingLevel, int size, int label, List<Entry> entries) {
        Name         = name;
        NestingLevel = nestingLevel;
        Size         = size;
        Label        = label;
        Entries      = entries;
    }

    public void AddEntry(String lexeme, TYPES type, KINDS kind, int size, List<String> paras) {
        if(kind == KINDS.VAR){
            Entries.Add(new Entry(lexeme, type, kind, size, this.Size, paras));
            this.Size += size;
        } else { //nonvars don't have size
            Entries.Add(new Entry(lexeme, type, kind, size, 0, paras));
        }
    }

    public int GetSize(){
        int totalSize = 0;
        foreach(Entry entry in Entries){
            totalSize += entry.Size;
        }
        return totalSize;
    }

    // These will need to be recursive descent to find
    // other symbol table entries when we do functions/procs
    public TYPES GetType(String identifier){
        foreach(Entry entry in Entries){
            if(entry.Lexeme == identifier) return entry.Type;
        }
        return TYPES.NONE;
    }

    public KINDS GetKind(String identifier){
        foreach(Entry entry in Entries){
            if(entry.Lexeme == identifier) return entry.Kind;
        }
        return KINDS.NONE;
    }


    public Entry GetEntry(String identifier){
        foreach(Entry entry in Entries){
            if(entry.Lexeme == identifier) return entry;
        }
        return null;
    }
}
