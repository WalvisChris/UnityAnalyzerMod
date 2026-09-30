using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_PlayerNetwork : EditorCardBase
    {
        private TextMeshProUGUI equippedItemText;
        private TextMeshProUGUI characterIdText;
        private TextMeshProUGUI hatIdText;
        private TextMeshProUGUI CrouchingText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI equippedItemText, TextMeshProUGUI characterIdText,
            TextMeshProUGUI hatIdText, TextMeshProUGUI CrouchingText)
        {
            this.titleText = titleText;
            this.equippedItemText = equippedItemText;
            this.hatIdText = hatIdText;
            this.CrouchingText = CrouchingText;
        }

        public void SetEquippedItem(int i) => UpdateStringText(equippedItemText, i.ToString());
        public void SetCharacterId(int i) => UpdateStringText(characterIdText, i.ToString());
        public void SetHatId(int i) => UpdateStringText(hatIdText, i.ToString());
        public void SetCrouching(bool b) => UpdateBoolText(CrouchingText, b);
        public void SetValues(int equippedItem, int characterId, int hatId, bool isCrouching)
        {
            SetEquippedItem(equippedItem);
            SetCharacterId(characterId);
            SetHatId(hatId);
            SetCrouching(isCrouching);
        }

        public static EditorCard_Custom_PlayerNetwork Create(Transform contentParent, string title,
            int equippedItem, int characterId, int hatId, bool isCrouching)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_PlayerNetwork", title);

            TextMeshProUGUI equippedItemTMP = EditorCardBuilder.CreateValueRow(body, "Equipped Item");
            TextMeshProUGUI characterIdTMP = EditorCardBuilder.CreateValueRow(body, "Character ID");
            TextMeshProUGUI hatIdTMP = EditorCardBuilder.CreateValueRow(body, "Hat ID");
            TextMeshProUGUI crouchingTMP = EditorCardBuilder.CreateBoolRow(body, "Is Crouching");

            EditorCard_Custom_PlayerNetwork card = cardObj.AddComponent<EditorCard_Custom_PlayerNetwork>();
            card.Initialize(titleTMP, equippedItemTMP, characterIdTMP, hatIdTMP, crouchingTMP);
            card.SetValues(equippedItem, characterId, hatId, isCrouching);

            return card;
        }
    }
}
