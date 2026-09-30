using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_SingleValue : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI valueText;
        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI valueText)
        {
            this.titleText = titleText;
            this.valueText = valueText;
        }

        public void SetValue(string s) { if (valueText != null) valueText.text = s; }

        public static EditorCard_SingleValue Create(Transform contentParent, string label, string value, int fieldWidth = 170)
        {
            GameObject cardObj = new GameObject($"Card_SingleValue_{label}");
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

            // START: COPY OF EditorCardBuilder.CreateValueRow
            GameObject rowObj = new GameObject($"Row_Value_{label}");
            rowObj.transform.SetParent(cardObj.transform, false);

            HorizontalLayoutGroup rowLayout = rowObj.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 4;
            rowLayout.childControlWidth = true;
            rowLayout.childForceExpandWidth = false;

            GameObject labelObj = new GameObject($"Text_Label_{label}");
            labelObj.transform.SetParent(rowObj.transform, false);

            TextMeshProUGUI labelTMP = labelObj.AddComponent<TextMeshProUGUI>();
            labelTMP.text = label;
            labelTMP.fontSize = 15; // Exception
            labelTMP.color = ColorThemes.cardTitleTextColor; // Exception
            labelTMP.alignment = TextAlignmentOptions.Left;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.flexibleWidth = 1;

            GameObject fieldObj = new GameObject($"Field_{label}");
            fieldObj.transform.SetParent(rowObj.transform, false);

            Image fieldBg = fieldObj.AddComponent<Image>();
            fieldBg.color = ColorThemes.panelColor;

            LayoutElement fieldLayout = fieldObj.AddComponent<LayoutElement>();
            fieldLayout.preferredWidth = fieldWidth;
            fieldLayout.flexibleWidth = 0;
            fieldLayout.preferredHeight = 20;

            GameObject textObj = new GameObject($"Text_{label}");
            textObj.transform.SetParent(fieldObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI valueTMP = textObj.AddComponent<TextMeshProUGUI>();
            valueTMP.fontSize = 12;
            valueTMP.color = ColorThemes.cardTitleTextColor;
            valueTMP.alignment = TextAlignmentOptions.Left;
            valueTMP.margin = new Vector4(4, 0, 0, 0);
            // END: COPY OF EditorCardBuilder.CreateValueRow

            EditorCard_SingleValue card = cardObj.AddComponent<EditorCard_SingleValue>();
            card.Initialize(labelTMP, valueTMP);
            card.SetValue(value);

            return card;
        }
    }
}
