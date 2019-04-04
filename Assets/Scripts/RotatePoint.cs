using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePoint : MonoBehaviour
{
    private Vector3 localForward;
    // Start is called before the first frame update
    void Start()
    {
        localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var walker = other.GetComponent<Walker>();
        if (walker)
        {
            walker.ContemplateDirectionChange(transform.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var walker = other.GetComponent<Walker>();
        if (walker)
        {
            walker.PreventDirectionChange(walker);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, localForward);
    }
}
