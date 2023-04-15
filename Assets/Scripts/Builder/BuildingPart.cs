using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
public class BuildingPart : MonoBehaviour
{
    public BuildingPartSO buildingPartSO;
    public int id;

    public SnapType snapType;
    public GameObject snapPoints;
    public GameObject part;
    public GameObject parentNode;

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

        if (snapPoints != null && !isPlaced)
        {
            snapPointsList.ForEach(x => x.DeactivateSnapPoints());
            part.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = false);
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
        var materials = part.GetComponentsInChildren<Renderer>();

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

    public void SwitchSnapPoints(SnapType snap)
    {
        if (isPlaced)
        {
            snapPointsList.ForEach(x =>
            {
                if(x.snapType == snap)
                {
                    x.ActivateSnapPoints();
                }
                else
                {
                    x.DeactivateSnapPoints();
                }
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
            if (parentNode != null)
            {
                transform.parent = parentNode.transform;
                parentNode.GetComponent<SnapPoint>().DeactivateSnapPoints();

            }
        }
    }


    public void DestroyPrefab()
    {
        if (!isPlaced)
        {
            return;
        }

        foreach (var snapPoint in snapPointsList)
        {
           
        }

        snapPointsList.ForEach(x =>
        {
            if (x.transform.childCount > 0)
            {
                var childParts = x.GetComponentsInChildren<BuildingPart>();
                childParts.ToList().ForEach(c => c.PerformDestroy());
            }
        });

        PerformDestroy();
    }


    void PerformDestroy()
    {
        if (!isDestroyed)
        {
            transform.SetParent(null);
            Transform[] children = part.GetComponentsInChildren<Transform>();

            foreach (Transform c in children)
            {
                c.gameObject.AddComponent<Rigidbody>();
                c.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 3f, ForceMode.Impulse);
                c.parent = null;

                Destroy(c.gameObject, 3f);
            }
            Destroy(gameObject, 3f);

        }

        isDestroyed = true;

    }

    void OnPartDestroyed()
    {
        if (isPlaced)
            StartCoroutine(TemporaryActivateAllSnaps());
    }

    IEnumerator TemporaryActivateAllSnaps()
    {
        List<SnapPoint> disabledSnaps = new List<SnapPoint>();
        foreach (var s in snapPointsList)
        {
            if (s.GetComponent<Collider>().enabled == false)
            {
                disabledSnaps.Add(s);
            }
        }

        disabledSnaps.ForEach(s => s.GetComponent<Collider>().enabled = true);
        yield return new WaitForSeconds(0.4f);
        disabledSnaps.ForEach(s => s.GetComponent<Collider>().enabled = false);

        if (isDestroyed && parentNode != null)
            parentNode.GetComponent<Collider>().enabled = true;

    }


    private void OnEnable()
    {

        BuilderManager.onPartChanged += SwitchSnapPoints;
        BuilderManager.onPartDestroyed += OnPartDestroyed;
    }

    private void OnDisable()
    {
        BuilderManager.onPartChanged -= SwitchSnapPoints;
        BuilderManager.onPartDestroyed -= OnPartDestroyed;
    }


}