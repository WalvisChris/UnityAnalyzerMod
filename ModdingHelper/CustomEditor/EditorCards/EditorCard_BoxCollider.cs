using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_BoxCollider : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI triggerText;
        private TextMeshProUGUI contactsText;
        private TextMeshProUGUI centerText;
        private TextMeshProUGUI sizeText;

        private string GetColoredBoolean(bool b) { return (b) ? $"<#00FF00>{b}</color>" : $"<#FF0000>{b}</color>"; }
        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI triggerText, TextMeshProUGUI contactsText, TextMeshProUGUI centerText, TextMeshProUGUI sizeText)
        {
            this.titleText = titleText;
            this.triggerText = triggerText;
            this.contactsText = contactsText;
            this.centerText = centerText;
            this.sizeText = sizeText;
        }

        public void SetTitle(string newTitle) { if (titleText != null) titleText.text = newTitle; }
        public void SetTriggerText(bool isTrigger) { if (triggerText != null) triggerText.text = $"Is Trigger: {GetColoredBoolean(isTrigger)}"; }
        public void SetContactsText(bool providesContacts) { if (contactsText != null) contactsText.text = $"Provides Contacts: {GetColoredBoolean(providesContacts)}";  }
        public void SetCenterText(Vector3 v) { if (centerText != null) centerText.text = $"Center: {v.ToString("F2")}"; }
        public void SetSizeText(Vector3 v) { if (sizeText != null) sizeText.text = $"Size: {v.ToString("F2")}"; }
        public void SetValues(bool isTrigger, bool providesContacts, Vector3 center, Vector3 size)
        {
            SetTriggerText(isTrigger);
            SetContactsText(providesContacts);
            SetCenterText(center);
            SetSizeText(size);
        }
        public void Show() { gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); }
        public static EditorCard_BoxCollider Create(Transform parent, string title, bool? isTrigger = false, bool? providesContacts = false, Vector3? defaultCenter = null, Vector3? defaultSize = null)
        {
            GameObject cardObj = new GameObject("Card_BoxCollider");
            cardObj.transform.SetParent(parent, false);

            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            //ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            //fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Vertical Layout for the card (Header + Body stacked)
            VerticalLayoutGroup mainLayout = cardObj.AddComponent<VerticalLayoutGroup>();
            mainLayout.padding = new RectOffset(10, 10, 10, 10);
            mainLayout.spacing = 8;
            mainLayout.childControlWidth = true;

            LayoutElement layoutElem = cardObj.AddComponent<LayoutElement>();
            layoutElem.preferredWidth = 330;
            layoutElem.flexibleHeight = 0;

            // 2. Title Header
            GameObject titleObj = new GameObject("Text_Title");
            titleObj.transform.SetParent(cardObj.transform, false);

            TextMeshProUGUI titleTMP = titleObj.AddComponent<TextMeshProUGUI>();
            titleTMP.text = title;
            titleTMP.fontSize = 15;
            titleTMP.fontStyle = FontStyles.Bold;
            titleTMP.alignment = TextAlignmentOptions.Left;
            titleTMP.color = Color.white;

            // 3. Body Container (Vertical Stack for Position, Rotation, Scale)
            GameObject bodyObj = new GameObject("Body_VerticalLayout");
            bodyObj.transform.SetParent(cardObj.transform, false);

            VerticalLayoutGroup bodyLayout = bodyObj.AddComponent<VerticalLayoutGroup>();
            bodyLayout.spacing = 4;
            bodyLayout.childControlWidth = true;

            // Helper to generate a single line of body text
            TextMeshProUGUI CreateVectorLine(string name)
            {
                GameObject lineObj = new GameObject($"Text_{name}");
                lineObj.transform.SetParent(bodyObj.transform, false);

                TextMeshProUGUI lineTMP = lineObj.AddComponent<TextMeshProUGUI>();
                lineTMP.fontSize = 13;
                lineTMP.color = new Color(0.6f, 0.6f, 0.6f);
                lineTMP.alignment = TextAlignmentOptions.Left;

                return lineTMP;
            }

            // Create lines for Position, Rotation, and Scale
            TextMeshProUGUI isTriggerTMP = CreateVectorLine("IsTrigger");
            TextMeshProUGUI providesContactsTMP = CreateVectorLine("ProvidesContacts");
            TextMeshProUGUI centerTMP = CreateVectorLine("Center");
            TextMeshProUGUI scaleTMP = CreateVectorLine("Scale");

            // 4. Attach & Initialize Component
            EditorCard_BoxCollider card = cardObj.AddComponent<EditorCard_BoxCollider>();
            card.Initialize(titleTMP, isTriggerTMP, providesContactsTMP, centerTMP, scaleTMP);

            // Apply default values
            card.SetValues(
                isTrigger ?? false,
                providesContacts ?? false,
                defaultCenter ?? Vector3.zero,
                defaultSize ?? Vector3.one
            );

            return card;
        }
    }
}
