using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private Transform startParent;
    private CanvasGroup canvasGroup;
    private bool isLocked = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        startPos = transform.position;
        startParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 pos
        );

        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        canvasGroup.blocksRaycasts = true;

        if (transform.parent == startParent)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        transform.SetParent(startParent);
    }

    public void LockItem()
    {
        isLocked = true;
        canvasGroup.blocksRaycasts = true;
    }
}