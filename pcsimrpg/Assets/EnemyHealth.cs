using UnityEngine;

public class EnemyHealth : MonoBehaviour, IInteractable
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth; // Public so the HealthBar script can see it

    [HideInInspector]
    public EnemySpawnerScript spawner;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // This is called by your Melee or Projectile scripts
    public void Interact()
    {
        TakeDamage(25);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        // Hide the enemy (don't destroy it so it can respawn)
        gameObject.SetActive(false);

        if (spawner != null)
        {
            spawner.RequestRespawn(this.gameObject);
        }

        // Reset for the next time it appears
        currentHealth = maxHealth;
    }
}