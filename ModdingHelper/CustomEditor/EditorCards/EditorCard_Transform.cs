using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Transform : MonoBehaviour
    {
        private Transform selectedTransform;

        // TMPro
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI positionText;
        private TextMeshProUGUI rotationText;
        private TextMeshProUGUI scaleText;

        public void Initialize(Transform selectedTransform, TextMeshProUGUI titleText, TextMeshProUGUI positionText, TextMeshProUGUI rotationText, TextMeshProUGUI scaleText)
        {
            this.selectedTransform = selectedTransform;
            this.titleText = titleText;
            this.positionText = positionText;
            this.rotationText = rotationText;
            this.scaleText = scaleText;
        }

        private void Update()
        {
            if (selectedTransform == null) return;
            if (positionText != null) positionText.text = $"Position: {selectedTransform.localPosition.ToString("F2")}";
            if (rotationText != null) rotationText.text = $"Rotation: {selectedTransform.eulerAngles.ToString("F2")}";
            if (scaleText != null) scaleText.text = $"Scale: {selectedTransform.localScale.ToString("F2")}";
        }

        public void SetTitle(string newTitle) { if (titleText != null) titleText.text = newTitle;  }
        public void SetPosition(Vector3 v) { if (positionText != null) positionText.text = $"Position: {v.ToString("F2")}"; }
        public void SetRotation(Vector3 v) { if (rotationText != null) rotationText.text = $"Rotation: {v.ToString("F2")}"; }
        public void SetScale(Vector3 v) { if (scaleText != null) scaleText.text = $"Scale: {v.ToString("F2")}"; }
        public void SetValues(Vector3 pos, Vector3 rot, Vector3 scale)
        {
            SetPosition(pos);
            SetRotation(rot);
            SetScale(scale);
        }
        public void Show() { gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); }
        public static EditorCard_Transform Create(Transform selectedTransform, Transform parent, string title, Vector3? defaultPos = null, Vector3? defaultRot = null, Vector3? defaultScale = null)
        {
            // 1. Root Card Container
            GameObject cardObj = new GameObject("Card_Transform");
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
            TextMeshProUGUI posTMP = CreateVectorLine("Position");
            TextMeshProUGUI rotTMP = CreateVectorLine("Rotation");
            TextMeshProUGUI scaleTMP = CreateVectorLine("Scale");

            // 4. Attach & Initialize Component
            EditorCard_Transform card = cardObj.AddComponent<EditorCard_Transform>();
            card.Initialize(selectedTransform, titleTMP, posTMP, rotTMP, scaleTMP);

            // Apply default values
            card.SetValues(
                defaultPos ?? Vector3.zero,
                defaultRot ?? Vector3.zero,
                defaultScale ?? Vector3.one
            );

            return card;
        }
    }
}
