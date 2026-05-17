using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<Quest.QuestProgress> activateQuests =
        new List<Quest.QuestProgress>();

    public List<string> handInQuestIDs =
        new List<string>();

    private QuestUI questUI;

    private void Awake()
    {
        // Singleton check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Ensure this GameObject is a root object before calling
        // DontDestroyOnLoad (required by Unity)
        transform.SetParent(null);

        // Persist across scene loads
        DontDestroyOnLoad(gameObject);

        // Find QuestUI if it exists in the current scene
        questUI = FindObjectOfType<QuestUI>();
    }

    private void Start()
    {
        if (InventoryController.Instance != null)
        {
            InventoryController.Instance.OnInventoryChanged +=
                CheckInventoryForQuests;
        }

        RefreshQuestUI();
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid dangling event references
        if (InventoryController.Instance != null)
        {
            InventoryController.Instance.OnInventoryChanged -=
                CheckInventoryForQuests;
        }
    }

    // =========================
    // ACCEPT QUEST
    // =========================

    public void AcceptQuest(Quest quest)
    {
        if (quest == null)
            return;

        if (IsQuestActive(quest.questID))
            return;

        if (IsQuestHandedIn(quest.questID))
            return;

        activateQuests.Add(
            new Quest.QuestProgress(quest));

        CheckInventoryForQuests();
        RefreshQuestUI();
    }

    // =========================
    // ACTIVE CHECK
    // =========================

    public bool IsQuestActive(string questID)
    {
        return activateQuests.Exists(
            q => q.QuestID == questID);
    }

    // =========================
    // COMPLETED CHECK
    // =========================

    public bool IsQuestCompleted(string questID)
    {
        Quest.QuestProgress quest =
            activateQuests.Find(
                q => q.QuestID == questID);

        if (quest == null)
            return false;

        return quest.objectives.TrueForAll(
            o => o.IsCompleted);
    }

    // =========================
    // HANDED IN CHECK
    // =========================

    public bool IsQuestHandedIn(string questID)
    {
        return handInQuestIDs.Contains(
            questID);
    }

    // =========================
    // HAND IN QUEST
    // =========================

    public void HandInQuest(string questID)
    {
        Quest.QuestProgress quest =
            activateQuests.Find(
                q => q.QuestID == questID);

        if (quest == null)
            return;

        // Remove required items from inventory
        if (InventoryController.Instance != null)
        {
            foreach (var objective in quest.objectives)
            {
                int itemID;

                if (int.TryParse(
                    objective.objectiveID,
                    out itemID))
                {
                    InventoryController.Instance
                        .RemoveItemsFromInventory(
                            itemID,
                            objective.requiredAmount);
                }
            }
        }

        // Mark as handed in
        if (!handInQuestIDs.Contains(questID))
        {
            handInQuestIDs.Add(questID);
        }

        // Remove from active quests
        activateQuests.Remove(quest);

        RefreshQuestUI();
    }

    // =========================
    // LOAD QUESTS
    // =========================

    public void LoadQuestProgress(
        List<Quest.QuestProgress> loadedQuests)
    {
        activateQuests.Clear();

        if (loadedQuests != null)
        {
            activateQuests.AddRange(
                loadedQuests);
        }

        CheckInventoryForQuests();
        RefreshQuestUI();
    }

    // =========================
    // INVENTORY CHECK
    // =========================

    public void CheckInventoryForQuests()
    {
        if (InventoryController.Instance == null)
            return;

        Dictionary<int, int> itemCounts =
            InventoryController.Instance.GetItemCounts();

        foreach (Quest.QuestProgress quest in activateQuests)
        {
            if (quest == null)
                continue;

            if (quest.objectives == null)
                continue;

            foreach (var objective in quest.objectives)
            {
                if (objective == null)
                    continue;

                int itemID;

                if (!int.TryParse(
                    objective.objectiveID,
                    out itemID))
                    continue;

                int count =
                    itemCounts.TryGetValue(
                        itemID,
                        out int value)
                    ? value
                    : 0;

                objective.currentAmount =
                    Mathf.Min(
                        count,
                        objective.requiredAmount);
            }
        }

        RefreshQuestUI();
    }

    // =========================
    // UI
    // =========================

    public void RefreshQuestUI()
    {
        // Re-find QuestUI after scene changes
        if (questUI == null)
        {
            questUI = FindObjectOfType<QuestUI>();
        }

        if (questUI != null)
        {
            questUI.UpdateQuestUI();
        }
    }
}