using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BuilderMenuUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform centerContainer;
    private BuilderStore store;

    private List<GameObject> buildingItemContainerList = new List<GameObject>();

    public GameObject itemContainer;
    public GameObject canvas;
    public GameObject activeBuildingModeCanvas;
    public GameObject descriptionPanel;
    public bool isDescriptionPanelActive;

    private void Awake()
    {
        store = GetComponentInParent<BuilderStore>();
        StorePartsInGrid();
        ShowDescriptionPanel(false);
    }

    public void GetActiveSection(string type)
    {
        buildingItemContainerList.ForEach(x => x.SetActive(true));
        var filtered = buildingItemContainerList.Where(x => x.GetComponent<ItemContainerUI>().partType.ToString() != type);
        filtered.ToList().ForEach(x => x.SetActive(false));
    }

    public void GetAllSections()
    {
        buildingItemContainerList.ForEach(x => x.SetActive(true));
    }

    public void ToggleDescriptionPanel(int id)
    {
        ShowDescriptionPanel(true);
        var partToShow = store.parts.Find(x => x.GetComponent<BuildingPart>().buildingPartSO.id == id).GetComponent<BuildingPart>();
        descriptionPanel.GetComponent<BuilderItemDescriptionPanelUI>().FillUpCurrentItemInfo(partToShow);
    }

    public void ShowDescriptionPanel(bool isShowing)
    {
        descriptionPanel.SetActive(isShowing);    
        isDescriptionPanelActive = isShowing;
    }

    private void StorePartsInGrid()
    {
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
            buildingItemContainerList.Add(_item);
        });
    }

    public void ShowBuildingModeCanvas(bool isVisible)
    {
        activeBuildingModeCanvas.SetActive(isVisible);
    }

    public void ShowMainCanvas(bool isVisible)
    {
        canvas.SetActive(isVisible);
    }





}
