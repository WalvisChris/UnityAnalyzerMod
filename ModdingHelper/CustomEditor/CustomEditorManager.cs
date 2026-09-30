using HighlightPlus;
using Mirror;
using ModdingHelper.CustomEditor;
using ModdingHelper.CustomEditor.ColliderOutlines;
using ModdingHelper.CustomEditor.EditorCards;
using StarterAssets;
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
        private GameObject cardContent;
        private List<GameObject> dynamicCards = new List<GameObject>();
        private TextMeshProUGUI versionText;

        // Selection
        private Transform selectedTransform;
        private ColliderOutlineBase currentOutline;

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

            // Unity Version
            if (versionText != null) versionText.text = $"Unity Version: {Application.unityVersion}";
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

            // -------------------------------------------------------------
            // 1a. Unity Version Text
            // -------------------------------------------------------------
            GameObject versionObj = new GameObject("VersionBackground");
            versionObj.transform.SetParent(moddedCanvas.transform, false);

            Image versionBg = versionObj.AddComponent<Image>();
            versionBg.color = ColorThemes.buttonColor;
            
            RectTransform versionRect = versionObj.GetComponent<RectTransform>();
            versionRect.anchorMin = new Vector2(0.5f, 1f);
            versionRect.anchorMax = new Vector2(0.5f, 1f);
            versionRect.pivot = new Vector2(0.5f, 1f);
            versionRect.anchoredPosition = new Vector2(0f, 0f);
            
            HorizontalLayoutGroup versionLayout = versionObj.AddComponent<HorizontalLayoutGroup>();
            versionLayout.padding = new RectOffset(5, 5, 5, 5);
            versionLayout.childControlWidth = true;
            versionLayout.childControlHeight = true;
            
            ContentSizeFitter versionFitter = versionObj.AddComponent<ContentSizeFitter>();
            versionFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            versionFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            GameObject versionTextObj = new GameObject("VersionText");
            versionTextObj.transform.SetParent(versionObj.transform, false);
            
            versionText = versionTextObj.AddComponent<TextMeshProUGUI>();
            versionText.text = "Failed to find Unity version";
            versionText.fontSize = 14;
            versionText.alignment = TextAlignmentOptions.Center;
            versionText.color = ColorThemes.buttonTextColor;

            // -------------------------------------------------------------
            // 2. Right Panel Setup
            // -------------------------------------------------------------
            rightPanelObj = new GameObject("RightPanel");
            rightPanelObj.transform.SetParent(moddedCanvas.transform, false);

            Image rightPanelImage = rightPanelObj.AddComponent<Image>();
            rightPanelImage.color = ColorThemes.panelColor;

            RectTransform rightPanelRect = rightPanelObj.GetComponent<RectTransform>();
            rightPanelRect.anchorMin = new Vector2(1, 0);
            rightPanelRect.anchorMax = new Vector2(1, 1);
            rightPanelRect.pivot = new Vector2(1, 1);
            rightPanelRect.anchoredPosition = new Vector2(0, 0);
            rightPanelRect.sizeDelta = new Vector2(360, 0);

            VerticalLayoutGroup rightLayout = rightPanelObj.AddComponent<VerticalLayoutGroup>();
            rightLayout.padding = new RectOffset(5, 5, 5, 5); // CHANGED
            rightLayout.spacing = 10;
            rightLayout.childControlWidth = true;
            rightLayout.childControlHeight = true;
            rightLayout.childForceExpandHeight = false;

            // 2a. Fixed Preview Image (top of right panel)
            previewUIObject = new GameObject("Preview_RawImage");
            previewUIObject.transform.SetParent(rightPanelObj.transform, false);

            RawImage rawImage = previewUIObject.AddComponent<RawImage>();
            rawImage.texture = previewRenderTexture;

            LayoutElement previewLayout = previewUIObject.AddComponent<LayoutElement>();
            previewLayout.preferredWidth = 320;
            previewLayout.preferredHeight = 180;
            previewLayout.flexibleHeight = 0;

            // 2b. Scroll View
            GameObject rightScrollObj = new GameObject("Cards_ScrollView", typeof(RectTransform));
            rightScrollObj.transform.SetParent(rightPanelObj.transform, false);

            // Transparent raycast target so the mouse wheel works over gaps between cards
            Image scrollBg = rightScrollObj.AddComponent<Image>();
            scrollBg.color = ColorThemes.transparent;

            ScrollRect rightScrollRect = rightScrollObj.AddComponent<ScrollRect>();
            rightScrollRect.horizontal = false;
            rightScrollRect.vertical = true;
            rightScrollRect.movementType = ScrollRect.MovementType.Clamped;
            rightScrollRect.scrollSensitivity = 30f;

            LayoutElement rightScrollElem = rightScrollObj.AddComponent<LayoutElement>();
            rightScrollElem.flexibleHeight = 1;

            // 2c. Viewport
            GameObject rightViewport = new GameObject("Viewport", typeof(RectTransform));
            rightViewport.transform.SetParent(rightScrollObj.transform, false);
            rightViewport.AddComponent<RectMask2D>();   // instead of Image + Mask

            RectTransform rightVpRect = rightViewport.GetComponent<RectTransform>();
            rightVpRect.anchorMin = Vector2.zero;
            rightVpRect.anchorMax = Vector2.one;
            rightVpRect.offsetMin = Vector2.zero;
            rightVpRect.offsetMax = Vector2.zero;

            // 2d. Content
            cardContent = new GameObject("CardContent", typeof(RectTransform));
            cardContent.transform.SetParent(rightViewport.transform, false);

            VerticalLayoutGroup cardContentLayout = cardContent.AddComponent<VerticalLayoutGroup>();
            cardContentLayout.spacing = 10;
            cardContentLayout.childControlWidth = true;
            cardContentLayout.childControlHeight = true;
            cardContentLayout.childForceExpandWidth = true;
            cardContentLayout.childForceExpandHeight = false;

            ContentSizeFitter cardContentFitter = cardContent.AddComponent<ContentSizeFitter>();
            cardContentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform cardContentRect = cardContent.GetComponent<RectTransform>();
            cardContentRect.anchorMin = new Vector2(0, 1);
            cardContentRect.anchorMax = new Vector2(1, 1);
            cardContentRect.pivot = new Vector2(0.5f, 1);
            cardContentRect.anchoredPosition = Vector2.zero;
            cardContentRect.sizeDelta = Vector2.zero;

            rightScrollRect.viewport = rightVpRect;
            rightScrollRect.content = cardContentRect;

            // -------------------------------------------------------------
            // 3. Left Panel Setup
            // -------------------------------------------------------------
            leftPanelObj = new GameObject("LeftPanel");
            leftPanelObj.transform.SetParent(moddedCanvas.transform, false);

            Image leftPanelImage = leftPanelObj.AddComponent<Image>();
            leftPanelImage.color = ColorThemes.panelColor;

            RectTransform leftPanelRect = leftPanelObj.GetComponent<RectTransform>();
            leftPanelRect.anchorMin = new Vector2(0, 0);
            leftPanelRect.anchorMax = new Vector2(0, 1);
            leftPanelRect.pivot = new Vector2(0, 1);
            leftPanelRect.anchoredPosition = new Vector2(0, 0);
            leftPanelRect.sizeDelta = new Vector2(360, 0);

            VerticalLayoutGroup leftLayout = leftPanelObj.AddComponent<VerticalLayoutGroup>();
            leftLayout.padding = new RectOffset(10, 10, 10, 10);
            leftLayout.spacing = 10;
            leftLayout.childControlWidth = true;
            leftLayout.childControlHeight = true;
            leftLayout.childForceExpandHeight = false;

            // 3a. Refresh hierarchy button
            GameObject btnObj = new GameObject("Button_RefreshHierarchy");
            btnObj.transform.SetParent(leftPanelObj.transform, false);

            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = ColorThemes.buttonColor;

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
            //btnText.fontStyle = FontStyles.Bold;
            btnText.color = ColorThemes.buttonTextColor;
            btnText.alignment = TextAlignmentOptions.Center;

            // 3b. Hierarchy Scroll view
            GameObject scrollObj = new GameObject("Hierarchy_ScrollView");
            scrollObj.transform.SetParent(leftPanelObj.transform, false);

            ScrollRect scrollRect = scrollObj.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            LayoutElement scrollElem = scrollObj.AddComponent<LayoutElement>();
            scrollElem.flexibleHeight = 1;

            // 3c. Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);

            Image viewportImg = viewport.AddComponent<Image>();
            viewportImg.color = ColorThemes.viewportColor;
            Mask viewportMask = viewport.AddComponent<Mask>();
            viewportMask.showMaskGraphic = true;

            RectTransform vpRect = viewport.GetComponent<RectTransform>();
            vpRect.anchorMin = Vector2.zero;
            vpRect.anchorMax = Vector2.one;
            vpRect.pivot = new Vector2(0.5f, 0.5f);
            vpRect.anchoredPosition = Vector2.zero;
            vpRect.sizeDelta = Vector2.zero;

            // 3d. Scroll Content Container
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

            // Voeg het juiste specifieke outline component toe
            if (target.TryGetComponent<BoxCollider>(out _))
            {
                currentOutline = target.gameObject.AddComponent<BoxColliderOutline>();
            }
            else if (target.TryGetComponent<CapsuleCollider>(out _))
            {
                currentOutline = target.gameObject.AddComponent<CapsuleColliderOutline>();
            }
            else if (target.TryGetComponent<MeshCollider>(out _))
            {
                currentOutline = target.gameObject.AddComponent<MeshColliderOutline>();
            }

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
            dynamicCards.Add(EditorCard_SingleValue.Create(
                cardContent.transform,
                selectedTransform.gameObject.GetType().Name,
                selectedTransform.gameObject.name,
                215
            ).gameObject);

            //Always: layer
            int layer = selectedTransform.gameObject.layer;
            dynamicCards.Add(EditorCard_SingleValue.Create(
                cardContent.transform,
                "Layer",
                $"{LayerMask.LayerToName(layer)} ({layer})"
            ).gameObject);

            Component[] components = selectedTransform.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null) continue;

                switch (component)
                {
                    // Default Unity Components
                    case Transform t:
                        dynamicCards.Add(EditorCard_Transform.Create(
                            cardContent.transform,
                            "Transform",
                            t.position,
                            t.eulerAngles,
                            t.localScale,
                            selectedTransform
                        ).gameObject);
                        break;

                    case BoxCollider boxCollider:
                        dynamicCards.Add(EditorCard_BoxCollider.Create(
                            cardContent.transform,
                            "Box Collider",
                            boxCollider.isTrigger,
                            boxCollider.providesContacts,
                            boxCollider.center,
                            boxCollider.size
                        ).gameObject);
                        break;

                    case CapsuleCollider capsuleCollider:
                        dynamicCards.Add(EditorCard_CapsuleCollider.Create(
                            cardContent.transform,
                            "Capsule Collider",
                            capsuleCollider.isTrigger,
                            capsuleCollider.providesContacts,
                            capsuleCollider.center,
                            capsuleCollider.radius,
                            capsuleCollider.height
                            ).gameObject);
                        break;

                    case MeshCollider meshCollider:
                        dynamicCards.Add(EditorCard_MeshCollider.Create(
                            cardContent.transform,
                            "Mesh Collider",
                            meshCollider.isTrigger,
                            meshCollider.providesContacts
                        ).gameObject);
                        break;

                    case Rigidbody rigidbody:
                        dynamicCards.Add(EditorCard_Rigidbody.Create(
                            cardContent.transform,
                            "Rigidbody",
                            rigidbody.mass,
                            rigidbody.drag,
                            rigidbody.angularDrag,
                            rigidbody.automaticCenterOfMass,
                            rigidbody.automaticInertiaTensor,
                            rigidbody.useGravity,
                            rigidbody.isKinematic
                        ).gameObject);
                        break;

                    // Supermarket Together Scripts
                    case BuildableInfo buildableInfo:
                        dynamicCards.Add(EditorCard_Custom_BuildableInfo.Create(
                            cardContent.transform,
                            "BuildableInfo (Script)",
                            buildableInfo.decorationID,
                            buildableInfo.cost,
                            buildableInfo.minY,
                            buildableInfo.maxY,
                            buildableInfo.isCool,
                            buildableInfo.energyCost,
                            buildableInfo.energyWorkingHours,
                            buildableInfo.employeeHappiness
                        ).gameObject);
                        break;

                    case PlayerObjectController playerObjectController:
                        dynamicCards.Add(EditorCard_Custom_PlayerObjectController.Create(
                            cardContent.transform,
                            "PlayerObjectController (Script)",
                            playerObjectController.ConnectionID,
                            playerObjectController.PlayerIdNumber,
                            playerObjectController.PlayerSteamID,
                            playerObjectController.PlayerName,
                            playerObjectController.PlayerSteamIDString
                        ).gameObject);
                        break;

                    case PlayerPermissions playerPermissions:
                        dynamicCards.Add(EditorCard_Custom_PlayerPermissions.Create(
                            cardContent.transform,
                            "PlayerPermissions (Script)",
                            playerPermissions.RequestGP(),
                            playerPermissions.RequestMP(),
                            playerPermissions.RequestCP(),
                            playerPermissions.RequestRP()
                        ).gameObject);
                        break;

                    case PlayerNetwork playerNetwork:
                        dynamicCards.Add(EditorCard_Custom_PlayerNetwork.Create(
                            cardContent.transform,
                            "PlayerNetwork (Script)",
                            playerNetwork.equippedItem,
                            playerNetwork.characterID,
                            playerNetwork.hatID,
                            playerNetwork.isCrouching // <- future: keep track of this since it may update
                        ).gameObject);
                        break;

                    case InteractableContainer interactableContainer:
                        dynamicCards.Add(EditorCard_Custom_InteractableContainer.Create(
                            cardContent.transform,
                            "InteractableContainer (Script)",
                            interactableContainer.isStorageShelf,
                            interactableContainer.isManufacturing
                        ).gameObject);
                        break;

                    case CardboardBaler cardboardBaler:
                        dynamicCards.Add(EditorCard_Custom_CardboardBaler.Create(
                            cardContent.transform,
                            "CardboardBaler (Script)",
                            cardboardBaler.numberOfBoxesInside,
                            cardboardBaler.isBroken,
                            cardboardBaler.brokenDay
                        ).gameObject);
                        break;

                    case HighlightEffect highlightEffect:
                        int cameraLayer = highlightEffect.camerasLayerMask;
                        int effectGroupLayer = highlightEffect.effectGroupLayer;
                        dynamicCards.Add(EditorCard_Custom_HighlightEffect.Create(
                            cardContent.transform,
                            "HighlightEffect (Script)",
                            highlightEffect.profileSync,
                            $"{LayerMask.LayerToName(cameraLayer)} ({cameraLayer})",
                            $"{LayerMask.LayerToName(effectGroupLayer)} ({effectGroupLayer})",
                            highlightEffect.effectNameFilter,
                            highlightEffect.highlighted,
                            highlightEffect.outline,
                            highlightEffect.outlineWidth,
                            highlightEffect.glow,
                            highlightEffect.glowWidth,
                            highlightEffect.isVisible
                        ).gameObject);
                        break;

                    case PlayerCrouch playerCrouch:
                        dynamicCards.Add(EditorCard_String.Create(
                            cardContent.transform,
                            "PlayerCrouch (Script)"
                            ).gameObject);
                        break;

                    case FirstPersonTransform firstPersonTransform:
                        dynamicCards.Add(EditorCard_String.Create(
                            cardContent.transform,
                            "FirstPersonTransform (Script)"
                            ).gameObject);
                        break;

                    // Starter Assets
                    case FirstPersonController fpsController:
                        int groundLayer = fpsController.GroundLayers;
                        dynamicCards.Add(EditorCard_StarterAssets_FirstPersonController.Create(
                            cardContent.transform,
                            "FirstPersonController (Script)",
                            fpsController.CrouchSpeed,
                            fpsController.MoveSpeed,
                            fpsController.SprintSpeed,
                            fpsController.airSpeed,
                            fpsController.SpeedChangeRate,
                            fpsController.JumpHeight,
                            fpsController.Gravity,
                            fpsController.JumpTimeout,
                            fpsController.FallTimeout,
                            fpsController.Grounded,
                            fpsController.GroundedOffset,
                            fpsController.GroundedRadius,
                            $"{LayerMask.LayerToName(groundLayer)} ({groundLayer})",
                            fpsController.allowPlayerInput,
                            fpsController.isTeleporting,
                            fpsController.inEvent,
                            fpsController.isBeingPushed,
                            fpsController.inCameraEvent,
                            fpsController.pushDirection,
                            fpsController.alwaysRun,
                            fpsController.inVehicle,
                            fpsController.isIndoors,
                            fpsController.IsCrouching
                        ).gameObject);
                        break;

                    // Mirror Scripts
                    case NetworkIdentity networkIdentity:
                        dynamicCards.Add(EditorCard_Mirror_NetworkIdentity.Create(
                            cardContent.transform,
                            "Network Identity",
                            networkIdentity.isServerOnly,
                            networkIdentity.assetId,
                            networkIdentity.netId,
                            networkIdentity.sceneId
                        ).gameObject);
                        break;

                    // Skip
                    case BoxColliderOutline _box:
                    case MeshColliderOutline _mesh:
                    case CapsuleColliderOutline _capsule:
                        break;

                    // Default
                    default:
                        dynamicCards.Add(EditorCard_String.Create(
                            cardContent.transform,
                            component.GetType().Name
                        ).gameObject);
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
