using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderSystem
{
    
    public static class DestroyObject
    {
        public delegate void Dest(GameObject go);

        public static void PerformDestroy(Transform transform, GameObject part, Dest d)
        {

            transform.SetParent(null);
            Transform[] children = part.GetComponentsInChildren<Transform>();

            foreach (Transform c in children)
            {
                c.gameObject.AddComponent<Rigidbody>();
                c.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 3f, ForceMode.Impulse);
                c.parent = null;
                d(c.gameObject);
            }

            d(part);

        }
    }
}
