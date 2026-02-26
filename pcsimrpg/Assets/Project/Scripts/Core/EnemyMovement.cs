using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f; // Important for topdown
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
