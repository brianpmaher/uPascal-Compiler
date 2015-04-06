using System;

public class LabelMaker{
    private static int label = 0;

    public static String genLabel(){
        label++;
        return "" + label;
    }
}
