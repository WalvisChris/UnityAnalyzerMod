using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_StarterAssets_FirstPersonController : EditorCardBase
    {
        private TextMeshProUGUI CrouchSpeedText;
        private TextMeshProUGUI MoveSpeedText;
        private TextMeshProUGUI SprintSpeedText;
        private TextMeshProUGUI airSpeedText;
        private TextMeshProUGUI SpeedChangeRateText;
        private TextMeshProUGUI JumpHeightText;
        private TextMeshProUGUI GravityText;
        private TextMeshProUGUI JumpTimeoutText;
        private TextMeshProUGUI FallTimeoutText;
        private TextMeshProUGUI GroundedText;
        private TextMeshProUGUI GroundedOffsetText;
        private TextMeshProUGUI GroundedRadiusText;
        private TextMeshProUGUI GroundLayersText;
        private TextMeshProUGUI allowPlayerInputText;
        private TextMeshProUGUI isTeleportingText;
        private TextMeshProUGUI inEventText;
        private TextMeshProUGUI isBeingPushedText;
        private TextMeshProUGUI inCameraEventText;
        private TextMeshProUGUI[] pushDirectionTexts = new TextMeshProUGUI[3];
        private TextMeshProUGUI alwaysRunText;
        private TextMeshProUGUI inVehicleText;
        private TextMeshProUGUI isIndoorsText;
        private TextMeshProUGUI isCrouchingText;

        public void Initialize(TextMeshProUGUI titleText,
            TextMeshProUGUI CrouchSpeedText,
            TextMeshProUGUI MoveSpeedText,
            TextMeshProUGUI SprintSpeedText,
            TextMeshProUGUI airSpeedText,
            TextMeshProUGUI SpeedChangeRateText,
            TextMeshProUGUI JumpHeightText,
            TextMeshProUGUI GravityText,
            TextMeshProUGUI JumpTimeoutText,
            TextMeshProUGUI FallTimeoutText,
            TextMeshProUGUI GroundedText,
            TextMeshProUGUI GroundedOffsetText,
            TextMeshProUGUI GroundedRadiusText,
            TextMeshProUGUI GroundLayersText,
            TextMeshProUGUI allowPlayerInputText,
            TextMeshProUGUI isTeleportingText,
            TextMeshProUGUI inEventText,
            TextMeshProUGUI isBeingPushedText,
            TextMeshProUGUI inCameraEventText,
            TextMeshProUGUI[] pushDirectionTexts,
            TextMeshProUGUI alwaysRunText,
            TextMeshProUGUI inVehicleText,
            TextMeshProUGUI isIndoorsText,
            TextMeshProUGUI isCrouchingText)
        {
            this.titleText = titleText;
            this.CrouchSpeedText = CrouchSpeedText;
            this.MoveSpeedText = MoveSpeedText;
            this.SprintSpeedText = SprintSpeedText;
            this.airSpeedText = airSpeedText;
            this.SpeedChangeRateText = SpeedChangeRateText;
            this.JumpHeightText = JumpHeightText;
            this.GravityText = GravityText;
            this.JumpTimeoutText = JumpTimeoutText;
            this.FallTimeoutText = FallTimeoutText;
            this.GroundedText = GroundedText;
            this.GroundedOffsetText = GroundedOffsetText;
            this.GroundedRadiusText = GroundedRadiusText;
            this.GroundLayersText = GroundLayersText;
            this.allowPlayerInputText = allowPlayerInputText;
            this.isTeleportingText = isTeleportingText;
            this.inEventText = inEventText;
            this.isBeingPushedText = isBeingPushedText;
            this.inCameraEventText = inCameraEventText;
            this.pushDirectionTexts = pushDirectionTexts;
            this.alwaysRunText = alwaysRunText;
            this.inVehicleText = inVehicleText;
            this.isIndoorsText = isIndoorsText;
            this.isCrouchingText = isCrouchingText;
        }

        public void SetCrouchSpeed(float f) => UpdateStringText(CrouchSpeedText, f.ToString("F2"));
        public void SetMoveSpeed(float f) => UpdateStringText(MoveSpeedText, f.ToString("F2"));
        public void SetSprintSpeed(float f) => UpdateStringText(SprintSpeedText, f.ToString("F2"));
        public void SetAirSpeed(float f) => UpdateStringText(airSpeedText, f.ToString("F2"));
        public void SetSpeedChangeRate(float f) => UpdateStringText(SpeedChangeRateText, f.ToString("F2"));
        public void SetJumpHeight(float f) => UpdateStringText(JumpHeightText, f.ToString("F2"));
        public void SetGravity(float f) => UpdateStringText(GravityText, f.ToString("F2"));
        public void SetJumpTimeout(float f) => UpdateStringText(JumpTimeoutText, f.ToString("F2"));
        public void SetFallTimeout(float f) => UpdateStringText(FallTimeoutText, f.ToString("F2"));
        public void SetGrounded(bool b) => UpdateBoolText(GroundedText, b);
        public void SetGroundedOffset(float f) => UpdateStringText(GroundedOffsetText, f.ToString("F2"));
        public void SetGroundedRadius(float f) => UpdateStringText(GroundedRadiusText, f.ToString("F2"));
        public void SetGroundLayers(string s) => UpdateStringText(GroundLayersText, s.ToString());
        public void SetAllowPlayerInput(bool b) => UpdateBoolText(allowPlayerInputText, b);
        public void SetIsTeleporting(bool b) => UpdateBoolText(isTeleportingText, b);
        public void SetInEvent(bool b) => UpdateBoolText(inEventText, b);
        public void SetIsBeingPushed(bool b) => UpdateBoolText(isBeingPushedText, b);
        public void SetInCameraEvent(bool b) => UpdateBoolText(inCameraEventText, b);
        public void SetPushDirection(Vector3 v) => UpdateVectorTexts(pushDirectionTexts, v);
        public void SetAlwaysRun(bool b) => UpdateBoolText(alwaysRunText, b);
        public void SetInVehicle(bool b) => UpdateBoolText(inVehicleText, b);
        public void SetIsIndoors(bool b) => UpdateBoolText(isIndoorsText, b);
        public void SetIsCrouching(bool b) => UpdateBoolText(isCrouchingText, b);

        public void SetValues(float crouchSpeed,
            float moveSpeed,
            float sprintSpeed,
            float airSpeed,
            float speedChangeRate,
            float jumpHeight,
            float gravity,
            float jumpTimeout,
            float fallTimeout,
            bool grounded,
            float groundedOffset,
            float groundedRadius,
            string groundLayers,
            bool allowPlayerInput,
            bool isTeleporting,
            bool inEvent,
            bool isBeingPushed,
            bool inCameraEvent,
            Vector3 pushDirection,
            bool alwaysRun,
            bool inVehicle,
            bool isIndoors,
            bool isCrouching)
        {
            SetCrouchSpeed(crouchSpeed);
            SetMoveSpeed(moveSpeed);
            SetSprintSpeed(sprintSpeed);
            SetAirSpeed(airSpeed);
            SetSpeedChangeRate(speedChangeRate);
            SetJumpHeight(jumpHeight);
            SetGravity(gravity);
            SetJumpTimeout(jumpTimeout);
            SetFallTimeout(fallTimeout);
            SetGrounded(grounded);
            SetGroundedOffset(groundedOffset);
            SetGroundedRadius(groundedRadius);
            SetGroundLayers(groundLayers);
            SetAllowPlayerInput(allowPlayerInput);
            SetIsTeleporting(isTeleporting);
            SetInEvent(inEvent);
            SetIsBeingPushed(isBeingPushed);
            SetInCameraEvent(inCameraEvent);
            SetPushDirection(pushDirection);
            SetAlwaysRun(alwaysRun);
            SetInVehicle(inVehicle);
            SetIsIndoors(isIndoors);
            SetIsCrouching(isCrouching);
        }

        public static EditorCard_StarterAssets_FirstPersonController Create(Transform contentParent,
            string title,
            float crouchSpeed,
            float moveSpeed,
            float sprintSpeed,
            float airSpeed,
            float speedChangeRate,
            float jumpHeight,
            float gravity,
            float jumpTimeout,
            float fallTimeout,
            bool grounded,
            float groundedOffset,
            float groundedRadius,
            string groundLayers,
            bool allowPlayerInput,
            bool isTeleporting,
            bool inEvent,
            bool isBeingPushed,
            bool inCameraEvent,
            Vector3 pushDirection,
            bool alwaysRun,
            bool inVehicle,
            bool isIndoors,
            bool isCrouching)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_FirstPersonController", title);

            TextMeshProUGUI crouchSpeedTMP = EditorCardBuilder.CreateValueRow(body, "Crouch Speed");
            TextMeshProUGUI moveSpeedTMP = EditorCardBuilder.CreateValueRow(body, "Move Speed");
            TextMeshProUGUI sprintSpeedTMP = EditorCardBuilder.CreateValueRow(body, "Sprint Speed");
            TextMeshProUGUI airSpeedTMP = EditorCardBuilder.CreateValueRow(body, "Air Speed");
            TextMeshProUGUI speedChangeRateTMP = EditorCardBuilder.CreateValueRow(body, "Speed Change Rate");
            TextMeshProUGUI jumpHeightTMP = EditorCardBuilder.CreateValueRow(body, "Jump Height");
            TextMeshProUGUI gravityTMP = EditorCardBuilder.CreateValueRow(body, "Gravity");
            TextMeshProUGUI jumpTimeoutTMP = EditorCardBuilder.CreateValueRow(body, "Jump Timeout");
            TextMeshProUGUI fallTimeoutTMP = EditorCardBuilder.CreateValueRow(body, "Fall Timeout");
            TextMeshProUGUI groundedTMP = EditorCardBuilder.CreateBoolRow(body, "Grounded");
            TextMeshProUGUI groundedOffsetTMP = EditorCardBuilder.CreateValueRow(body, "Grounded Offset");
            TextMeshProUGUI groundedRadiusTMP = EditorCardBuilder.CreateValueRow(body, "Grounded Radius");
            TextMeshProUGUI groundLayersTMP = EditorCardBuilder.CreateValueRow(body, "Ground Layers");
            TextMeshProUGUI allowPlayerInputTMP = EditorCardBuilder.CreateBoolRow(body, "Allow Player Input");
            TextMeshProUGUI isTeleportingTMP = EditorCardBuilder.CreateBoolRow(body, "Is Teleporting");
            TextMeshProUGUI inEventTMP = EditorCardBuilder.CreateBoolRow(body, "In Event");
            TextMeshProUGUI isBeingPushedTMP = EditorCardBuilder.CreateBoolRow(body, "Is Being Pushed");
            TextMeshProUGUI inCameraEventTMP = EditorCardBuilder.CreateBoolRow(body, "In Camera Event");
            TextMeshProUGUI[] pushDirectionTMP = EditorCardBuilder.CreateVectorRow(body, "Push Direction");
            TextMeshProUGUI alwaysRunTMP = EditorCardBuilder.CreateBoolRow(body, "Always Run");
            TextMeshProUGUI inVehicleTMP = EditorCardBuilder.CreateBoolRow(body, "In Vehicle");
            TextMeshProUGUI isIndoorsTMP = EditorCardBuilder.CreateBoolRow(body, "Is Indoors");
            TextMeshProUGUI isCrouchingTMP = EditorCardBuilder.CreateBoolRow(body, "Is Crouching");

            EditorCard_StarterAssets_FirstPersonController card = cardObj.AddComponent<EditorCard_StarterAssets_FirstPersonController>();
            card.Initialize(titleTMP, crouchSpeedTMP, moveSpeedTMP, sprintSpeedTMP, airSpeedTMP, speedChangeRateTMP, jumpHeightTMP, gravityTMP, jumpTimeoutTMP, fallTimeoutTMP, groundedTMP, groundedOffsetTMP, groundedRadiusTMP, groundLayersTMP, allowPlayerInputTMP, isTeleportingTMP, inEventTMP, isBeingPushedTMP, inCameraEventTMP, pushDirectionTMP, alwaysRunTMP, inVehicleTMP, isIndoorsTMP, isCrouchingTMP);
            card.SetValues(crouchSpeed, moveSpeed, sprintSpeed, airSpeed, speedChangeRate, jumpHeight, gravity, jumpTimeout, fallTimeout, grounded, groundedOffset, groundedRadius, groundLayers, allowPlayerInput, isTeleporting, inEvent, isBeingPushed, inCameraEvent, pushDirection, alwaysRun, inVehicle, isIndoors, isCrouching);

            return card;
        }
    }
}