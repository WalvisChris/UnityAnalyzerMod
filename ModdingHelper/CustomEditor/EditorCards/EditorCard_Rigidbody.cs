using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Rigidbody : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI massText;
        private TextMeshProUGUI linearDampingText;
        private TextMeshProUGUI angularDampingText;
        private TextMeshProUGUI automaticCenterOfMassText;
        private TextMeshProUGUI automaticTensorText;
        private TextMeshProUGUI useGravityText;
        private TextMeshProUGUI isKinematicText;
        private string GetColoredBoolean(bool b) { return (b) ? $"<#00FF00>{b}</color>" : $"<#FF0000>{b}</color>"; }
        private void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI massText, TextMeshProUGUI linearDampingText, TextMeshProUGUI angularDampingText, TextMeshProUGUI automaticCenterOfMassText, TextMeshProUGUI automaticTensorText, TextMeshProUGUI useGravityText, TextMeshProUGUI isKinematicText)
        {
            this.titleText = titleText;
            this.massText = massText;
            this.linearDampingText = linearDampingText;
            this.angularDampingText = angularDampingText;
            this.automaticCenterOfMassText = automaticCenterOfMassText;
            this.automaticTensorText = automaticTensorText;
            this.useGravityText = useGravityText;
            this.isKinematicText = isKinematicText;
        }
        public void SetTitle(string newTitle) { if (titleText != null) titleText.text = newTitle; }
        public void SetMass(float f) { if (massText != null) massText.text = $"Mass: {f.ToString("F2")}"; }
        public void SetLinearDampign(float f) { if (linearDampingText != null) linearDampingText.text = $"Linear Damping: {f.ToString("F2")}"; }
        public void SetAngularDamping(float f) { if (angularDampingText != null) angularDampingText.text = $"Angular Damping: {f.ToString("F2")}"; }
        public void SetAutomaticCenterOfMass(bool b) { if (automaticCenterOfMassText != null) automaticCenterOfMassText.text = $"Automatic Center Of Mass: {GetColoredBoolean(b)}"; }
        public void SetAutomaticTensor(bool b) { if (automaticTensorText != null) automaticTensorText.text = $"Automatic Tensor: {GetColoredBoolean(b)}"; }
        public void SetUseGravity(bool b) { if (useGravityText != null) useGravityText.text = $"Use Gravity: {GetColoredBoolean(b)}"; }
        public void SetIsKinematic(bool b) { if (isKinematicText != null) isKinematicText.text = $"Is Kinematic: {GetColoredBoolean(b)}"; }
        public void SetValues(float mass, float linearDamping, float angularDamping, bool automaticCenterOfMass, bool automaticTensor, bool useGravity, bool isKinematic)
        {
            SetMass(mass);
            SetLinearDampign(linearDamping);
            SetAngularDamping(angularDamping);
            SetAutomaticCenterOfMass(automaticCenterOfMass);
            SetAutomaticTensor(automaticTensor);
            SetUseGravity(useGravity);
            SetIsKinematic(isKinematic);
        }
        public void Show() { gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); }
        public static EditorCard_Rigidbody Create(Transform parent, string title, float? mass = 0f, float? linearDamping = 0f, float? angularDamping = 0f, bool? automaticCenterOfMass = false, bool? automaticTensor = false, bool? useGravity = false, bool? isKinematic = false)
        {
            GameObject cardObj = new GameObject("Card_Rigidbody");
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
            TextMeshProUGUI massTMP = CreateVectorLine("Mass");
            TextMeshProUGUI linearDampingTMP = CreateVectorLine("LinearDamping");
            TextMeshProUGUI angularDampingTMP = CreateVectorLine("AngularDamping");
            TextMeshProUGUI automaticCenterOfMassTMP = CreateVectorLine("AutomaticCenterOfMass");
            TextMeshProUGUI automaticTensorTMP = CreateVectorLine("AutomaticTensor");
            TextMeshProUGUI useGravityTMP = CreateVectorLine("UseGravity");
            TextMeshProUGUI isKinematicTMP = CreateVectorLine("IsKinematic");

            // 4. Attach & Initialize Component
            EditorCard_Rigidbody card = cardObj.AddComponent<EditorCard_Rigidbody>();
            card.Initialize(titleTMP, massTMP, linearDampingTMP, angularDampingTMP, automaticCenterOfMassTMP, automaticTensorTMP, useGravityTMP, isKinematicTMP);

            // Apply default values
            card.SetValues(
                mass ?? 0f,
                linearDamping ?? 0f,
                angularDamping ?? 0f,
                automaticCenterOfMass ?? false,
                automaticTensor ?? false,
                useGravity ?? false,
                isKinematic ?? false
            );

            return card;
        }
    }
}
