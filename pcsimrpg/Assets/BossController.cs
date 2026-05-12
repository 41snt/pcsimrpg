using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRadius = 20f;
    public float stopDistance = 3f;
    public float returnSpeed = 2f;

    [Header("Projectile Attack")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;
    public float shootCooldown = 2f;
    public int bulletCount = 16;

    // WARNING
    public GameObject warningPrefab;
    public float warningDuration = 2f;

    private bool preparingShot = false;

    [Header("AOE Attack")]
    public CircleCollider2D aoeCollider;
    public Transform aoeVisual;

    public float aoeMaxRadius = 4f;
    public float aoeExpandSpeed = 3f;
    public float aoeCooldown = 5f;

    [Header("Damage")]
    public int aoeDamage = 20;

    private Rigidbody2D rb;

    // ORIGINAL POSITION
    private Vector3 startPosition;

    // MOVEMENT
    private Vector2 moveDirection;

    // STATES
    private bool chasingPlayer = false;

    // SHOOTING
    private float shootTimer;

    // AOE
    private float aoeTimer;
    private bool aoeAttacking = false;
    private float currentAOERadius = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;

        if (player == null)
        {
            GameObject playerObj =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // AOE SETUP
        if (aoeCollider != null)
        {
            aoeCollider.isTrigger = true;
            aoeCollider.radius = 0f;
        }

        if (aoeVisual != null)
        {
            aoeVisual.localScale = Vector3.zero;
            aoeVisual.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        HandleProjectileAttack();
        HandleAOEAttack();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer =
            Vector2.Distance(
                transform.position,
                player.position);

        // DETECT PLAYER
        if (distanceToPlayer <= detectionRadius)
        {
            chasingPlayer = true;
        }
        else
        {
            chasingPlayer = false;
        }

        // CHASE PLAYER
        if (chasingPlayer)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            ReturnToStart();
        }
    }

    // =========================
    // MOVEMENT
    // =========================

    void ChasePlayer(float distance)
    {
        if (distance <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection =
            (player.position - transform.position)
            .normalized;

        rb.MovePosition(
            rb.position +
            moveDirection *
            moveSpeed *
            Time.fixedDeltaTime);
    }

    void ReturnToStart()
    {
        float distanceToStart =
            Vector2.Distance(
                transform.position,
                startPosition);

        if (distanceToStart <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection =
            (startPosition - transform.position)
            .normalized;

        rb.MovePosition(
            rb.position +
            moveDirection *
            returnSpeed *
            Time.fixedDeltaTime);
    }

    // =========================
    // PROJECTILE ATTACK
    // =========================

    void HandleProjectileAttack()
    {
        if (!chasingPlayer) return;

        if (preparingShot) return;

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootCooldown)
        {
            StartCoroutine(
                PrepareShoot());
        }
    }

    IEnumerator PrepareShoot()
    {
        preparingShot = true;

        shootTimer = 0f;

        GameObject warningObject = null;

        // SPAWN WARNING ON PLAYER
        if (warningPrefab != null &&
            player != null)
        {
            warningObject =
                Instantiate(
                    warningPrefab,
                    player.position,
                    Quaternion.identity);

            // FOLLOW PLAYER
            warningObject.transform.SetParent(
                player);
        }

        // WAIT BEFORE SHOOTING
        yield return new WaitForSeconds(
            warningDuration);

        // FIRE BULLETS
        Shoot();

        // KEEP WARNING ACTIVE
        yield return new WaitForSeconds(1f);

        // REMOVE WARNING
        if (warningObject != null)
        {
            Destroy(warningObject);
        }

        preparingShot = false;
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        float angleStep =
            360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle =
                i * angleStep;

            float rad =
                angle * Mathf.Deg2Rad;

            Vector2 direction =
                new Vector2(
                    Mathf.Cos(rad),
                    Mathf.Sin(rad))
                .normalized;

            float bulletAngle =
                Mathf.Atan2(
                    direction.y,
                    direction.x)
                * Mathf.Rad2Deg;

            Quaternion rotation =
                Quaternion.Euler(
                    0,
                    0,
                    bulletAngle + 90f);

            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    transform.position,
                    rotation);

            Rigidbody2D bulletRB =
                bullet.GetComponent<Rigidbody2D>();

            if (bulletRB != null)
            {
                bulletRB.velocity =
                    direction *
                    bulletSpeed;
            }
        }
    }

    // =========================
    // AOE ATTACK
    // =========================

    void HandleAOEAttack()
    {
        if (!chasingPlayer) return;

        aoeTimer += Time.deltaTime;

        if (!aoeAttacking &&
            aoeTimer >= aoeCooldown)
        {
            aoeAttacking = true;

            if (aoeVisual != null)
                aoeVisual.gameObject.SetActive(true);
        }

        if (aoeAttacking)
        {
            ExpandAOE();
        }
    }

    void ExpandAOE()
    {
        currentAOERadius +=
            aoeExpandSpeed *
            Time.deltaTime;

        if (aoeCollider != null)
            aoeCollider.radius =
                currentAOERadius;

        // SCALE VISUAL
        if (aoeVisual != null)
        {
            float size =
                currentAOERadius * 2f;

            aoeVisual.localScale =
                new Vector3(
                    size,
                    size,
                    1f);
        }

        if (currentAOERadius >= aoeMaxRadius)
        {
            DamagePlayersInside();

            ResetAOE();
        }
    }

    void ResetAOE()
    {
        currentAOERadius = 0f;

        if (aoeCollider != null)
            aoeCollider.radius = 0f;

        aoeTimer = 0f;
        aoeAttacking = false;

        if (aoeVisual != null)
        {
            aoeVisual.localScale =
                Vector3.zero;

            aoeVisual.gameObject.SetActive(false);
        }
    }

    void DamagePlayersInside()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                aoeMaxRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth =
                    hit.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(
                        aoeDamage);
                }
            }
        }
    }

    // =========================
    // GIZMOS
    // =========================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRadius);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            stopDistance);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            aoeMaxRadius);
    }
}