using UnityEngine;

public class AOEQuestSpawner : MonoBehaviour
{
    public GameObject aoeEnemyPrefab;
    public Transform spawnCenter;

    public int spawnCount = 5;
    public float spawnRadius = 3f;

    public void SpawnAOEEnemies()
    {
        if (aoeEnemyPrefab == null || spawnCenter == null)
        {
            Debug.LogError("❌ Missing prefab or spawn center!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            float angle = i * Mathf.PI * 2 / spawnCount;

            // ✅ FIXED FOR 2D
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * spawnRadius,
                Mathf.Sin(angle) * spawnRadius,
                0
            );

            Vector3 spawnPos = spawnCenter.position + offset;

            GameObject enemy = Instantiate(aoeEnemyPrefab, spawnPos, Quaternion.identity);

            // 🔥 CRITICAL FIX: ensure active
            enemy.SetActive(true);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.currentHealth = health.maxHealth;
                health.isAOEEnemy = true;
            }

            Debug.Log("Spawned AOE enemy: " + enemy.name + " Active: " + enemy.activeSelf);
        }

        Debug.Log("🔥 Spawned AOE quest group: " + spawnCount);
    }
}
