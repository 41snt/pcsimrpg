using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour
{
    [Header("Continuous Spawning")]
    public GameObject enemyPrefab;

    public Transform spawnPoint;

    public float spawnInterval = 5f;

    [Header("Respawn Settings")]
    public float respawnDelay = 10f;

    private void Start()
    {
        if (enemyPrefab != null)
        {
            StartCoroutine(
                ContinuousSpawnRoutine()
            );
        }
        else
        {
            Debug.LogError(
                "Enemy Prefab Missing!"
            );
        }
    }

    IEnumerator ContinuousSpawnRoutine()
    {
        while (true)
        {
            SpawnNewEnemy();

            yield return new WaitForSeconds(
                spawnInterval
            );
        }
    }

    void SpawnNewEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError(
                "Enemy Prefab Missing!"
            );

            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError(
                "Spawn Point Missing!"
            );

            return;
        }

        GameObject newEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        EnemyHealth health =
            newEnemy.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.spawner = this;

            health.currentHealth =
                health.maxHealth;
        }
    }

    public void RequestRespawn(
        GameObject enemyToRespawn
    )
    {
        StartCoroutine(
            RespawnTimer(enemyToRespawn)
        );
    }

    IEnumerator RespawnTimer(GameObject enemy)
    {
        yield return new WaitForSeconds(
            respawnDelay
        );

        if (enemy != null &&
            spawnPoint != null)
        {
            enemy.transform.position =
                spawnPoint.position;

            EnemyHealth health =
                enemy.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.currentHealth =
                    health.maxHealth;
            }

            enemy.SetActive(true);
        }
    }
}