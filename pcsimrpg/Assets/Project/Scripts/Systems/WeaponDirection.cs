using UnityEngine;
using System.Collections;

public class WeaponDirection : MonoBehaviour
{
    public VirtualJoystick joystick;

    [Header("Swing Settings")]
    public float swingAngle = 90f;
    public float swingDuration = 0.15f;

    private bool isSwinging = false;

    void Update()
    {
        if (joystick == null) return;

        Vector2 moveInput = joystick.GetInput();
        if (moveInput.x > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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

        while (timer < swingDuration)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, timer / swingDuration);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        isSwinging = false;
    }
}
