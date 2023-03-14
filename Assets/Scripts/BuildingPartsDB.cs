
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Builder/BuildingPartsDB",fileName ="BuildingPartsDB")]
public class BuildingPartsDB : ScriptableObject
{
    public List<GameObject> floors = new List<GameObject>();
    public List<GameObject> walls = new List<GameObject>();
    public List<GameObject> roofs = new List<GameObject>();
    public List<GameObject> doors = new List<GameObject>();
    public List<GameObject> stairs = new List<GameObject>();

    public List<GameObject> mergedParts = new List<GameObject>();

    private void OnEnable()
    {
        mergedParts.AddRange(floors);
        mergedParts.AddRange(walls);
        mergedParts.AddRange(roofs);
        mergedParts.AddRange(doors);
        mergedParts.AddRange(stairs);
    }

}
