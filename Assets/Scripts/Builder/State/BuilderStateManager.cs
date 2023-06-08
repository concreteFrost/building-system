
using UnityEngine;
using UnityEngine.Events;

public enum BuildingState
{
    BuildingMode,
    DestuctionMode,
    MenuMode,
    NormalMode
}
public class BuilderStateManager : MonoBehaviour
{
    BuildingPlacer buildingPlacer;
    BuilderMenuUI buildingMenuUI;
    BuilderDesctructionMode builderDesctructionMode;
    BuilderStore store;
 
    ActiveBuildingModePanelUI activeBuildingModePanelUI;

    public static UnityAction onBuildingModeOff;
    public BuildingState state;

    public FirstPersonController firstPersonController;

    void Start()
    {
        store = GetComponent<BuilderStore>();   
        buildingPlacer = GetComponent<BuildingPlacer>();
        buildingMenuUI = GetComponentInChildren<BuilderMenuUI>();
        builderDesctructionMode = GetComponent<BuilderDesctructionMode>();
        activeBuildingModePanelUI = GetComponentInChildren<ActiveBuildingModePanelUI>();
        EnterNormalMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (state)
            {
                case BuildingState.BuildingMode:
                case BuildingState.DestuctionMode:
                    EnterMenuMode();
                    break;
                case BuildingState.MenuMode:
                    EnterNormalMode();
                    break;
                default:
                    EnterMenuMode();
                    break;

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterNormalMode();
        }
    }

    void SetCursorMode()
    {
        Cursor.visible = state == BuildingState.MenuMode;
        Cursor.lockState = state == BuildingState.MenuMode ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    void EnterMenuMode()
    {
        state = BuildingState.MenuMode;

        buildingMenuUI.ShowMainCanvas(true);
        buildingMenuUI.ShowDescriptionPanel(false);
        buildingMenuUI.GetPlayerMoney();
        buildingPlacer.currentPart = null;
        store.HideAllParts();
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        builderDesctructionMode.ShowDestructionPanel(false);
        onBuildingModeOff?.Invoke();
        SetCursorMode();
        firstPersonController.enabled = false;
        activeBuildingModePanelUI.HidePanel();
        
    }

    void EnterNormalMode()
    {
        state = BuildingState.NormalMode;

        buildingMenuUI.ShowMainCanvas(false);
        buildingPlacer.currentPart = null;
        store.HideAllParts();
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        builderDesctructionMode.ShowDestructionPanel(false);
        onBuildingModeOff?.Invoke();
        SetCursorMode();
        firstPersonController.enabled = true;
        activeBuildingModePanelUI.HidePanel();

    }

    public void EnterDestructionMode()
    {
        state = BuildingState.DestuctionMode;

        buildingMenuUI.ShowMainCanvas(false);
        buildingPlacer.currentPart = null;
        store.HideAllParts();
        builderDesctructionMode.ShowDestructionPanel(true);
        builderDesctructionMode.isDestructionModeActive = true;
        onBuildingModeOff?.Invoke();
        SetCursorMode();
        firstPersonController.enabled = true;
    

    }

    //this
    public void EnterBuildingMode(int id)
    {
        state = BuildingState.BuildingMode;

        //prefab id is derriving from ItemContainerUI 
        //when we click on the item icon
        buildingPlacer.GetActivePrefab(id);
        buildingMenuUI.ShowMainCanvas(false);
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        SetCursorMode();
        firstPersonController.enabled = true;
        activeBuildingModePanelUI.ShowPanel();
        
    }

}
