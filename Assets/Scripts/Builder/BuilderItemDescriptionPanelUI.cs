
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuilderItemDescriptionPanelUI : MonoBehaviour
{
    public RawImage itemIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI componentsRequiredText;
    public TextMeshProUGUI playerComponentsText;
    private BuilderMenuUI builderManagerUI;
    private PlayerBuildingStore playerStore;
    public BuildingPart currentPart;

    private void Awake()
    {
        playerStore = GetComponentInParent<PlayerBuildingStore>();
        builderManagerUI = GetComponentInParent<BuilderMenuUI>();
    }
    //this function is called when we toggle description panel.
    //it also updates when we pick up the new ingredient
    public void FillUpCurrentItemInfo(BuildingPart part)
    {
        ResetInfo();
        currentPart = part;
        itemIcon.texture = currentPart.buildingPartSO.icon.texture;
        nameText.text = currentPart.buildingPartSO.name;
        descriptionText.text = "Description: " + currentPart.buildingPartSO.description;

        currentPart.buildingPartSO.ingredients.ForEach(x => componentsRequiredText.text += "\n"+ x.ingredient.ingredientType + ": " + x.quantity ) ;

        currentPart.buildingPartSO.ingredients.ForEach(x =>
        {
            playerStore.ingredients.ForEach(y =>
            {
                if(y.ingredient.ingredientType == x.ingredient.ingredientType)
                {
                    playerComponentsText.text += "\n" + y.ingredient.ingredientType + ":" + y.quantity;
                }       
            });
        });
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
        playerComponentsText.text = "You have: ";
    }

    


}
