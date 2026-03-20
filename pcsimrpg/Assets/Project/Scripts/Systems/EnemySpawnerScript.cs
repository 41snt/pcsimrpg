using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour
{
    [Header("Continuous Spawning (New Enemies)")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5.0f;

    [Header("Respawn Settings (Dead Enemies)")]
    public float respawnDelay = 10.0f;

    private void Start()
    {
        if (enemyPrefab != null)
        {
            StartCoroutine(ContinuousSpawnRoutine());
        }
        else
        {
            Debug.LogError("Please drag the Enemy Prefab into the Spawner slot!");
        }
    }

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

            if (health != null)
            {
                health.spawner = this;

                // ✅ Force reset (extra safety)
                health.currentHealth = health.maxHealth;
            }
        }
    }

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

            // ✅ Reset health before enabling
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.currentHealth = health.maxHealth;
            }

            enemy.SetActive(true);
        }
    }
}