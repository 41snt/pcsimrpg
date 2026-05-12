using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController =
            FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(
        Collider2D collision)
    {
        if (!collision.CompareTag("Item"))
            return;

        Item item =
            collision.GetComponent<Item>();

        if (item == null)
            return;

        bool itemAdded =
            inventoryController.AddItem(
                collision.gameObject);

        if (!itemAdded)
            return;

        // =========================
        // UPDATE QUESTS
        // =========================

        if (QuestController.Instance != null)
        {
            foreach (QuestProgress quest
                in QuestController.Instance
                .activateQuests)
            {
                if (quest == null)
                    continue;

                if (quest.objectives == null)
                    continue;

                foreach (QuestObjective objective
                    in quest.objectives)
                {
                    if (objective == null)
                        continue;

                    // MATCH ITEM NAME
                    if (objective.objectiveID ==
                        item.Name)
                    {
                        objective.currentAmount++;

                        Debug.Log(
                            "Quest Updated: "
                            + objective.description
                            + " "
                            + objective.currentAmount
                            + "/"
                            + objective.requiredAmount);
                    }
                }
            }
        }

        // PICKUP UI
        if (ItemPickupUIController.Instance != null)
        {
            Sprite itemIcon =
                item.GetComponent<UnityEngine.UI.Image>()
                ?.sprite;

            ItemPickupUIController.Instance
                .ShowItemPickup(
                    item.Name,
                    itemIcon);
        }

        item.PickUp();

        Destroy(collision.gameObject);
    }
}