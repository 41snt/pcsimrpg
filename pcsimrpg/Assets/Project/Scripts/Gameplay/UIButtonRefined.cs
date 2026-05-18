using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonRefined : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Button Feel")]
    public float buttonSnapSpeed = 15f; // Slightly slower than 'instant' for weight
    public float shrinkFactor = 0.9f;   // A bit more shrink to show the animation

    public GameObject panelToOpen;
    public MenuManager manager;

    private Vector3 _originalScale;
    private Vector3 _targetScale;

    void Awake()
    {
        // Lock the original scale immediately
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
    }

    void Update()
    {
        // Constantly move toward the target. This fixes the 'fast click' bug.
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * buttonSnapSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _targetScale = _originalScale * shrinkFactor;

        if (manager != null && panelToOpen != null)
        {
            manager.OpenPanel(panelToOpen);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }
}