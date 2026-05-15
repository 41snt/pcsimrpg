using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 500;

    public int currentHealth;

    [HideInInspector]
    public EnemySpawnerScript spawner;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log(
            "Enemy HP: " + currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("💀 Enemy Destroyed!");

        Destroy(gameObject);
    }
}