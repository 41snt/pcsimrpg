using UnityEngine;
using System.Collections;

public class WeaponDirection : MonoBehaviour
{
    public VirtualJoystick joystick;

    [Header("Swing Settings")]
    public float swingAngle = 90f;
    public float swingDuration = 0.15f;

    private bool isSwinging = false;

    // ORIGINAL SCALE
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (joystick == null) return;

        Vector2 moveInput = joystick.GetInput();

        // FACE RIGHT
        if (moveInput.x > 0.1f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z);
        }

        // FACE LEFT
        else if (moveInput.x < -0.1f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z);
        }
    }

    public void Swing()
    {
        if (!isSwinging)
        {
            StartCoroutine(SwingRoutine());
        }
    }

    private IEnumerator SwingRoutine()
    {
        isSwinging = true;

        float timer = 0f;

        float startAngle = -swingAngle;
        float endAngle = swingAngle;

        // SWING FORWARD
        while (timer < swingDuration)
        {
            float angle =
                Mathf.Lerp(
                    startAngle,
                    endAngle,
                    timer / swingDuration);

            transform.localRotation =
                Quaternion.Euler(0, 0, angle);

            timer += Time.deltaTime;

            yield return null;
        }

        // RESET TIMER
        timer = 0f;

        // SWING BACK
        while (timer < swingDuration)
        {
            float angle =
                Mathf.Lerp(
                    endAngle,
                    0f,
                    timer / swingDuration);

            transform.localRotation =
                Quaternion.Euler(0, 0, angle);

            timer += Time.deltaTime;

            yield return null;
        }

        transform.localRotation = Quaternion.identity;

        isSwinging = false;
    }
}