using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctTag;
    public Transform snapPoint;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null) return;

        DragItem drag = dropped.GetComponent<DragItem>();

        if (dropped.CompareTag(correctTag))
        {
            dropped.transform.position = snapPoint.position;

            // LOCK ONLY ON CORRECT SLOT
            drag.enabled = false;
        }
        else
        {
            // WRONG SLOT → go back
            drag.ResetPosition();
        }
    }
}