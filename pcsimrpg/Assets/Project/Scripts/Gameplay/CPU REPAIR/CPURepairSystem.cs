using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CPURepairSystem : MonoBehaviour
{
    [Header("CPU Images")]
    public GameObject normalCPU;
    public GameObject brokenCPU;
    public Image fixedCPUFadeImage;

    [Header("Buttons")]
    public Button repairButton;
    public Button thermalPasteButton;

    [Header("Buttons To Disable")]
    public Button cleanButton;
    public Button backButton;

    [Header("Repair UI")]
    public Slider repairBar;
    public TextMeshProUGUI repairText;

    [Header("Instruction Panel")]
    public GameObject instructionPanel;
    public float animationSpeed = 10f;
    public float startScale = 0.1f;

    private Vector3 targetScale;
    private bool isClosing = false;

    [Header("Settings")]
    public int totalPinsToFix = 15;

    private int currentFixedPins = 0;
    private bool repairMode = false;
    private bool fullyFixed = false;

    private Color fadeColor;

    void Start()
    {
        normalCPU.SetActive(true);
        brokenCPU.SetActive(false);

        fixedCPUFadeImage.enabled = true;

        fadeColor = fixedCPUFadeImage.color;
        fadeColor.a = 0f;
        fixedCPUFadeImage.color = fadeColor;

        repairBar.minValue = 0;
        repairBar.maxValue = totalPinsToFix;
        repairBar.value = 0;

        repairBar.gameObject.SetActive(false);
        repairText.gameObject.SetActive(false);

        repairButton.interactable = false;
        thermalPasteButton.interactable = false;

        // INSTRUCTION PANEL INIT
        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale = Vector3.one * startScale;
            instructionPanel.SetActive(false);
        }

        targetScale = Vector3.one;

        thermalPasteButton.onClick.AddListener(ApplyThermalPaste);
    }

    void Update()
    {
        // PANEL ANIMATION (OPEN/CLOSE)
        if (instructionPanel != null && instructionPanel.activeSelf)
        {
            instructionPanel.transform.localScale =
                Vector3.Lerp(instructionPanel.transform.localScale,
                targetScale,
                Time.deltaTime * animationSpeed);

            if (isClosing && instructionPanel.transform.localScale.x < 0.15f)
            {
                instructionPanel.SetActive(false);
                isClosing = false;
            }
        }

        if (!repairMode || fullyFixed)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverBrokenCPU())
            {
                currentFixedPins++;

                currentFixedPins =
                    Mathf.Clamp(currentFixedPins, 0, totalPinsToFix);

                repairBar.value = currentFixedPins;

                UpdateRepairText();

                float alpha =
                    (float)currentFixedPins / totalPinsToFix;

                fadeColor.a = alpha;
                fixedCPUFadeImage.color = fadeColor;

                if (currentFixedPins >= totalPinsToFix)
                {
                    FinishRepair();
                }
            }
        }
    }

    public void EnableRepairButton()
    {
        repairButton.interactable = true;
    }

    public void StartRepair()
    {
        repairMode = true;
        fullyFixed = false;
        currentFixedPins = 0;

        normalCPU.SetActive(false);
        brokenCPU.SetActive(true);

        repairBar.gameObject.SetActive(true);
        repairText.gameObject.SetActive(true);

        repairButton.interactable = false;

        cleanButton.interactable = false;
        backButton.interactable = false;

        repairBar.value = 0;

        fadeColor.a = 0f;
        fixedCPUFadeImage.color = fadeColor;

        UpdateRepairText();

        // SHOW INSTRUCTION PANEL (ANIMATED)
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale = Vector3.one * startScale;
            targetScale = Vector3.one;
            isClosing = false;
        }
    }

    void FinishRepair()
    {
        repairMode = false;
        fullyFixed = true;

        brokenCPU.SetActive(false);

        fadeColor.a = 1f;
        fixedCPUFadeImage.color = fadeColor;

        thermalPasteButton.interactable = true;

        repairBar.gameObject.SetActive(false);
        repairText.gameObject.SetActive(false);

        repairText.text = "CPU FIXED!";

        // CLOSE INSTRUCTION PANEL (ANIMATED OUT)
        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isClosing = true;
        }
    }

    void ApplyThermalPaste()
    {
        fixedCPUFadeImage.color =
            new Color(
                fixedCPUFadeImage.color.r,
                fixedCPUFadeImage.color.g,
                fixedCPUFadeImage.color.b,
                0f
            );

        normalCPU.SetActive(true);

        thermalPasteButton.interactable = false;
    }

    bool IsMouseOverBrokenCPU()
    {
        RectTransform rect = brokenCPU.GetComponent<RectTransform>();

        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.mousePosition
        );
    }

    void UpdateRepairText()
    {
        repairText.text =
            "FIXED PINS: " +
            currentFixedPins +
            " / " +
            totalPinsToFix;
    }
}