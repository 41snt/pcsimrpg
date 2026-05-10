using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public VirtualJoystick joystick;

    public float dashForce = 14f;
    public float dashDuration = 0.2f;
    public int dashManaCost = 20;

    private bool isDashing = false;

    private Vector2 moveInput;
    private Animator animator;
    private PlayerMana playerMana;

    public SpriteRenderer playerSprite;
    public GameObject afterImagePrefab;

    string currentAnimation;
    int lastDirection = 1; // 1 = right, -1 = left

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMana = GetComponent<PlayerMana>();

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // PAUSE SYSTEM
        if (PauseController.IsGamePaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (joystick != null)
        {
            moveInput = joystick.GetInput();
        }
        else
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        moveInput = moveInput.normalized;
        HandleAnimation();
    }

    void FixedUpdate()
    {
        // PAUSE SYSTEM
        if (PauseController.IsGamePaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!isDashing)
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    void HandleAnimation()
    {
        if (moveInput.x > 0.1f)
        {
            lastDirection = 1;
            PlayAnimation("RightRun");
        }
        else if (moveInput.x < -0.1f)
        {
            lastDirection = -1;
            PlayAnimation("LeftRun");
        }
        else if (Mathf.Abs(moveInput.y) > 0.1f)
        {
            if (lastDirection == 1) PlayAnimation("RightRun");
            else PlayAnimation("LeftRun");
        }
        else
        {
            if (lastDirection == 1) PlayAnimation("idleright");
            else PlayAnimation("idleleft");
        }
    }

    void PlayAnimation(string anim)
    {
        if (currentAnimation == anim) return;
        animator.Play(anim);
        currentAnimation = anim;
    }

    public bool DashButton()
    {
        // BLOCK DASH WHILE PAUSED
        if (PauseController.IsGamePaused)
            return false;

        return TryDash();
    }

    private bool TryDash()
    {
        if (isDashing) return false;

        if (playerMana != null && playerMana.UseMana(dashManaCost))
        {
            StartCoroutine(Dash());
            return true;
        }

        return false;
    }

    IEnumerator Dash()
    {
        isDashing = true;

        if (playerSprite != null)
            playerSprite.color = Color.cyan;

        Vector2 dashDirection = moveInput;

        // If no input, dash based on last direction
        if (dashDirection == Vector2.zero)
        {
            dashDirection = (lastDirection == 1) ? Vector2.right : Vector2.left;
        }

        float timer = 0f;

        while (timer < dashDuration)
        {
            // STOP DASH WHEN PAUSED
            if (PauseController.IsGamePaused)
            {
                rb.velocity = Vector2.zero;
                yield return null;
                continue;
            }

            rb.velocity = dashDirection * dashForce;

            SpawnAfterImage();

            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        if (playerSprite != null)
            playerSprite.color = Color.white;
    }

    void SpawnAfterImage()
    {
        if (afterImagePrefab == null || playerSprite == null) return;

        GameObject img = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        SpriteRenderer sr = img.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = playerSprite.sprite;
            sr.flipX = playerSprite.flipX;
        }

        Destroy(img, 0.3f);
    }
}