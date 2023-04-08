using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuilderManagerUI : MonoBehaviour
{
    public RectTransform centerContainer;
    public BuilderStore store;
    private List<GameObject> buildings = new List<GameObject>();   
    
    public GameObject itemContainer;
    public GameObject canvas;
    public GameObject descriptionPanel;
    
    public bool isDescriptionPanelActive;

    int currentActiveId;

    private void Awake()
    {
        descriptionPanel.SetActive(false);
        
        store.parts.Sort((o1, o2) =>  o1.name.CompareTo(o2.name));  

        store.parts.ForEach(x =>
        {
            GameObject _item = Instantiate(itemContainer);
            var itemData = x.GetComponent<BuildingPart>().buildingPartSO;
            _item.GetComponent<ItemContainerUI>().id = itemData.id;
            _item.GetComponent<ItemContainerUI>().icon.texture = itemData.icon.texture;
            _item.GetComponent<ItemContainerUI>().text.text = itemData.name;
            _item.GetComponent<ItemContainerUI>().partType = itemData.partType;
            _item.name = itemData.name;
            _item.transform.SetParent(centerContainer.transform);
            buildings.Add(_item);
        });
    }

    public void GetBuildingModeState(bool inBuildingMode)
    {
        canvas.SetActive(inBuildingMode);
            
    }


    public void GetActiveSection(string type)
    {
        buildings.ForEach(x => x.SetActive(true));
        var filtered = buildings.Where(x => x.GetComponent<ItemContainerUI>().partType.ToString() != type);
        filtered.ToList().ForEach(x => x.SetActive(false));

    }

    public void GetAllSections()
    {
        buildings.ForEach(x => x.SetActive(true));
    }

    public void ToggleDescriptionPanel(int id)
    {

        if (currentActiveId == id && isDescriptionPanelActive)
            ShowDescriptionPanel(false);

        else
        {
            ShowDescriptionPanel(true);
            currentActiveId = id;
        }


        if (isDescriptionPanelActive)
        {
            ShowDescriptionPanel(true);
            var partToShow = store.parts.Find(x => x.GetComponent<BuildingPart>().buildingPartSO.id == id).GetComponent<BuildingPart>();
            descriptionPanel.GetComponent<BuilderItemDescriptionPanelUI>().FillUpCurrentItemInfo(partToShow);

        }

        else
            ShowDescriptionPanel(false);
  
    }

    public void ShowDescriptionPanel(bool isShowing)
    {
        descriptionPanel.SetActive(isShowing);
        isDescriptionPanelActive = isShowing;
    }


   


}
