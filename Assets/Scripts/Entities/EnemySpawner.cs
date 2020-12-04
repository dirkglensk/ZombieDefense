using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float xSize = 7;
    public float zSize = 7;
    private float ySize = 1;

    public float spawnInterval = 0.5f;
    private bool IsActive = true;
    private int maxNumEnemies = 100;

    public PoolableProfile poolableEnemyType;
    private ObjectPools _objectPools;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(xSize, ySize, zSize));
    }

    public Vector3 GetRandomSpawnPosition()
    {
        var centerPoint = transform.position;
        
        float randomMinX = centerPoint.x - xSize * 0.5f;
        var randomMaxX = centerPoint.x + xSize * 0.5f;

        var randomMinZ = centerPoint.z - zSize * 0.5f;
        var randomMaxZ = centerPoint.z + zSize * 0.5f;

        var randomPointX = Random.Range(randomMinX, randomMaxX);
        var randomPointZ = Random.Range(randomMinZ, randomMaxZ);
        
        return new Vector3(randomPointX, 1.75f, randomPointZ);
    }

    public void SetMaxNumEnemies(int count)
    {
        maxNumEnemies = count;
    }

    private void Start()
    {
        _objectPools = ObjectPools.SharedInstance;
        var enemySpawnTag = poolableEnemyType.tag;
        StartCoroutine(SpawnTimer(enemySpawnTag));
    }

    private IEnumerator SpawnTimer(string objectPoolTag)
    {
        while (IsActive)
        {
            var attackerPool = _objectPools.GetQueue(objectPoolTag);
            if (attackerPool.Count < 1)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }
            
            var newEnemy = attackerPool.Dequeue();
            newEnemy.SetActive(true);
            newEnemy.transform.position = GetRandomSpawnPosition();
            newEnemy.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, -90, 0);
            newEnemy.GetComponent<Walker>().RestartCoroutines();
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
