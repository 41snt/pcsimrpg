using UnityEngine;

public class InventorySplitButton : MonoBehaviour
{
    public GameObject inventoryPage;

    void Update()
    {
        // Hide button when inventory is closed
        gameObject.SetActive(inventoryPage.activeSelf);
    }

    public void SplitSelectedItem()
    {
        if (ItemDragHandler.selectedItem != null)
        {
            ItemDragHandler.selectedItem.SplitStack();
        }
    }
}