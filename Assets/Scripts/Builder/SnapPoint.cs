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

 
    private void OnEnable()
    {
        BuilderManager.onBuildingModeExit += DeactivateSnapPoints;

    }

    private void OnDisable()
    {
        BuilderManager.onBuildingModeExit -= DeactivateSnapPoints;
    }


}

