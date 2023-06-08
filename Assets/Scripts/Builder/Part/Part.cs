
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public abstract class Part : OutlineObject
{
    public DestroyBuildingPart destroyBuilding;

    public GameObject part;
    
    public PartSO partSO;
    public Vector3 overlapSize;
    public Vector3 overlapPosition;

    public PhysicMaterial buildingMaterial;

    public bool canAfford;
    public bool isPlaced = false;
    public bool isTouchingGround;
    public bool isRequeriedOverlap;
    public bool isDestroyed = false;

    public PlayerMoneyEventSO addToPlayerBalance;
    private void Awake()
    {
        objectRenderer = part.GetComponentsInChildren<MeshRenderer>();

        destroyBuilding = GetComponent<DestroyBuildingPart>();
        if (!isPlaced)
        {
            part.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.enabled = false);
        }

    }

    public virtual void SetColor(Color tempColor)
    {
        Renderer[] materials = part.GetComponentsInChildren<Renderer>();

        foreach (var material in materials)
        {
            if (isPlaced)
            {
                tempColor.a = 1f;
            }
            else
            {
                tempColor.a = 0.5f;
            }
            material.material.color = tempColor;
        }

    }

    public virtual void DestroyPrefab()
    {

        if (!isPlaced)
        {
            return;
        }

        if (!isDestroyed)
        {
            addToPlayerBalance.Raise(partSO.itemPrice);
            isDestroyed = true;
        }
        

    }

    public virtual bool CheckOverlap()
    {
        if (isRequeriedOverlap)
        {

            var mask = 1 << LayerMask.NameToLayer("SnapPoint");

            var pos = part.transform.position + overlapPosition;
            var cols = Physics.OverlapBox(pos, overlapSize / 2, Quaternion.identity, ~mask);

            if (cols.Length > 0)
            {
                return false;
            }
        }


        return true;
    }

    public abstract bool CanBuild(RaycastHit hit);

    public void PlacePrefab()
    {
        isPlaced = true;
        SetColor(Color.white);
        part.transform.GetComponentsInChildren<Collider>().All(x => x.enabled = true);
    }
}
