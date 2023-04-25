using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ItemContainerUI : MonoBehaviour
{
    public int id;
    public RawImage icon;
    public TextMeshProUGUI text;
    public PartType partType;
    BuilderManager builderManager;
    BuilderManagerUI builderManagerUI;
    

    private void Start()
    {
        builderManagerUI = GetComponentInParent<BuilderManagerUI>();
    }

    public void TogglePanelAction()
    {
        builderManagerUI.ToggleDescriptionPanel(id);
    }

    public void OnPartIconClicked()
    {
        builderManager.GetActivePrefab(id);
    }
    //this function enables/disables icons every time when the component is active or 
    //every time when player picks up item or remove the ingredient
    //or build something
    public void ItemIsAvailable()
    {
        builderManager = GetComponentInParent<BuilderManager>();
        if (builderManager != null)
        {
            var buildingPart = builderManager.buildingParts.FirstOrDefault(x => x.GetComponent<BuildingPart>().buildingPartSO.id == id).GetComponent<BuildingPart>();
            if (buildingPart != null)
            {
                icon.color = buildingPart.canAfford ? Color.white : Color.red;
                icon.GetComponent<Button>().enabled = buildingPart.canAfford;
            }
        }

    }

    private void OnEnable()
    {
        PlayerBuildingStore.onAvailabilityCheck += ItemIsAvailable;
        ItemIsAvailable();
    }

    private void OnDisable()
    {
        PlayerBuildingStore.onAvailabilityCheck -= ItemIsAvailable;
    }





}
