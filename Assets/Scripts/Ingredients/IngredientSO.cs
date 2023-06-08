using UnityEngine;
using System;

public enum IngredientType
{
    wood,
    rock,
    metal,
    rope,
    slate,
    fabric
}
[CreateAssetMenu(menuName = "Craft/Ingridients", fileName = "New Ingridient")]
public class IngredientSO : ScriptableObject
{
    public int id;

    public IngredientType ingredientType;

    public GameObject ingredientInstance;

    public float ingredientPrice;
    
    public string ingredientName;

    [TextArea]
    public string ingredientDescrption;

    public Sprite ingedientIcon;

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



