
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerBuildingStore : MonoBehaviour
{

    public static UnityAction onAvailabilityCheck;
    public float playerMoney;

    public float GetPlayerMoney()
    {
        return playerMoney;
    }


    public void DeductFromBallance(float amount)
    {
        playerMoney -= amount;
    }

    public void AddToBalance(float amount)
    {
        playerMoney += amount;
    }


}
