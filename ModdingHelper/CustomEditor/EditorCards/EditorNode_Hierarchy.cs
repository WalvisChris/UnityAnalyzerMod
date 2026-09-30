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
    internal class EditorNode_Hierarchy : MonoBehaviour
    {
        private Transform targetTransform;
        private Action<Transform> onSelectCallback;
        private bool isExpanded = false;
        private Button expandButton;
        private TextMeshProUGUI expandText;
        private TextMeshProUGUI nameText;
        private GameObject childrenContainer;
        private List<EditorNode_Hierarchy> childNodes = new List<EditorNode_Hierarchy>();

        public void Initialize(Transform target, int indentLevel, Action<Transform> onSelect, Func<Transform, Transform, GameObject> createNodeFunc)
        {
            targetTransform = target;
            onSelectCallback = onSelect;

            // Root row container
            GameObject rowObj = new GameObject("Row");
            rowObj.transform.SetParent(transform, false);

            // Horizontal Layout for the row
            HorizontalLayoutGroup rowLayout = rowObj.AddComponent<HorizontalLayoutGroup>();
            rowLayout.spacing = 2;
            rowLayout.childControlWidth = true;  // KEY FIX: MUST be true for preferredWidth to work
            rowLayout.childControlHeight = true;
            rowLayout.childForceExpandWidth = false;
            rowLayout.childForceExpandHeight = false;

            LayoutElement rowLayoutElem = rowObj.AddComponent<LayoutElement>();
            rowLayoutElem.preferredHeight = 22;
            rowLayoutElem.flexibleHeight = 0;

            // Indentation spacing based on tree depth
            if (indentLevel > 0)
            {
                GameObject indentObj = new GameObject("Indent");
                indentObj.transform.SetParent(rowObj.transform, false);
                LayoutElement indentElem = indentObj.AddComponent<LayoutElement>();
                indentElem.preferredWidth = indentLevel * 10; // Tight indentation
            }

            // Expand / Collapse Arrow Button
            GameObject arrowObj = new GameObject("ExpandButton");
            arrowObj.transform.SetParent(rowObj.transform, false);
            expandButton = arrowObj.AddComponent<Button>();

            expandText = arrowObj.AddComponent<TextMeshProUGUI>();
            expandText.fontSize = 11;
            expandText.alignment = TextAlignmentOptions.Center;

            LayoutElement arrowElem = arrowObj.AddComponent<LayoutElement>();
            arrowElem.preferredWidth = 14; // Controls exact arrow width

            int childCount = target.childCount;
            if (childCount > 0)
            {
                expandText.text = ">";
                expandButton.onClick.AddListener(ToggleExpand);
            }
            else
            {
                expandText.text = " ";
                expandButton.interactable = false;
            }

            // Object Name Button (Selection)
            GameObject nameObj = new GameObject("NameButton");
            nameObj.transform.SetParent(rowObj.transform, false);
            Button nameBtn = nameObj.AddComponent<Button>();

            nameText = nameObj.AddComponent<TextMeshProUGUI>();
            nameText.text = target.name;
            nameText.fontSize = 13;
            nameText.color = target.gameObject.activeInHierarchy ? Color.white : new Color(0.5f, 0.5f, 0.5f);
            nameText.alignment = TextAlignmentOptions.Left;

            LayoutElement nameElem = nameObj.AddComponent<LayoutElement>();
            nameElem.preferredWidth = 200; // Gives name text room to display
            nameElem.flexibleWidth = 1;

            nameBtn.onClick.AddListener(() => onSelectCallback?.Invoke(targetTransform));

            // Children Container (Hidden by default)
            childrenContainer = new GameObject("ChildrenContainer");
            childrenContainer.transform.SetParent(transform, false);

            VerticalLayoutGroup childLayout = childrenContainer.AddComponent<VerticalLayoutGroup>();
            childLayout.spacing = 2;
            childLayout.childControlWidth = true;
            childLayout.childControlHeight = true;
            childLayout.childForceExpandHeight = false;

            // Build child rows recursively
            for (int i = 0; i < childCount; i++)
            {
                Transform child = target.GetChild(i);
                createNodeFunc(child, childrenContainer.transform);
            }

            childrenContainer.SetActive(false);
        }

        public void ToggleExpand()
        {
            isExpanded = !isExpanded;
            childrenContainer.SetActive(isExpanded);
            expandText.text = isExpanded ? "v" : ">";
        }
    }
}
