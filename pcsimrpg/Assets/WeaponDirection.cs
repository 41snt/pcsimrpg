using UnityEngine;

public class WeaponDirection : MonoBehaviour
{
    public VirtualJoystick joystick;

    void Update()
    {
        if (joystick == null) return;

        Vector2 moveInput = joystick.GetInput();

        if (moveInput.x > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1); // face right
        }
        else if (moveInput.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1); // face left
        }
    }
}