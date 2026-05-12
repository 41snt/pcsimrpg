using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Tooltip("Boss only chases player inside this radius")]
    public float detectionRadius = 20f;

    [Tooltip("Boss stops this close to player")]
    public float stopDistance = 2.5f;

    [Header("Return")]
    public float returnSpeed = 2f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;

    // ORIGINAL POSITION
    private Vector3 startPosition;

    // MOVEMENT
    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    // STATES
    private bool chasingPlayer = false;

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

        if (animator == null)
            animator = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer =
            Vector2.Distance(transform.position, player.position);

        // PLAYER INSIDE DETECTION RADIUS
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
        // RETURN HOME
        else
        {
            ReturnToStart();
        }

        UpdateAnimation();
    }

    void ChasePlayer(float distanceToPlayer)
    {
        // STOP WHEN CLOSE
        if (distanceToPlayer <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection =
            (player.position - transform.position).normalized;

        lastDirection = moveDirection;

        rb.MovePosition(
            rb.position +
            moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void ReturnToStart()
    {
        float distanceToStart =
            Vector2.Distance(transform.position, startPosition);

        // ALREADY HOME
        if (distanceToStart <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection =
            (startPosition - transform.position).normalized;

        lastDirection = moveDirection;

        rb.MovePosition(
            rb.position +
            moveDirection * returnSpeed * Time.fixedDeltaTime);
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        Vector2 dir =
            moveDirection == Vector2.zero
            ? lastDirection
            : moveDirection;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }

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
    }
}