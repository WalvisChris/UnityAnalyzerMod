using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_PlayerPermissions : EditorCardBase
    {
        private TextMeshProUGUI gpText; // RequestGP -> prevents "Open Builder"
        private TextMeshProUGUI mpText; // RequestMP -> prevents "Open Builder", "Painting", "Demolition", "Dismiss Employee", "Hire Employee"
        private TextMeshProUGUI cpText; // RequestCP -> prevents "Cash Register"
        private TextMeshProUGUI rpText; // RequestRP -> prevents any storage box interactions
        // private TextMeshProUGUI spText; // RequestSP -> never used
        // private TextMeshProUGUI tpText; // RequestTP -> never used

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI gpText, TextMeshProUGUI mpText, 
            TextMeshProUGUI cpText, TextMeshProUGUI rpText)
        {
            this.titleText = titleText;
            this.gpText = gpText;
            this.mpText = mpText;
            this.cpText = cpText;
            this.rpText = rpText;
        }

        public void SetGP(bool b) => UpdateBoolText(gpText, b);
        public void SetMP(bool b) => UpdateBoolText(mpText, b);
        public void SetCP(bool b) => UpdateBoolText(cpText, b);
        public void SetRP(bool b) => UpdateBoolText(rpText, b);
        public void SetValues(bool gp, bool mp, bool cp, bool rp)
        {
            SetGP(gp);
            SetMP(mp);
            SetCP(cp);
            SetRP(rp);
        }

        public static EditorCard_Custom_PlayerPermissions Create(Transform contentParent, string title,
            bool gp, bool mp, bool cp, bool rp)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_PlayerObjectController", title);

            TextMeshProUGUI gpTMP = EditorCardBuilder.CreateBoolRow(body, "G Permission");
            TextMeshProUGUI mpTMP = EditorCardBuilder.CreateBoolRow(body, "M Permission");
            TextMeshProUGUI cpTMP = EditorCardBuilder.CreateBoolRow(body, "C Permission");
            TextMeshProUGUI rpTMP = EditorCardBuilder.CreateBoolRow(body, "R Permission");

            EditorCard_Custom_PlayerPermissions card = cardObj.AddComponent<EditorCard_Custom_PlayerPermissions>();
            card.Initialize(titleTMP, gpTMP, mpTMP, cpTMP, rpTMP);
            card.SetValues(gp, mp, cp, rp);

            return card;
        }
    }
}
