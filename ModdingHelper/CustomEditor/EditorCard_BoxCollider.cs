using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor
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
    }
}
