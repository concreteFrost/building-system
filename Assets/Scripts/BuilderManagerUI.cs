using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuilderManagerUI : MonoBehaviour
{

    public TextMeshProUGUI partNameText;
    public TextMeshProUGUI isBuildingModeOn;

    public GameObject canvas;

    private void Awake()
    {
        isBuildingModeOn.text = "Building Mode: On";
    }

    public void GetPartName(GameObject go)
    {
        partNameText.text = go.name;
    }

    public void GetBuildingModeState(bool inBuildingMode)
    {
        
        if (inBuildingMode)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }
    
}
