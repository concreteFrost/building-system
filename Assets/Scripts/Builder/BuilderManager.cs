using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class BuilderManager : MonoBehaviour
{
    private BuilderStore store;
    public BuilderManagerUI builderManagerUI;
    private BuildingPart objectToDestroy;
    public List<GameObject> buildingParts = new List<GameObject>();
    public GameObject partsParent;
    public GameObject currentPart;

    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction<SnapType> onPartPlaced;
    public static UnityAction onBuildingModeExit;

    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;

    //building state booleans
    public bool isDestructionModeActive;
    public bool isMenuOpen;

    public Color canBuildColor;
    public Color cantBuildColor;

    private void Awake()
    {
        
        store = GetComponent<BuilderStore>();
        store.parts.ForEach(x =>
        {
            GameObject part = Instantiate(x);
            buildingParts.Add(part);
            part.transform.SetParent(partsParent.transform);
        });

        ToggleMainCanvas(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuOpen = !isMenuOpen;
            ToggleMainCanvas(isMenuOpen);
        }
      
        
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

        if (isDestructionModeActive)
        {
            currentPart = null;
            RemovePrefab();
        }

    }


    void RemovePrefab()
    {
        RaycastHit hit;
        int buildingLayer = LayerMask.NameToLayer("Building");
        int mask = (1 << buildingLayer);

        ResetObjectToDestroy();

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, mask))
        {
            if(hit.transform.GetComponentInParent<BuildingPart>() != null)
            {
  
                objectToDestroy = hit.transform.GetComponentInParent<BuildingPart>();  
                
                if (!objectToDestroy.isOutlined)
                {
                    objectToDestroy.AddOutline();
                    objectToDestroy.isOutlined = true;

                }

                if (Input.GetMouseButtonDown(0))
                {
                    objectToDestroy.RemoveOutline();
                    objectToDestroy.isOutlined = false;
                    objectToDestroy.DestroyPrefab();
                }
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

    public void EnterDestructionMode()
    {
        isDestructionModeActive = true;
        isMenuOpen = false;
        ToggleMainCanvas(false);
    }

    public void ToggleMainCanvas(bool _isMenuOpen)
    {
        isMenuOpen = _isMenuOpen;
        builderManagerUI.ShowMainCanvas(isMenuOpen);
        onBuildingModeExit?.Invoke();
       
        ResetObjectToDestroy();
        
        Cursor.visible = isMenuOpen;
        Cursor.lockState = _isMenuOpen ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (isMenuOpen)
        {
            
            isDestructionModeActive=false;
            currentPart = null;       
        }


        if (currentPart == null)
        {
            buildingParts.ForEach(x => x.SetActive(false));
            builderManagerUI.ShowBuildingModeCanvas(false);
        }
        
    }

    private void ResetObjectToDestroy()
    {
        if (objectToDestroy != null)
        {
            if (objectToDestroy.isOutlined)
            {
                objectToDestroy.RemoveOutline();
                objectToDestroy.isOutlined = false;
                objectToDestroy = null;
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
                Debug.Log(currentPart.name);
            }
            else
            {
                x.SetActive(false);
            }

        });
        ToggleMainCanvas(false);
        builderManagerUI.ShowBuildingModeCanvas(true);
        onPartChanged?.Invoke(currentPart.GetComponent<BuildingPart>().snapType);

    }


}
