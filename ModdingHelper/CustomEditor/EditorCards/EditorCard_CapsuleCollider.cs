using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_CapsuleCollider : EditorCardBase
    {
        private TextMeshProUGUI triggerText;
        private TextMeshProUGUI contactsText;
        private TextMeshProUGUI[] centerTexts = new TextMeshProUGUI[3];
        private TextMeshProUGUI radiusText;
        private TextMeshProUGUI heightText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI triggerText,
            TextMeshProUGUI contactsText, TextMeshProUGUI[] centerTexts, TextMeshProUGUI radiusText, TextMeshProUGUI heightText)
        {
            this.titleText = titleText;
            this.triggerText = triggerText;
            this.contactsText = contactsText;
            this.centerTexts = centerTexts;
            this.radiusText = radiusText;
            this.heightText = heightText;
        }

        public void SetTrigger(bool b) => UpdateBoolText(triggerText, b);
        public void SetContacts(bool b) => UpdateBoolText(contactsText, b);
        public void SetCenter(Vector3 v) => UpdateVectorTexts(centerTexts, v);
        public void SetRadius(float f) => UpdateStringText(radiusText, f.ToString("F2"));
        public void SetHeight(float f) => UpdateStringText(heightText, f.ToString("F2"));
        public void SetValues(bool isTrigger, bool providesContacts, Vector3 center, float radius, float height)
        {
            SetTrigger(isTrigger);
            SetContacts(providesContacts);
            SetCenter(center);
            SetRadius(radius);
            SetHeight(height);
        }

        public static EditorCard_CapsuleCollider Create(Transform contentParent, string title,
            bool isTrigger, bool providesContacts, Vector3 center, float radius, float height)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_CapsuleCollider", title);

            TextMeshProUGUI triggerTMP = EditorCardBuilder.CreateBoolRow(body, "Is Trigger");
            TextMeshProUGUI contactsTMP = EditorCardBuilder.CreateBoolRow(body, "Provides Contacts");
            TextMeshProUGUI[] centerTMP = EditorCardBuilder.CreateVectorRow(body, "Center");
            TextMeshProUGUI radiusTMP = EditorCardBuilder.CreateValueRow(body, "Radius");
            TextMeshProUGUI heightTMP = EditorCardBuilder.CreateValueRow(body, "Height");

            EditorCard_CapsuleCollider card = cardObj.AddComponent<EditorCard_CapsuleCollider>();
            card.Initialize(titleTMP, triggerTMP, contactsTMP, centerTMP, radiusTMP, heightTMP);
            card.SetValues(isTrigger, providesContacts, center, radius, height);

            return card;
        }
    }
}
