using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform joystickBase;
    public RectTransform joystickKnob;
    public float maxDistance = 75f;

    private Vector2 inputVector;

    public Vector2 GetInput()
    {
        return inputVector;
    }

    void Update()
    {
        // ADD: WASD / Arrow key input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 keyboardInput = new Vector2(horizontal, vertical).normalized;

        if (keyboardInput.magnitude > 0)
        {
            inputVector = keyboardInput;
            joystickKnob.anchoredPosition = keyboardInput * maxDistance;
        }
        else
        {
            // ADD: Reset when keys are released
            joystickKnob.anchoredPosition = Vector2.zero;
            inputVector = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBase,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        position = Vector2.ClampMagnitude(position, maxDistance);
        joystickKnob.anchoredPosition = position;
        inputVector = position / maxDistance;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickKnob.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}
