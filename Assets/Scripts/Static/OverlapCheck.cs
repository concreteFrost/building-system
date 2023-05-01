using UnityEngine;

public static class OverlapCheck
{
    static Vector3 floorOverlapSize = new Vector3(0.1f, 999, 0.1f);


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

   
}