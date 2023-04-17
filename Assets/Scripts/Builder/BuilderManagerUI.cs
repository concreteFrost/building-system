using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BuilderManagerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform centerContainer;
    private BuilderStore store;

    private BuilderManager builderManager;
    private List<GameObject> buildingItemContainerList = new List<GameObject>();

    public GameObject itemContainer;
    public GameObject canvas;
    public GameObject descriptionPanel;
    public bool isDestructionModeActive;
    public bool isDescriptionPanelActive;
    public bool isMenuOpen;
    public static UnityAction onBuildingModeExit;

    private void Awake()
    {
        builderManager = GetComponentInParent<BuilderManager>();
        store = GetComponentInParent<BuilderStore>();
        store.parts.Sort((o1, o2) => o1.name.CompareTo(o2.name));
        StorePartsInGrid();
        ToggleMainCanvas();
        ShowDescriptionPanel(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetMenuOpenState(!isMenuOpen);

            if (isDestructionModeActive)
            {
                isDestructionModeActive = false;
            }

            if (isDescriptionPanelActive)
            {
                ShowDescriptionPanel(!isDescriptionPanelActive);
            }
            

            onBuildingModeExit?.Invoke();

        }

    }

    private void ToggleMainCanvas()
    {
        if (isMenuOpen)
        {
            canvas.SetActive(true);
            builderManager.currentPart = null;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            canvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetMenuOpenState(bool _isMenuOpen)
    {
        isMenuOpen = _isMenuOpen;
        ToggleMainCanvas();

        if (isMenuOpen)
        {
            builderManager.buildingParts.ForEach(x => x.SetActive(false));
        }
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

    public void EnterDestructionMode()
    {
        isDestructionModeActive = true;
        isMenuOpen = false;
        ToggleMainCanvas();
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





}
