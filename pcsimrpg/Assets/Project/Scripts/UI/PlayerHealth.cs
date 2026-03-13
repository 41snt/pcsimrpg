using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public DamagePopup damagePopup;

    public int maxHealth = 100;
    public int currentHealth;

    public Health healthBar;

    public int damagePerEnemy = 10;
    public float damageInterval = 1f;

    private float damageTimer = 0f;
    private Vector3 startPosition;

    private List<GameObject> touchingEnemies = new List<GameObject>();

    // GLOW EFFECT
    private SpriteRenderer sprite;
    public float glowTime = 0.15f;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        sprite = GetComponent<SpriteRenderer>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
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

        // Test damage with Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        // DAMAGE POPUP
        if (damagePopup != null)
            damagePopup.ShowDamage(damage);

        // RED GLOW EFFECT
        StartCoroutine(DamageGlow());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator DamageGlow()
    {
        if (sprite != null)
            sprite.color = new Color(1f, 0f, 0f, 0.5f);

        yield return new WaitForSeconds(glowTime);

        if (sprite != null)
            sprite.color = Color.white;
    }

    void Die()
    {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        Debug.Log("Player Dead!");

        // Small delay so damage effects play
        yield return new WaitForSeconds(0.5f);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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