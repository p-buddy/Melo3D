using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPalette
{
    static ColorPalette()
    {
        GreenLight = TryGet("#55efc4");
        GreenDark = TryGet("#00b894");
        AquaLight = TryGet("#81ecec");
        AquaDark = TryGet("#00cec9");
        BlueLight = TryGet("#74b9ff");
        BlueDark = TryGet("#0984e3");
        PurpleLight = TryGet("#a29bfe");
        PurpleDark = TryGet("#6c5ce7");
        GreyLightest = TryGet("#dfe6e9");
        GreyLighter = TryGet("#b2bec3");
        GreyDarker = TryGet("#636e72");
        GreyDarkest = TryGet("#2d3436");
        YellowLight = TryGet("#ffeaa7");
        YellowDark = TryGet("#fdcb6e");
        OrangeLight = TryGet("#fab1a0");
        OrangeDark = TryGet("#e17055");
        RedLight = TryGet("#ff7675");
        RedDark = TryGet("#d63031");
        PinkLight = TryGet("#fd79a8");
        PinkDark = TryGet("#e84393");
    }
    
    public static Color GreenLight;
    public static Color GreenDark;
    public static Color AquaLight;
    public static Color AquaDark;
    public static Color BlueLight;
    public static Color BlueDark;
    public static Color PurpleLight;
    public static Color PurpleDark;
    public static Color GreyLightest;
    public static Color GreyLighter;
    public static Color GreyDarker;
    public static Color GreyDarkest;
    public static Color YellowLight;
    public static Color YellowDark;
    public static Color OrangeLight;
    public static Color OrangeDark;
    public static Color RedLight;
    public static Color RedDark;
    public static Color PinkLight;
    public static Color PinkDark;

    public static Color TryGet(string hexString)
    {
        if (ColorUtility.TryParseHtmlString(hexString, out Color toSet))
        {
            return toSet;
        }
        
        return Color.red;
    }
}
