using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    public static InventoryController instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
    }

    // =========================
    // ADD ITEM
    // =========================

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd =
            itemPrefab.GetComponent<Item>();

        if (itemToAdd == null)
            return false;

        // STACK ITEMS
        foreach (Transform slotTransform
            in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot != null &&
                slot.currentItem != null)
            {
                Item slotItem =
                    slot.currentItem.GetComponent<Item>();

                if (slotItem != null &&
                    slotItem.ID == itemToAdd.ID)
                {
                    slotItem.AddToStack();

                    return true;
                }
            }
        }

        // FIND EMPTY SLOT
        foreach (Transform slotTransform
            in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot != null &&
                slot.currentItem == null)
            {
                GameObject newItem =
                    Instantiate(
                        itemPrefab,
                        slotTransform);

                newItem.GetComponent<RectTransform>()
                    .anchoredPosition =
                    Vector2.zero;

                newItem.transform.localScale =
                    Vector3.one;

                slot.currentItem = newItem;

                return true;
            }
        }

        return false;
    }

    // =========================
    // SPLIT ALL STACKS
    // =========================

    public void SplitAllStacks()
    {
        foreach (Transform slotTransform
            in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null ||
                slot.currentItem == null)
                continue;

            Item item =
                slot.currentItem.GetComponent<Item>();

            if (item == null)
                continue;

            // ONLY SPLIT STACKS > 1
            if (item.quantity <= 1)
                continue;

            int splitAmount =
                item.quantity / 2;

            if (splitAmount <= 0)
                continue;

            // FIND EMPTY SLOT
            foreach (Transform emptySlotTransform
                in inventoryPanel.transform)
            {
                Slot emptySlot =
                    emptySlotTransform
                    .GetComponent<Slot>();

                if (emptySlot != null &&
                    emptySlot.currentItem == null)
                {
                    // REMOVE HALF
                    item.RemoveFromStack(
                        splitAmount);

                    // CREATE NEW STACK
                    GameObject newItem =
                        item.CloneItem(splitAmount);

                    newItem.transform.SetParent(
                        emptySlotTransform);

                    newItem.GetComponent<RectTransform>()
                        .anchoredPosition =
                        Vector2.zero;

                    newItem.transform.localScale =
                        Vector3.one;

                    emptySlot.currentItem =
                        newItem;

                    break;
                }
            }
        }
    }

    // =========================
    // SAVE INVENTORY
    // =========================

    public List<InventorySaveData>
        GetInventoryItems()
    {
        List<InventorySaveData> invData =
            new List<InventorySaveData>();

        foreach (Transform slotTransform
            in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot.currentItem != null)
            {
                Item item =
                    slot.currentItem.GetComponent<Item>();

                invData.Add(
                    new InventorySaveData
                    {
                        itemID = item.ID,
                        slotIndex =
                            slotTransform.GetSiblingIndex(),
                        quantity = item.quantity
                    });
            }
        }

        return invData;
    }

    // =========================
    // LOAD INVENTORY
    // =========================

    public void SetInventoryItems(
        List<InventorySaveData> inventorySaveData)
    {
        // CLEAR INVENTORY
        foreach (Transform child
            in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // CREATE NEW SLOTS
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(
                slotPrefab,
                inventoryPanel.transform);
        }

        // LOAD ITEMS
        foreach (InventorySaveData data
            in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot =
                    inventoryPanel.transform
                    .GetChild(data.slotIndex)
                    .GetComponent<Slot>();

                GameObject itemPrefab =
                    itemDictionary.GetItemPrefab(
                        data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item =
                        Instantiate(
                            itemPrefab,
                            slot.transform);

                    item.GetComponent<RectTransform>()
                        .anchoredPosition =
                        Vector2.zero;

                    item.transform.localScale =
                        Vector3.one;

                    Item itemComponent =
                        item.GetComponent<Item>();

                    if (itemComponent != null)
                    {
                        itemComponent.quantity =
                            data.quantity;

                        itemComponent
                            .UpdateQuantityDisplay();
                    }

                    slot.currentItem = item;
                }
            }
        }
    }
}