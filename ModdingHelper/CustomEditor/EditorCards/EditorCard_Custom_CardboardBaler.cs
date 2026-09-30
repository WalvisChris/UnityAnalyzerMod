using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_CardboardBaler : EditorCardBase
    {
        private TextMeshProUGUI numberOfBoxesInsideText;
        private TextMeshProUGUI isBrokenText;
        private TextMeshProUGUI brokenDayText;

        public void Initialize(TextMeshProUGUI titleText,
            TextMeshProUGUI numberOfBoxesInsideText,
            TextMeshProUGUI isBrokenText,
            TextMeshProUGUI brokenDayText)
        {
            this.titleText = titleText;
            this.numberOfBoxesInsideText = numberOfBoxesInsideText;
            this.isBrokenText = isBrokenText;
            this.brokenDayText = brokenDayText;
        }

        public void SetNumberOfBoxesInside(int i) => UpdateStringText(numberOfBoxesInsideText, i.ToString());
        public void SetIsBroken(bool b) => UpdateBoolText(isBrokenText, b);
        public void SetBrokenDay(int i) => UpdateStringText(brokenDayText, i.ToString());

        public void SetValues(int numberOfBoxesInside, bool isBroken, int brokenDay)
        {
            SetNumberOfBoxesInside(numberOfBoxesInside);
            SetIsBroken(isBroken);
            SetBrokenDay(brokenDay);
        }

        public static EditorCard_Custom_CardboardBaler Create(Transform contentParent,
            string title,
            int numberOfBoxesInside,
            bool isBroken,
            int brokenDay)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_CardboardBaler", title);

            TextMeshProUGUI numberOfBoxesInsideTMP = EditorCardBuilder.CreateValueRow(body, "Number Of Boxes Inside");
            TextMeshProUGUI isBrokenTMP = EditorCardBuilder.CreateBoolRow(body, "Is Broken");
            TextMeshProUGUI brokenDayTMP = EditorCardBuilder.CreateValueRow(body, "Broken Day");

            EditorCard_Custom_CardboardBaler card = cardObj.AddComponent<EditorCard_Custom_CardboardBaler>();
            card.Initialize(titleTMP, numberOfBoxesInsideTMP, isBrokenTMP, brokenDayTMP);
            card.SetValues(numberOfBoxesInside, isBroken, brokenDay);

            return card;
        }
    }
}