using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Transform : EditorCardBase
    {
        private static Transform selectedTransform;
        private TextMeshProUGUI[] positionTexts = new TextMeshProUGUI[3];
        private TextMeshProUGUI[] rotationTexts = new TextMeshProUGUI[3];
        private TextMeshProUGUI[] scaleTexts = new TextMeshProUGUI[3];

        public void Initialize(TextMeshProUGUI titleText, 
            TextMeshProUGUI[] positionTexts, TextMeshProUGUI[] rotationTexts, TextMeshProUGUI[] scaleTexts)
        {
            this.titleText = titleText;
            this.positionTexts = positionTexts;
            this.rotationTexts = rotationTexts;
            this.scaleTexts = scaleTexts;
        }

        public void SetPosition(Vector3 v) => UpdateVectorTexts(positionTexts, v);
        public void SetRotation(Vector3 v) => UpdateVectorTexts(rotationTexts, v);
        public void SetScale(Vector3 v) => UpdateVectorTexts(scaleTexts, v);
        public void SetValues(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            SetPosition(position);
            SetRotation(rotation);
            SetScale(scale);
        }

        public static EditorCard_Transform Create(Transform contentParent, string title,
            Vector3 position, Vector3 rotation, Vector3 scale,
            Transform selection)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Transform", title);

            TextMeshProUGUI[] positionTMP = EditorCardBuilder.CreateVectorRow(body, "Position");
            TextMeshProUGUI[] rotationTMP = EditorCardBuilder.CreateVectorRow(body, "Rotation");
            TextMeshProUGUI[] scaleTMP = EditorCardBuilder.CreateVectorRow(body, "Scale");

            EditorCard_Transform card = cardObj.AddComponent<EditorCard_Transform>();
            card.Initialize(titleTMP, positionTMP, rotationTMP, scaleTMP);
            card.SetValues(position, rotation, scale);

            selectedTransform = selection;

            return card;
        }

        private void Update()
        {
            if (selectedTransform == null) return;
            SetPosition(selectedTransform.localPosition);
            SetRotation(selectedTransform.eulerAngles);
            SetScale(selectedTransform.localScale);
        }
    }
}
