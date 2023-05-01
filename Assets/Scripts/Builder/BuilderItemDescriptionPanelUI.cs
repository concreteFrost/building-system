
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
    public Part currentPart;

    private void Awake()
    {
        playerStore = GetComponentInParent<PlayerBuildingStore>();
        builderManagerUI = GetComponentInParent<BuilderMenuUI>();
    }
    //this function is called when we toggle description panel.
    //it also updates when we pick up the new ingredient
    public void FillUpCurrentItemInfo(Part part)
    {
        ResetInfo();
        currentPart = part;
        itemIcon.texture = currentPart.partSO.icon.texture;
        nameText.text = currentPart.partSO.name;
        descriptionText.text = "Description: " + currentPart.partSO.description;

        currentPart.partSO.ingredients.ForEach(x => componentsRequiredText.text += "\n"+ x.ingredient.ingredientType + ": " + x.quantity ) ;

        currentPart.partSO.ingredients.ForEach(x =>
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
