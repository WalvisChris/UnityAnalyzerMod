using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_InteractableContainer : EditorCardBase
    {
        private TextMeshProUGUI isStorageShelfText;
        private TextMeshProUGUI isManufacturingText;

        public void Initialize(TextMeshProUGUI titleText,
            TextMeshProUGUI isStorageShelfText,
            TextMeshProUGUI isManufacturingText)
        {
            this.titleText = titleText;
            this.isStorageShelfText = isStorageShelfText;
            this.isManufacturingText = isManufacturingText;
        }

        public void SetIsStorageShelf(bool b) => UpdateBoolText(isStorageShelfText, b);
        public void SetIsManufacturing(bool b) => UpdateBoolText(isManufacturingText, b);

        public void SetValues(bool isStorageShelf, bool isManufacturing)
        {
            SetIsStorageShelf(isStorageShelf);
            SetIsManufacturing(isManufacturing);
        }

        public static EditorCard_Custom_InteractableContainer Create(Transform contentParent,
            string title,
            bool isStorageShelf,
            bool isManufacturing)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_InteractableContainer", title);

            TextMeshProUGUI isStorageShelfTMP = EditorCardBuilder.CreateBoolRow(body, "Is Storage Shelf");
            TextMeshProUGUI isManufacturingTMP = EditorCardBuilder.CreateBoolRow(body, "Is Manufacturing");

            EditorCard_Custom_InteractableContainer card = cardObj.AddComponent<EditorCard_Custom_InteractableContainer>();
            card.Initialize(titleTMP, isStorageShelfTMP, isManufacturingTMP);
            card.SetValues(isStorageShelf, isManufacturing);

            return card;
        }
    }
}