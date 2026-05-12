using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // =========================
    // SINGLETON
    // =========================

    public static InventoryController instance
    {
        get;
        private set;
    }

    public static InventoryController Instance => instance;

    private Dictionary<int, int> itemsCountCache =
        new Dictionary<int, int>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        itemDictionary =
            FindObjectOfType<ItemDictionary>();

        // CREATE SLOTS
        if (inventoryPanel.transform.childCount == 0)
        {
            for (int i = 0; i < slotCount; i++)
            {
                GameObject slot =
                    Instantiate(
                        slotPrefab,
                        inventoryPanel.transform);

                slot.transform.localScale =
                    Vector3.one;
            }
        }

        RebuildItemCounts();
    }

    // =========================
    // ITEM COUNTS
    // =========================

    public void RebuildItemCounts()
    {
        itemsCountCache.Clear();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
                continue;

            Item item =
                slot.currentItem.GetComponent<Item>();

            if (item == null)
                continue;

            if (!itemsCountCache.ContainsKey(item.ID))
            {
                itemsCountCache[item.ID] = 0;
            }

            itemsCountCache[item.ID] += item.quantity;
        }

        OnInventoryChanged?.Invoke();
    }

    public Dictionary<int, int> GetItemCounts()
    {
        return itemsCountCache;
    }

    // =========================
    // ADD ITEM
    // =========================

    public bool AddItem(GameObject itemPrefab)
    {
        if (itemPrefab == null)
            return false;

        Item itemToAdd =
            itemPrefab.GetComponent<Item>();

        if (itemToAdd == null)
            return false;

        // STACK FIRST
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
                continue;

            Item slotItem =
                slot.currentItem.GetComponent<Item>();

            if (slotItem != null &&
                slotItem.ID == itemToAdd.ID)
            {
                slotItem.AddToStack();

                RebuildItemCounts();

                return true;
            }
        }

        // EMPTY SLOT
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
            {
                GameObject newItem =
                    Instantiate(
                        itemPrefab,
                        slotTransform);

                RectTransform rect =
                    newItem.GetComponent<RectTransform>();

                if (rect != null)
                {
                    rect.anchoredPosition =
                        Vector2.zero;
                }

                newItem.transform.localScale =
                    Vector3.one;

                slot.currentItem =
                    newItem;

                RebuildItemCounts();

                return true;
            }
        }

        Debug.Log("Inventory Full");

        return false;
    }

    // =========================
    // REMOVE ITEMS
    // =========================

    public void RemoveItemsFromInventory(
        int itemID,
        int amountToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (amountToRemove <= 0)
                break;

            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
                continue;

            Item item =
                slot.currentItem.GetComponent<Item>();

            if (item == null)
                continue;

            if (item.ID != itemID)
                continue;

            int removed =
                Mathf.Min(
                    amountToRemove,
                    item.quantity);

            item.RemoveFromStack(removed);

            amountToRemove -= removed;

            if (item.quantity <= 0)
            {
                Destroy(slot.currentItem);

                slot.currentItem = null;
            }
        }

        RebuildItemCounts();
    }

    // =========================
    // SAVE
    // =========================

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData =
            new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
                continue;

            Item item =
                slot.currentItem.GetComponent<Item>();

            if (item == null)
                continue;

            invData.Add(
                new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex =
                        slotTransform.GetSiblingIndex(),
                    quantity = item.quantity
                });
        }

        return invData;
    }

    // =========================
    // LOAD
    // =========================

    public void SetInventoryItems(
        List<InventorySaveData> inventorySaveData)
    {
        if (inventorySaveData == null)
            return;

        // CLEAR
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot != null &&
                slot.currentItem != null)
            {
                Destroy(slot.currentItem);

                slot.currentItem = null;
            }
        }

        // LOAD
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex >= slotCount)
                continue;

            Slot slot =
                inventoryPanel.transform
                .GetChild(data.slotIndex)
                .GetComponent<Slot>();

            GameObject itemPrefab =
                itemDictionary.GetItemPrefab(
                    data.itemID);

            if (itemPrefab == null)
                continue;

            GameObject item =
                Instantiate(
                    itemPrefab,
                    slot.transform);

            RectTransform rect =
                item.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.anchoredPosition =
                    Vector2.zero;
            }

            item.transform.localScale =
                Vector3.one;

            Item itemComponent =
                item.GetComponent<Item>();

            if (itemComponent != null)
            {
                itemComponent.quantity =
                    data.quantity;

                itemComponent.UpdateQuantityDisplay();
            }

            slot.currentItem = item;
        }

        RebuildItemCounts();
    }
}