
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class ItemContainerUI : MonoBehaviour
{

    public int id;
    public RawImage icon;
    public TextMeshProUGUI text;
    public PartType partType;
    BuilderManager builderManager;
    BuilderManagerUI builderManagerUI;

    private void Start()
    {
        builderManager = GetComponentInParent<BuilderManager>();
        builderManagerUI = GetComponentInParent<BuilderManagerUI>();
    }

    public void TogglePanelAction()
    {          
        builderManagerUI.ToggleDescriptionPanel(id);
    }

    public void OnPartIconClicked()
    {
        builderManager.GetActivePrefab(id);
        builderManagerUI.canvas.SetActive(false);
        
    }


}
