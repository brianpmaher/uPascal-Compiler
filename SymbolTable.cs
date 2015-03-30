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
    public List<Entry> Entries {get; private set;}

    public SymbolTable(string name, int nestingLevel, int label, List<Entry> entries) {
        Name         = name;
        NestingLevel = nestingLevel;
        Label        = label;
        Entries      = entries;
    }

    void addEntry(Entry entry) {
        Entries.Add(entry);
    }

    int getSize(){
        int totalSize = 0;
        foreach(Entry entry in Entries){
            totalSize += entry.Size;
        }
        return totalSize;
    }

    KINDS getKind(String identifier){
        for(Entry entry in Entries){
            if(entry.Lexeme == identifier) return entry.Kind;
        }
    }
}
