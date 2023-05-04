using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerBuildingStore : MonoBehaviour
{
    public List<Ingredient> ingredients;
    private BuildingPlacer builder;
    private BuilderMenuUI managerUI;
    private BuilderItemDescriptionPanelUI descriptionPanelUI;
    public static UnityAction onAvailabilityCheck;

    private void Start()
    {
        builder = GetComponent<BuildingPlacer>();
       
        managerUI = GetComponentInChildren<BuilderMenuUI>();
        descriptionPanelUI = managerUI.descriptionPanel.GetComponent<BuilderItemDescriptionPanelUI>();
        CheckIfAffordable();
    }

    private void CheckIfAffordable()
    {
        foreach (var _part in builder.buildingParts)
        {
            var part = _part.GetComponent<Part>();
            var partIngredients = part.partSO.ingredients;
            part.canAfford = true;

            foreach (var ingredient in partIngredients)
            {
                if (ingredient == null) continue;

                var sameType = ingredients.FirstOrDefault(x => x.ingredient.ingredientType == ingredient.ingredient.ingredientType);

                if (sameType == null || sameType.quantity < ingredient.quantity)
                {   
                    part.canAfford = false;
                    break;
                }
            }
        }

        foreach(var _part in builder.buildingParts)
        {
            var part = _part.GetComponent<BuildingPart>();
            if(descriptionPanelUI.currentPart != null && managerUI.isDescriptionPanelActive)
            {
                if (part.partSO.id == descriptionPanelUI.currentPart.partSO.id)
                {
                    descriptionPanelUI.FillUpCurrentItemInfo(part);
                    break;
                }
            }
   
        }

        onAvailabilityCheck?.Invoke();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        var sameType = ingredients.FirstOrDefault(x => x.ingredient.ingredientType == ingredient.ingredient.ingredientType);

        if (sameType != null)
        {
            sameType.quantity += ingredient.quantity;
        }
        else
        {
            ingredients.Add(ingredient);
        }
        CheckIfAffordable();
    }

    public void RemoveIngredient(List<Ingredient> targetIngridients)
    {
        targetIngridients.ForEach(i =>
        {
            var ingredientToRemove = ingredients.FirstOrDefault(x => x.ingredient.ingredientType == i.ingredient.ingredientType);

            if (ingredientToRemove != null)
            {
                ingredientToRemove.quantity -= i.quantity;

                if (ingredientToRemove.quantity <= 0)
                {
                    ingredients.Remove(ingredientToRemove);
                }
            }
        });
        CheckIfAffordable();

    }


}