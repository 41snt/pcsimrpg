using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Health healthBar;
    public int damagePerEnemy = 10; // Damage per enemy per tick
    public float damageInterval = 1f; // Time between damage ticks

    private float damageTimer = 0f;
    private Vector3 startPosition; // Respawn position
    private List<GameObject> touchingEnemies = new List<GameObject>(); // Track enemies touching player

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        startPosition = transform.position;
    }

    void Update()
    {
        // Apply continuous damage based on number of enemies touching
        if (touchingEnemies.Count > 0)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                int totalDamage = damagePerEnemy * touchingEnemies.Count;
                TakeDamage(totalDamage);
                damageTimer = 0f;
            }
        }

        // Test damage manually
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead!");

        // Reset position
        transform.position = startPosition;

        // Reset health
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);

        // Clear touching enemies so damage stops
        touchingEnemies.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !touchingEnemies.Contains(collision.gameObject))
        {
            touchingEnemies.Add(collision.gameObject);
            damageTimer = 0f; // Reset timer when new enemy touches
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && touchingEnemies.Contains(collision.gameObject))
        {
            touchingEnemies.Remove(collision.gameObject);
        }
    }
}