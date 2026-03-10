using UnityEngine;
using UnityEngine.UI; // Essential for UI Image control

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;        // Drag the 'Fill' image object here
    public EnemyHealth enemyHealth; // Drag the 'Enemy' parent object here

    void Update()
    {
        if (enemyHealth != null && fillImage != null)
        {
            // Converts health (0-100) to a decimal (0.0-1.0) for the Fill Amount
            float healthPercent = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
            fillImage.fillAmount = healthPercent;
        }

        // Stops the health bar from flipping/rotating if the enemy turns around
        transform.rotation = Quaternion.identity;
    }
}