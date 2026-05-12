using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    public Quest testQuest;
    public int testQuestAmount = 1;

    private List<QuestProgress> testQuests = new();

    void Start()
    {
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }

        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // Destroy old entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // Create quest entries
        foreach (var quest in testQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);

            // Quest Name
            TMP_Text questNameText =
                entry.transform.Find("QuestNameText")
                .GetComponent<TMP_Text>();

            // YOUR OBJECT NAME
            Transform objectiveList =
                entry.transform.Find("ObjectivesText");

            questNameText.text = quest.quest.questName;

            // Create objectives
            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO =
                    Instantiate(objectiveTextPrefab, objectiveList);

                TMP_Text objText =
                    objTextGO.GetComponent<TMP_Text>();

                objText.text =
                    objective.description +
                    " (" +
                    objective.currentAmount +
                    "/" +
                    objective.requiredAmount +
                    ")";
            }
        }
    }
}