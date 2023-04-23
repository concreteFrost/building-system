using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public IngredientSO ingredientSO;
    Ingredient itemIngredient;
    public static UnityAction<Ingredient> onItemPicked;
    public int quantity;

    private void Start()
    {
        itemIngredient = new Ingredient();

        itemIngredient.ingredient = ingredientSO;
        itemIngredient.quantity = quantity;
        Debug.Log(quantity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponentInChildren<PlayerBuildingStore>().AddIngredient(itemIngredient);
            Destroy(gameObject);
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Collider>().isTrigger = true;        
        }
    }
}
