using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
public class BuildingPart : MonoBehaviour
{
    public BuildingPartSO buildingPartSO;
    DestroyBuildingPart destroyBuilding;

    public SnapType snapType;

    public GameObject snapPoints;
    public GameObject part;
    public GameObject parentNode;

    public List<SnapPoint> snapPointsList = new List<SnapPoint>();

    public Vector3 overlapSize;
    public Vector3 overlapPosition;

    public bool isPlaced = false;
    public bool isTouchingGround;
    public bool isRequeriedOverlap;

    public PhysicMaterial buildingMaterial;

    private void Awake()
    {
        destroyBuilding = GetComponent<DestroyBuildingPart>();
        part.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = false);
    }

    private void Start()
    {
        snapPointsList = snapPoints.GetComponentsInChildren<SnapPoint>().ToList();

        if (snapPoints != null && !isPlaced)
        {
            snapPointsList.ForEach(x => x.DeactivateSnapPoints());
            
        }
    }
    private void Update()
    {

        if (!isPlaced && snapType == SnapType.Surface)
        {
            isTouchingGround = OverlapCheck.TerrainCheck(gameObject);
        }
    }


    public void SetColor(Color tempColor)
    {
        Renderer[] materials = part.GetComponentsInChildren<Renderer>();

        foreach (var material in materials)
        {
            if (isPlaced)
            {
                tempColor.a = 1f;
            }
            else
            {
                tempColor.a = 0.5f;
            }
            material.material.color = tempColor;
        }

    }

    private void SwitchSnapPoints(SnapType snap)
    {
        if (isPlaced)
        {
            foreach (var s in snapPointsList)
            {
                if (s.snapType == snap)
                {
                    s.ActivateSnapPoints();
                }
                else
                {
                    s.DeactivateSnapPoints();
                }
            }
        }
      
    }

    public void OnPartPlaced(SnapType snap)
    {
        isPlaced = true;
        
        if (isPlaced)
        {
            SwitchSnapPoints(snap);
            SetColor(Color.white);
            part.transform.GetComponentsInChildren<Collider>().All(x => x.enabled = true);

        }
        if (parentNode != null)
        {
            transform.parent = parentNode.transform;
            parentNode.GetComponentInChildren<SnapPoint>().DeactivateSnapPoints();
        }
    }
    public void DestroyPrefab()
    {
        if (!isPlaced)
        {
            return;
        }
 
        snapPointsList.ForEach(x => x.DeactivateSnapPoints());

        //checking if the child part can be passed to another object
        destroyBuilding.CheckingGroundedNeighbours(snapPointsList);

        //destroying all children that were not passed to other objects
        destroyBuilding.DestroyChildPart(snapPointsList);

        //actual prefab destroy
        destroyBuilding.PerformDestroy(this);

        if (parentNode != null)
        {
            parentNode.GetComponent<SnapPoint>().ActivateSnapPoints();
        }
    }

    private void OnEnable()
    {
        BuilderManager.onPartChanged += SwitchSnapPoints;
    }

    private void OnDisable()
    {
        BuilderManager.onPartChanged -= SwitchSnapPoints;
    }


}