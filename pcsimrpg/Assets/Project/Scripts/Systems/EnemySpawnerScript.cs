using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour
{
    [Header("Continuous Spawning (New Enemies)")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5.0f; // A new enemy every 5 seconds

    [Header("Respawn Settings (Dead Enemies)")]
    public float respawnDelay = 10.0f; // Dead ones come back after 10 seconds

    private void Start()
    {
        if (enemyPrefab != null)
        {
            // Start the infinite loop for BRAND NEW enemies
            StartCoroutine(ContinuousSpawnRoutine());
        }
        else
        {
            Debug.LogError("Please drag the Enemy Prefab into the Spawner slot!");
        }
    }

    // This loop runs forever to keep the world full
    IEnumerator ContinuousSpawnRoutine()
    {
        while (true)
        {
            SpawnNewEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnNewEnemy()
    {
        if (enemyPrefab != null && spawnPoint != null)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyHealth health = newEnemy.GetComponent<EnemyHealth>();
            if (health != null) health.spawner = this;
        }
    }

    // This is called by an enemy when it "dies"
    public void RequestRespawn(GameObject enemyToRespawn)
    {
        StartCoroutine(RespawnTimer(enemyToRespawn));
    }

    IEnumerator RespawnTimer(GameObject enemy)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (enemy != null && spawnPoint != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true); // Turn the dead one back on
        }
    }
}