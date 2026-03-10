using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public VirtualJoystick joystick;

    private Vector2 moveInput;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = joystick.GetInput();

        if (moveInput.x > 0.1f)
        {
            animator.speed = 1f;
            animator.Play("RightRun");
        }
        else if (moveInput.x < -0.1f)
        {
            animator.speed = 1f;
            animator.Play("LeftRun");
        }
        else
        {
            animator.speed = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}