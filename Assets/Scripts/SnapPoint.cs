using UnityEngine;
using System.Linq;

public enum SnapType
{
    Surface,
    Wall,
    Stairs,
    RoofBase,
    RoofSlope,
    RoofFlat,
    RoofFront,
}

public class SnapPoint : MonoBehaviour
{
    public SnapType snapType;
    public Vector3 targetChildOffset;

}

