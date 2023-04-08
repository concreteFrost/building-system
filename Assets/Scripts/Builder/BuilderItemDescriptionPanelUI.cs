
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class BuilderItemDescriptionPanelUI : MonoBehaviour
{
    public RawImage itemIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI componentsRequiredText;
    public TextMeshProUGUI playerComponentsText;
    private BuilderManagerUI builderManagerUI;

    private void Start()
    {
        builderManagerUI = GetComponentInParent<BuilderManagerUI>();
    }

    // Start is called before the first frame update
    public void FillUpCurrentItemInfo(BuildingPart part)
    {
        ResetInfo();

        itemIcon.texture = part.buildingPartSO.icon.texture;
        nameText.text = part.buildingPartSO.name;
        descriptionText.text = "Description: " + part.buildingPartSO.description;

        part.buildingPartSO.ingredients.ForEach(x => componentsRequiredText.text += x.ingredientType + ": " + x.quantity + "\n");
    }

    public void ClosePanel()
    {
        builderManagerUI.ShowDescriptionPanel(false);
      
    }

    void ResetInfo()
    {
        itemIcon.texture = null;
        nameText.text = null;
        descriptionText.text = "";
        componentsRequiredText.text = "Components Required: ";
    }


}
