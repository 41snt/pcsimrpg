using UnityEngine;

public class EnemyMovementAOE : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;
    public float speed = 2f;
    public float attackRange = 2f;

    [Header("Animation")]
    public Animator animator;

    private EnemyAOEAttack aoeAttack;
    private Vector2 moveInput;

    void Start()
    {
        // Find the AOE attack script on this enemy
        aoeAttack = GetComponent<EnemyAOEAttack>();

        // Automatically find the player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Automatically find the Animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Calculate direction and distance to player
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move enemy toward the player
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            // Snap direction to 4 cardinal directions for animation
            moveInput = SnapDirection(direction);
            UpdateAnimation(moveInput);
        }
        else
        {
            // Stop moving and attack
            moveInput = Vector2.zero;
            UpdateAnimation(moveInput);
            StopAndAttack();
        }
    }

    void StopAndAttack()
    {
        if (aoeAttack != null)
        {
            aoeAttack.StartAOE();
        }
    }

    void UpdateAnimation(Vector2 direction)
    {
        if (animator == null) return;

        // Update animator parameters
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        // Removed isMoving because it doesn’t exist in your Animator
        // animator.SetBool("isMoving", direction != Vector2.zero);
    }

    Vector2 SnapDirection(Vector2 dir)
    {
        Vector2 snapped = dir;

        // Determine whether horizontal or vertical movement is dominant
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            snapped.y = 0;                // Horizontal movement
            snapped.x = Mathf.Sign(dir.x); // Snap to -1 or 1
        }
        else if (Mathf.Abs(dir.y) > 0)
        {
            snapped.x = 0;                // Vertical movement
            snapped.y = Mathf.Sign(dir.y); // Snap to -1 or 1
        }
        else
        {
            snapped = Vector2.zero;
        }

        return snapped;
    }
}
