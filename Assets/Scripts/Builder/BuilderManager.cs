using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class BuilderManager : MonoBehaviour
{
    private BuilderStore store;
    public BuilderManagerUI builderManagerUI;
    FirstPersonController firstPersonController;
    public  List<GameObject> buildingParts = new List<GameObject>();
    public GameObject partsParent;
    public GameObject currentPart;

    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction<SnapType> onPartPlaced;
    

    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;

    public Color canBuildColor;
    public Color cantBuildColor;

    private void Awake()
    {
        firstPersonController = FindObjectOfType<FirstPersonController>();
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
        
        //firstPersonController.enabled = !isBuildingMenuIsOpen || inDestructionMode || isPartChosen;

        if (currentPart !=null)
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

        }


        if (builderManagerUI.isDestructionModeActive)
        {
            currentPart = null;
            if (Input.GetMouseButtonDown(0))
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
                    
                    hit.transform.GetComponent<SnapPoint>().DeactivateSnapPoints();
                    go.GetComponent<BuildingPart>().parentNode = hit.transform.gameObject;
                    go.transform.SetParent(hit.transform);
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

        onPartChanged(currentPart.GetComponent<BuildingPart>().snapType);
    }


}
