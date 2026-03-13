using UnityEngine;
using UnityEngine.UI;

public class DamageDealtCounter : MonoBehaviour
{
    public Text damageText;

    private int totalDamage = 0;
    private float timer = 0f;

    public float hideDelay = 5f;

    void Start()
    {
        damageText.text = "";
    }

    void Update()
    {
        if (totalDamage > 0)
        {
            timer += Time.deltaTime;

            if (timer >= hideDelay)
            {
                damageText.text = "";
                totalDamage = 0;
                timer = 0f;
            }
        }
    }

    public void AddDamage(int damage)
    {
        totalDamage += damage;

        damageText.text = "x" + totalDamage;

        timer = 0f;
    }
}