
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

    public static UnityAction onBuildingModeOff;
    public BuildingState state;

    void Start()
    {

        buildingPlacer = GetComponent<BuildingPlacer>();
        buildingMenuUI = GetComponentInChildren<BuilderMenuUI>();
        builderDesctructionMode = GetComponent<BuilderDesctructionMode>();
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
        buildingPlacer.currentPart = null;
        buildingPlacer.HideAllParts();
        buildingPlacer.SetActiveBuildingModePanelState(false);
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        onBuildingModeOff?.Invoke();
        SetCursorMode();
        
    }

    void EnterNormalMode()
    {
        state = BuildingState.NormalMode;

        buildingMenuUI.ShowMainCanvas(false);
        buildingPlacer.currentPart = null;
        buildingPlacer.HideAllParts();
        buildingPlacer.SetActiveBuildingModePanelState(false);
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        onBuildingModeOff?.Invoke();
        SetCursorMode();

    }

    public void EnterDestructionMode()
    {
        state = BuildingState.DestuctionMode;

        buildingMenuUI.ShowMainCanvas(false);
        buildingPlacer.currentPart = null;
        buildingPlacer.HideAllParts();
        buildingPlacer.SetActiveBuildingModePanelState(false);
        builderDesctructionMode.isDestructionModeActive = true;
        onBuildingModeOff?.Invoke();
        SetCursorMode();

    }

    //this
    public void EnterBuildingMode(int id)
    {
        state = BuildingState.BuildingMode;

        //prefab id is derriving from ItemContainerUI 
        //when we click on the item icon
        buildingPlacer.GetActivePrefab(id);
        buildingPlacer.SetActiveBuildingModePanelState(true);
        buildingMenuUI.ShowMainCanvas(false);
        builderDesctructionMode.isDestructionModeActive = false;
        builderDesctructionMode.ResetObjectToDestroy();
        SetCursorMode();

    }
}
