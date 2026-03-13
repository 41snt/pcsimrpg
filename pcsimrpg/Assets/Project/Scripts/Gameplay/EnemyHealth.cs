using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public bool isAOEEnemy = false; // Only AOE enemies affect quest

    public EnemySpawnerScript spawner;

    public DamagePopup damagePopup;
    public DamageDealtCounter damageCounter;

    private QuestManager questManager;

    void Start()
    {
        currentHealth = maxHealth;

        if (damageCounter == null)
            damageCounter = FindObjectOfType<DamageDealtCounter>();

        questManager = FindObjectOfType<QuestManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damagePopup != null)
            damagePopup.ShowDamage(damage);

        // Add damage to counter immediately
        if (isAOEEnemy && damageCounter != null)
        {
            damageCounter.AddDamage(damage);
        }

        // Update quest UI immediately even if the enemy dies this frame
        if (isAOEEnemy && questManager != null && currentHealth <= 0)
        {
            // Increment the quest counter BEFORE deactivating enemy
            questManager.AOEEnemyKilled();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        EnemyDrop drop = GetComponent<EnemyDrop>();

        if (drop != null)
            drop.DropLoot();

        if (spawner != null)
            spawner.RequestRespawn(gameObject);

        // Make sure we only deactivate AFTER quest and counter are updated
        gameObject.SetActive(false);
    }
}
