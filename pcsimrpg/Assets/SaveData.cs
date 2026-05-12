using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;

    public string mapBoundary;

    public List<InventorySaveData> inventorySaveData =
        new List<InventorySaveData>();

    public List<InventorySaveData> hotbarSaveData =
        new List<InventorySaveData>();

    public List<ChestSaveData> chestSaveData =
        new List<ChestSaveData>();

    public List<Quest.QuestProgress> questProgressData =
        new List<Quest.QuestProgress>();

    // ADD THIS
    public List<string> handedInQuestIDs =
        new List<string>();
}

[System.Serializable]
public class ChestSaveData
{
    public string chestID;

    public bool isOpened;
}