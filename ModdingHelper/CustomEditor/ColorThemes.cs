using UnityEngine;

namespace ModdingHelper.CustomEditor
{
    internal class ColorThemes
    {
        internal static Color panelColor = new Color(0.078f, 0.078f, 0.078f); // Color: Panel, RGB(255): 20, 20, 20 -> 0.078f
        internal static Color transparent = new Color(0, 0, 0, 0); // Color: Transparent
        internal static Color buttonColor = new Color(0.317f, 0.317f, 0.317f); // Color: Button, RGB(255): 81, 81, 81 -> 0.317f
        internal static Color buttonTextColor = Color.white; // Color: Button Text, White
        internal static Color viewportColor = new Color(0.05f, 0.05f, 0.05f, 0.5f); // Color: Viewport?
        internal static Color cardBackgroundColor = new Color(0.243f, 0.243f, 0.243f); // Color: Card Background, RGB(255): 62, 62, 62 -> 0.243f
        internal static Color cardTitleTextColor = Color.white; // Color: Card Title Text, White
        internal static Color cardBodyTextColor = new Color(0.768f, 0.768f, 0.768f); // Color: Card Body Text, RGB(255): 196, 196, 196 -> 0.768f
        internal static Color inactiveGameObjectTextColor = new Color(0.5f, 0.5f, 0.5f); // Color: Inactive Game Object Text, Gray
    }
}
