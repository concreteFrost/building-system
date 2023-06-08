using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class OutlineObject : MonoBehaviour
{
    public Material outlineMaterial;

    [SerializeField]
    protected MeshRenderer[] objectRenderer;
    int originalMaterialStackSize;

    public bool isOutlined = false;

    private void Start()
    {
        originalMaterialStackSize = objectRenderer.Length;
    }

    public void AddOutline()
    {
 
        foreach (var render in objectRenderer)
        {
            var materials = render.materials;

            Array.Resize(ref materials, materials.Length + 1);
            materials[materials.Length - 1] = outlineMaterial;
            render.materials = materials;
        }
    }

    public void RemoveOutline()
    {

            foreach (var render in objectRenderer)
            {
                var materials = render.materials;
                Array.Resize(ref materials, materials.Length - 1);
                render.materials = materials;
            }
            
    }


}
