using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    public SwarmEntity entityToSpawn;
    public SwarmManager swarmManager;
    public float timeBetweenSpawns;
    public float spawnTimeVarience;
    private float spawnRadius = 1.5f;
    public int maxSpawnCount = 1000;

    int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnCount = 0;
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns + Random.Range(-spawnTimeVarience, spawnTimeVarience));
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            if(swarmManager.SettingsClone.is2D) spawnPos.y = 0;
            Instantiate(entityToSpawn, spawnPos, transform.rotation);
            spawnCount++;
            if (spawnCount >= maxSpawnCount) { yield break; }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
    }
}
