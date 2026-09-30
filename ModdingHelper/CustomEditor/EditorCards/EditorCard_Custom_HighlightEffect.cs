using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_HighlightEffect : EditorCardBase
    {
        private TextMeshProUGUI profileSyncText;
        private TextMeshProUGUI camerasLayerMaskText;
        private TextMeshProUGUI effectGroupLayerText;
        private TextMeshProUGUI effectNameFilterText;
        private TextMeshProUGUI _highlightedText;
        private TextMeshProUGUI outlineIntensityText;
        private TextMeshProUGUI outlineWidthText;
        private TextMeshProUGUI glowIntensityText;
        private TextMeshProUGUI glowWidthText;
        private TextMeshProUGUI isVisibleText;

        public void Initialize(TextMeshProUGUI titleText,
            TextMeshProUGUI profileSyncText,
            TextMeshProUGUI camerasLayerMaskText,
            TextMeshProUGUI effectGroupLayerText,
            TextMeshProUGUI effectNameFilterText,
            TextMeshProUGUI _highlightedText,
            TextMeshProUGUI outlineIntensityText,
            TextMeshProUGUI outlineWidthText,
            TextMeshProUGUI glowIntensityText,
            TextMeshProUGUI glowWidthText,
            TextMeshProUGUI isVisibleText)
        {
            this.titleText = titleText;
            this.profileSyncText = profileSyncText;
            this.camerasLayerMaskText = camerasLayerMaskText;
            this.effectGroupLayerText = effectGroupLayerText;
            this.effectNameFilterText = effectNameFilterText;
            this._highlightedText = _highlightedText;
            this.outlineIntensityText = outlineIntensityText;
            this.outlineWidthText = outlineWidthText;
            this.glowIntensityText = glowIntensityText;
            this.glowWidthText = glowWidthText;
            this.isVisibleText = isVisibleText;
        }

        public void SetProfileSync(bool b) => UpdateBoolText(profileSyncText, b);
        public void SetCamerasLayerMask(string s) => UpdateStringText(camerasLayerMaskText, s.ToString());
        public void SetEffectGroupLayer(string s) => UpdateStringText(effectGroupLayerText, s.ToString());
        public void SetEffectNameFilter(string s) => UpdateStringText(effectNameFilterText, s.ToString());
        public void Set_highlighted(bool b) => UpdateBoolText(_highlightedText, b);
        public void SetOutlineIntensity(float f) => UpdateStringText(outlineIntensityText, f.ToString("F2"));
        public void SetOutlineWidth(float f) => UpdateStringText(outlineWidthText, f.ToString("F2"));
        public void SetGlowIntensity(float f) => UpdateStringText(glowIntensityText, f.ToString("F2"));
        public void SetGlowWidth(float f) => UpdateStringText(glowWidthText, f.ToString("F2"));
        public void SetIsVisible(bool b) => UpdateBoolText(isVisibleText, b);

        public void SetValues(bool profileSync,
            string camerasLayerMask,
            string effectGroupLayer,
            string effectNameFilter,
            bool _highlighted,
            float outlineIntensity,
            float outlineWidth,
            float glowIntensity,
            float glowWidth,
            bool isVisible)
        {
            SetProfileSync(profileSync);
            SetCamerasLayerMask(camerasLayerMask);
            SetEffectGroupLayer(effectGroupLayer);
            SetEffectNameFilter(effectNameFilter);
            Set_highlighted(_highlighted);
            SetOutlineIntensity(outlineIntensity);
            SetOutlineWidth(outlineWidth);
            SetGlowIntensity(glowIntensity);
            SetGlowWidth(glowWidth);
            SetIsVisible(isVisible);
        }

        public static EditorCard_Custom_HighlightEffect Create(Transform contentParent,
            string title,
            bool profileSync,
            string camerasLayerMask,
            string effectGroupLayer,
            string effectNameFilter,
            bool _highlighted,
            float outlineIntensity,
            float outlineWidth,
            float glowIntensity,
            float glowWidth,
            bool isVisible)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_HighlightEffect", title);

            TextMeshProUGUI profileSyncTMP = EditorCardBuilder.CreateBoolRow(body, "Profile Sync");
            TextMeshProUGUI camerasLayerMaskTMP = EditorCardBuilder.CreateValueRow(body, "Cameras Layer Mask");
            TextMeshProUGUI effectGroupLayerTMP = EditorCardBuilder.CreateValueRow(body, "Effect Group Layer");
            TextMeshProUGUI effectNameFilterTMP = EditorCardBuilder.CreateValueRow(body, "Effect Name Filter");
            TextMeshProUGUI _highlightedTMP = EditorCardBuilder.CreateBoolRow(body, " Highlighted");
            TextMeshProUGUI outlineIntensityTMP = EditorCardBuilder.CreateValueRow(body, "Outline Intensity");
            TextMeshProUGUI outlineWidthTMP = EditorCardBuilder.CreateValueRow(body, "Outline Width");
            TextMeshProUGUI glowIntensityTMP = EditorCardBuilder.CreateValueRow(body, "Glow Intensity");
            TextMeshProUGUI glowWidthTMP = EditorCardBuilder.CreateValueRow(body, "Glow Width");
            TextMeshProUGUI isVisibleTMP = EditorCardBuilder.CreateBoolRow(body, "Is Visible");

            EditorCard_Custom_HighlightEffect card = cardObj.AddComponent<EditorCard_Custom_HighlightEffect>();
            card.Initialize(titleTMP, profileSyncTMP, camerasLayerMaskTMP, effectGroupLayerTMP, effectNameFilterTMP, _highlightedTMP, outlineIntensityTMP, outlineWidthTMP, glowIntensityTMP, glowWidthTMP, isVisibleTMP);
            card.SetValues(profileSync, camerasLayerMask, effectGroupLayer, effectNameFilter, _highlighted, outlineIntensity, outlineWidth, glowIntensity, glowWidth, isVisible);

            return card;
        }
    }
}