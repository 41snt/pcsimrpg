using Cinemachine;
using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents()
    {
        // Define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();

        chests = FindObjectsOfType<Chest>();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,

            mapBoundary = FindObjectOfType<CinemachineConfiner>()
                .m_BoundingShape2D.gameObject.name,

            inventorySaveData = inventoryController.GetInventoryItems(),

            hotbarSaveData = hotbarController.GetHotbarItems(),

            chestSaveData = GetChestState()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData, true));
    }

    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened,
            };

            chestStates.Add(chestSaveData);
        }

        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData =
                JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position =
                saveData.playerPosition;

            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
                GameObject.Find(saveData.mapBoundary)
                .GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            SaveGame();

            inventoryController.SetInventoryItems(new List<InventorySaveData>());

            hotbarController.SetHotbarItems(new List<InventorySaveData>());
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            ChestSaveData chestsaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (chestsaveData != null)
            {
                chest.SetOpened(chestsaveData.isOpened);
            }
        }
    }
}