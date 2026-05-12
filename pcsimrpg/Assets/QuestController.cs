using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance;

    // =========================
    // ACTIVE QUESTS
    // =========================

    public List<QuestProgress> activateQuests =
        new List<QuestProgress>();

    // =========================
    // HANDED IN QUESTS
    // =========================

    public List<string> handedInQuestIDs =
        new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // ADD QUEST
    // =========================

    public void AddQuest(Quest quest)
    {
        if (quest == null)
            return;

        foreach (QuestProgress progress
            in activateQuests)
        {
            if (progress.QuestID == quest.questID)
            {
                return;
            }
        }

        QuestProgress newQuest =
            new QuestProgress(quest);

        activateQuests.Add(newQuest);
    }

    // =========================
    // COMPLETE QUEST
    // =========================

    public void CompleteQuest(string questID)
    {
        foreach (QuestProgress progress
            in activateQuests)
        {
            if (progress.QuestID == questID)
            {
                foreach (QuestObjective objective
                    in progress.objectives)
                {
                    objective.currentAmount =
                        objective.requiredAmount;
                }

                return;
            }
        }
    }

    // =========================
    // IS QUEST COMPLETED
    // =========================

    public bool IsQuestCompleted(string questID)
    {
        foreach (QuestProgress progress
            in activateQuests)
        {
            if (progress.QuestID == questID)
            {
                return progress.IsCompleted;
            }
        }

        return false;
    }

    // =========================
    // IS QUEST HANDED IN
    // =========================

    public bool IsQuestHandedIn(string questID)
    {
        return handedInQuestIDs.Contains(questID);
    }

    // =========================
    // HAND IN QUEST
    // =========================

    public void HandInQuest(string questID)
    {
        if (!handedInQuestIDs.Contains(questID))
        {
            handedInQuestIDs.Add(questID);
        }
    }

    // =========================
    // LOAD QUESTS
    // =========================

    public void LoadQuestProgress(
        List<QuestProgress> loadedQuests)
    {
        activateQuests =
            loadedQuests;

        if (activateQuests == null)
        {
            activateQuests =
                new List<QuestProgress>();
        }
    }

    // =========================
    // IS QUEST ACTIVE
    // =========================

    public bool IsQuestActive(string questID)
    {
        foreach (QuestProgress progress
            in activateQuests)
        {
            if (progress.QuestID == questID)
            {
                return true;
            }
        }

        return false;
    }

    // =========================
    // ACCEPT QUEST
    // =========================

    public void AcceptQuest(Quest quest)
    {
        AddQuest(quest);
    }
}