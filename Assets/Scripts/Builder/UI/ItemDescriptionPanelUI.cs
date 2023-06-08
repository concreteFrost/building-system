
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPanelUI : MonoBehaviour
{
    public RawImage itemIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    private BuilderMenuUI builderManagerUI;
    public Part currentPart;

    private void Awake()
    {
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
        descriptionText.text = currentPart.partSO.description;   
       
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
    }

    


}
