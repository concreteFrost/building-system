using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePart : Part
{
    public override bool CanBuild(RaycastHit hit)
    {
        return CheckOverlap() && OverlapCheck.TerrainCheck(this.gameObject) || OverlapCheck.FloorCheck(gameObject, Vector3.one * .1f);
    }

    public override void DestroyPrefab()
    {
        base.DestroyPrefab();
        destroyBuilding.PerformDestroy(this);
    }
}
