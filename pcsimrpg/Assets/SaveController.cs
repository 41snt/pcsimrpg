using Cinemachine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string autoSaveLocation;
    private string manualSaveLocation;

    private InventoryController inventoryController;
    private HotbarController hotbarController;

    private Chest[] chests;

    // =========================
    // START
    // =========================

    private void Start()
    {
        InitializeComponents();

        // CREATE START SAVE
        if (!File.Exists(autoSaveLocation))
        {
            CreateAutoSave();
        }

        // LOAD AUTO SAVE
        LoadAutoSave();
    }

    private void InitializeComponents()
    {
        autoSaveLocation =
            Path.Combine(
                Application.persistentDataPath,
                "autoSave.json");

        manualSaveLocation =
            Path.Combine(
                Application.persistentDataPath,
                "manualSave.json");

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

    public void CreateAutoSave()
    {
        SaveToFile(autoSaveLocation);

        Debug.Log("Auto Save Created");
    }

    public void LoadAutoSave()
    {
        LoadFromFile(autoSaveLocation);

        Debug.Log("Loaded Auto Save");
    }

    // =========================
    // MANUAL SAVE
    // =========================

    public void ManualSave()
    {
        SaveToFile(manualSaveLocation);

        Debug.Log("Manual Save Complete");
    }

    public void LoadManualSave()
    {
        LoadFromFile(manualSaveLocation);

        Debug.Log("Manual Save Loaded");
    }

    // =========================
    // SAVE CORE
    // =========================

    private void SaveToFile(string path)
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
                    : new List<QuestProgress>(),

                handedInQuestIDs =
                    QuestController.Instance != null
                    ? QuestController.Instance.handedInQuestIDs
                    : new List<string>()
            };

        string json =
            JsonUtility.ToJson(
                saveData,
                true);

        File.WriteAllText(
            path,
            json);
    }

    // =========================
    // LOAD CORE
    // =========================

    private void LoadFromFile(string path)
    {
        if (!File.Exists(path))
            return;

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(
                File.ReadAllText(path));

        if (saveData == null)
            return;

        // =========================
        // PLAYER
        // =========================

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position =
                saveData.playerPosition;
        }

        // =========================
        // MAP
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

        if (inventoryController != null)
        {
            inventoryController.SetInventoryItems(
                saveData.inventorySaveData);
        }

        // =========================
        // HOTBAR
        // =========================

        if (hotbarController != null)
        {
            hotbarController.SetHotbarItems(
                saveData.hotbarSaveData);
        }

        // =========================
        // CHESTS
        // =========================

        LoadChestStates(
            saveData.chestSaveData);

        // =========================
        // QUESTS
        // =========================

        if (QuestController.Instance != null)
        {
            QuestController.Instance.LoadQuestProgress(
                saveData.questProgressData);

            QuestController.Instance.handedInQuestIDs =
                saveData.handedInQuestIDs;
        }
    }

    // =========================
    // CHESTS
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
                        chest.chestID,

                    isOpened =
                        chest.isOpened
                };

            chestStates.Add(
                chestSaveData);
        }

        return chestStates;
    }

    private void LoadChestStates(
        List<ChestSaveData> chestStates)
    {
        if (chestStates == null)
            return;

        foreach (Chest chest in chests)
        {
            if (chest == null)
                continue;

            ChestSaveData chestSaveData =
                chestStates.FirstOrDefault(
                    c => c.chestID == chest.chestID);

            if (chestSaveData != null)
            {
                chest.SetOpened(
                    chestSaveData.isOpened);
            }
        }
    }

    // =========================
    // PLAYER DEATH
    // =========================

    public void RespawnFromAutoSave()
    {
        LoadAutoSave();
    }
}