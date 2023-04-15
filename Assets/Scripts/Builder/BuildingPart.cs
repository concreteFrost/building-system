using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuildingPart : MonoBehaviour
{
    public BuildingPartSO buildingPartSO;
    public int id;

    public SnapType snapType;
    public GameObject snapPoints;
    public GameObject part;

    public List<SnapPoint> snapPointsList = new List<SnapPoint>();

    public Vector3 overlapSize;
    public Vector3 overlapPosition;

    public bool isPlaced = false;
    public bool isDestroyed = false;
    public bool isTouchingGround;
    public bool isRequeriedOverlap;

    public PhysicMaterial buildingMaterial;

    private void Start()
    {
        snapPointsList = snapPoints.GetComponentsInChildren<SnapPoint>().ToList();
        snapPointsList.ForEach(x => x.DeactivateSnapPoints());
        part.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = false);
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
        var materials = part.GetComponentsInChildren<Renderer>();

        foreach (var material in materials)
        {
            material.material.color = tempColor;

        }
    }

    public void SwitchSnapPoints(SnapType snap)
    {
        if (isPlaced)
        {
            snapPointsList.ForEach(x =>
            {
                if (x.snapType == snap)
                    x.ActivateSnapPoints();

                else
                    x.DeactivateSnapPoints();
            });
        }

    }


    public void OnPartPlaced(SnapType snap)
    {
        isPlaced = true;
        part.transform.GetComponentsInChildren<Collider>().All(x => x.enabled = true);
       
        if (isPlaced)
        {
            
            SwitchSnapPoints(snap);
            SetColor(Color.white);

        }
    }


    public void DestroyPrefab()
    {

        if (!isPlaced)
        {
            return;
        }

        snapPointsList.ForEach(x => x.DeactivateSnapPoints());

        foreach (var snapPoint in snapPointsList)
        {
            if (snapPoint.transform.childCount > 0)
            {
                var children = snapPoint.transform.GetComponentsInChildren<BuildingPart>();

              
                

            }

        }
        PerformDestroy();
    }

    


    void PerformDestroy()
    {
        if (!isDestroyed)
        {           
            //transform.SetParent(null);
            Transform[] children = part.GetComponentsInChildren<Transform>();

            foreach (Transform c in children)
            {
                c.gameObject.AddComponent<Rigidbody>();
                c.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 3f, ForceMode.Impulse);
                c.parent = null;

                Destroy(c.gameObject, 1f);
            }
            Destroy(gameObject, 1f);

        }

        isDestroyed = true;

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