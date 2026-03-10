using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{

    public DamagePopup damagePopup;
    public int maxHealth = 100;
    public int currentHealth;

    public Health healthBar; // Drag the UI Canvas/Object with the Health script here

    public int damagePerEnemy = 10;
    public float damageInterval = 1f;

    private float damageTimer = 0f;
    private Vector3 startPosition;
    private List<GameObject> touchingEnemies = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        // Continuous damage from enemies touching
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

        // Spacebar test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null) healthBar.SetHealth(currentHealth);

        // TRIGGER THE INDICATOR HERE
        if (damagePopup != null) damagePopup.ShowDamage(damage);

        if (currentHealth <= 0) Die();
    }
    void Die()
    {
        Debug.Log("Player Dead!");
        transform.position = startPosition;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        touchingEnemies.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // IMPORTANT: Your Enemy Prefabs must be tagged "Enemy"
        if (collision.gameObject.CompareTag("Enemy") && !touchingEnemies.Contains(collision.gameObject))
        {
            touchingEnemies.Add(collision.gameObject);
            damageTimer = 0f;
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