using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI")]
    public TextMeshProUGUI goalText;

    [Header("Settings")]
    public int enemiesRequired = 5;

    [Header("Spawner")]
    public AOEQuestSpawner aoeSpawner;

    private int enemiesDefeated;
    private bool questStarted;
    private bool questCompleted;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    // 🔥 ONLY ENTRY POINT
    public void TryStartAOEQuest()
    {
        if (questStarted)
        {
            Debug.Log("Quest already started - blocked.");
            return;
        }

        StartAOEQuest();
    }

    void StartAOEQuest()
    {
        questStarted = true;
        questCompleted = false;
        enemiesDefeated = 0;

        Debug.Log("QUEST STARTED");

        if (aoeSpawner != null)
        {
            aoeSpawner.SpawnAOEEnemies();
        }

        UpdateUI();
    }

    public void AOEEnemyKilled()
    {
        if (!questStarted || questCompleted) return;

        enemiesDefeated++;

        if (enemiesDefeated >= enemiesRequired)
        {
            questCompleted = true;
            Debug.Log("QUEST COMPLETED");
        }

        UpdateUI();
    }

    public void ResetQuest()
    {
        questStarted = false;
        questCompleted = false;
        enemiesDefeated = 0;

        if (aoeSpawner != null)
            aoeSpawner.ResetSpawner();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (goalText == null) return;

        if (!questStarted)
        {
            goalText.text = "No Active Quest";
        }
        else if (questCompleted)
        {
            goalText.text = "[COMPLETED]";
        }
        else
        {
            goalText.text = $"Defeat Enemies: {enemiesDefeated}/{enemiesRequired}";
        }
    }
}
