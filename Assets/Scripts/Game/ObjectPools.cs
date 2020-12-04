using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPools : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> _poolDict;
    public List<PoolableProfile> pools;
    public static ObjectPools SharedInstance;

    private void Awake()
    {
        SharedInstance = this;
        
        _poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            var newQ = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                var newGameObject = Instantiate(pool.prefab, this.gameObject.transform);
                newGameObject.SetActive(false);
                newQ.Enqueue(newGameObject);
            }
            _poolDict.Add(pool.tag, newQ);
        }
    }

    public Queue<GameObject> GetQueue(string tag)
    {
        _poolDict.TryGetValue(tag, out var queue);
        return queue;
    }

    public GameObject GetObjectFromQueue(string queueTag)
    {
        var queue = GetQueue(queueTag);
        return queue.Dequeue();
    }
    public T GetObjectFromQueue<T>(string queueTag)
    {
        var queue = GetQueue(queueTag);
        var newObject = queue.Dequeue().GetComponent<T>();
        return newObject;
    }

    public void ReturnToQueue(string queueTag, GameObject go)
    {
        var queue = GetQueue(queueTag);
        queue.Enqueue(go);
    }
}
