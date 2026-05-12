using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;

    [TextArea]
    public string description;

    public List<QuestObjective> objectives;

    // Called when scriptable obj is created
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(questName))
        {
            string cleanName = questName.Replace(" ", "_");

            // Auto update quest ID with quest name
            if (string.IsNullOrEmpty(questID) || !questID.StartsWith(cleanName))
            {
                questID = cleanName + "_" + Guid.NewGuid().ToString();
            }
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveID; // Match with item ID that you need to collect, enemy ID that you need to kill etc
    public string description;
    public ObjectiveType type;

    public int requiredAmount;
    public int currentAmount;

    public bool IsCompleted => currentAmount >= requiredAmount;
}

public enum ObjectiveType
{
    CollectItem,
    DefeatEnemy,
    ReachLocation,
    TalkNPC,
    Custom
}

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();

        // Deep copy avoid modifying original
        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);

    public string QuestID => quest.questID;
}