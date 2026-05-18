using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PSUVoltageGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject miniGameUI;
    public Slider voltageSlider;
    public Slider stabilitySlider;
    public TMP_Text statusText;

    [Header("Voltage Settings")]
    public float voltage = 50f;
    public float voltageSpeed = 30f;

    [Header("Safe Zone")]
    public float safeMin = 40f;
    public float safeMax = 60f;

    [Header("Repair")]
    public float stabilityGain = 25f;

    [Header("Controller")]
    public PartRepairController partRepairController;

    private float stability = 0f;
    private bool holding = false;
    private bool repaired = false;

    void Start()
    {
        miniGameUI.SetActive(false);
        enabled = false;

        voltageSlider.minValue = 0;
        voltageSlider.maxValue = 100;

        stabilitySlider.minValue = 0;
        stabilitySlider.maxValue = 100;

        statusText.text = "Stabilize PSU Voltage";
    }

    public void StartMiniGame()
    {
        miniGameUI.SetActive(true);
        enabled = true;

        voltage = 50f;
        stability = 0f;

        stabilitySlider.value = 0;
        voltageSlider.value = voltage;

        repaired = false;
        statusText.text = "HOLD INSIDE SAFE VOLTAGE";
    }

    void Update()
    {
        if (repaired) return;

        MoveVoltage();

        if (Input.GetMouseButtonDown(0))
            holding = true;

        if (Input.GetMouseButtonUp(0))
            holding = false;

        if (holding)
            CheckVoltage();
    }

    void MoveVoltage()
    {
        voltage += Mathf.Sin(Time.time * voltageSpeed) * Time.deltaTime * 20f;
        voltage = Mathf.Clamp(voltage, 0, 100);

        voltageSlider.value = voltage;
    }

    void CheckVoltage()
    {
        if (voltage >= safeMin && voltage <= safeMax)
        {
            stability += stabilityGain * Time.deltaTime;
            stabilitySlider.value = stability;

            statusText.text = "STABLE";

            if (stability >= 100)
                CompleteRepair();
        }
        else
        {
            statusText.text = "UNSTABLE - RELEASE!";
        }
    }

    void CompleteRepair()
    {
        repaired = true;

        statusText.text = "PSU REPAIRED";

        miniGameUI.SetActive(false);
        enabled = false;

        if (partRepairController != null)
            partRepairController.FinishRepair();
    }
}