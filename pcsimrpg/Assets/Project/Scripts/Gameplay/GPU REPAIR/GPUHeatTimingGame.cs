using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GPUHeatTimingGame : MonoBehaviour
{
    [Header("Repair UI")]
    public GameObject repairUI;

    [Header("GPU Images")]
    public GameObject normalGPU;
    public GameObject overheatedGPU;
    public Image fixedGPUImage;

    [Header("Heat Slider")]
    public Slider heatSlider;

    [Header("Repair Progress")]
    public Slider repairProgressSlider;

    [Header("Status Text")]
    public TMP_Text statusText;

    [Header("Safe Zone")]
    public GameObject safeZone;

    public float safeZoneMin = 40f;
    public float safeZoneMax = 60f;

    [Header("Heat Movement")]
    public float moveSpeed = 80f;

    [Header("Repair Settings")]
    public float repairAmount = 20f;

    [Header("Repair Controller")]
    public PartRepairController partRepairController;

    private float currentHeat = 0f;
    private bool movingRight = true;
    private float repairProgress = 0f;
    private bool repairCompleted = false;

    void Start()
    {
        // Hide repair UI
        HideRepairUI();

        // Hide fixed GPU at start
        if (fixedGPUImage != null)
        {
            Color c = fixedGPUImage.color;
            c.a = 0f;
            fixedGPUImage.color = c;
        }

        // Hide overheated GPU at start
        if (overheatedGPU != null)
            overheatedGPU.SetActive(false);

        enabled = false;

        // Slider setup
        heatSlider.minValue = 0;
        heatSlider.maxValue = 100;

        repairProgressSlider.minValue = 0;
        repairProgressSlider.maxValue = 100;

        statusText.text = "Stabilize GPU Heat";
    }

    // Called by Repair Button
    public void StartMiniGame()
    {
        ShowRepairUI();

        enabled = true;

        repairCompleted = false;

        repairProgress = 0f;
        repairProgressSlider.value = 0f;

        currentHeat = 0f;
        heatSlider.value = 0f;

        movingRight = true;

        statusText.text = "Stabilize GPU Heat";

        // Hide normal GPU
        if (normalGPU != null)
            normalGPU.SetActive(false);

        // Show overheated GPU
        if (overheatedGPU != null)
            overheatedGPU.SetActive(true);

        // Reset fixed GPU alpha
        if (fixedGPUImage != null)
        {
            Color c = fixedGPUImage.color;
            c.a = 0f;
            fixedGPUImage.color = c;
        }
    }

    void Update()
    {
        if (repairCompleted)
            return;

        MoveHeatBar();

        if (Input.GetMouseButtonDown(0))
        {
            CheckTiming();
        }
    }

    void MoveHeatBar()
    {
        if (movingRight)
        {
            currentHeat += moveSpeed * Time.deltaTime;

            if (currentHeat >= 100f)
            {
                currentHeat = 100f;
                movingRight = false;
            }
        }
        else
        {
            currentHeat -= moveSpeed * Time.deltaTime;

            if (currentHeat <= 0f)
            {
                currentHeat = 0f;
                movingRight = true;
            }
        }

        heatSlider.value = currentHeat;
    }

    void CheckTiming()
    {
        if (currentHeat >= safeZoneMin &&
            currentHeat <= safeZoneMax)
        {
            repairProgress += repairAmount;

            repairProgress =
                Mathf.Clamp(repairProgress, 0f, 100f);

            repairProgressSlider.value = repairProgress;

            statusText.text = "GOOD TIMING";

            // Gradually fade in fixed GPU
            UpdateFixedGPUFade();

            if (repairProgress >= 100f)
            {
                CompleteRepair();
            }
        }
        else
        {
            statusText.text = "BAD TIMING";
        }
    }

    void UpdateFixedGPUFade()
    {
        if (fixedGPUImage != null)
        {
            Color c = fixedGPUImage.color;

            c.a = repairProgress / 100f;

            fixedGPUImage.color = c;
        }
    }

    void CompleteRepair()
    {
        repairCompleted = true;

        statusText.text = "GPU REPAIRED";

        HideRepairUI();

        enabled = false;

        // Hide overheated GPU
        if (overheatedGPU != null)
            overheatedGPU.SetActive(false);

        // Fully show fixed GPU
        if (fixedGPUImage != null)
        {
            Color c = fixedGPUImage.color;
            c.a = 1f;
            fixedGPUImage.color = c;
        }

        if (partRepairController != null)
        {
            partRepairController.FinishRepair();
        }
    }

    void ShowRepairUI()
    {
        if (repairUI != null)
            repairUI.SetActive(true);

        heatSlider.gameObject.SetActive(true);
        repairProgressSlider.gameObject.SetActive(true);
        statusText.gameObject.SetActive(true);

        if (safeZone != null)
            safeZone.SetActive(true);
    }

    void HideRepairUI()
    {
        if (repairUI != null)
            repairUI.SetActive(false);

        heatSlider.gameObject.SetActive(false);
        repairProgressSlider.gameObject.SetActive(false);
        statusText.gameObject.SetActive(false);

        if (safeZone != null)
            safeZone.SetActive(false);
    }
}