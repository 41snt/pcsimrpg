using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI")]
    public TextMeshProUGUI goalText;

    [Header("Quest Settings")]
    public int enemiesRequired = 5;

    [Header("AOE Spawner")]
    public AOEQuestSpawner aoeSpawner;

    private int enemiesDefeated = 0;
    private bool questStarted = false;
    private bool questCompleted = false;

    void Awake()
    {
        // Simple singleton WITHOUT DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FindGoalText();
        UpdateUI();
    }

    public void StartAOEQuest()
    {
        Debug.Log("🎯 QUEST STARTED");

        questStarted = true;
        questCompleted = false;
        enemiesDefeated = 0;

        if (aoeSpawner != null)
        {
            aoeSpawner.SpawnAOEEnemies();
        }
        else
        {
            Debug.LogError("AOE Spawner not assigned!");
        }

        UpdateUI();
    }

    public void AOEEnemyKilled()
    {
        if (!questStarted || questCompleted) return;

        enemiesDefeated++;

        if (enemiesDefeated >= enemiesRequired)
        {
            enemiesDefeated = enemiesRequired;
            questCompleted = true;
            Debug.Log(" QUEST COMPLETED!");
        }

        UpdateUI();
    }

    void FindGoalText()
    {
        if (goalText == null)
        {
            goalText = FindObjectOfType<TextMeshProUGUI>();
        }
    }

    void UpdateUI()
    {
        if (goalText == null)
        {
            FindGoalText();
            if (goalText == null) return;
        }

        goalText.gameObject.SetActive(true);

        if (!questStarted)
        {
            goalText.text = "No Active Quest";
            return;
        }

        if (questCompleted)
        {
            goalText.text = "[COMPLETED]\n\nDefeat AOE Enemies: "
                + enemiesRequired + " / " + enemiesRequired;
        }
        else
        {
            goalText.text = "GOAL\n\nDefeat AOE Enemies: "
                + enemiesDefeated + " / " + enemiesRequired;
        }
    }
}
