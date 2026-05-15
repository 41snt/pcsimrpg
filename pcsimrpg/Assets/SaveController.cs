using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;

    private InventoryController inventoryController;
    private HotbarController hotbarController;

    private Chest[] chests;

    // =========================
    // START
    // =========================

    private IEnumerator Start()
    {
        InitializeComponents();

        // Wait 1 frame so player systems initialize
        yield return null;

        LoadGame();
    }

    // =========================
    // INITIALIZE
    // =========================

    private void InitializeComponents()
    {
        saveLocation =
            Path.Combine(
                Application.persistentDataPath,
                "saveData.json");

        inventoryController =
            FindObjectOfType<InventoryController>();

        hotbarController =
            FindObjectOfType<HotbarController>();

        chests =
            FindObjectsOfType<Chest>();
    }

    // =========================
    // AUTO SAVE
    // =========================

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // =========================
    // SAVE GAME
    // =========================

    public void SaveGame()
    {
        if (inventoryController == null)
            return;

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        CinemachineConfiner confiner =
            FindObjectOfType<CinemachineConfiner>();

        SaveData saveData =
            new SaveData
            {
                playerPosition =
                    player.transform.position,

                mapBoundary =
                    confiner != null &&
                    confiner.m_BoundingShape2D != null
                    ? confiner.m_BoundingShape2D.gameObject.name
                    : "",

                inventorySaveData =
                    inventoryController.GetInventoryItems(),

                hotbarSaveData =
                    hotbarController != null
                    ? hotbarController.GetHotbarItems()
                    : new List<InventorySaveData>(),

                chestSaveData =
                    GetChestState(),

                questProgressData =
                    QuestController.Instance != null
                    ? QuestController.Instance.activateQuests
                    : new List<Quest.QuestProgress>(),

                handedInQuestIDs =
                    QuestController.Instance != null
                    ? QuestController.Instance.handInQuestIDs
                    : new List<string>()
            };

        string json =
            JsonUtility.ToJson(
                saveData,
                true);

        File.WriteAllText(
            saveLocation,
            json);

        Debug.Log("Game Saved");
    }

    // =========================
    // CHEST SAVE
    // =========================

    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates =
            new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            if (chest == null)
                continue;

            ChestSaveData chestSaveData =
                new ChestSaveData
                {
                    chestID =
                        chest.ChestID,

                    isOpened =
                        chest.IsOpened
                };

            chestStates.Add(
                chestSaveData);
        }

        return chestStates;
    }

    // =========================
    // LOAD GAME
    // =========================

    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            CreateNewSave();

            return;
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(
                File.ReadAllText(saveLocation));

        if (saveData == null)
            return;

        // =========================
        // PLAYER
        // =========================

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Rigidbody2D rb =
                player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Stop movement before teleporting
                rb.velocity = Vector2.zero;

                // Move player safely
                rb.position =
                    saveData.playerPosition;
            }
            else
            {
                // Fallback if no Rigidbody2D
                player.transform.position =
                    saveData.playerPosition;
            }
        }

        // =========================
        // MAP BOUNDARY
        // =========================

        CinemachineConfiner confiner =
            FindObjectOfType<CinemachineConfiner>();

        if (confiner != null &&
            !string.IsNullOrEmpty(
                saveData.mapBoundary))
        {
            GameObject boundaryObject =
                GameObject.Find(
                    saveData.mapBoundary);

            if (boundaryObject != null)
            {
                PolygonCollider2D boundary =
                    boundaryObject.GetComponent<PolygonCollider2D>();

                if (boundary != null)
                {
                    confiner.m_BoundingShape2D =
                        boundary;
                }
            }
        }

        // =========================
        // INVENTORY
        // =========================

        if (saveData.inventorySaveData != null)
        {
            inventoryController.SetInventoryItems(
                saveData.inventorySaveData);
        }

        // =========================
        // HOTBAR
        // =========================

        if (hotbarController != null &&
            saveData.hotbarSaveData != null)
        {
            hotbarController.SetHotbarItems(
                saveData.hotbarSaveData);
        }

        // =========================
        // CHESTS
        // =========================

        if (saveData.chestSaveData != null)
        {
            LoadChestStates(
                saveData.chestSaveData);
        }

        // =========================
        // QUESTS
        // =========================

        if (QuestController.Instance != null)
        {
            QuestController.Instance.LoadQuestProgress(
                saveData.questProgressData);

            QuestController.Instance.handInQuestIDs =
                saveData.handedInQuestIDs != null
                ? saveData.handedInQuestIDs
                : new List<string>();
        }

        Debug.Log("Game Loaded");
    }

    // =========================
    // LOAD CHESTS
    // =========================

    private void LoadChestStates(
        List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            if (chest == null)
                continue;

            ChestSaveData chestSaveData =
                chestStates.FirstOrDefault(
                    c => c.chestID == chest.ChestID);

            if (chestSaveData != null)
            {
                chest.SetOpened(
                    chestSaveData.isOpened);
            }
        }
    }

    // =========================
    // CREATE NEW SAVE
    // =========================

    private void CreateNewSave()
    {
        // Clear inventory
        inventoryController.SetInventoryItems(
            new List<InventorySaveData>());

        // Clear hotbar
        if (hotbarController != null)
        {
            hotbarController.SetHotbarItems(
                new List<InventorySaveData>());
        }

        // Reset chests
        foreach (Chest chest in chests)
        {
            if (chest != null)
            {
                chest.SetOpened(false);
            }
        }

        // Reset quests
        if (QuestController.Instance != null)
        {
            QuestController.Instance.activateQuests.Clear();

            QuestController.Instance.handInQuestIDs.Clear();
        }

        SaveGame();

        Debug.Log("New Save Created");
    }
}