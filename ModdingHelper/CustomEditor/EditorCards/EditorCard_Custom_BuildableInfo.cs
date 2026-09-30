using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdingHelper.CustomEditor.EditorCards
{
    internal class EditorCard_Custom_BuildableInfo : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI decorationIdText;
        private TextMeshProUGUI costText;
        private TextMeshProUGUI minYText;
        private TextMeshProUGUI maxYText;
        private TextMeshProUGUI isCoolText;
        private TextMeshProUGUI energyCostText;
        private TextMeshProUGUI energyWorkingHoursText;
        private TextMeshProUGUI employeeHappinessText;
        private string GetColoredBoolean(bool b) { return (b) ? $"<#00FF00>{b}</color>" : $"<#FF0000>{b}</color>"; }
        private void Initialize(TextMeshProUGUI titleText, TextMeshProUGUI decorationIdText, TextMeshProUGUI costText, TextMeshProUGUI minYText, TextMeshProUGUI maxYText, TextMeshProUGUI isCoolText, TextMeshProUGUI energyCostText, TextMeshProUGUI energyWorkingHoursText, TextMeshProUGUI employeeHappinessText)
        {
            this.titleText = titleText;
            this.decorationIdText = decorationIdText;
            this.costText = costText;
            this.minYText = minYText;
            this.maxYText = maxYText;
            this.isCoolText = isCoolText;
            this.energyCostText = energyCostText;
            this.energyWorkingHoursText = energyWorkingHoursText;
            this.employeeHappinessText = employeeHappinessText;
        }
        public void SetTitle(string s) { if (titleText != null) titleText.text = s; }
        public void SetDecorationId(int i) { if (decorationIdText != null) decorationIdText.text = $"Decoration ID: {i}"; }
        public void SetCost(float f) { if (costText != null) costText.text = $"Cost: {f.ToString("F2")}"; }
        public void SetMinY(float f) { if (minYText != null) minYText.text = $"Min Y: {f.ToString("F2")}"; }
        public void SetMaxY(float f) { if (maxYText != null) maxYText.text = $"Max Y: {f.ToString("F2")}"; }
        public void SetIsCool(bool b) { if (isCoolText != null) isCoolText.text = $"Is Cool: {GetColoredBoolean(b)}"; }
        public void SetEnergyCost(float f) { if (energyCostText != null) energyCostText.text = $"Energy Cost: {f.ToString("F2")}"; }
        public void SetEnergyWorkingHours(float f) { if (energyWorkingHoursText != null) energyWorkingHoursText.text = $"Energy Working Hours: {f.ToString("F2")}"; }
        public void SetEmployeeHappiness(float f) { if (employeeHappinessText != null) employeeHappinessText.text = $"Employee Happiness: {f.ToString("F2")}"; }
        public void SetValues(int decorationId, float cost, float minY, float maxY, bool isCool, float energyCost, float energyWorkingHours, float employeeHappiness)
        {
            SetDecorationId(decorationId);
            SetCost(cost);
            SetMinY(minY);
            SetMaxY(maxY);
            SetIsCool(isCool);
            SetEnergyCost(energyCost);
            SetEnergyWorkingHours(energyWorkingHours);
            SetEmployeeHappiness(employeeHappiness);
        }
        public void Show() { gameObject.SetActive(true); }
        public void Hide() { gameObject.SetActive(false); }
        public static EditorCard_Custom_BuildableInfo Create(Transform parent, string title, int? decorationId = 0, float? cost = 0f, float? minY = 0f, float? maxY = 0f, bool? isCool = false, float? energyCost = 0f, float? energyWorkingHours = 0f, float? employeeHappiness = 0f)
        {         
            GameObject cardObj = new GameObject("Card_Custom_BuildableInfo");
            cardObj.transform.SetParent(parent, false);

            Image bg = cardObj.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.05f, 0.05f);

            //ContentSizeFitter fitter = cardObj.AddComponent<ContentSizeFitter>();
            //fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

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
            TextMeshProUGUI decorationIdTMP = CreateVectorLine("DecorationId");
            TextMeshProUGUI costTMP = CreateVectorLine("Cost");
            TextMeshProUGUI minYTMP = CreateVectorLine("MinY");
            TextMeshProUGUI maxYTMP = CreateVectorLine("MaxY");
            TextMeshProUGUI isCoolTMP = CreateVectorLine("IsCool");
            TextMeshProUGUI energyCostTMP = CreateVectorLine("EnergyCost");
            TextMeshProUGUI energyWorkingHoursTMP = CreateVectorLine("EnergyWorkingHours");
            TextMeshProUGUI employeeHappinessTMP = CreateVectorLine("EmployeeHappiness");

            // 4. Attach & Initialize Component
            EditorCard_Custom_BuildableInfo card = cardObj.AddComponent<EditorCard_Custom_BuildableInfo>();
            card.Initialize(titleTMP, decorationIdTMP, costTMP, minYTMP, maxYTMP, isCoolTMP, energyCostTMP, energyWorkingHoursTMP, employeeHappinessTMP);

            // Apply default values
            card.SetValues(
                decorationId ?? 0,
                cost ?? 0f,
                minY ?? 0f,
                maxY ?? 0f,
                isCool ?? false,
                energyCost ?? 0f,
                energyWorkingHours ?? 0f,
                employeeHappiness ?? 0f
            );

            return card;
        }
    }
}
