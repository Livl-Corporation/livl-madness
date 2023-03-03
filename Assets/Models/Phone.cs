using System.Collections;
using System.Collections.Generic;

public static class Phone
{

    public enum Screen
    {
        ProductList,
        Pause,
    }

    public static Dictionary<Screen, string> ScreenTitles = new Dictionary<Screen, string>
    {
        { Screen.ProductList, "Shopping List" },
        { Screen.Pause, "Paused" },
    };
}