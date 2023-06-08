using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuilderStore : MonoBehaviour
{
    public List<GameObject> partsPrefabList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> parts = new List<GameObject>();    
    public List<Ingredient> basketIngredients = new List<Ingredient>();
    public GameObject partsParent;
    public PlayerBuildingStore playerStore;
    public float moneyToSpent;
    private void Awake()
    {
        playerStore = GetComponent<PlayerBuildingStore>();
        moneyToSpent = playerStore.playerMoney;
        partsPrefabList.Sort((o1, o2) => o1.name.CompareTo(o2.name));
        partsPrefabList.ForEach(x =>
        {
            GameObject part = Instantiate(x);
            part.transform.SetParent(partsParent.transform);
            parts.Add(part);
        });

        CheckIfAffordable();
    }

    public void DeductFromPrice(float price, int id, int quantity)
    {
        moneyToSpent -= price;
       
    }


    public void GetBackFromPrice(float price, int id, int quantity)
    {
        moneyToSpent += price;
     
    }

    public void CheckIfAffordable()
    {
        foreach(GameObject part in parts)
        {
            var partData = part.GetComponent<Part>();

            partData.canAfford = playerStore.playerMoney >= partData.partSO.itemPrice;
        }
    }


    public void ResetPurchase()
    {
        moneyToSpent = playerStore.playerMoney;
    }

    public void MakePurchase()
    {
        if (moneyToSpent >= 0)
        {
            var diff = playerStore.playerMoney - moneyToSpent;
            playerStore.playerMoney -= diff;

        }
        ResetPurchase();

    }

    public void HideAllParts()
    {
        parts.ForEach(x => x.SetActive(false));
    }


}
