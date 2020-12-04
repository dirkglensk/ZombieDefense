using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var poolable = other.GetComponent<Poolable>();
        
        if (poolable != null)
        {
            poolable.Requeue();
        }
    }
}
