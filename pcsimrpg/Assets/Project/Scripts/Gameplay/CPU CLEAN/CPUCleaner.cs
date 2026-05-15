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

    [Header("Buttons")]
    public Button thermalPasteButton;
    public Button repairButton;

    [Header("Repair Image")]
    public GameObject repairImage;

    [Header("Instruction Panel")]
    public GameObject instructionPanel;
    public float animationSpeed = 10f;
    public float startScale = 0.1f;

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
        dustBar.minValue = 0f;
        dustBar.maxValue = 1f;
        dustBar.value = 1f;

        if (thermalPasteButton != null)
            thermalPasteButton.interactable = false;

        if (repairButton != null)
            repairButton.interactable = false;

        if (repairImage != null)
            repairImage.SetActive(false);

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

        Vector3 mousePos = Input.mousePosition;

        if (!cleaningFinished &&
            brushUI.activeSelf &&
            Input.GetMouseButton(0) &&
            IsMouseOverCPU() &&
            mousePos != lastMousePos)
        {
            float speed = (mousePos - lastMousePos).magnitude * 0.01f;

            dustAmount -= cleanSpeed * Time.deltaTime * speed;
            dustAmount = Mathf.Clamp01(dustAmount);

            Color c = dustImage.color;
            c.a = dustAmount;
            dustImage.color = c;

            dustBar.value = dustAmount;

            UpdateDustText();

            if (dustAmount <= 0.01f)
                FinishCleaning();
        }

        if (cleaningFinished && !mouseReleased && !Input.GetMouseButton(0))
        {
            mouseReleased = true;

            if (thermalPasteButton != null)
                thermalPasteButton.interactable = true;

            if (repairButton != null)
                repairButton.interactable = true;
        }

        lastMousePos = mousePos;
    }

    public void EnableCleaning()
    {
        brushUI.SetActive(true);

        dustBar.gameObject.SetActive(true);
        dustText.gameObject.SetActive(true);

        cleanButton.interactable = false;

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale = Vector3.one * startScale;
            targetScale = Vector3.one;
            isClosing = false;
        }

        cleaningFinished = false;
        mouseReleased = false;

        lastMousePos = Input.mousePosition;
    }

    void FinishCleaning()
    {
        dustAmount = 0f;

        brushUI.SetActive(false);

        dustBar.gameObject.SetActive(false);
        dustText.gameObject.SetActive(false);

        cleanButton.interactable = false;

        cleaningFinished = true;
        mouseReleased = false;

        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale;
            isClosing = true;
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

        if (percent > 50)
            dustText.color = Color.red;
        else if (percent > 10)
            dustText.color = Color.yellow;
        else
            dustText.color = Color.green;
    }
}