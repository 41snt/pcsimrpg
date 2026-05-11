using UnityEngine;

public class EnemyMovementProjectile : MonoBehaviour
{
    public float speed = 3f;
    public float stopDistance = 3f;

    public GameObject bulletPrefab;
    public float shootCooldown = 2f;
    public float bulletSpeed = 6f;

    private float shootTimer;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    private bool touchingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (touchingPlayer)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            moveDirection = (player.position - transform.position).normalized;
            lastDirection = moveDirection;

            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    void Update()
    {
        UpdateAnimation();
        ShootLogic();
    }

    void UpdateAnimation()
    {
        Vector2 dir = moveDirection == Vector2.zero ? lastDirection : moveDirection;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }

    void ShootLogic()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        if (bulletRB != null)
            bulletRB.velocity = direction * bulletSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}