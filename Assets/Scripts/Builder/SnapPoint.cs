using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum SnapType
{
    Surface,
    Wall,
    Stairs,
    RoofJoist,
    RoofRake,
    RoofRakeFlat,
    RoofFront,
    RoofCover,
    RoofCoverFlat
}

public class SnapPoint : MonoBehaviour
{
    public SnapType snapType;
    public Vector3 targetChildOffset;
    SphereCollider _collider;
    MeshRenderer _renderer;
    public GameObject childPart;
    public GameObject neighbourSnap;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _renderer = GetComponent<MeshRenderer>();

    }

    public void ActivateSnapPoints()
    {
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    public void DeactivateSnapPoints()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
    }
    public void AddChildPart(GameObject child)
    {
        child.transform.SetParent(transform);
        childPart = child;
    }
    private void OnEnable()
    {
        BuilderManager.onBuildingModeExit += DeactivateSnapPoints;
    }

    private void OnDisable()
    {
        BuilderManager.onBuildingModeExit -= DeactivateSnapPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("snapPoint"))
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            Debug.Log("collides");
        }
    }

}

