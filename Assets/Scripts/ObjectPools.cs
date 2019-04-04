using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> _poolDict;
    public List<Pool> Pools;
    public static ObjectPools SharedInstance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    private void Awake()
    {
        SharedInstance = this;
        
        _poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in Pools)
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

    public void ReturnToQueue(string queueTag, GameObject go)
    {
        var queue = GetQueue(queueTag);
        queue.Enqueue(go);
    }
}
