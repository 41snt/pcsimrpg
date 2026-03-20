using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        UpdateObjective("Find the missing CPU core.");
    }

    public void UpdateObjective(string newObjective)
    {
        objectiveText.text = "Current Objective: " + newObjective;
    }
}
