using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BuildingPart : Part
{
  
    public GameObject snapPoints;

    public GameObject parentNode;

    public List<SnapPoint> snapPointsList = new List<SnapPoint>();

    public SnapType snapType;

    public BuildingType buildingType;

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

    public override void SetColor(Color tempColor)
    {
        base.SetColor(tempColor);
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
           
        if (isPlaced)
        {
            SwitchSnapPoints(snap);         
            
        }
        if (parentNode != null)
        {
            transform.parent = parentNode.transform;
            parentNode.GetComponent<SnapPoint>().DeactivateSnapPoints();
        }
    }
    public override void DestroyPrefab()
    {
        base.DestroyPrefab();
 
        snapPointsList.ForEach(x => x.DeactivateSnapPoints());

        //checking if the child part can be passed to another object
        destroyBuilding.CheckingGroundedNeighbours(snapPointsList);

        //destroying all children that were not passed to other objects
        destroyBuilding.DestroyChildPart(snapPointsList);

        //actual prefab destroy
        destroyBuilding.PerformDestroy(this);

    }

    public override void SetItemAvailable(List<Ingredient> playerIngredients)
    {
        base.SetItemAvailable(playerIngredients);
    }

    public override bool CanBuild(RaycastHit hit)
    {
        if (isTouchingGround && snapType == SnapType.Surface)
        {
            return CheckOverlap();
        }
        else
        {
            return hit.transform.CompareTag("snapPoint") && CheckOverlap() && OverlapCheck.FloorCheck(part);
        }
    }



    private void OnEnable()
    {
        BuildingPlacer.onPartChanged += SwitchSnapPoints;

    }

    private void OnDisable()
    {
        BuildingPlacer.onPartChanged -= SwitchSnapPoints;

    }


}