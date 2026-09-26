
using HutongGames.PlayMaker.Actions;
using ModdingHelper.CustomEditor;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ModdingHelper.UnityEditor
{
    internal class CustomEditorManager : MonoBehaviour
    {
        // Cameras
        private Camera playerCamera;
        private Camera sceneCamera;
        private Camera previewCamera;
        private RenderTexture previewRenderTexture;
        private float sceneCameraX;
        private float sceneCameraY;
        private float sceneCameraSensitivity = 70f;
        private float sceneCameraSpeed = 8f;
        public static bool isSceneCameraActive = false;
        private bool isCursorLocked;

        // UI
        private Canvas moddedCanvas;
        private GameObject previewUIObject;
        private GameObject leftPanelObj;
        private GameObject hierarchyContent;
        private List<GameObject> activeHierarchyNodes = new List<GameObject>();
        private GameObject rightPanelObj;
        private List<GameObject> dynamicCards = new List<GameObject>();

        // Selection
        private Transform selectedTransform;
        private SelectionOutline currentOutline;

        private void Start()
        {
            // Get player camera
            playerCamera = Camera.main;

            // Create scene camera
            GameObject secondCamera = new GameObject("SceneCamera");
            sceneCamera = secondCamera.AddComponent<Camera>();
            sceneCameraX = 0;
            sceneCameraY = 0f;

            // Scene camera starting values
            sceneCamera.transform.position = new Vector3(0, 5, -10);
            sceneCamera.transform.rotation = Quaternion.Euler(15, 0, 0);

            // Create preview camera
            GameObject thirdCamera = new GameObject("PreviewCamera");
            previewCamera = thirdCamera.AddComponent<Camera>();

            // Create preview render texture
            previewRenderTexture = new RenderTexture(320, 180, 16);
            previewCamera.targetTexture = previewRenderTexture;

            // Initialize
            CreateUI();
            previewUIObject.SetActive(false);
            previewCamera.enabled = false;
            playerCamera.cullingMask = 0; // not deactived, just not rendering
            sceneCamera.enabled = true;
            isSceneCameraActive = true;
            isCursorLocked = true;
        }

        private void CreateUI()
        {
            // 1. Create the main canvas
            GameObject canvasObj = new GameObject("ModdedCanvas");
            moddedCanvas = canvasObj.AddComponent<Canvas>();
            moddedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            moddedCanvas.sortingOrder = 999;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObj.AddComponent<GraphicRaycaster>();

            // 2a. Create the right panel
            rightPanelObj = new GameObject("RightPanel");
            rightPanelObj.transform.SetParent(moddedCanvas.transform, false);

            Image rightPanelImage = rightPanelObj.AddComponent<Image>();
            rightPanelImage.color = new Color(0.1f, 0.1f, 0.1f);

            RectTransform rightPanelRect = rightPanelObj.GetComponent<RectTransform>();
            rightPanelRect.anchorMin = new Vector2(1, 0);
            rightPanelRect.anchorMax = new Vector2(1, 1);
            rightPanelRect.pivot = new Vector2(1, 1);
            rightPanelRect.anchoredPosition = new Vector2(0, 0);
            rightPanelRect.sizeDelta = new Vector2(360, 0);

            // 2b. Add vertical layout to right panel
            VerticalLayoutGroup rightLayout = rightPanelObj.AddComponent<VerticalLayoutGroup>();
            rightLayout.padding = new RectOffset(15, 15, 20, 20);
            rightLayout.spacing = 10;
            rightLayout.childAlignment = TextAnchor.UpperCenter;
            rightLayout.childControlWidth = true;
            rightLayout.childForceExpandWidth = true;
            rightLayout.childControlHeight = true;
            rightLayout.childForceExpandHeight = false;

            // 3a. Create the left panel
            leftPanelObj = new GameObject("LeftPanel");
            leftPanelObj.transform.SetParent(moddedCanvas.transform, false);

            Image leftPanelImage = leftPanelObj.AddComponent<Image>();
            leftPanelImage.color = new Color(0.1f, 0.1f, 0.1f);

            RectTransform leftPanelRect = leftPanelObj.GetComponent<RectTransform>();
            leftPanelRect.anchorMin = new Vector2(0, 0);
            leftPanelRect.anchorMax = new Vector2(0, 1);
            leftPanelRect.pivot = new Vector2(0, 1);
            leftPanelRect.anchoredPosition = new Vector2(0, 0);
            leftPanelRect.sizeDelta = new Vector2(360, 0);

            // 3b. Add vertical layout to left panel
            VerticalLayoutGroup leftLayout = leftPanelObj.AddComponent<VerticalLayoutGroup>();
            leftLayout.padding = new RectOffset(10, 10, 10, 10);
            leftLayout.spacing = 10;
            leftLayout.childControlWidth = true;
            leftLayout.childControlHeight = true;
            leftLayout.childForceExpandHeight = false;

            // 3c. Refresh hierarchy button
            GameObject btnObj = new GameObject("Button_RefreshHierarchy");
            btnObj.transform.SetParent(leftPanelObj.transform, false);

            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.2f, 0.2f, 0.2f);

            Button refreshBtn = btnObj.AddComponent<Button>();
            refreshBtn.onClick.AddListener(RefreshSceneHierarchy);

            LayoutElement btnElem = btnObj.AddComponent<LayoutElement>();
            btnElem.preferredHeight = 35;
            btnElem.flexibleHeight = 0;

            GameObject btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI btnText = btnTextObj.AddComponent<TextMeshProUGUI>();
            btnText.text = "Refresh Hierarchy";
            btnText.fontSize = 14;
            btnText.fontStyle = FontStyles.Bold;
            btnText.color = Color.white;
            btnText.alignment = TextAlignmentOptions.Center;

            // 3d. Scroll view
            GameObject scrollObj = new GameObject("Hierarchy_ScrollView");
            scrollObj.transform.SetParent(leftPanelObj.transform, false);

            ScrollRect scrollRect = scrollObj.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            LayoutElement scrollElem = scrollObj.AddComponent<LayoutElement>();
            scrollElem.flexibleHeight = 1; // Fills remaining vertical space below button

            // 3e. Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);

            Image viewportImg = viewport.AddComponent<Image>();
            viewportImg.color = new Color(0.05f, 0.05f, 0.05f, 0.5f);
            Mask viewportMask = viewport.AddComponent<Mask>();
            viewportMask.showMaskGraphic = true;

            RectTransform vpRect = viewport.GetComponent<RectTransform>();
            vpRect.anchorMin = Vector2.zero; // Bottom-Left
            vpRect.anchorMax = Vector2.one;  // Top-Right
            vpRect.pivot = new Vector2(0.5f, 0.5f);
            vpRect.anchoredPosition = Vector2.zero;
            vpRect.sizeDelta = Vector2.zero; // Stretches to fill ScrollView bounds

            // 3f. Scroll Content Container
            hierarchyContent = new GameObject("Content");
            hierarchyContent.transform.SetParent(viewport.transform, false);

            VerticalLayoutGroup contentLayout = hierarchyContent.AddComponent<VerticalLayoutGroup>();
            contentLayout.spacing = 2;
            contentLayout.childControlWidth = true;
            contentLayout.childControlHeight = true;
            contentLayout.childForceExpandHeight = false;

            ContentSizeFitter contentFitter = hierarchyContent.AddComponent<ContentSizeFitter>();
            contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform contentRect = hierarchyContent.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0, 1);
            contentRect.sizeDelta = new Vector2(0, 0);

            scrollRect.content = contentRect;
            scrollRect.viewport = vpRect;

            // 4. Create the preview image
            previewUIObject = new GameObject("Preview_RawImage");
            previewUIObject.transform.SetParent(rightPanelObj.transform, false);

            RawImage rawImage = previewUIObject.AddComponent<RawImage>();
            rawImage.texture = previewRenderTexture;

            LayoutElement previewLayout = previewUIObject.AddComponent<LayoutElement>();
            previewLayout.preferredWidth = 320;
            previewLayout.preferredHeight = 180;
            previewLayout.flexibleHeight = 0;
        }

        private void RefreshSceneHierarchy()
        {
            foreach (GameObject node in activeHierarchyNodes)
            {
                if (node != null) Destroy(node);
            }
            activeHierarchyNodes.Clear();

            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = currentScene.GetRootGameObjects();

            foreach (GameObject rootObj in rootObjects)
            {
                if (rootObj == moddedCanvas.gameObject || rootObj.name == "SceneCamera" || rootObj.name == "PreviewCamera")
                    continue;

                BuildNodeRecursive(rootObj.transform, hierarchyContent.transform, 0);
            }
        }

        private GameObject BuildNodeRecursive(Transform target, Transform parentUI, int indentLevel)
        {
            GameObject nodeObj = new GameObject($"Node_{target.name}");
            nodeObj.transform.SetParent(parentUI, false);

            VerticalLayoutGroup nodeLayout = nodeObj.AddComponent<VerticalLayoutGroup>();
            nodeLayout.childControlWidth = true;
            nodeLayout.childControlHeight = true;
            nodeLayout.childForceExpandHeight = false;

            LayoutElement nodeElem = nodeObj.AddComponent<LayoutElement>();
            nodeElem.flexibleHeight = 0;

            EditorNode_Hierarchy nodeComp = nodeObj.AddComponent<EditorNode_Hierarchy>();
            nodeComp.Initialize(
                target,
                indentLevel,
                SelectTransform, // Select callback triggered on click
                (childTransform, childParentUI) => BuildNodeRecursive(childTransform, childParentUI, indentLevel + 1)
            );

            activeHierarchyNodes.Add(nodeObj);
            return nodeObj;
        }

        private EditorCard_Title CreateCard_Title(string title)
        {
            // Base Object
            GameObject cardObj = new GameObject("Card_Title");
            cardObj.transform.SetParent(rightPanelObj.transform, false);

            // Background Color
            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            // Sizing & Layout
            ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layout = cardObj.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.childControlWidth = true;

            LayoutElement layoutElem = cardObj.AddComponent<LayoutElement>();
            layoutElem.preferredWidth = 330;
            layoutElem.flexibleHeight = 0;

            // Title Text
            GameObject textObj = new GameObject("Text_Title");
            textObj.transform.SetParent(cardObj.transform, false);

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = title;
            tmp.fontSize = 16;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Left;

            // Attach & Initialize Component
            EditorCard_Title card = cardObj.AddComponent<EditorCard_Title>();
            card.Initialize(tmp);

            return card;
        }

        private EditorCard_Transform CreateCard_Transform(string title, Vector3? defaultPos = null, Vector3? defaultRot = null, Vector3? defaultScale = null)
        {
            // 1. Root Card Container
            GameObject cardObj = new GameObject("Card_Transform");
            cardObj.transform.SetParent(rightPanelObj.transform, false);

            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

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
            card.Initialize(titleTMP, posTMP, rotTMP, scaleTMP);

            // Apply default values
            card.SetValues(
                defaultPos ?? Vector3.zero,
                defaultRot ?? Vector3.zero,
                defaultScale ?? Vector3.one
            );

            return card;
        }

        private EditorCard_BoxCollider CreateCard_BoxCollider(string title, bool? isTrigger = false, bool? providesContacts = false, Vector3? defaultCenter = null, Vector3? defaultSize = null)
        {
            GameObject cardObj = new GameObject("Card_BoxCollider");
            cardObj.transform.SetParent(rightPanelObj.transform, false);

            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

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

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                ToggleMouseLock();
            }
            else if (isCursorLocked)
            {
                CameraMovement();

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(sceneCamera.transform.position, sceneCamera.transform.forward, out hit, 100f)) SelectTransform(hit.transform);
                }
            }
        }

        private void LateUpdate()
        {
            if (selectedTransform == null && currentOutline != null)
            {
                Deselect();
                return;
            }

            // Follow selected target if active
            if (selectedTransform != null && previewCamera != null && previewCamera.enabled)
            {
                UpdatePreviewCameraPosition();
            }
        }

        private void UpdatePreviewCameraPosition()
        {
            // Calculate focus center using renderer bounds or raw position
            Vector3 targetCenter = selectedTransform.position;
            float boundsRadius = 1.0f;

            Renderer targetRenderer = selectedTransform.GetComponentInChildren<Renderer>();
            if (targetRenderer != null)
            {
                targetCenter = targetRenderer.bounds.center;
                boundsRadius = Mathf.Max(targetRenderer.bounds.extents.x, targetRenderer.bounds.extents.y, targetRenderer.bounds.extents.z);
            }

            // Position camera at a fixed offset relative to the object's size
            float distance = Mathf.Clamp(boundsRadius * 3.0f, 2.0f, 15.0f);
            Vector3 cameraOffset = new Vector3(0, boundsRadius * 0.5f, -distance);

            // Position & Look At
            previewCamera.transform.position = targetCenter + cameraOffset;
            previewCamera.transform.LookAt(targetCenter);
        }

        private void CameraMovement()
        {
            // Rotation
            sceneCameraX += Input.GetAxis("Mouse X") * sceneCameraSensitivity * Time.deltaTime;
            sceneCameraY -= Input.GetAxis("Mouse Y") * sceneCameraSensitivity * Time.deltaTime;
            sceneCameraY = Mathf.Clamp(sceneCameraY, -90f, 90f);
            sceneCamera.transform.rotation = Quaternion.Euler(sceneCameraY, sceneCameraX, 0f);

            // Movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 forward = sceneCamera.transform.forward;
            Vector3 right = sceneCamera.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            float upDown = 0f;
            if (Input.GetKey(KeyCode.Space)) upDown += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) upDown -= 1f;
            Vector3 moveDirection = (forward * vertical) + (right * horizontal) + (Vector3.up * upDown);
            if (Input.GetKey(KeyCode.LeftShift)) moveDirection *= 2f;
            sceneCamera.transform.position += moveDirection * sceneCameraSpeed * Time.deltaTime;
        }

        private void ToggleMouseLock()
        {
            isCursorLocked = !isCursorLocked;
            Cursor.lockState = (isCursorLocked) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCursorLocked;
        }

        private void SelectTransform(Transform target)
        {
            if (currentOutline != null) Destroy(currentOutline);
            selectedTransform = target;
            currentOutline = target.gameObject.AddComponent<SelectionOutline>();

            // Preview
            previewUIObject.SetActive(true);
            previewCamera.enabled = true;

            // Dynamic Cards
            ClearDynamicCards();
            CreateDynamicCards();
        }

        private void CreateDynamicCards()
        {
            // Always: name
            string objectTitle = $"{selectedTransform.name} <#AAAAAA>({selectedTransform.GetType().Name})</color>";
            EditorCard_Title nameCard = CreateCard_Title(objectTitle);
            dynamicCards.Add(nameCard.gameObject);

            // Always: layer
            int layer = selectedTransform.gameObject.layer;
            string layerName = LayerMask.LayerToName(layer);
            string layerText = $"Layer: <#AAAAAA>({layer}) {layerName}</color>";
            EditorCard_Title layerCard = CreateCard_Title(layerText);
            dynamicCards.Add(layerCard.gameObject);

            Component[] components = selectedTransform.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null) continue;

                string type = component.GetType().Name;

                switch (type)
                {
                    case "Transform":
                        EditorCard_Transform transformCard = CreateCard_Transform(
                            type,
                            selectedTransform.position,
                            selectedTransform.eulerAngles,
                            selectedTransform.localScale
                        );
                        dynamicCards.Add(transformCard.gameObject);
                        break;

                    case "BoxCollider":
                        if (component is BoxCollider boxCollider)
                        {
                            EditorCard_BoxCollider boxColliderCard = CreateCard_BoxCollider(
                                type,
                                boxCollider.isTrigger,
                                boxCollider.providesContacts,
                                boxCollider.center,
                                boxCollider.size
                            );
                            dynamicCards.Add(boxColliderCard.gameObject);
                        }
                        break;

                    case "SelectionOutline":
                        break;

                    default:
                        EditorCard_Title titleCard = CreateCard_Title(type);
                        dynamicCards.Add(titleCard.gameObject);
                        break;
                }
            }
        }

        private void ClearDynamicCards()
        {
            foreach (GameObject cardObj in dynamicCards)
            {
                if (cardObj != null) Destroy(cardObj);
            }
            dynamicCards.Clear();
        }

        private void Deselect()
        {
            selectedTransform = null;

            if (currentOutline != null)
            {
                Destroy(currentOutline);
                currentOutline = null;
            }

            previewUIObject.SetActive(false);
            previewCamera.enabled = false;
            ClearDynamicCards();
        }
    }
}
