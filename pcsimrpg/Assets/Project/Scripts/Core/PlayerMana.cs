using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public int maxMana = 100;
    public int currentMana;

    [SerializeField] private Slider manaBar;

    public int regenAmount = 10;
    public float regenInterval = 5f;

    private float regenTimer;

    void Start()
    {
        currentMana = maxMana;

        if (manaBar != null)
        {
            manaBar.maxValue = maxMana;
            manaBar.value = currentMana;
        }
    }

    void Update()
    {
        if (currentMana >= maxMana)
            return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval)
        {
            currentMana += regenAmount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            UpdateManaBar();

            regenTimer = 0f;
        }
    }

    public bool UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;

            UpdateManaBar();

            return true;
        }

        return false;
    }

    void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.value = currentMana;
        }
    }
}