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

    // Start is called before the first frame update
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        /* for (int i = 0; i < slotCount; i++)
         {
             Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

             if (i < itemPrefabs.Length)
             {
                 GameObject item = Instantiate(itemPrefabs[i], slot.transform);

                 item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                 slot.currentItem = item;
             }
         } */
    }

<<<<<<< Updated upstream
=======
    public static InventoryController Instance => instance;

    private Dictionary<int, int> itemsCountCache =
        new Dictionary<int, int>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (instance != null &&
            instance != this)
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
            for (int i = 0;
                i < slotCount;
                i++)
            {
                GameObject slot =
                    Instantiate(
                        slotPrefab,
                        inventoryPanel.transform);

                slot.transform.localScale =
                    Vector3.one;
            }
        }

        ForceCleanInventory();

        RebuildItemCounts();
    }

    // =========================
    // CLEAN INVENTORY
    // =========================

    public void ForceCleanInventory()
    {
        foreach (Transform slotTransform
            in inventoryPanel.transform)
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
            {
                Destroy(slot.currentItem);

                slot.currentItem = null;
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

        foreach (Transform slotTransform
            in inventoryPanel.transform)
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

            itemsCountCache[item.ID] +=
                item.quantity;
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
>>>>>>> Stashed changes

    public bool AddItem(GameObject itemPrefab)
    {
        //Look for empty slot
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
<<<<<<< Updated upstream
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
=======
            Slot slot =
                slotTransform.GetComponent<Slot>();

            if (slot == null)
                continue;

            if (slot.currentItem == null)
                continue;

            Item slotItem =
                slot.currentItem.GetComponent<Item>();

            if (slotItem == null)
                continue;

            if (slotItem.ID ==
                itemToAdd.ID)
            {
                slotItem.AddToStack();

                RebuildItemCounts();

>>>>>>> Stashed changes
                return true;
            }
        }

        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTranform.GetSiblingIndex() });
            }
        }

        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        //Clear inventory panel - avoid duplicates
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        //Create new slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        //Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();

                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);

                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    slot.currentItem = item;
                }
            }
        }
    }
} 