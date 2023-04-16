using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestroyBuildingPart : MonoBehaviour
{
    public void PerformDestroy(BuildingPart part)
    {

        transform.SetParent(null);
        part.snapPointsList.ForEach(x => x.GetComponent<SnapPoint>().DeactivateSnapPoints());
        Transform[] children = part.GetComponentsInChildren<Transform>();

        foreach (Transform c in children)
        {
            c.gameObject.AddComponent<Rigidbody>();
            c.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 3f, ForceMode.Impulse);
            c.parent = null;

            Destroy(c.gameObject, 3f);
        }
        Destroy(gameObject, 3f);
    }

    public void DestroyChildPart(List<SnapPoint> snapPointsList)
    {
        snapPointsList.ForEach(x =>
        {
            if (x.transform.childCount > 0)
            {
                BuildingPart[] childParts = x.GetComponentsInChildren<BuildingPart>();
                childParts.ToList().ForEach(c =>
                {
                    c.DestroyPrefab();
                    c.snapPointsList.ForEach(s => s.DeactivateSnapPoints());
                });

            }

        });
    }

    public void CheckingGroundedNeighbours(List<SnapPoint> snapPointsList)
    {
        foreach (var snapPoint in snapPointsList)
        {
            var childParts = snapPoint.GetComponentsInChildren<BuildingPart>();

            foreach (var childPart in childParts)
            {
                var childSnaps = childPart.snapPointsList;

                foreach (var childSnap in childSnaps)
                {
                    var mask = 1 << LayerMask.NameToLayer("Building");
                    Collider[] cols = Physics.OverlapBox(childSnap.transform.position, Vector3.one * .3f, Quaternion.identity, mask);

                    if (cols.Length > 0)
                    {
                        foreach (var col in cols)
                        {
                            if (!col.transform.IsChildOf(transform))
                            {
                                if (col.GetComponentInParent<BuildingPart>() != null)
                                {
                                    childPart.transform.SetParent(ReasignedParentNode(col.GetComponentInParent<BuildingPart>(), childSnap, childParts));
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    private Transform ReasignedParentNode(BuildingPart col, SnapPoint childSnap, BuildingPart[] childParts)
    {
        var colSnaps = col.GetComponentInParent<BuildingPart>().snapPointsList;

        foreach (var colSnap in colSnaps)
        {
            float dist = Vector3.Distance(childSnap.transform.position, colSnap.transform.position);

            if (dist < 0.2f && colSnap.snapType == col.snapType)
            {
                for (int i = childParts.Length - 2; i >= 0; i--)
                {
                    foreach (var s in childParts[i].snapPointsList)
                    {
                        foreach (var s2 in childParts[i + 1].snapPointsList)
                        {
                            float dist2 = Vector3.Distance(s.transform.position, s2.transform.position);

                            if (dist2 < 0.2 && s.snapType == s2.snapType)
                            {
                                childParts[i].transform.SetParent(s2.transform);
                                childParts[i].parentNode = s2.gameObject;
                            }
                        }
                    }
                }
                return colSnap.transform;
            }
        }

        return null;
    }
}
