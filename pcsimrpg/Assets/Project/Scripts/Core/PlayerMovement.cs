using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public VirtualJoystick joystick;

    private Vector2 moveInput;

    void Update()
    {
        moveInput = joystick.GetInput();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}
