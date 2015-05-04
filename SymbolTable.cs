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
    public SymbolTable Next {get; private set;}

    public SymbolTable(String name, int nestingLevel, int size,
                       int label, List<Entry> entries, SymbolTable next) {
        Name         = name;
        NestingLevel = nestingLevel;
        Size         = size;
        Label        = label;
        Entries      = entries;
        Next         = next;
    }

    public void AddEntry(Entry entry){
        if(entry != null){
            if(entry.Kind == KINDS.VAR || entry.Kind == KINDS.PARAMETER){
                entry.Offset = this.Size + 1;
                Entries.Add(entry);
                this.Size += entry.Size;
            } else {
                Entries.Add(entry);
            }
        }
    }

    public void AddEntry(String lexeme, TYPES type, KINDS kind, int size, List<Parameter> paras) {
        if(kind == KINDS.VAR || kind == KINDS.PARAMETER){
            Entries.Add(new Entry(lexeme, type, kind, size, this.Size + 1, paras));
            this.Size += size;
        } else { //nonvars don't have size
            Entries.Add(new Entry(lexeme, type, kind, size, 0, paras));
        }
    }

    public void AddEntry(String lexeme, string label, TYPES type, KINDS kind, int size, List<Parameter> paras) {
        if(kind == KINDS.VAR || kind == KINDS.PARAMETER){
            Entries.Add(new Entry(lexeme, label, type, kind, size, this.Size + 1, paras));
            this.Size += size;
        } else { //nonvars don't have size
            Entries.Add(new Entry(lexeme, label, type, kind, size, 0, paras));
        }
    }

    public void RemoveEntry(Entry entryRemoving){
        if(!Entries.Remove(entryRemoving)){
            throw new Exception("Tried to remove an entry for the symbol table that was not in the symbol table");
        }
    }

    public int GetSize(){
        int totalSize = 0;
        foreach(Entry entry in Entries){
            totalSize += entry.Size;
        }
        return totalSize;
    }

    // This method should be called after adding all parameter entries to the symbol table
    // This makes room for the PC in the symbol table, to avoid incorrect offsets in
    // the vars
    public void IncSize(){
        Size++;
    }

    public void DecSize(){
        Size--;
    }

    public TYPES GetType(String identifier){
        Entry entry = GetEntry(identifier);
        if(entry != null){
            return entry.Type;
        } else {
            return TYPES.NONE;
        }
    }

    public KINDS GetKind(String identifier){
        Entry entry = GetEntry(identifier);
        if(entry != null){
            return entry.Kind;
        } else {
            return KINDS.NONE;
        }
    }

    public Entry GetEntry(String identifier){
        Entry result = null;
        foreach(Entry entry in Entries){
            if(entry.Lexeme == identifier){
                result = entry;
            }
        }
        if(result == null && Next != null){
            result = Next.GetEntry(identifier);
        }
        return result;
    }

    public int GetNestingLevel(string identifier){
        int nestingLevel = -1;
        foreach(Entry entry in Entries){
            if(entry.Lexeme == identifier){
                nestingLevel = NestingLevel;
            }
        }
        if(nestingLevel == -1 && Next != null){
            nestingLevel = Next.GetNestingLevel(identifier);
        }
        return nestingLevel;
    }

    public void printSymbolTable(){
        Console.WriteLine("--------------PRINTING SYMBOLTABLE-------------------");
        foreach(Entry entry in Entries){
            Console.WriteLine("Entry ---> Lexeme: " + entry.Lexeme + ", Type: " + entry.Type);
        }
        Console.WriteLine("-----------------------END---------------------------");
    }
}

public class Parameter {
    public bool VarType {get; set;}
    public TYPES Type {get; set;}

    public Parameter(bool vartype, TYPES type) {
        VarType = vartype;
        Type = type;
    }
}
