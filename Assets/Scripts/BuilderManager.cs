using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class BuilderManager : MonoBehaviour
{
    BuilderManagerUI builderManagerUI;

    public GameObject partsContainer;
    public List<GameObject> buildingParts = new List<GameObject>();
    public GameObject currentPart;

    public Material canBuildMaterial;
    public Material cantBuildMaterial;

    public static UnityAction<SnapType> onPartChanged;
    public static UnityAction onPartDestroyed;

    private int _prefabIndex;
    public int prefabIndex
    {
        get { return _prefabIndex; }
        set
        {
            _prefabIndex = (value > buildingParts.Count - 1) ? 0 :
                           (value < 0) ? buildingParts.Count - 1 : value;
        }
    }
    public float zOffset = 5f;
    public float angle = 0f;
    private float rayDistance = 10f;

    public bool canBuild = false;
    public bool inBuildingMode = true;

    private void Awake()
    {
        builderManagerUI = GetComponent<BuilderManagerUI>();
        partsContainer.GetComponentsInChildren<BuildingPart>().ToList().ForEach(x => buildingParts.Add(x.gameObject));
        buildingParts.Sort((x, y) => x.GetComponent<BuildingPart>().snapType.CompareTo(y.GetComponent<BuildingPart>().snapType));
    }

    private void Start()
    {

        GetActivePrefab();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inBuildingMode = !inBuildingMode;
            builderManagerUI.GetBuildingModeState(inBuildingMode);

            if (inBuildingMode)
            {
                GetActivePrefab();
            }
          
        }

        if (!inBuildingMode)
        {
            buildingParts.ForEach(x => x.SetActive(false));
            return;
        }

        

        if (currentPart != null)
        {
            currentPart.GetComponent<BuildingPart>().SetColor(canBuild ? canBuildMaterial : cantBuildMaterial);
            currentPart.transform.localRotation *= TransformManipulator.RotatePart(angle);
            currentPart.transform.position = TransformManipulator.CameraCenter(zOffset);

            BuildingPart buildingPart = currentPart.GetComponent<BuildingPart>();
            SwitchPrefab();
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


    private void PlacePrefab(BuildingPart buildingPart)
    {

        GameObject go = Instantiate(buildingParts[prefabIndex], currentPart.transform.position, currentPart.transform.rotation);
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

    private void SwitchPrefab()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            prefabIndex--;
            GetActivePrefab();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            prefabIndex++;
            GetActivePrefab();
        }

    }

    private void GetActivePrefab()
    {

        angle = 0;
        buildingParts.ForEach(prefab => prefab.SetActive(buildingParts.IndexOf(prefab) == prefabIndex));
        currentPart = buildingParts[prefabIndex];
        builderManagerUI.GetPartName(currentPart);
        onPartChanged(currentPart.GetComponent<BuildingPart>().snapType);

    }

}
