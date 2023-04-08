using UnityEngine;
using System.Linq;

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

    private void Start()
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
        _renderer.enabled = false;
        _collider.enabled = false;
    }

}

