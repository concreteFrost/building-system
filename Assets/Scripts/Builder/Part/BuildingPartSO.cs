
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum BuildingType
{
    floor,
    wall,
    stairs,
    window,
    door,
    roof
}


[CreateAssetMenu(menuName ="Building Part/Part", fileName ="New Part")]
public class BuildingPartSO : PartSO
{
    public BuildingType buildingType;


}



