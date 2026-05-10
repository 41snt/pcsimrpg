using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 15;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // Destroy on walls/objects
        if (!collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}