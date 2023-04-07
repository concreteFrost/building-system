using UnityEngine;

[CreateAssetMenu(menuName ="Craft/Ingridients", fileName ="New Ingridient")]
public class IngridientSO : ScriptableObject
{
    public string ingridientName;
    public int quantity;
}

