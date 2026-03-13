using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public EnemySpawnerScript spawner;

    public DamagePopup damagePopup;
    public DamageDealtCounter damageCounter;

    void Start()
    {
        currentHealth = maxHealth;

        if (damageCounter == null)
            damageCounter = FindObjectOfType<DamageDealtCounter>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damagePopup != null)
            damagePopup.ShowDamage(damage);

        if (damageCounter != null)
            damageCounter.AddDamage(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Drop loot
        EnemyDrop drop = GetComponent<EnemyDrop>();

        if (drop != null)
        {
            drop.DropLoot();
        }

        // Tell spawner to respawn enemy later
        if (spawner != null)
        {
            spawner.RequestRespawn(gameObject);
        }

        // Disable enemy instead of destroying
        gameObject.SetActive(false);
    }
}