using UnityEngine;

public class AOEQuestSpawner : MonoBehaviour
{
    public GameObject aoeEnemyPrefab;
    public Transform spawnCenter;

    public int spawnCount = 5;
    public float spawnRadius = 3f;

    private bool hasSpawned;

    public void SpawnAOEEnemies()
    {
        if (hasSpawned)
        {
            Debug.Log("Spawn blocked (already spawned).");
            return;
        }

        if (aoeEnemyPrefab == null || spawnCenter == null)
        {
            Debug.LogError("Missing prefab or spawn center!");
            return;
        }

        hasSpawned = true;

        for (int i = 0; i < spawnCount; i++)
        {
            float angle = i * Mathf.PI * 2f / spawnCount;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * spawnRadius,
                Mathf.Sin(angle) * spawnRadius,
                0
            );

            Vector3 spawnPos = spawnCenter.position + offset;

            GameObject enemy = Instantiate(aoeEnemyPrefab, spawnPos, Quaternion.identity);

            EnemyHealth hp = enemy.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                hp.currentHealth = hp.maxHealth;
                hp.isAOEEnemy = true;
            }
        }

        Debug.Log("AOE enemies spawned: " + spawnCount);
    }

    public void ResetSpawner()
    {
        hasSpawned = false;
    }
}
