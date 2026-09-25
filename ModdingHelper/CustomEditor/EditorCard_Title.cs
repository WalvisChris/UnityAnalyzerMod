using TMPro;
using UnityEngine;

namespace ModdingHelper.UnityEditor
{
    internal class EditorCard_Title : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        public void Initialize(TextMeshProUGUI title)
        {
            titleText = title;
        }

        public void SetTitle(string newTitle)
        {
            if (titleText != null) titleText.text = newTitle;
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
