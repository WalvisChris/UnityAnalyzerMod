using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_BoxCollider : EditorCardBase
    {
        private TextMeshProUGUI triggerText;
        private TextMeshProUGUI contactsText;
        private TextMeshProUGUI[] centerTexts;
        private TextMeshProUGUI[] sizeTexts;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI triggerText,
            TextMeshProUGUI contactsText, TextMeshProUGUI[] centerTexts, TextMeshProUGUI[] sizeTexts)
        {
            this.titleText = titleText;
            this.triggerText = triggerText;
            this.contactsText = contactsText;
            this.centerTexts = centerTexts;
            this.sizeTexts = sizeTexts;
        }

        public void SetTrigger(bool b) => UpdateBoolText(triggerText, b);
        public void SetContacts(bool b) => UpdateBoolText(contactsText, b);
        public void SetCenter(Vector3 v) => UpdateVectorTexts(centerTexts, v);
        public void SetSize(Vector3 v) => UpdateVectorTexts(sizeTexts, v);

        public void SetValues(bool isTrigger, bool providesContacts, Vector3 center, Vector3 size)
        {
            SetTrigger(isTrigger);
            SetContacts(providesContacts);
            SetCenter(center);
            SetSize(size);
        }

        public static EditorCard_BoxCollider Create(Transform contentParent, string title,
            bool isTrigger, bool providesContacts, Vector3 center, Vector3 size)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_BoxCollider", title);

            TextMeshProUGUI triggerTMP = EditorCardBuilder.CreateBoolRow(body, "Is Trigger");
            TextMeshProUGUI contactsTMP = EditorCardBuilder.CreateBoolRow(body, "Provides Contacts");
            TextMeshProUGUI[] centerTMP = EditorCardBuilder.CreateVectorRow(body, "Center");
            TextMeshProUGUI[] sizeTMP = EditorCardBuilder.CreateVectorRow(body, "Size");

            EditorCard_BoxCollider card = cardObj.AddComponent<EditorCard_BoxCollider>();
            card.Initialize(titleTMP, triggerTMP, contactsTMP, centerTMP, sizeTMP);
            card.SetValues(isTrigger, providesContacts, center, size);

            return card;
        }
    }
}