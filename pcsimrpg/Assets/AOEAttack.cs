using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    public CircleCollider2D aoeCollider;
    public float maxRadius = 4f;
    public float expandSpeed = 3f;
    public float attackCooldown = 5f;
    public int damage = 15;

    float currentRadius = 0f;
    float cooldownTimer = 0f;
    bool attacking = false;

    void Awake()
    {
        if (aoeCollider == null)
            aoeCollider = GetComponent<CircleCollider2D>();

        aoeCollider.isTrigger = true;
        aoeCollider.radius = 0f;
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (!attacking && cooldownTimer >= attackCooldown)
        {
            attacking = true;
        }

        if (attacking)
        {
            ExpandAOE();
        }
    }

    void ExpandAOE()
    {
        currentRadius += expandSpeed * Time.deltaTime;
        aoeCollider.radius = currentRadius;

        if (currentRadius >= maxRadius)
        {
            DamagePlayersInside();

            currentRadius = 0f;
            aoeCollider.radius = 0f;
            cooldownTimer = 0f;
            attacking = false;
        }
    }

    void DamagePlayersInside()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth player = hit.GetComponent<PlayerHealth>();

                if (player != null)
                    player.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeCollider != null ? aoeCollider.radius : 0);
    }
}