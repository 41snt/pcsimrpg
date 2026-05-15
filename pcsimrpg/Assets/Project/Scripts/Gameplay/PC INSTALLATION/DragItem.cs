using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        canvasGroup.blocksRaycasts = false;

        transform.SetAsLastSibling(); // always on top
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // IMPORTANT: if NOT dropped on slot, return
        if (!eventData.pointerEnter)
        {
            transform.position = startPos;
        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }
}