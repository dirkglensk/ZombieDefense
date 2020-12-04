    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Poolable : MonoBehaviour
{
    [SerializeField]
    public string objectPoolTag;
    
    public void Requeue()
    {
        GameObject go;
        (go = gameObject).SetActive(false);
        ObjectPools.SharedInstance.ReturnToQueue(objectPoolTag, go);
    }
    
    
}
