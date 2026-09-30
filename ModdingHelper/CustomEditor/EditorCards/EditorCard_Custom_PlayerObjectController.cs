using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_PlayerObjectController : EditorCardBase
    {
        private TextMeshProUGUI connectionIdText;
        private TextMeshProUGUI playerIdNumberText;
        private TextMeshProUGUI playerSteamIdText;
        private TextMeshProUGUI playerNameText;
        private TextMeshProUGUI playerSteamIdStringText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI connectionIdText, TextMeshProUGUI playerIdNumberText,
            TextMeshProUGUI playerSteamIdText, TextMeshProUGUI playerNameText, TextMeshProUGUI playerSteamIdStringText)
        {
            this.titleText = titleText;
            this.connectionIdText = connectionIdText;
            this.playerIdNumberText = playerIdNumberText;
            this.playerSteamIdText = playerSteamIdText;
            this.playerNameText = playerNameText;
            this.playerSteamIdStringText = playerSteamIdStringText;
        }

        public void SetConnectionId(int i) => UpdateStringText(connectionIdText, i.ToString());
        public void SetPlayerIdNumber(int i) => UpdateStringText(playerIdNumberText, i.ToString());
        public void SetPlayerSteamId(ulong u) => UpdateStringText(playerSteamIdText, u.ToString());
        public void SetPlayerName(string s) => UpdateStringText(playerNameText, s);
        public void SetPlayerSteamIdString(string s) => UpdateStringText(playerSteamIdText, s);
        public void SetValues(int connectionId, int playerIdNumber, ulong playerSteamId, string playerName, string playerSteamIdString)
        {
            SetConnectionId(connectionId);
            SetPlayerIdNumber(playerIdNumber);
            SetPlayerSteamId(playerSteamId);
            SetPlayerName(playerName);
            SetPlayerSteamIdString(playerSteamIdString);
        }

        public static EditorCard_Custom_PlayerObjectController Create(Transform contentParent, string title,
            int connectionId, int playerIdNumber, ulong playerSteamId, string playerName, string playerSteamIdString)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_PlayerObjectController", title);

            TextMeshProUGUI connectionIdTMP = EditorCardBuilder.CreateValueRow(body, "Connection ID");
            TextMeshProUGUI playerIdNumberTMP = EditorCardBuilder.CreateValueRow(body, "Player ID Number");
            TextMeshProUGUI playerSteamIdTMP = EditorCardBuilder.CreateValueRow(body, "Player Steam ID");
            TextMeshProUGUI playerNameTMP = EditorCardBuilder.CreateValueRow(body, "Player Name");
            TextMeshProUGUI playerSteamIdStringTMP = EditorCardBuilder.CreateValueRow(body, "Player Steam ID String");

            EditorCard_Custom_PlayerObjectController card = cardObj.AddComponent<EditorCard_Custom_PlayerObjectController>();
            card.Initialize(titleTMP, connectionIdTMP, playerIdNumberTMP, playerSteamIdTMP, playerNameTMP, playerSteamIdStringTMP);
            card.SetValues(connectionId, playerIdNumber, playerSteamId, playerName, playerSteamIdString);

            return card;
        }
    }
}
