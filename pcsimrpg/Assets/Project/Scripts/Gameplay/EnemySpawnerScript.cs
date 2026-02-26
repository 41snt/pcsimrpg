using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnDistance = 10f; // How far from center enemies spawn

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = Vector2.zero;

        int side = Random.Range(0, 4); // 0=Top, 1=Bottom, 2=Left, 3=Right

        switch (side)
        {
            case 0: // Top
                spawnPosition = new Vector2(Random.Range(-8f, 8f), spawnDistance);
                break;

            case 1: // Bottom
                spawnPosition = new Vector2(Random.Range(-8f, 8f), -spawnDistance);
                break;

            case 2: // Left
                spawnPosition = new Vector2(-spawnDistance, Random.Range(-4f, 4f));
                break;

            case 3: // Right
                spawnPosition = new Vector2(spawnDistance, Random.Range(-4f, 4f));
                break;
        }

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}