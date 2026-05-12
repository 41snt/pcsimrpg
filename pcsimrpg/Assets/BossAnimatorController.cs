using UnityEngine;

public class BossAnimatorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;

    void Start()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        lastPosition =
            transform.position;
    }

    void Update()
    {
        Vector3 movement =
            transform.position - lastPosition;

        // FACE LEFT
        if (movement.x < -0.001f)
        {
            spriteRenderer.flipX = true;
        }

        // FACE RIGHT
        else if (movement.x > 0.001f)
        {
            spriteRenderer.flipX = false;
        }

        lastPosition =
            transform.position;
    }
}