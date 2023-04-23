
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PartType
{
    floor,
    wall,
    stairs,
    window,
    door,
    roof
}


[CreateAssetMenu(menuName ="Building Part/Part", fileName ="New Part")]
public class BuildingPartSO : ScriptableObject
{
    public int id;
    public string name;
    public Sprite icon;
    public PartType partType;

    [TextArea]
    public string description;

    [SerializeField]
    public List<Ingredient> ingredients;
    
}

