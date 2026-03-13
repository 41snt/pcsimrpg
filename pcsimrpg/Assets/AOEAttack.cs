using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    public CircleCollider2D aoeCollider;

    public Transform aoeVisual;   // electric circle sprite

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

        if (aoeVisual != null)
            aoeVisual.localScale = Vector3.zero;
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (!attacking && cooldownTimer >= attackCooldown)
        {
            attacking = true;

            if (aoeVisual != null)
                aoeVisual.gameObject.SetActive(true);
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

        // Scale visual circle
        if (aoeVisual != null)
        {
            float size = currentRadius * 2f;
            aoeVisual.localScale = new Vector3(size, size, 1f);
        }

        if (currentRadius >= maxRadius)
        {
            DamagePlayersInside();

            ResetAOE();
        }
    }

    void ResetAOE()
    {
        currentRadius = 0f;
        aoeCollider.radius = 0f;
        cooldownTimer = 0f;
        attacking = false;

        if (aoeVisual != null)
        {
            aoeVisual.localScale = Vector3.zero;
            aoeVisual.gameObject.SetActive(false);
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