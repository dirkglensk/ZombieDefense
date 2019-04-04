using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var walker = other.GetComponent<Walker>();
        if (walker != null)
        {
            walker.Despawn();
        }
    }
}
