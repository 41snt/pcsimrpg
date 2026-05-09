using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector2 inputVector;

    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            position.x = (position.x / joystickBackground.sizeDelta.x) * 2;
            position.y = (position.y / joystickBackground.sizeDelta.y) * 2;

            inputVector = new Vector2(position.x, position.y);

            if (inputVector.magnitude > 1)
                inputVector = inputVector.normalized;

            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * (joystickBackground.sizeDelta.x / 3),
                inputVector.y * (joystickBackground.sizeDelta.y / 3));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInput()
    {
        // Keyboard WASD input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 keyboardInput = new Vector2(horizontal, vertical).normalized;

        // If keyboard is being used, return keyboard input
        if (keyboardInput != Vector2.zero)
            return keyboardInput;

        // Otherwise return joystick input
        return inputVector;
    }
}