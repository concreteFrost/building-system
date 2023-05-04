using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PartType
{
    building,
    furniture
}

[CreateAssetMenu(menuName = "Building Part/Part Template", fileName = "New Template")]
public class PartSO : ScriptableObject
{
    public int id;
    public string name;
    public Sprite icon;

    public PartType partType;

    [TextArea]
    public string description;

    [SerializeField]
    public List<Ingredient> ingredients;

    private void OnEnable()
    {
        if (id == 0)
        {
            id = GenerateUniqueID();
        }
    }

    private int GenerateUniqueID()
    {
        Guid guid = Guid.NewGuid();
        byte[] bytes = guid.ToByteArray();
        return BitConverter.ToInt32(bytes, 0);
    }

}
