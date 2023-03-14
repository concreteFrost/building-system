
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public enum FlipType
{
    zAxis,
    xAxis
}
public class BuildingPart : MonoBehaviour
{
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

    public Material defaultMaterial;
    public GameObject parentNode;
    public PhysicMaterial buildingMaterial;
    public FlipType flipType;
    
    


    private void Awake()
    {
        if (snapPoints != null)
        {
            snapPointsList = snapPoints.GetComponentsInChildren<SnapPoint>().ToList();
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

    public void SetColor(Material mat)
    {
        if (!isDestroyed)
            part.GetComponentsInChildren<MeshRenderer>().ToList().ForEach(x => x.material = mat);
    }

    public void SwitchSnapPoints(SnapType snap)
    {
        if (isPlaced)
            snapPointsList.ForEach(s => s.GetComponent<Collider>().enabled = s.snapType == snap);
    }

    public void OnPartPlaced(SnapType snap)
    {
        isPlaced = true;
        part.transform.GetComponentsInChildren<Collider>().All(x => x.enabled = true);

        if (isPlaced)
        {
            SetColor(defaultMaterial);
            SwitchSnapPoints(snap);

            if (parentNode != null)
            {
                gameObject.transform.parent = parentNode.transform;
                parentNode.GetComponent<Collider>().enabled = false;

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
            if (snapPoint.transform.childCount > 0)
            {
                var child = snapPoint.transform.GetChild(0).GetComponent<BuildingPart>();
                var snaps = child.snapPointsList;

                foreach (var snap in snaps)
                {
                    //Consider to change overlap size if parts will brake unneccessary (better to increase)
                    Collider[] colliders = Physics.OverlapSphere(snap.transform.position, 0.5f);

                    foreach (Collider collider in colliders)
                    {
                        if (collider.CompareTag("snapPoint") && collider.gameObject != snap.gameObject
                            && collider.gameObject != parentNode
                            && !collider.transform.IsChildOf(parentNode.transform.parent))
                        {
                            child.transform.SetParent(collider.transform);
                            child.parentNode = collider.gameObject;
                            break;

                        }
                    }
                }

            }
        }

        snapPointsList.ForEach(x =>
        {
            if(x.transform.childCount > 0)
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
            snapPointsList.ForEach(x => x.GetComponent<Collider>().enabled = false);
            transform.SetParent(null);
            Transform[] children = part.GetComponentsInChildren<Transform>();
            foreach (Transform c in children)
            {
                c.gameObject.AddComponent<Rigidbody>();
                c.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
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


}
