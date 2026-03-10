using UnityEngine;

public class EnemyMovementAOE : MonoBehaviour
{
    public float speed = 2.5f;
    public float stopDistance = 1f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }

        FacePlayer();
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}