using HutongGames.PlayMaker.Actions;
using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Rigidbody : EditorCardBase
    {
        private TextMeshProUGUI massText;
        private TextMeshProUGUI linearDampingText;
        private TextMeshProUGUI angularDampingText;
        private TextMeshProUGUI automaticCenterOfMassText;
        private TextMeshProUGUI automaticTensorText;
        private TextMeshProUGUI gravityText;
        private TextMeshProUGUI kinematicText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI massText, TextMeshProUGUI linearDampingText,
            TextMeshProUGUI angularDampingText, TextMeshProUGUI automaticCenterOfMassText, TextMeshProUGUI automaticTensorText,
            TextMeshProUGUI gravityText, TextMeshProUGUI kinematicText)
        {
            this.titleText = titleText;
            this.massText = massText;
            this.linearDampingText = linearDampingText;
            this.angularDampingText = angularDampingText;
            this.automaticCenterOfMassText = automaticCenterOfMassText;
            this.automaticTensorText = automaticTensorText;
            this.gravityText = gravityText;
            this.kinematicText = kinematicText;
        }

        public void SetMass(float f) => UpdateStringText(massText, f.ToString("F2"));
        public void SetLinearDamping(float f) => UpdateStringText(linearDampingText, f.ToString("F2"));
        public void SetAngularDamping(float f) => UpdateStringText(angularDampingText, f.ToString("F2"));
        public void SetAutomaticCenterOfMassText(bool b) => UpdateBoolText(automaticCenterOfMassText, b);
        public void SetAutomaticTensor(bool b) => UpdateBoolText(automaticTensorText, b);
        public void SetGravity(bool b) => UpdateBoolText(gravityText, b);
        public void SetKinematic(bool b) => UpdateBoolText(kinematicText, b);
        public void SetValues(float mass, float linearDamping, float angularDamping, bool automaticCenterOfMass, bool automaticTensor, bool useGravity, bool isKinematic)
        {
            SetMass(mass);
            SetLinearDamping(linearDamping);
            SetAngularDamping(angularDamping);
            SetAutomaticCenterOfMassText(automaticCenterOfMass);
            SetAutomaticTensor(automaticTensor);
            SetGravity(useGravity);
            SetKinematic(isKinematic);
        }

        public static EditorCard_Rigidbody Create(Transform contentParent, string title,
            float mass, float linearDamping, float angularDamping, bool automaticCenterOfMass, bool automaticTensor, bool useGravity, bool isKinematic)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Rigidbody", title);

            TextMeshProUGUI massTMP = EditorCardBuilder.CreateValueRow(body, "Mass");
            TextMeshProUGUI linearDampingTMP = EditorCardBuilder.CreateValueRow(body, "Linear Damping");
            TextMeshProUGUI angularDampingTMP = EditorCardBuilder.CreateValueRow(body, "Angular Damping");
            TextMeshProUGUI automaticCenterOfMassTMP = EditorCardBuilder.CreateBoolRow(body, "Automatic Center Of Mass");
            TextMeshProUGUI automaticTensorTMP = EditorCardBuilder.CreateBoolRow(body, "Automatic Tensor");
            TextMeshProUGUI gravityTMP = EditorCardBuilder.CreateBoolRow(body, "Use Gravity");
            TextMeshProUGUI kinematicTMP = EditorCardBuilder.CreateBoolRow(body, "Is Kinematic");

            EditorCard_Rigidbody card = cardObj.AddComponent<EditorCard_Rigidbody>();
            card.Initialize(titleTMP, massTMP, linearDampingTMP, angularDampingTMP, automaticCenterOfMassTMP, automaticTensorTMP, gravityTMP, kinematicTMP);
            card.SetValues(mass, linearDamping, angularDamping, automaticCenterOfMass, automaticTensor, useGravity, isKinematic);

            return card;
        }
    }
}
