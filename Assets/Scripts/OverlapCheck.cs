using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class OverlapCheck
{
    static Vector3 floorOverlapSize = new Vector3(0.1f, 999, 0.1f);
    public static bool CheckOverlap(BuildingPart carcass)
    {
        if (carcass.isRequeriedOverlap)
        {

            var mask = 1 << LayerMask.NameToLayer("SnapPoint");

            var pos = carcass.part.transform.position + carcass.overlapPosition;
            var cols = Physics.OverlapBox(pos, carcass.overlapSize / 2, Quaternion.identity, ~mask);

            if (cols.Length > 0)
            {
                return false;
            }
        }


        return true;
    }

    public static bool FloorCheck(GameObject part)
    {
        var mask = 1 << LayerMask.NameToLayer("SnapPoint");

        var cols = Physics.OverlapBox(part.transform.position, floorOverlapSize, Quaternion.identity, ~mask);

        foreach (var c in cols)
        {
            if (c.GetComponentInParent<BuildingPart>() != null)
            {
                var s = c.GetComponentInParent<BuildingPart>();
                if (s.snapType == SnapType.Surface)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool TerrainCheck(GameObject part)
    {
        var mask = 1 << LayerMask.NameToLayer("SnapPoint");
        var cols = Physics.OverlapBox(part.transform.position, Vector3.one * 0.2f, Quaternion.identity, ~mask);

        foreach (var c in cols)
        {
            if (c.CompareTag("Terrain"))
                return true;
        }
        return false;
    }

    public static bool CanBuild(BuildingPart buildingPart, RaycastHit hit)
    {
        if (buildingPart.isTouchingGround && buildingPart.snapType == SnapType.Surface)
        {
            return CheckOverlap(buildingPart);
        }
        else
        {
            return hit.transform.CompareTag("snapPoint") && CheckOverlap(buildingPart) && FloorCheck(buildingPart.part);
        }
    }
}
