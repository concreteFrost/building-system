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

    public List<GameObject> topBuildingButtons;
    public List<GameObject> topFurnitureButtons;

    public string currentSection;

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
        currentSection = type;
        GetAllInCurrentSection();

        if (type == "building")
        {
            topBuildingButtons.ForEach(x => x.SetActive(true));
            topFurnitureButtons.ForEach(x => x.SetActive(false));
            
        }
        else
        {
            topBuildingButtons.ForEach(x => x.SetActive(false));
            topFurnitureButtons.ForEach(x => x.SetActive(true));
        }

        
    }

    public void GetAllInCurrentSection()
    {
        itemContainerList.ForEach(x => x.SetActive(true));
        var filtered = itemContainerList.Where(x => x.GetComponent<ItemContainerUI>().sectionType.ToString() != currentSection);
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
            var itemContainerUI = _item.GetComponent<ItemContainerUI>();
            itemContainerUI.id = itemData.id;
            itemContainerUI.icon.texture = itemData.icon.texture;
            itemContainerUI.text.text = itemData.name;
            itemContainerUI.name = itemData.name;

            itemContainerUI.sectionType = itemData.partType.ToString();

            if(itemData is FurnitureSO furniture)
            {
                itemContainerUI.subSectionType = furniture.furnitureType.ToString();
            }
            if(itemData is BuildingPartSO building) 
            {
                itemContainerUI.subSectionType = building.buildingType.ToString();
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
