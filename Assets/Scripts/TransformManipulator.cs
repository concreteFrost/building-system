using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformManipulator
{

    public static Vector3 CameraCenter(float z)
    {
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 3f);
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewportCenter);

        return worldPoint;
    }

    public static void ResetPosition(BuildingPart part)
    {
        part.part.transform.localPosition = Vector3.zero;
        part.snapPoints.transform.localPosition = Vector3.zero;

    }

    public static Quaternion RotatePart(float angle)
    {
        return Quaternion.Euler(0, angle += Input.GetAxis("Mouse ScrollWheel") * 180f, 0);
    }

    public static void SnapToSnapPoint(BuildingPart buildingPart, SnapPoint snapPoint)
    {
        if (buildingPart.snapType == snapPoint.snapType)
        {
            buildingPart.transform.position = snapPoint.transform.position;

            buildingPart.part.transform.localPosition = snapPoint.targetChildOffset;
            buildingPart.snapPoints.transform.localPosition = snapPoint.targetChildOffset;

        }

    }

    public static void FlipPart(GameObject part)
    {
        part.transform.localScale = new Vector3(1, 1, part.transform.localScale.z * -1);

    }

}