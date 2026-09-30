using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Title : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        public void Initialize(TextMeshProUGUI titleText) { this.titleText = titleText; }
        public void SetTitle(string newTitle) { if (titleText != null) titleText.text = newTitle; }
        public void Show() { gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); }
        public static EditorCard_Title Create(Transform parent, string title)
        {
            // Base Object
            GameObject cardObj = new GameObject("Card_Title");
            cardObj.transform.SetParent(parent, false);

            // Background Color
            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            //ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            //fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layout = cardObj.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.childControlWidth = true;

            LayoutElement layoutElem = cardObj.AddComponent<LayoutElement>();
            layoutElem.preferredWidth = 330;
            layoutElem.flexibleHeight = 0;

            // Title Text
            GameObject textObj = new GameObject("Text_Title");
            textObj.transform.SetParent(cardObj.transform, false);

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = title;
            tmp.fontSize = 16;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Left;

            // Attach & Initialize Component
            EditorCard_Title card = cardObj.AddComponent<EditorCard_Title>();
            card.Initialize(tmp);

            return card;
        }
    }
}
