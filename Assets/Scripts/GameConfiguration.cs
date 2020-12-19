using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfiguration
{
    public static bool fixedBackground = false;
    public static List<Color> colorsList = new List<Color>() 
    {
        new Color32(53, 90, 53, 255),
        new Color32(0, 150, 88, 255),
        new Color32(0, 102, 255, 255),
        new Color32(74, 134, 232, 255),
        new Color32(171, 61, 244, 255),
        new Color32(89, 70, 164, 255)
    };
    public static bool soundOn = true;
}
