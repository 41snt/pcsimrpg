using UnityEngine;

public class EnemyMovementMelee : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastDirection;

    private bool isAttacking = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        moveDirection = (player.position - transform.position).normalized;

        if (distance > attackRange)
        {
            isAttacking = false;

            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

            lastDirection = moveDirection;

            UpdateMovementAnimation(moveDirection);
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (!isAttacking)
            {
                Attack();
            }
        }
    }

    void UpdateMovementAnimation(Vector2 dir)
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }

    void Attack()
    {
        if (animator == null) return;

        isAttacking = true;

        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);

        animator.SetTrigger("Attack");
    }
}