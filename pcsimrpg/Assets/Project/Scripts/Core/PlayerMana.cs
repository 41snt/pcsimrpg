using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public int maxMana = 100;
    public int currentMana;

    public Slider manaBar;

    public int regenAmount = 10;
    public float regenInterval = 5f;

    private float regenTimer;

    void Start()
    {
        currentMana = maxMana;

        manaBar.maxValue = maxMana;
        manaBar.value = maxMana;
    }

    void Update()
    {
        if (currentMana >= maxMana) return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval)
        {
            currentMana += regenAmount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            manaBar.value = currentMana;

            regenTimer = 0f;
        }
    }

    public bool UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            manaBar.value = currentMana;
            return true;
        }

        return false;
    }
}