using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_MeshCollider : EditorCardBase
    {
        private TextMeshProUGUI triggerText;
        private TextMeshProUGUI contactsText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI triggerText, TextMeshProUGUI contactsText)
        {
            this.titleText = titleText;
            this.triggerText = triggerText;
            this.contactsText = contactsText;
        }

        public void SetTrigger(bool b) => UpdateBoolText(triggerText, b);
        public void SetContacts(bool b) => UpdateBoolText(contactsText, b);
        public void SetValues(bool isTrigger, bool providesContacts)
        {
            SetTrigger(isTrigger);
            SetContacts(providesContacts);
        }

        public static EditorCard_MeshCollider Create(Transform contentParent, string title,
            bool isTrigger, bool providesContacts)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_MeshCollider", title);

            TextMeshProUGUI triggerTMP = EditorCardBuilder.CreateBoolRow(body, "Is Trigger");
            TextMeshProUGUI contactsTMP = EditorCardBuilder.CreateBoolRow(body, "Provides Contacts");

            EditorCard_MeshCollider card = cardObj.AddComponent<EditorCard_MeshCollider>();
            card.Initialize(titleTMP, triggerTMP, contactsTMP);
            card.SetValues(isTrigger, providesContacts);

            return card;
        }
    }
}
