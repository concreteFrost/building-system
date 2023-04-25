using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderDesctructionMode : MonoBehaviour
{
    private BuildingPart objectToDestroy;
    private float rayDistance = 10f;
    public bool isDestructionModeActive = false;

    private void Update()
    {
        if (isDestructionModeActive)
        {
            RemovePrefab();
        }
    }
    // Start is called before the first frame update
    public void RemovePrefab()
    {
        
        RaycastHit hit;
        int buildingLayer = LayerMask.NameToLayer("Building");
        int mask = (1 << buildingLayer);

        ResetObjectToDestroy();

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, mask))
        {
            if (hit.transform.GetComponentInParent<BuildingPart>() != null)
            {

                objectToDestroy = hit.transform.GetComponentInParent<BuildingPart>();

                if (!objectToDestroy.isOutlined)
                {
                    objectToDestroy.AddOutline();
                    objectToDestroy.isOutlined = true;

                }

                if (Input.GetMouseButtonDown(0))
                {
                    objectToDestroy.RemoveOutline();
                    objectToDestroy.isOutlined = false;
                    objectToDestroy.DestroyPrefab();
                }
            }
        }

    }
    public void ResetObjectToDestroy()
    {
        if (objectToDestroy != null)
        {
            if (objectToDestroy.isOutlined)
            {
                objectToDestroy.RemoveOutline();
                objectToDestroy.isOutlined = false;
                objectToDestroy = null;
            }
        }
    }
}
