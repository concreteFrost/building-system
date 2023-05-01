using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BuilderMenuUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform centerContainer;
    private BuilderStore store;

    private List<GameObject> itemContainerList = new List<GameObject>();

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

    private void Start()
    {
        GetSection("building");
    }

    public void GetSubSection(string type)
    {
        itemContainerList.ForEach(x => x.SetActive(true));
        var filtered = itemContainerList.Where(x => x.GetComponent<ItemContainerUI>().subSectionType.ToString() != type);
        filtered.ToList().ForEach(x => x.SetActive(false));
    }

    public void GetSection(string type)
    {
        itemContainerList.ForEach(x => x.SetActive(true));
        var filtered = itemContainerList.Where(x => x.GetComponent<ItemContainerUI>().sectionType.ToString() != type);
        filtered.ToList().ForEach(x => x.SetActive(false));
    }

    public void ToggleDescriptionPanel(int id)
    {
        ShowDescriptionPanel(true);
        var partToShow = store.parts.Find(x => x.GetComponent<Part>().partSO.id == id).GetComponent<Part>();
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
           
            var itemData = x.GetComponent<Part>().partSO;
            _item.GetComponent<ItemContainerUI>().id = itemData.id;
            _item.GetComponent<ItemContainerUI>().icon.texture = itemData.icon.texture;
            _item.GetComponent<ItemContainerUI>().text.text = itemData.name;      
            _item.name = itemData.name;

            _item.GetComponent<ItemContainerUI>().sectionType = itemData.partType.ToString();

            if (itemData.partType == PartType.building)
            {
                _item.GetComponent<ItemContainerUI>().subSectionType = x.GetComponent<BuildingPart>().buildingType.ToString();
            }

            if(itemData.partType == PartType.furniture)
            {
                _item.GetComponent<ItemContainerUI>().subSectionType = x.GetComponent<FurniturePart>().furnitureType.ToString();
            }
           
            _item.transform.SetParent(centerContainer.transform);
            _item.transform.GetComponent<RectTransform>().localScale = Vector3.one * .7f;
            itemContainerList.Add(_item);
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
