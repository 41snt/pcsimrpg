using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public EnemySpawnerScript spawner;

    private bool isDead = false;

    void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("HIT: " + gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("💀 Enemy died");

        if (spawner != null)
        {
            spawner.RequestRespawn(gameObject);
        }

        gameObject.SetActive(false);
    }
}