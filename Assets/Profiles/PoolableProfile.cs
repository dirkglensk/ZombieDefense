using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "System Profiles/Poolable Profile")]
public class PoolableProfile : ScriptableObject
{
    public string tag;
    public GameObject prefab;
    public int size;
}
