using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwarmSpawner : MonoBehaviour
{
    public SwarmEntity entityToSpawn;
    public SwarmManager swarmManager;
    public float timeBetweenSpawns;
    public float spawnTimeVarience;
    public UnityAction<int> OnSpawn;
    private float spawnRadius = 1.5f;

    int totalSpawnCount = 0;
    int spawnCount = 0;
    bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEntities(int count)
    {
        if(isSpawning) return;
        StartCoroutine(SpawnRoutine(count));
    }

    IEnumerator SpawnRoutine(int count)
    {
        spawnCount = 0;
        isSpawning = true;
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns + Random.Range(-spawnTimeVarience, spawnTimeVarience));
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            if(swarmManager.SettingsClone.is2D) spawnPos.y = 0;
            Instantiate(entityToSpawn, spawnPos, transform.rotation);
            spawnCount++;
            totalSpawnCount++;
            OnSpawn?.Invoke(totalSpawnCount);
            if (spawnCount >= count) 
            {
                isSpawning = false;
                yield break; 
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
    }
}
