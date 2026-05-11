using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 15;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore enemy collision
        if (collision.CompareTag("Enemy"))
            return;

        // Damage player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // Destroy only on BoxCollider2D
        if (collision.GetComponent<BoxCollider2D>() != null)
        {
            Destroy(gameObject);
        }
    }
}