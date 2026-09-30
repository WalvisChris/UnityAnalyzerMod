using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_String : MonoBehaviour
    {
        private TextMeshProUGUI text;

        public void Initialize(TextMeshProUGUI text)
        {
            this.text = text;
        }

        public void SetText(string s) { if (text != null) text.text = s; }

        public static EditorCard_String Create(Transform contentParent, string s)
        {
            // Root Card
            GameObject cardObj = new GameObject("Card_String");
            cardObj.transform.SetParent(contentParent, false);

            Image bg = cardObj.AddComponent<Image>();
            bg.color = ColorThemes.cardBackgroundColor;

            VerticalLayoutGroup mainLayout = cardObj.AddComponent<VerticalLayoutGroup>();
            mainLayout.padding = new RectOffset(10, 10, 10, 10);
            mainLayout.spacing = 6;
            mainLayout.childControlWidth = true;

            LayoutElement layoutElem = cardObj.AddComponent<LayoutElement>();
            layoutElem.preferredWidth = 330;
            layoutElem.flexibleHeight = 0;

            // Title
            GameObject titleObj = new GameObject("Text_Title");
            titleObj.transform.SetParent(cardObj.transform, false);

            TextMeshProUGUI titleTMP = titleObj.AddComponent<TextMeshProUGUI>();
            titleTMP.text = s;
            titleTMP.fontSize = 15;
            titleTMP.alignment = TextAlignmentOptions.Left;
            titleTMP.color = ColorThemes.cardTitleTextColor;

            EditorCard_String card = cardObj.AddComponent<EditorCard_String>();
            card.Initialize(titleTMP);
            
            return card;
        }
    }
}
