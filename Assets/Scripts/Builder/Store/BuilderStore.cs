using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderStore : MonoBehaviour
{
   public List<GameObject> parts = new List<GameObject> ();

    private void Awake()
    {
        parts.Sort((o1, o2) => o1.name.CompareTo(o2.name));
    }
}
