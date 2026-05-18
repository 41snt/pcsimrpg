using UnityEngine;
using UnityEngine.UI;

public class PCRepairManager : MonoBehaviour
{
    public static PCRepairManager Instance;

    [Header("System Progress")]
    public float systemHealth = 0f; // 0 - 100
    public Slider healthBar;

    void Awake()
    {
        Instance = this;
    }

    public void AddProgress(float value)
    {
        systemHealth += value;
        systemHealth = Mathf.Clamp(systemHealth, 0f, 100f);

        if (healthBar != null)
            healthBar.value = systemHealth;

        Debug.Log("System Health: " + systemHealth);
    }
}