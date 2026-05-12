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

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        inventoryController = InventoryController.instance;
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

                // STACK ITEMS
                if (draggedItem.ID == targetItem.ID)
                {
                    targetItem.AddToStack(
                        draggedItem.quantity);

                    originalSlot.currentItem = null;

                    Destroy(gameObject);

                    inventoryController.RebuildItemCounts();

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

            inventoryController.RebuildItemCounts();

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

    void ReturnToOriginalSlot()
    {
        transform.SetParent(originalParent);

        rectTransform.anchoredPosition =
            Vector2.zero;

        transform.localScale = Vector3.one;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect =
            originalParent.parent
            .GetComponent<RectTransform>();

        return RectTransformUtility
            .RectangleContainsScreenPoint(
                inventoryRect,
                mousePosition);
    }

    // =========================
    // DROP ITEM
    // =========================

    void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();

        if (item == null)
            return;

        // FIND PLAYER
        Transform playerTransform =
            GameObject.FindGameObjectWithTag("Player")
            ?.transform;

        if (playerTransform == null)
            return;

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

            inventoryController.RebuildItemCounts();

            ReturnToOriginalSlot();

            return;
        }

        // REMOVE FROM SLOT
        originalSlot.currentItem = null;

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

        inventoryController.RebuildItemCounts();
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

        // NO SPACE
        item.AddToStack(splitAmount);

        Destroy(newItem);
    }
}