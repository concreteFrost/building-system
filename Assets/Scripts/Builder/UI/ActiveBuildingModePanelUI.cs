using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActiveBuildingModePanelUI : MonoBehaviour
{
    public TextMeshProUGUI currentPartText;
    public TextMeshProUGUI quantityRemainedText;
    public TextMeshProUGUI playerMoneyText;
    public GameObject panel;


    public void RefreshStateInfo(float playerMoney, string currentPartName, float partPrice)
    {
        int quantityRemained = (int)Mathf.Floor(playerMoney / partPrice);

        currentPartText.text = currentPartName;
        quantityRemainedText.text = "Still Can Afford: " + quantityRemained;
        playerMoneyText.text = "Money Available: " + playerMoney;
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}
