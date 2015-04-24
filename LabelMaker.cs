/*
 *  CSCI 468
 *  Group 2
 *  Jesse Brown
 *  Brian Maher
 *  Sean Rogers
 */

using System;

public class LabelMaker{
    private static int label = 0;

    public static String genLabel(){
        string labelStr = "" + label;
        label++;
        return labelStr;
    }
}
