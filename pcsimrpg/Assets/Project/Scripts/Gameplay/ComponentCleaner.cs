using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComponentCleaner : MonoBehaviour
{
    [Header("References")]
    public RectTransform componentArea;   // The target component (CPU, GPU, RAM, etc.)
    public GameObject brushUI;            // Brush GameObject
    public Image dustImage;               // Dust overlay image
    public Slider dustBar;                // Slider showing dust amount
    public TextMeshProUGUI dustText;      // Text showing dust percentage
    public Button cleanButton;            // Optional clean button (disable when done)

    [Header("Instruction Panel & Animation")]
    public GameObject instructionPanel;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float startScale = 0.1f;

    private Vector3 targetScale = Vector3.one;
    private bool isPanelClosing = false;

    [Header("Settings")]
    public float cleanSpeed = 0.5f;

    private float dustAmount = 1f;
    private Vector3 lastMousePos;
    private bool cleaningFinished = false;

    void Start()
    {
        // Initialize dust slider
        if (dustBar != null)
        {
            dustBar.minValue = 0f;
            dustBar.maxValue = 1f;
            dustBar.value = 1f;
        }

        // Initialize instruction panel
        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale = Vector3.one * startScale;
            instructionPanel.SetActive(false);
        }

        UpdateDustText();
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // --- Instruction Panel Animation ---
        if (instructionPanel != null && instructionPanel.activeSelf)
        {
            instructionPanel.transform.localScale = Vector3.Lerp(
                instructionPanel.transform.localScale,
                targetScale,
                Time.deltaTime * animationSpeed
            );

            if (isPanelClosing && instructionPanel.transform.localScale.x < 0.15f)
            {
                instructionPanel.SetActive(false);
                isPanelClosing = false;
            }
        }

        // --- Cleaning Logic ---
        if (!cleaningFinished && brushUI != null && brushUI.activeSelf &&
            Input.GetMouseButton(0) && IsMouseOverComponent() && mousePos != lastMousePos)
        {
            float speedMultiplier = (mousePos - lastMousePos).magnitude * 0.01f;
            dustAmount -= cleanSpeed * Time.deltaTime * speedMultiplier;
            dustAmount = Mathf.Clamp01(dustAmount);

            // Update dust overlay
            if (dustImage != null)
            {
                Color c = dustImage.color;
                c.a = dustAmount;
                dustImage.color = c;
            }

            // Update slider & text
            if (dustBar != null) dustBar.value = dustAmount;
            UpdateDustText();

            // Check if cleaning finished
            if (dustAmount <= 0.01f)
            {
                dustAmount = 0f;
                FinishCleaning();
            }
        }

        lastMousePos = mousePos;
    }

    // --- Called by Button to Enable Cleaning ---
    public void EnableCleaning()
    {
        Debug.Log("Cleaning Enabled!");

        if (brushUI != null) brushUI.SetActive(true);

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale = Vector3.one * startScale;
            targetScale = Vector3.one;
            isPanelClosing = false;
        }

        if (dustBar != null) dustBar.gameObject.SetActive(true);
        if (dustText != null) dustText.gameObject.SetActive(true);
        if (cleanButton != null) cleanButton.interactable = false;

        cleaningFinished = false;
        dustAmount = 1f;

        // Reset dust overlay
        if (dustImage != null)
        {
            Color c = dustImage.color;
            c.a = dustAmount;
            dustImage.color = c;
        }

        if (dustBar != null) dustBar.value = dustAmount;
        UpdateDustText();

        lastMousePos = Input.mousePosition;
    }

    // --- Finish Cleaning ---
    private void FinishCleaning()
    {
        cleaningFinished = true;

        if (brushUI != null) brushUI.SetActive(false);
        if (dustBar != null) dustBar.gameObject.SetActive(false);
        if (dustText != null) dustText.gameObject.SetActive(false);
        if (cleanButton != null) cleanButton.interactable = false;

        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isPanelClosing = true;
        }
    }

    // --- Mouse Over Component Check ---
    bool IsMouseOverComponent()
    {
        if (componentArea == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(componentArea, Input.mousePosition);
    }

    // --- Update Dust Text ---
    void UpdateDustText()
    {
        if (dustText == null) return;

        int percent = Mathf.RoundToInt(dustAmount * 100f);
        dustText.text = "DUST: " + percent + "%";

        if (percent > 50) dustText.color = Color.red;
        else if (percent > 10) dustText.color = Color.yellow;
        else dustText.color = Color.green;
    }

    // --- Check if Component is Clean ---
    public bool IsClean()
    {
        return dustAmount <= 0.01f;
    }
}