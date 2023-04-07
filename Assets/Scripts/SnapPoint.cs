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

}

