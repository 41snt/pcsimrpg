using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;

    public GameObject questEntryPrefab;

    public GameObject objectiveTextPrefab;

    private void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // CLEAR OLD UI
        foreach (Transform child
            in questListContent)
        {
            Destroy(child.gameObject);
        }

        if (QuestController.Instance == null)
            return;

        List<QuestProgress> quests =
            QuestController.Instance.activateQuests;

        foreach (QuestProgress progress
            in quests)
        {
            if (progress == null)
                continue;

            if (progress.quest == null)
                continue;

            // CREATE QUEST ENTRY
            GameObject questEntry =
                Instantiate(
                    questEntryPrefab,
                    questListContent);

            TMP_Text questNameText =
                questEntry.GetComponentInChildren<TMP_Text>();

            if (questNameText != null)
            {
                questNameText.text =
                    progress.quest.questName;
            }

            // OBJECTIVES
            foreach (QuestObjective objective
                in progress.objectives)
            {
                GameObject objectiveObj =
                    Instantiate(
                        objectiveTextPrefab,
                        questEntry.transform);

                TMP_Text objectiveText =
                    objectiveObj
                    .GetComponent<TMP_Text>();

                if (objectiveText != null)
                {
                    objectiveText.text =
                        objective.description
                        + " "
                        + objective.currentAmount
                        + "/"
                        + objective.requiredAmount;
                }
            }
        }
    }
}