using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CPUThermalPaste : MonoBehaviour
{
    [Header("References")]
    public Image pasteImage;
    public RectTransform cpuArea;
    public GameObject brushUI;
    public Button pasteButton;
    public Slider pasteBar;
    public TextMeshProUGUI pasteText;

    [Header("Instruction Panel & Animation")]
    public GameObject instructionPanel;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float startScale = 0.1f; // Starts tiny

    private Vector3 targetScale;
    private bool isPanelClosing = false;

    [Header("Settings")]
    public float pasteSpeed = 0.5f;

    private float alpha = 0f;
    private Vector3 lastMousePos;
    private bool isPastingActive = false;

    void Start()
    {
        // Initial setup
        if (pasteImage != null)
        {
            Color c = pasteImage.color;
            c.a = 0f;
            pasteImage.color = c;
        }

        brushUI.SetActive(false);
        pasteBar.gameObject.SetActive(false);
        pasteText.gameObject.SetActive(false);

        if (instructionPanel != null)
        {
            instructionPanel.transform.localScale = Vector3.one * startScale;
            instructionPanel.SetActive(false);
        }

        targetScale = Vector3.one;
        pasteButton.interactable = false;
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

            // If we are shrinking it and it's small enough, shut it off
            if (isPanelClosing && instructionPanel.transform.localScale.x < 0.15f)
            {
                instructionPanel.SetActive(false);
                isPanelClosing = false;
            }
        }

        // --- THERMAL PASTE LOGIC ---
        Vector3 mousePos = Input.mousePosition;

        if (isPastingActive && Input.GetMouseButton(0) && IsMouseOverCPU() && mousePos != lastMousePos)
        {
            alpha += pasteSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color c = pasteImage.color;
            c.a = alpha;
            pasteImage.color = c;

            pasteBar.value = alpha;
            pasteText.text = "PASTE: " + Mathf.RoundToInt(alpha * 100f) + "%";

            if (alpha >= 1f)
            {
                FinishPasting();
            }
        }

        lastMousePos = mousePos;
    }

    public void EnablePaste()
    {
        // UI Setup
        brushUI.SetActive(true);
        pasteBar.gameObject.SetActive(true);
        pasteText.gameObject.SetActive(true);

        // Panel Pop-In Animation
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            instructionPanel.transform.localScale = Vector3.one * startScale; // Start small
            targetScale = Vector3.one; // Grow to full size
            isPanelClosing = false;
        }

        // Reset Paste Data
        alpha = 0f;
        pasteBar.value = 0f;
        pasteText.text = "PASTE: 0%";
        Color c = pasteImage.color;
        c.a = 0f;
        pasteImage.color = c;

        pasteButton.interactable = false;
        isPastingActive = true;
        lastMousePos = Input.mousePosition;
    }

    private void FinishPasting()
    {
        brushUI.SetActive(false);
        pasteBar.gameObject.SetActive(false);
        pasteText.gameObject.SetActive(false);

        // Panel Shrink-Out Animation
        if (instructionPanel != null)
        {
            targetScale = Vector3.one * startScale; // Shrink to tiny
            isPanelClosing = true; // Update loop will disable object when small enough
        }

        pasteButton.interactable = false;
        isPastingActive = false;
    }

    bool IsMouseOverCPU()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(cpuArea, Input.mousePosition);
    }
}