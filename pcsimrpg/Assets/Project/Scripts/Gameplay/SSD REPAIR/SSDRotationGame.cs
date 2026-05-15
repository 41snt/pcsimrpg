using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SSDRotationGame : MonoBehaviour
{
    [Header("Pointer")]
    public RectTransform pointer;

    [Header("Safe Zone")]
    public float safeMin = -25f;
    public float safeMax = 25f;

    [Header("Rotation")]
    public float rotateSpeed = 200f;

    [Header("Progress")]
    public Slider progressSlider;
    public float repairAmount = 25f;

    [Header("UI")]
    public TMP_Text statusText;

    [Header("Repair Controller")]
    public PartRepairController repairController;

    private float currentRotation;
    private float repairProgress;
    private bool completed = false;

    void Start()
    {
        progressSlider.maxValue = 100;
        progressSlider.value = 0;
    }

    void Update()
    {
        if (completed)
            return;

        RotatePointer();

        if (Input.GetMouseButtonDown(0))
        {
            CheckTiming();
        }
    }

    void RotatePointer()
    {
        currentRotation -= rotateSpeed * Time.deltaTime;

        if (currentRotation <= -360)
            currentRotation = 0;

        pointer.rotation =
            Quaternion.Euler(0, 0, currentRotation);
    }

    void CheckTiming()
    {
        float angle = Mathf.DeltaAngle(0, currentRotation);

        if (angle >= safeMin && angle <= safeMax)
        {
            repairProgress += repairAmount;

            progressSlider.value = repairProgress;

            statusText.text = "GOOD TIMING";

            if (repairProgress >= 100)
            {
                CompleteRepair();
            }
        }
        else
        {
            statusText.text = "BAD TIMING";
        }
    }

    void CompleteRepair()
    {
        completed = true;

        statusText.text = "SSD REPAIRED";

        repairController.FinishRepair();
    }
}