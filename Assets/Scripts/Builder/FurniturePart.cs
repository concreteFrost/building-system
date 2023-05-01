using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePart : Part
{

    public FurnitureType furnitureType;
    public override bool CanBuild(RaycastHit hit)
    {
        return CheckOverlap() && OverlapCheck.TerrainCheck(this.gameObject);
    }
}
