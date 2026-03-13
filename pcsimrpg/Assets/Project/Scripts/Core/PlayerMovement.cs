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
    }

    void Update()
    {
        moveInput = joystick.GetInput();

        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
        }
    }

    void FixedUpdate()
    {
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
            // Moving up or down
            if (lastDirection == 1)
                PlayAnimation("RightRun");
            else
                PlayAnimation("LeftRun");
        }
        else
        {
            if (lastDirection == 1)
                PlayAnimation("idleright");
            else
                PlayAnimation("idleleft");
        }
    }

    void PlayAnimation(string anim)
    {
        if (currentAnimation == anim) return;

        animator.Play(anim);
        currentAnimation = anim;
    }

    void TryDash()
    {
        if (isDashing) return;

        if (playerMana != null && playerMana.UseMana(dashManaCost))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        if (playerSprite != null)
            playerSprite.color = Color.cyan;

        Vector2 dashDirection = moveInput;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = lastDirection == 1 ? Vector2.right : Vector2.left;
        }

        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.velocity = dashDirection.normalized * dashForce;

            SpawnAfterImage();

            timer += 0.03f;
            yield return new WaitForSeconds(0.03f);
        }

        if (playerSprite != null)
            playerSprite.color = Color.white;

        isDashing = false;
    }

    void SpawnAfterImage()
    {
        if (afterImagePrefab == null || playerSprite == null) return;

        GameObject img = Instantiate(
            afterImagePrefab,
            playerSprite.transform.position,
            Quaternion.identity
        );

        SpriteRenderer sr = img.GetComponent<SpriteRenderer>();
        sr.sprite = playerSprite.sprite;
    }

      public void DashButton()
     {
        TryDash();
     }
}