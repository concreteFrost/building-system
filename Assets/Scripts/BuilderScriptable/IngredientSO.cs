using UnityEngine;
using System.Collections.Generic;

public enum IngredientType
{
    wood,
    rock,
    metal,
    rope,
    slate
}
[CreateAssetMenu(menuName = "Craft/Ingridients", fileName = "New Ingridient")]
public class IngredientSO : ScriptableObject
{
    public IngredientType ingredientType;

    public GameObject ingredientInstance;

}



