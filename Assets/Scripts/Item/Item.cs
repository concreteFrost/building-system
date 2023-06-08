using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable
{
    public IngredientSO ingredientSO;
    private Ingredient itemIngredient;
    public int quantity;

    private void Start()
    {
        itemIngredient = new Ingredient();

        itemIngredient.ingredient = ingredientSO;
        itemIngredient.quantity = quantity;

    }

    public void Interact(GameObject other)
    {
        
        Destroy(gameObject);
    }


}
