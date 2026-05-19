using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctTag;
    public Transform snapPoint;
    public GameObject nextSlot;
    public Transform placedParent;

    public float snapSpeed = 12f;
    public float scalePop = 1.1f;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        GameObject dropped = eventData.pointerDrag;
        DragItem drag = dropped.GetComponent<DragItem>();

        if (drag == null) return;

        if (!dropped.CompareTag(correctTag))
        {
            drag.ResetPosition();
            return;
        }

        dropped.transform.SetParent(placedParent);
        dropped.transform.SetAsLastSibling();

        CanvasGroup cg = dropped.GetComponent<CanvasGroup>();
        if (cg != null)
            cg.blocksRaycasts = true;

        StartCoroutine(SmoothSnap(dropped.transform, drag));

        if (nextSlot != null)
            nextSlot.SetActive(true);

        Image img = GetComponent<Image>();
        if (img != null)
            img.enabled = false;

        enabled = false;
    }

    IEnumerator SmoothSnap(Transform item, DragItem drag)
    {
        Vector3 startPos = item.position;
        Vector3 targetPos = snapPoint.position;

        Vector3 startScale = item.localScale;
        Vector3 popScale = startScale * scalePop;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;
            item.position = Vector3.Lerp(startPos, targetPos, t);
            item.localScale = Vector3.Lerp(startScale, popScale, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;
            item.localScale = Vector3.Lerp(popScale, startScale, t);
            yield return null;
        }

        item.position = targetPos;
        item.localScale = startScale;

        drag.LockItem();
    }
}