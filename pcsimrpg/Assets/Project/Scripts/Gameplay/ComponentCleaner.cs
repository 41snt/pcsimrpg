using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComponentCleaner : MonoBehaviour
{
    [Header("References")]
    public RectTransform componentArea;
    public GameObject brushUI;
    public Image dustImage;
    public Slider dustBar;
    public TextMeshProUGUI dustText;
    public Button cleanButton;

    [Header("Buttons To Unlock")]
    public Button repairButton;
    public Button nextStepButton;

    [Header("Repair Image")]
    public GameObject repairImage;

    [Header("Instruction Panel")]
    public GameObject instructionPanel;
    public float animationSpeed = 10f;
    public float startScale = 0.1f;

    [Header("UI Layer Fix (IMPORTANT)")]
    public bool bringDustToFront = true;

    private Vector3 targetScale;
    private bool isClosing = false;

    [Header("Settings")]
    public float cleanSpeed = 0.5f;

    private float dustAmount = 1f;
    private Vector3 lastMousePos;

    private bool cleaningFinished = false;
    private bool mouseReleased = false;

    void Start()
    {
        if (dustBar != null)
        {
            dustBar.minValue = 0f;
            dustBar.maxValue = 1f;
            dustBar.value = 1f;
        }

        if (repairButton != null)
            repairButton.interactable = false;

        if (nextStepButton != null)
            nextStepButton.interactable = false;

        if (repairImage != null)
            repairImage.SetActive(false);

        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale =
                Vector3.one * startScale;

            instructionPanel.SetActive(false);
        }

        targetScale = Vector3.one;

        UpdateDustText();

        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        // 🔥 FORCE DUST TO BE ON TOP (fixes your issue)
        if (bringDustToFront && dustImage != null)
        {
            dustImage.transform.SetAsLastSibling();
        }

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

        Vector3 mousePos = Input.mousePosition;

        if (!cleaningFinished &&
            brushUI != null &&
            brushUI.activeSelf &&
            Input.GetMouseButton(0) &&
            IsMouseOverComponent() &&
            mousePos != lastMousePos)
        {
            float speedMultiplier =
                (mousePos - lastMousePos).magnitude * 0.01f;

            dustAmount -= cleanSpeed * Time.deltaTime * speedMultiplier;
            dustAmount = Mathf.Clamp01(dustAmount);

            if (dustImage != null)
            {
                Color c = dustImage.color;
                c.a = dustAmount;
                dustImage.color = c;
            }

            if (dustBar != null)
                dustBar.value = dustAmount;

            UpdateDustText();

            if (dustAmount <= 0.01f)
                FinishCleaning();
        }

        if (cleaningFinished &&
            !mouseReleased &&
            !Input.GetMouseButton(0))
        {
            mouseReleased = true;

            if (repairButton != null)
                repairButton.interactable = true;

            if (nextStepButton != null)
                nextStepButton.interactable = true;
        }

        lastMousePos = mousePos;
    }

    public void EnableCleaning()
    {
        Debug.Log("Cleaning Enabled!");

        if (brushUI != null)
            brushUI.SetActive(true);

        if (dustBar != null)
            dustBar.gameObject.SetActive(true);

        if (dustText != null)
            dustText.gameObject.SetActive(true);

        if (cleanButton != null)
            cleanButton.interactable = false;

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale =
                Vector3.one * startScale;

            targetScale = Vector3.one;
            isClosing = false;
        }

        cleaningFinished = false;
        mouseReleased = false;

        dustAmount = 1f;

        if (dustImage != null)
        {
            Color c = dustImage.color;
            c.a = dustAmount;
            dustImage.color = c;
        }

        if (dustBar != null)
            dustBar.value = dustAmount;

        UpdateDustText();

        lastMousePos = Input.mousePosition;
    }

    void FinishCleaning()
    {
        dustAmount = 0f;
        cleaningFinished = true;
        mouseReleased = false;

        if (brushUI != null)
            brushUI.SetActive(false);

        if (dustBar != null)
            dustBar.gameObject.SetActive(false);

        if (dustText != null)
            dustText.gameObject.SetActive(false);

        if (cleanButton != null)
            cleanButton.interactable = false;

        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isClosing = true;
        }
    }

    bool IsMouseOverComponent()
    {
        if (componentArea == null)
            return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            componentArea,
            Input.mousePosition
        );
    }

    void UpdateDustText()
    {
        if (dustText == null)
            return;

        int percent = Mathf.RoundToInt(dustAmount * 100f);

        dustText.text = "DUST: " + percent + "%";

        if (percent > 50)
            dustText.color = Color.red;
        else if (percent > 10)
            dustText.color = Color.yellow;
        else
            dustText.color = Color.green;
    }

    public bool IsClean()
    {
        return dustAmount <= 0.01f;
    }
}