using TMPro;
using TMPro.Examples;
using UnityEngine;

namespace ModdingHelper.UnityEditor
{
    internal class EditorCard_Transform : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI positionText;
        private TextMeshProUGUI rotationText;
        private TextMeshProUGUI scaleText;

        public void Initialize(TextMeshProUGUI title, TextMeshProUGUI pos, TextMeshProUGUI rot, TextMeshProUGUI scale)
        {
            titleText = title;
            positionText = pos;
            rotationText = rot;
            scaleText = scale;
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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
