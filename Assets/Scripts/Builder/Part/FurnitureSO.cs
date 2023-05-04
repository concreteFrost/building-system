
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum FurnitureType
{
    chair,
    table,
    bed
}


[CreateAssetMenu(menuName = "Building Part/Furniture", fileName = "New Furniture")]
public class FurnitureSO : PartSO
{

    public FurnitureType furnitureType;

}



