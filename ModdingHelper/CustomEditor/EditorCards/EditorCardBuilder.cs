using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal static class EditorCardBuilder
    {
        public static (GameObject cardObj, Transform bodyContainer, TextMeshProUGUI titleTMP) CreateBaseCard(
            Transform contentParent, string cardName, string title)
        {
            // Root Card
            GameObject cardObj = new GameObject(cardName);
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
            titleTMP.text = title;
            titleTMP.fontSize = 15;
            titleTMP.alignment = TextAlignmentOptions.Left;
            titleTMP.color = ColorThemes.cardTitleTextColor;

            // Body
            GameObject bodyObj = new GameObject("Body_VerticalLayout");
            bodyObj.transform.SetParent(cardObj.transform, false);

            VerticalLayoutGroup bodyLayout = bodyObj.AddComponent<VerticalLayoutGroup>();
            bodyLayout.spacing = 2;
            bodyLayout.childControlWidth = true;

            return (cardObj, bodyObj.transform, titleTMP);
        }

        public static TextMeshProUGUI CreateValueRow(Transform parent, string labelName)
        {
            GameObject rowObj = new GameObject($"Row_Value_{labelName}");
            rowObj.transform.SetParent(parent, false);

            HorizontalLayoutGroup rowLayout = rowObj.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 4;
            rowLayout.childControlWidth = true;
            rowLayout.childForceExpandWidth = false;

            GameObject labelObj = new GameObject($"Text_Label_{labelName}");
            labelObj.transform.SetParent(rowObj.transform, false);

            TextMeshProUGUI labelTMP = labelObj.AddComponent<TextMeshProUGUI>();
            labelTMP.text = labelName;
            labelTMP.fontSize = 13;
            labelTMP.color = ColorThemes.cardBodyTextColor;
            labelTMP.alignment = TextAlignmentOptions.Left;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.flexibleWidth = 1;

            GameObject fieldObj = new GameObject($"Field_{labelName}");
            fieldObj.transform.SetParent(rowObj.transform, false);

            Image fieldBg = fieldObj.AddComponent<Image>();
            fieldBg.color = ColorThemes.panelColor;

            LayoutElement fieldLayout = fieldObj.AddComponent<LayoutElement>();
            fieldLayout.preferredWidth = 170; // CHANGED
            fieldLayout.flexibleWidth = 0;
            fieldLayout.preferredHeight = 20;

            GameObject textObj = new GameObject($"Text_{labelName}");
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

            return valueTMP;
        }

        public static TextMeshProUGUI CreateBoolRow(Transform parent, string labelName)
        {
            GameObject rowObj = new GameObject($"Row_Bool_{labelName}");
            rowObj.transform.SetParent(parent, false);

            HorizontalLayoutGroup rowLayout = rowObj.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 4;
            rowLayout.childControlWidth = true;
            rowLayout.childForceExpandWidth = false;

            GameObject labelObj = new GameObject($"Text_Label_{labelName}");
            labelObj.transform.SetParent(rowObj.transform, false);

            TextMeshProUGUI labelTMP = labelObj.AddComponent<TextMeshProUGUI>();
            labelTMP.text = labelName;
            labelTMP.fontSize = 13;
            labelTMP.color = ColorThemes.cardBodyTextColor;
            labelTMP.alignment = TextAlignmentOptions.Left;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.flexibleWidth = 1;

            GameObject boxObj = new GameObject($"Box_{labelName}");
            boxObj.transform.SetParent(rowObj.transform, false);

            Image boxBg = boxObj.AddComponent<Image>();
            boxBg.color = ColorThemes.panelColor;

            LayoutElement boxLayout = boxObj.AddComponent<LayoutElement>();
            boxLayout.preferredWidth = 20;
            boxLayout.preferredHeight = 20;

            GameObject textObj = new GameObject("Text_X");
            textObj.transform.SetParent(boxObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI xTMP = textObj.AddComponent<TextMeshProUGUI>();
            xTMP.fontSize = 12;
            xTMP.fontStyle = FontStyles.Bold;
            xTMP.color = ColorThemes.cardTitleTextColor;
            xTMP.alignment = TextAlignmentOptions.Center;

            return xTMP;
        }

        public static TextMeshProUGUI[] CreateVectorRow(Transform parent, string labelName)
        {
            GameObject rowObj = new GameObject($"Row_{labelName}");
            rowObj.transform.SetParent(parent, false);

            HorizontalLayoutGroup rowLayout = rowObj.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 4;
            rowLayout.childControlWidth = true;
            rowLayout.childForceExpandWidth = false;

            GameObject labelObj = new GameObject($"Text_Label_{labelName}");
            labelObj.transform.SetParent(rowObj.transform, false);

            TextMeshProUGUI labelTMP = labelObj.AddComponent<TextMeshProUGUI>();
            labelTMP.text = labelName;
            labelTMP.fontSize = 13;
            labelTMP.color = ColorThemes.cardBodyTextColor;
            labelTMP.alignment = TextAlignmentOptions.Left;

            LayoutElement labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.preferredWidth = 65;

            TextMeshProUGUI[] valueTexts = new TextMeshProUGUI[3];
            string[] axisLabels = { "X", "Y", "Z" };

            for (int i = 0; i < 3; i++)
            {
                GameObject axisGroupObj = new GameObject($"Group_{axisLabels[i]}");
                axisGroupObj.transform.SetParent(rowObj.transform, false);

                HorizontalLayoutGroup axisGroupLayout = axisGroupObj.AddComponent<HorizontalLayoutGroup>();
                axisGroupLayout.spacing = 3;
                axisGroupLayout.childControlWidth = true;
                axisGroupLayout.childForceExpandWidth = false;

                LayoutElement axisGroupElement = axisGroupObj.AddComponent<LayoutElement>();
                axisGroupElement.flexibleWidth = 1;

                GameObject axisLabelObj = new GameObject($"Label_{axisLabels[i]}");
                axisLabelObj.transform.SetParent(axisGroupObj.transform, false);

                TextMeshProUGUI axisLabelTMP = axisLabelObj.AddComponent<TextMeshProUGUI>();
                axisLabelTMP.text = axisLabels[i];
                axisLabelTMP.fontSize = 12;
                axisLabelTMP.color = ColorThemes.cardBodyTextColor;
                axisLabelTMP.alignment = TextAlignmentOptions.Center;

                LayoutElement axisLabelLayout = axisLabelObj.AddComponent<LayoutElement>();
                axisLabelLayout.preferredWidth = 12;

                GameObject fieldObj = new GameObject($"Field_{axisLabels[i]}");
                fieldObj.transform.SetParent(axisGroupObj.transform, false);

                Image fieldBg = fieldObj.AddComponent<Image>();
                fieldBg.color = ColorThemes.panelColor;

                LayoutElement fieldLayout = fieldObj.AddComponent<LayoutElement>();
                fieldLayout.flexibleWidth = 1;
                fieldLayout.preferredHeight = 20;

                GameObject textObj = new GameObject($"Text_{axisLabels[i]}");
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

                valueTexts[i] = valueTMP;
            }

            return valueTexts;
        }
    }
}