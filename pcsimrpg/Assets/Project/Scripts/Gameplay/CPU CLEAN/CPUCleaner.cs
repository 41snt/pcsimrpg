using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CPUCleaner : MonoBehaviour
{
    [Header("References")]
    public Image dustImage;
    public Slider dustBar;
    public RectTransform cpuArea;
    public GameObject brushUI;
    public Button cleanButton;
    public TextMeshProUGUI dustText;
    public Button thermalPasteButton;

    [Header("Instruction Panel & Animation")]
    public GameObject instructionPanel;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float startScale = 0.1f;

    private Vector3 targetScale;
    private bool isPanelClosing = false;

    [Header("Settings")]
    public float cleanSpeed = 0.5f;

    private float dustAmount = 1f;
    private Vector3 lastMousePos;

    private bool cleaningFinished = false;
    private bool mouseReleasedAfterClean = false;

    void Start()
    {
        dustBar.minValue = 0f;
        dustBar.maxValue = 1f;
        dustBar.value = 1f;

        if (thermalPasteButton != null)
            thermalPasteButton.interactable = false;

        // Setup Animation Initial State
        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale = Vector3.one * startScale;
            instructionPanel.SetActive(false);
        }

        targetScale = Vector3.one;
        UpdateDustText();
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        // --- PANEL ANIMATION LOGIC ---
        if (instructionPanel != null && instructionPanel.activeSelf)
        {
            instructionPanel.transform.localScale = Vector3.Lerp(
                instructionPanel.transform.localScale,
                targetScale,
                Time.deltaTime * animationSpeed
            );

            // Turn off object once it has shrunk enough
            if (isPanelClosing && instructionPanel.transform.localScale.x < 0.15f)
            {
                instructionPanel.SetActive(false);
                isPanelClosing = false;
            }
        }

        // --- CLEANING LOGIC ---
        Vector3 mousePos = Input.mousePosition;

        if (!cleaningFinished && brushUI.activeSelf && Input.GetMouseButton(0) && IsMouseOverCPU() && mousePos != lastMousePos)
        {
            float speedMultiplier = (mousePos - lastMousePos).magnitude * 0.01f;
            dustAmount -= cleanSpeed * Time.deltaTime * speedMultiplier;
            dustAmount = Mathf.Clamp01(dustAmount);

            Color c = dustImage.color;
            c.a = dustAmount;
            dustImage.color = c;

            dustBar.value = dustAmount;
            UpdateDustText();

            if (dustAmount <= 0.01f)
            {
                FinishCleaning();
            }
        }

        // Wait for mouse release before enabling paste
        if (cleaningFinished && !mouseReleasedAfterClean && !Input.GetMouseButton(0))
        {
            mouseReleasedAfterClean = true;
            if (thermalPasteButton != null)
                thermalPasteButton.interactable = true;
        }

        lastMousePos = mousePos;
    }

    public void EnableCleaning()
    {
        brushUI.SetActive(true);
        dustBar.gameObject.SetActive(true);
        dustText.gameObject.SetActive(true);
        cleanButton.interactable = false;

        // Show and Start Animation
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale = Vector3.one * startScale;
            targetScale = Vector3.one;
            isPanelClosing = false;
        }

        lastMousePos = Input.mousePosition;
        cleaningFinished = false;
        mouseReleasedAfterClean = false;
    }

    private void FinishCleaning()
    {
        dustAmount = 0f;
        brushUI.SetActive(false);
        dustBar.gameObject.SetActive(false);
        dustText.gameObject.SetActive(false);
        cleanButton.interactable = false;

        cleaningFinished = true;
        mouseReleasedAfterClean = false;

        // Start Shrink Animation
        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isPanelClosing = true;
        }
    }

    bool IsMouseOverCPU()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(cpuArea, Input.mousePosition);
    }

    void UpdateDustText()
    {
        int percent = Mathf.RoundToInt(dustAmount * 100f);
        dustText.text = "DUST: " + percent + "%";

        if (percent > 50) dustText.color = Color.red;
        else if (percent > 10) dustText.color = Color.yellow;
        else dustText.color = Color.green;
    }

    public bool IsClean() => dustAmount <= 0.01f;
}