using UnityEngine;

public class EnemyMovementMelee : MonoBehaviour
{
    public float speed = 3f;
    public float stopDistance = 1f; // distance to stop from player

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // For topdown
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

            rb.MovePosition(newPosition);
        }
    }
}