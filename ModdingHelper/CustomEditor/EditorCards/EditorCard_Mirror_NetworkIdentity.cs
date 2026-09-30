using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Mirror_NetworkIdentity : EditorCardBase
    {
        private TextMeshProUGUI serverText;
        private TextMeshProUGUI assetIdText;
        private TextMeshProUGUI netIdText;
        private TextMeshProUGUI sceneIdText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI serverText, TextMeshProUGUI assetIdText, 
            TextMeshProUGUI netIdText, TextMeshProUGUI sceneIdText)
        {
            this.titleText = titleText;
            this.serverText = serverText;
            this.assetIdText = assetIdText;
            this.netIdText = netIdText;
            this.sceneIdText = sceneIdText;
        }

        public void SetServer(bool b) => UpdateBoolText(serverText, b);
        public void SetAssetId(uint u) => UpdateStringText(assetIdText, u.ToString());
        public void SetNetId(uint u) => UpdateStringText(netIdText, u.ToString());
        public void SetSceneId(ulong u) => UpdateStringText(sceneIdText, u.ToString());
        public void SetValues(bool serverOnly, uint assetId, uint netId, ulong sceneId)
        {
            SetServer(serverOnly);
            SetAssetId(assetId);
            SetNetId(netId);
            SetSceneId(sceneId);
        }

        public static EditorCard_Mirror_NetworkIdentity Create(Transform contentParent, string title,
            bool serverOnly, uint assetId, uint netId, ulong sceneId)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Mirror_NetworkIdentity", title);

            TextMeshProUGUI serverTMP = EditorCardBuilder.CreateBoolRow(body, "Server Only");
            TextMeshProUGUI assetIdTMP = EditorCardBuilder.CreateValueRow(body, "Asset ID");
            TextMeshProUGUI netIdTMP = EditorCardBuilder.CreateValueRow(body, "Net ID");
            TextMeshProUGUI sceneIdTMP = EditorCardBuilder.CreateValueRow(body, "Scene ID");

            EditorCard_Mirror_NetworkIdentity card = cardObj.AddComponent<EditorCard_Mirror_NetworkIdentity>();
            card.Initialize(titleTMP, serverTMP, assetIdTMP, netIdTMP, sceneIdTMP);
            card.SetValues(serverOnly, assetId, netId, sceneId);

            return card;
        }
    }
}
