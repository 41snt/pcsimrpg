using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Quest UI - Drag your GoalText here")]
    public TextMeshProUGUI goalText;

    public static QuestManager Instance;

    [Header("Quest Settings")]
    public int enemiesRequired = 5;
    private int enemiesDefeated = 0;

    private bool questStarted = false;
    private bool questCompleted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Try multiple ways to find the text
        FindQuestUIText();

        if (goalText != null)
        {
            goalText.text = "No Active Quest";
            Debug.Log("✅ QuestManager: goalText READY!");
        }
        else
        {
            Debug.LogWarning("⚠️ QuestManager: goalText not found YET - will search when inventory opens");
        }
    }

    // Called when inventory opens (add this to your Inventory script!)
    public void OnInventoryOpened()
    {
        FindQuestUIText();
        if (goalText != null && questStarted)
        {
            UpdateUI(); // Refresh quest display
            Debug.Log("✅ Quest UI refreshed when inventory opened!");
        }
    }

    void FindQuestUIText()
    {
        if (goalText != null) return; // Already found

        // Method 1: Manual reference (best)
        if (goalText == null)
        {
            goalText = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Method 2: Find in Canvas hierarchy
        if (goalText == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                // Common inventory paths
                Transform[] paths = {
                    canvas.transform.Find("InventoryPanel/QuestPanel/GoalText"),
                    canvas.transform.Find("Inventory/QuestPanel/GoalText"),
                    canvas.transform.Find("QuestPanel/GoalText")
                };

                foreach (Transform path in paths)
                {
                    if (path != null)
                    {
                        goalText = path.GetComponent<TextMeshProUGUI>();
                        if (goalText != null) break;
                    }
                }
            }
        }

        // Method 3: Find ANY active TextMeshProUGUI with "Goal" or "Quest" in name
        if (goalText == null)
        {
            TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (var text in allTexts)
            {
                if (text.name.ToLower().Contains("goal") || text.name.ToLower().Contains("quest"))
                {
                    goalText = text;
                    Debug.Log($"✅ Auto-found goalText: {text.name}");
                    break;
                }
            }
        }

        // Method 4: Nuclear option - first active TextMeshProUGUI
        if (goalText == null)
        {
            TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (var text in allTexts)
            {
                if (text.gameObject.activeInHierarchy)
                {
                    goalText = text;
                    Debug.LogWarning($"⚠️ Using fallback text: {text.name}");
                    break;
                }
            }
        }
    }

    public void StartAOEQuest()
    {
        Debug.Log("🎯 Starting AOE Quest...");

        if (questStarted) return;

        questStarted = true;
        questCompleted = false;
        enemiesDefeated = 0;

        // Always try to find text when starting quest
        FindQuestUIText();
        UpdateUI();
    }

    public void AOEEnemyKilled()
    {
        if (!questStarted || questCompleted) return;

        enemiesDefeated++;
        Debug.Log($"🔥 AOE #{enemiesDefeated}/{enemiesRequired}");

        if (enemiesDefeated >= enemiesRequired)
        {
            questCompleted = true;
            SafeSetText("[COMPLETED]\nDefeat 5 AOE Enemies");
        }
        else
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        SafeSetText("GOAL\n\nDefeat AOE Enemies: " + enemiesDefeated + " / " + enemiesRequired);
    }

    // SAFE text setting - finds text if needed
    void SafeSetText(string text)
    {
        FindQuestUIText();
        if (goalText != null)
        {
            goalText.text = text;
        }
        else
        {
            Debug.LogWarning("⚠️ goalText still null - waiting for inventory...");
        }
    }


}