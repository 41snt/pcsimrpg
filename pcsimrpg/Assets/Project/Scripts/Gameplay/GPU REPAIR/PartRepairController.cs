using UnityEngine;
using UnityEngine.UI;

public class PartRepairController : MonoBehaviour
{
    [Header("Part States")]
    public GameObject normalPart;
    public GameObject brokenPart;
    public GameObject fixedPart;

    [Header("UI")]
    public GameObject miniGameUI;
    public GameObject instructionPanel;

    [Header("Buttons")]
    public Button repairButton;   // ✅ ADD THIS

    public float animationSpeed = 10f;
    public float startScale = 0.1f;

    private Vector3 targetScale;
    private bool isClosing = false;

    private bool repaired = false;

    void Start()
    {
        if (miniGameUI != null)
            miniGameUI.SetActive(false);

        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale =
                Vector3.one * startScale;

            instructionPanel.SetActive(false);
        }

        targetScale = Vector3.one;
    }

    void Update()
    {
        if (instructionPanel != null &&
            instructionPanel.activeSelf)
        {
            instructionPanel.transform.localScale =
                Vector3.Lerp(
                    instructionPanel.transform.localScale,
                    targetScale,
                    Time.deltaTime * animationSpeed
                );

            if (isClosing &&
                instructionPanel.transform.localScale.x < 0.15f)
            {
                instructionPanel.SetActive(false);
                isClosing = false;
            }
        }
    }

    public void StartRepair()
    {
        repaired = false;

        if (repairButton != null)
            repairButton.interactable = true;

        if (miniGameUI != null)
            miniGameUI.SetActive(true);

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);

            instructionPanel.transform.localScale =
                Vector3.one * startScale;

            targetScale = Vector3.one;

            isClosing = false;
        }

        if (normalPart != null)
            normalPart.SetActive(false);

        if (brokenPart != null)
            brokenPart.SetActive(true);

        if (fixedPart != null)
            fixedPart.SetActive(false);
    }

    public void FinishRepair()
    {
        repaired = true;

        // ❌ DISABLE REPAIR BUTTON AFTER DONE
        if (repairButton != null)
            repairButton.interactable = false;

        if (miniGameUI != null)
            miniGameUI.SetActive(false);

        if (brokenPart != null)
            brokenPart.SetActive(false);

        if (fixedPart != null)
            fixedPart.SetActive(true);

        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isClosing = true;
        }

        Debug.Log("PART REPAIRED");
    }

    public bool IsRepaired()
    {
        return repaired;
    }
}