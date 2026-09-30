using TMPro;
using UnityEngine;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_BuildableInfo : EditorCardBase
    {
        private TextMeshProUGUI decorationIdText;
        private TextMeshProUGUI costText;
        private TextMeshProUGUI minYText;
        private TextMeshProUGUI maxYText;
        private TextMeshProUGUI coolText;
        private TextMeshProUGUI energyCostText;
        private TextMeshProUGUI energyWorkingHoursText;
        private TextMeshProUGUI employeeHappinessText;

        public void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI decorationIdText, TextMeshProUGUI costText,
            TextMeshProUGUI minYText, TextMeshProUGUI maxYText, TextMeshProUGUI coolText, 
            TextMeshProUGUI energyCostText, TextMeshProUGUI energyWorkingHoursText, TextMeshProUGUI employeeHappinessText)
        {
            this.titleText = titleText;
            this.decorationIdText = decorationIdText;
            this.minYText = minYText;
            this.maxYText = maxYText;
            this.coolText = coolText;
            this.energyCostText = energyCostText;
            this.energyWorkingHoursText = energyWorkingHoursText;
            this.employeeHappinessText = employeeHappinessText;
        }

        public void SetDecorationId(int i) => UpdateStringText(decorationIdText, i.ToString());
        public void SetCost(float f) => UpdateStringText(costText, f.ToString("F2"));
        public void SetMinY(float f) => UpdateStringText(minYText, f.ToString("F2"));
        public void SetMaxY(float f) => UpdateStringText(maxYText, f.ToString("F2"));
        public void SetCool(bool b) => UpdateBoolText(coolText, b);
        public void SetEnergyCost(float f) => UpdateStringText(energyCostText, f.ToString("F2"));
        public void SetEnergyWorkingHours(float f) => UpdateStringText(energyWorkingHoursText, f.ToString("F2"));
        public void SetEmployeeHappiness(float f) => UpdateStringText(employeeHappinessText, f.ToString("F2"));
        public void SetValues(int decorationId, float cost, float minY, float maxY, bool isCool, float energyCost, float energyWorkingHours, float employeehappiness)
        {
            SetDecorationId(decorationId);
            SetCost(cost);
            SetMinY(minY);
            SetMaxY(maxY);
            SetCool(isCool);
            SetEnergyCost(cost);
            SetEnergyWorkingHours(energyWorkingHours);
            SetEmployeeHappiness(employeehappiness);
        }

        public static EditorCard_Custom_BuildableInfo Create(Transform contentParent, string title,
            int decorationId, float cost, float minY, float maxY, bool isCool, float energyCost, float energyWorkingHours, float employeeHappiness)
        {
            var (cardObj, body, titleTMP) = EditorCardBuilder.CreateBaseCard(contentParent, "Card_Custom_BuildableInfo", title);

            TextMeshProUGUI decorationTMP = EditorCardBuilder.CreateValueRow(body, "Decoration ID");
            TextMeshProUGUI costTMP = EditorCardBuilder.CreateValueRow(body, "Cost");
            TextMeshProUGUI minYTMP = EditorCardBuilder.CreateValueRow(body, "Min Y");
            TextMeshProUGUI maxYTMP = EditorCardBuilder.CreateValueRow(body, "Max Y");
            TextMeshProUGUI coolTMP = EditorCardBuilder.CreateBoolRow(body, "Is Cool");
            TextMeshProUGUI energyCostTMP = EditorCardBuilder.CreateValueRow(body, "Energy Cost");
            TextMeshProUGUI energyWorkingHoursTMP = EditorCardBuilder.CreateValueRow(body, "Energy Working Hours");
            TextMeshProUGUI employeeHappinessTMP = EditorCardBuilder.CreateValueRow(body, "Employee Happiness");

            EditorCard_Custom_BuildableInfo card = cardObj.AddComponent<EditorCard_Custom_BuildableInfo>();
            card.Initialize(titleTMP, decorationTMP, costTMP, minYTMP, maxYTMP, coolTMP, energyCostTMP, energyWorkingHoursTMP, employeeHappinessTMP);
            card.SetValues(decorationId, cost, minY, maxY, isCool, energyCost, energyWorkingHours, employeeHappiness);

            return card;
        }
    }
}
