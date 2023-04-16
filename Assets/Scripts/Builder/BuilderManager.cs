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
    public static UnityAction<SnapType> onPartPlaced;
    public static UnityAction onBuildingModeExit;

    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;

    public bool isBuildingMenuIsOpen = false;
    public bool isPartChosen = false;

    public Color canBuildColor;
    public Color cantBuildColor;

    private void Awake()
    {
        builderManagerUI.SetBuildingModeState(isBuildingMenuIsOpen);
        store = GetComponent<BuilderStore>();
        store.parts.ForEach(x =>
        {
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
            isPartChosen = false;
            isBuildingMenuIsOpen = !isBuildingMenuIsOpen;
            builderManagerUI.SetBuildingModeState(isBuildingMenuIsOpen);

            if (isBuildingMenuIsOpen && onBuildingModeExit!=null)
            {
                onBuildingModeExit.Invoke();
            }
        }



        if (isBuildingMenuIsOpen)
        {
            buildingParts.ForEach(x => x.SetActive(false));
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            

        }

        if (isPartChosen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

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
        }

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
                
                hit.transform.GetComponentInParent<BuildingPart>().DestroyPrefab();

            }
        }
    }

    private void SnapToSurface(BuildingPart buildingPart)
    {
        RaycastHit hit;
        canBuild = false;
        
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
                GameObject go = Instantiate(buildingPart.gameObject, buildingPart.transform.position,buildingPart.transform.rotation);
                go.GetComponent<BuildingPart>().OnPartPlaced(currentPart.GetComponent<BuildingPart>().snapType);
                
                TransformManipulator.ResetPosition(buildingPart);

                if (hit.transform.CompareTag("snapPoint") && !buildingPart.isTouchingGround)
                {
                    hit.transform.GetComponent<SnapPoint>().AddChildPart(go);
                    hit.transform.GetComponent<SnapPoint>().DeactivateSnapPoints();
                    go.GetComponent<BuildingPart>().parentNode = hit.transform.gameObject;
                }      
            }
        }

    }

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
        isPartChosen = true;
        isBuildingMenuIsOpen = false;
        onPartChanged(currentPart.GetComponent<BuildingPart>().snapType);

    }


}
