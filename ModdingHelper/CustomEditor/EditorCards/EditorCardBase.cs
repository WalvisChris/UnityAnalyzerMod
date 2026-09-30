using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal abstract class EditorCardBase : MonoBehaviour
    {
        protected TextMeshProUGUI titleText;

        public virtual void SetTitle(string title)
        {
            if (titleText != null) titleText.text = title;
        }

        protected static void UpdateVectorTexts(TextMeshProUGUI[] texts, Vector3 vector)
        {
            if (texts == null || texts.Length < 3) return;
            if (texts[0] != null) texts[0].text = vector.x.ToString("F2");
            if (texts[1] != null) texts[1].text = vector.y.ToString("F2");
            if (texts[2] != null) texts[2].text = vector.z.ToString("F2");
        }

        protected static void UpdateBoolText(TextMeshProUGUI text, bool value)
        {
            if (text != null) text.text = value ? "X" : "";
        }

        protected static void UpdateStringText(TextMeshProUGUI text, string s)
        {
            if (text != null) text.text = s;
        }
    }
}