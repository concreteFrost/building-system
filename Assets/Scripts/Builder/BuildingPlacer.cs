
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    private BuilderStore store;
    public List<GameObject> buildingParts = new List<GameObject>();
    public GameObject partsParent;
    public GameObject currentPart;
    private PlayerBuildingStore playerStore;

    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction<SnapType> onPartPlaced;
    //public static UnityAction onBuildingModeToggle;

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
        store.parts.ForEach(x =>
        {
            GameObject part = Instantiate(x);
            buildingParts.Add(part);
            part.transform.SetParent(partsParent.transform);
        });
        HideAllParts();
        
    }


    private void Update()
    {
        
        if (currentPart !=null)
        {
            currentPart.GetComponent<BuildingPart>().SetColor(canBuild ? canBuildColor : cantBuildColor);
            currentPart.transform.localRotation *= TransformManipulator.RotatePart(angle);
            currentPart.transform.position = TransformManipulator.CameraCenter(zOffset);

            SnapToSurface(currentPart.GetComponent<BuildingPart>());

            if (Input.GetKeyDown(KeyCode.F))
            {
                TransformManipulator.FlipPart(currentPart);
            }
        }
    }

    public void HideAllParts()
    {
        buildingParts.ForEach(x => x.SetActive(false));
    }
    

    private void SnapToSurface(BuildingPart buildingPart)
    {
        RaycastHit hit;
        canBuild = false;

        if (buildingPart.canAfford)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
            {
                if (!hit.transform.CompareTag("snapPoint"))
                {
                    TransformManipulator.ResetPosition(buildingPart);
                }
                if (hit.transform.CompareTag("snapPoint"))
                {
                    SnapPoint snapPoint = hit.transform.GetComponent<SnapPoint>();
                    TransformManipulator.SnapToSnapPoint(buildingPart, snapPoint);

                }
                if (hit.transform.CompareTag("Terrain"))
                {
                    currentPart.transform.position = new Vector3(currentPart.transform.position.x, hit.point.y, currentPart.transform.position.z);
                }

                canBuild = OverlapCheck.CanBuild(buildingPart, hit);

                if (Input.GetMouseButtonDown(0) && canBuild)
                {
                    PlacePrefab(buildingPart, hit);
                }
            }
        }
        //set object to non active mode if not enough recources
        else
        {
            buildingPart.gameObject.SetActive(false);
            currentPart = null;
        }
      

    }

    void PlacePrefab(BuildingPart buildingPart, RaycastHit hit)
    {
        if (buildingPart.canAfford)
        {
            GameObject go = Instantiate(buildingPart.gameObject, buildingPart.transform.position, buildingPart.transform.rotation);
            go.GetComponent<BuildingPart>().OnPartPlaced(currentPart.GetComponent<BuildingPart>().snapType);

            TransformManipulator.ResetPosition(buildingPart);

            if (hit.transform.CompareTag("snapPoint") && !buildingPart.isTouchingGround)
            {
                hit.transform.GetComponent<SnapPoint>().DeactivateSnapPoints();
                go.GetComponent<BuildingPart>().parentNode = hit.transform.gameObject;
                go.transform.SetParent(hit.transform);
            }

            playerStore.RemoveIngredient(buildingPart.buildingPartSO.ingredients);
            buildingPart.SetItemAvailable(playerStore.ingredients);


        }
    }

    //current part is asigning here through the ItemContainerUI
    public void GetActivePrefab(int id)
    {
        angle = 0;
        buildingParts.ForEach(x =>
        {
            if (x.GetComponent<BuildingPart>().buildingPartSO.id == id)
            {
                x.SetActive(true);
                currentPart = x;
            }
            else
            {
                x.SetActive(false);
            }

        });
        onPartChanged?.Invoke(currentPart.GetComponent<BuildingPart>().snapType);

    }


}
