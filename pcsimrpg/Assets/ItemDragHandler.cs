using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerClickHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    private InventoryController inventoryController;

    // MOBILE SELECTED ITEM
    public static ItemDragHandler selectedItem;

    private void Start()
    {
        // Get required components
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // Ensure a CanvasGroup exists
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Use the correct singleton property name
        inventoryController = InventoryController.Instance;
    }

    // =========================
    // SELECT ITEM (MOBILE)
    // =========================

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedItem = this;

        Debug.Log("Selected Item: " + gameObject.name);
    }

    // =========================
    // DRAGGING
    // =========================

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // Move to root canvas while dragging
        transform.SetParent(transform.root);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        transform.localScale = Vector3.one;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = null;

        if (eventData.pointerEnter != null)
        {
            dropSlot =
                eventData.pointerEnter.GetComponent<Slot>();

            if (dropSlot == null)
            {
                dropSlot =
                    eventData.pointerEnter
                    .GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot =
            originalParent.GetComponent<Slot>();

        // Safety check
        if (originalSlot == null)
        {
            ReturnToOriginalSlot();
            return;
        }

        // =========================
        // DROP INTO SLOT
        // =========================

        if (dropSlot != null)
        {
            // SAME SLOT
            if (dropSlot == originalSlot)
            {
                ReturnToOriginalSlot();
                return;
            }

            // SLOT HAS ITEM
            if (dropSlot.currentItem != null)
            {
                Item draggedItem =
                    GetComponent<Item>();

                Item targetItem =
                    dropSlot.currentItem.GetComponent<Item>();

                if (draggedItem != null && targetItem != null)
                {
                    // STACK ITEMS
                    if (draggedItem.ID == targetItem.ID)
                    {
                        targetItem.AddToStack(
                            draggedItem.quantity);

                        originalSlot.currentItem = null;

                        Destroy(gameObject);

                        if (inventoryController != null)
                        {
                            inventoryController.RebuildItemCounts();
                        }

                        return;
                    }
                    else
                    {
                        // SWAP ITEMS
                        GameObject targetObject =
                            dropSlot.currentItem;

                        targetObject.transform.SetParent(
                            originalSlot.transform);

                        targetObject.GetComponent<RectTransform>()
                            .anchoredPosition = Vector2.zero;

                        targetObject.transform.localScale =
                            Vector3.one;

                        originalSlot.currentItem =
                            targetObject;
                    }
                }
            }
            else
            {
                originalSlot.currentItem = null;
            }

            // MOVE DRAGGED ITEM
            transform.SetParent(dropSlot.transform);

            rectTransform.anchoredPosition =
                Vector2.zero;

            transform.localScale = Vector3.one;

            dropSlot.currentItem = gameObject;

            if (inventoryController != null)
            {
                inventoryController.RebuildItemCounts();
            }

            return;
        }

        // =========================
        // DROP OUTSIDE INVENTORY
        // =========================

        if (!IsWithinInventory(eventData.position))
        {
            DropItem(originalSlot);
            return;
        }

        // RETURN BACK
        ReturnToOriginalSlot();
    }

    private void ReturnToOriginalSlot()
    {
        if (originalParent == null)
            return;

        transform.SetParent(originalParent);

        rectTransform.anchoredPosition =
            Vector2.zero;

        transform.localScale = Vector3.one;
    }

    private bool IsWithinInventory(Vector2 mousePosition)
    {
        if (originalParent == null ||
            originalParent.parent == null)
            return false;

        RectTransform inventoryRect =
            originalParent.parent
            .GetComponent<RectTransform>();

        if (inventoryRect == null)
            return false;

        return RectTransformUtility
            .RectangleContainsScreenPoint(
                inventoryRect,
                mousePosition);
    }

    // =========================
    // DROP ITEM
    // =========================

    private void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();

        if (item == null)
            return;

        // FIND PLAYER
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")
            ?.transform;

        if (playerTransform == null)
        {
            ReturnToOriginalSlot();
            return;
        }

        // RANDOM DROP POSITION
        Vector2 dropOffset =
            Random.insideUnitCircle.normalized *
            Random.Range(
                minDropDistance,
                maxDropDistance);

        Vector2 dropPosition =
            (Vector2)playerTransform.position
            + dropOffset;

        // STACK DROP
        if (item.quantity > 1)
        {
            item.RemoveFromStack(1);

            GameObject droppedItem =
                item.CloneItem(1);

            droppedItem.transform.SetParent(null);

            droppedItem.transform.position =
                dropPosition;

            droppedItem.transform.localScale =
                Vector3.one;

            BounceEffect bounce =
                droppedItem.GetComponent<BounceEffect>();

            if (bounce != null)
            {
                bounce.StartBounce();
            }

            if (inventoryController != null)
            {
                inventoryController.RebuildItemCounts();
            }

            ReturnToOriginalSlot();
            return;
        }

        // REMOVE FROM SLOT
        if (originalSlot != null)
        {
            originalSlot.currentItem = null;
        }

        // DROP ENTIRE ITEM
        transform.SetParent(null);

        transform.position = dropPosition;

        transform.localScale = Vector3.one;

        BounceEffect itemBounce =
            GetComponent<BounceEffect>();

        if (itemBounce != null)
        {
            itemBounce.StartBounce();
        }

        if (inventoryController != null)
        {
            inventoryController.RebuildItemCounts();
        }
    }

    // =========================
    // SPLIT STACK
    // =========================

    public void SplitStack()
    {
        Item item = GetComponent<Item>();

        if (item == null)
            return;

        if (item.quantity <= 1)
            return;

        int splitAmount =
            item.quantity / 2;

        if (splitAmount <= 0)
            return;

        // Ensure inventory controller exists
        if (inventoryController == null)
            return;

        // REMOVE HALF
        item.RemoveFromStack(splitAmount);

        // CREATE NEW STACK
        GameObject newItem =
            item.CloneItem(splitAmount);

        newItem.transform.localScale =
            Vector3.one;

        // FIND EMPTY SLOT
        foreach (Transform slotTransform
            in inventoryController.inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot != null &&
                slot.currentItem == null)
            {
                newItem.transform.SetParent(
                    slotTransform);

                newItem.GetComponent<RectTransform>()
                    .anchoredPosition =
                    Vector2.zero;

                newItem.transform.localScale =
                    Vector3.one;

                slot.currentItem = newItem;

                inventoryController.RebuildItemCounts();
                return;
            }
        }

        // NO SPACE - restore original stack
        item.AddToStack(splitAmount);

        Destroy(newItem);
    }
}