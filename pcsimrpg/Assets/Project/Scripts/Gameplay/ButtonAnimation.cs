using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float shrinkSize = 0.9f; // How small it gets
    [SerializeField] private float animationSpeed = 10f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    void Update()
    {
        // Smoothly transition to the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Shrink on click
        targetScale = initialScale * shrinkSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Return to normal scale
        targetScale = initialScale;
    }
}