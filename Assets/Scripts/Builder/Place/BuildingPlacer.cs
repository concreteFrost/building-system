
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    private BuilderStore store;
    public GameObject partsParent;
    public GameObject currentPart;
    private PlayerBuildingStore playerStore;
    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction<SnapType> onPartPlaced;
    public PlayerMoneyEventSO deductFromBalance;
    //public static UnityAction onBuildingModeToggle;
    ActiveBuildingModePanelUI activeModePanelUI;

    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;

    public Color canBuildColor;
    public Color cantBuildColor;

    private void Awake()
    {
        playerStore = GetComponentInParent<PlayerBuildingStore>();
        store = GetComponent<BuilderStore>();
        activeModePanelUI = GetComponentInChildren<ActiveBuildingModePanelUI>();
    }


    private void Update()
    {
        
        if (currentPart !=null)
        {
            currentPart.GetComponent<Part>().SetColor(canBuild ? canBuildColor : cantBuildColor);
            currentPart.transform.localRotation *= TransformManipulator.RotatePart(angle);
            currentPart.transform.position = TransformManipulator.CameraCenter(zOffset);

            SnapToSurface(currentPart.GetComponent<Part>());

            if (Input.GetKeyDown(KeyCode.F))
            {
                TransformManipulator.FlipPart(currentPart);
            }
        }
    }

    private void SnapToSurface(Part part)
    {
        RaycastHit hit;
        canBuild = false;

        if (part.canAfford)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
            {
                if(part.partSO.partType == PartType.building)
                {
                    SnapToSnapPoint(hit, part);
                }

                else
                {
                    currentPart.transform.position = new Vector3(currentPart.transform.position.x, hit.point.y, currentPart.transform.position.z);
                }
              
                canBuild = part.CanBuild(hit);

                if (Input.GetMouseButtonDown(0) && canBuild)
                {
                    PlacePrefab(part, hit);
                }
            }
        }
        //set object to non active mode if not enough recources
        else
        {
            part.gameObject.SetActive(false);
            currentPart = null;
        }
      
    }

    void SnapToSnapPoint(RaycastHit hit, Part part)
    {
        if (!hit.transform.CompareTag("snapPoint"))
        {
            TransformManipulator.ResetPosition(part);
        }
        if (hit.transform.CompareTag("snapPoint"))
        {
            SnapPoint snapPoint = hit.transform.GetComponent<SnapPoint>();
            TransformManipulator.SnapToSnapPoint(part.GetComponent<BuildingPart>(), snapPoint);
        }
        if (hit.transform.CompareTag("Terrain"))
        {
            currentPart.transform.position = new Vector3(currentPart.transform.position.x, hit.point.y, currentPart.transform.position.z);
        }
    }

    void PlacePrefab(Part part, RaycastHit hit)
    {
        if (part.canAfford)
        {
            GameObject go = Instantiate(part.gameObject, part.transform.position, part.transform.rotation);
            go.GetComponent<Part>().PlacePrefab();

            if (go.GetComponent<BuildingPart>() != null)
            {
                go.GetComponent<BuildingPart>().OnPartPlaced(currentPart.GetComponent<BuildingPart>().snapType);

                if (hit.transform.CompareTag("snapPoint") && !part.isTouchingGround)
                {
                    hit.transform.GetComponent<SnapPoint>().DeactivateSnapPoints();
                    go.GetComponent<BuildingPart>().parentNode = hit.transform.gameObject;
                    go.transform.SetParent(hit.transform);
                }
            }

            TransformManipulator.ResetPosition(part);
            playerStore.DeductFromBallance(part.partSO.itemPrice);
            activeModePanelUI.RefreshStateInfo(playerStore.playerMoney, currentPart.GetComponent<Part>().partSO.itemName, currentPart.GetComponent<Part>().partSO.itemPrice);
            deductFromBalance.Raise(currentPart.GetComponent<Part>().partSO.itemPrice);
            store.CheckIfAffordable();
      
        }

        
    }

    //current part is asigning here through the ItemContainerUI
    public void GetActivePrefab(int id)
    {
        angle = 0;
        store.parts.ForEach(x =>
        {
            if (x.GetComponent<Part>().partSO.id == id)
            {
                x.SetActive(true);
                currentPart = x;
            }
            else
            {
                x.SetActive(false);
            }

        });

        if(currentPart.GetComponent<Part>().partSO.partType == PartType.building)
        onPartChanged?.Invoke(currentPart.GetComponent<BuildingPart>().snapType);
        activeModePanelUI.RefreshStateInfo(playerStore.playerMoney, currentPart.GetComponent<Part>().partSO.itemName, currentPart.GetComponent<Part>().partSO.itemPrice);

    }


   

}
