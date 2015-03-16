/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;

/*
 *  SymbolTable : SymbolTable object to contain Entries. In addition to the information stored
 *              : the Entry object. SymbolTable will also contain information such as a name,
 *              : nesting level, and a label.
 */
public class SymbolTable {
    public string Name {get; private set};
    public int NestingLevel {get; private set};
    public int Label {get; private set};
    List<Entry> Entries {get; private set};

    SymbolTable(string name, int nestingLevel, int label, List<Entry> entries) {
        Name         = name;
        NestingLevel = nestingLevel;
        Label        = label;
        Entries      = entries;
    }

    addEntry(Entry entry) {
        Entries.add(entry);
    }
}
