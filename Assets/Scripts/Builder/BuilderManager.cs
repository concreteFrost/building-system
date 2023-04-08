using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class BuilderManager : MonoBehaviour
{
    private BuilderStore store;
    public BuilderManagerUI builderManagerUI;

    private List<GameObject> buildingParts = new List<GameObject>();
    public GameObject partsParent; 
    public GameObject currentPart;

    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction onPartDestroyed;
    public static UnityAction<SnapType> onPartPlaced;

    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;
    public bool inBuildingMode = true;
    public bool isPartChosen = false;

    public Color canBuildColor;
    public Color cantBuildColor;


    private void Awake()
    {
       
        store =  GetComponent<BuilderStore>();
        store.parts.ForEach(x => {
            GameObject part = Instantiate(x);
            buildingParts.Add(part);
            part.transform.SetParent(partsParent.transform);
            });

        buildingParts.ForEach(x => x.SetActive(false));
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuildingMode = !inBuildingMode;
            isPartChosen = false;
          
        }

        if (!inBuildingMode)
        {
            buildingParts.ForEach(x => x.SetActive(false));
            builderManagerUI.GetBuildingModeState(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        if (inBuildingMode)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            if (!isPartChosen)
            {
                builderManagerUI.GetBuildingModeState(true);
            }

            if (isPartChosen)
            {
                builderManagerUI.GetBuildingModeState(false);
            }
           
            if (currentPart != null)
            {
                currentPart.GetComponent<BuildingPart>().SetColor(canBuild ? canBuildColor : cantBuildColor);
                currentPart.transform.localRotation *= TransformManipulator.RotatePart(angle);
                currentPart.transform.position = TransformManipulator.CameraCenter(zOffset);

                BuildingPart buildingPart = currentPart.GetComponent<BuildingPart>();
                SnapToSurface(buildingPart);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    TransformManipulator.FlipPart(currentPart);
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    RemovePrefab();
                }

                if (Input.GetMouseButtonDown(0) && canBuild)
                {
                    PlacePrefab(buildingPart);
                }
            }
        }
 
    }

    private void PlacePrefab(BuildingPart buildingPart)
    {
        GameObject go = Instantiate(currentPart, currentPart.transform.position, currentPart.transform.rotation);
        go.GetComponent<BuildingPart>().OnPartPlaced(currentPart.GetComponent<BuildingPart>().snapType);
        TransformManipulator.ResetPosition(buildingPart);
    }

    void RemovePrefab()
    {
        RaycastHit hit;
        int buildingLayer = LayerMask.NameToLayer("Building");
        int mask = (1 << buildingLayer);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, mask))
        {
            if (hit.transform.GetComponentInParent<BuildingPart>() != null)
            {
                onPartDestroyed.Invoke();
                hit.transform.GetComponentInParent<BuildingPart>().DestroyPrefab();

            }
            
        }
    }

    private void SnapToSurface(BuildingPart buildingPart)
    {
        RaycastHit hit;
        canBuild = false;
        buildingPart.parentNode = null;
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
                
                if (!buildingPart.isTouchingGround)
                    buildingPart.parentNode = hit.transform.gameObject;
            }
            if (hit.transform.CompareTag("Terrain"))
            {
                currentPart.transform.position = new Vector3(currentPart.transform.position.x, hit.point.y, currentPart.transform.position.z);
            }
            canBuild = OverlapCheck.CanBuild(buildingPart, hit);

        }

    }

    public void GetActivePrefab(int id)
    {
        angle = 0;
        buildingParts.ForEach(x =>
        {
            if(x.GetComponent<BuildingPart>().buildingPartSO.id == id)
            {
                x.SetActive(true);
                currentPart = x;
            }
            else
            {
                x.SetActive(false);
            }
               
        });
        isPartChosen = true;
        onPartChanged(currentPart.GetComponent<BuildingPart>().snapType);
       
    }


}
