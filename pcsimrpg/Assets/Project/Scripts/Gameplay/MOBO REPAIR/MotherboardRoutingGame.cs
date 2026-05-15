using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MotherboardRoutingGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject miniGameUI;

    public Slider progressSlider;

    public TMP_Text statusText;

    [Header("Buttons")]
    public Button[] nodes;

    [Header("Repair Controller")]
    public PartRepairController partRepairController;

    private int currentStep = 0;

    private bool completed = false;

    void Start()
    {
        miniGameUI.SetActive(false);

        progressSlider.minValue = 0;
        progressSlider.maxValue = nodes.Length;
        progressSlider.value = 0;

        statusText.text = "ROUTE POWER";

        for (int i = 0; i < nodes.Length; i++)
        {
            int index = i;

            nodes[i].onClick.AddListener(() => ClickNode(index));
        }
    }

    // Called by repair button
    public void StartMiniGame()
    {
        miniGameUI.SetActive(true);

        currentStep = 0;

        completed = false;

        progressSlider.value = 0;

        statusText.text = "ROUTE POWER";
    }

    void ClickNode(int nodeIndex)
    {
        if (completed)
            return;

        // Correct node
        if (nodeIndex == currentStep)
        {
            currentStep++;

            progressSlider.value = currentStep;

            statusText.text = "POWER CONNECTED";

            // Finished
            if (currentStep >= nodes.Length)
            {
                CompleteRepair();
            }
        }
        else
        {
            // Wrong order
            currentStep = 0;

            progressSlider.value = 0;

            statusText.text = "WRONG ROUTE";
        }
    }

    void CompleteRepair()
    {
        completed = true;

        statusText.text = "MOTHERBOARD REPAIRED";

        miniGameUI.SetActive(false);

        if (partRepairController != null)
        {
            partRepairController.FinishRepair();
        }
    }
}