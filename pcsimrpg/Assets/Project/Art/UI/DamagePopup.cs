using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamagePopup : MonoBehaviour
{
    public Text damageText; // Drag the Text component here
    public float floatSpeed = 1f;
    public float duration = 0.8f;

    public void ShowDamage(int amount)
    {
        StopAllCoroutines(); // Reset if already showing
        damageText.text = "-" + amount.ToString();
        gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Vector3 startPos = transform.localPosition;
        float elapsed = 0;
        Color originalColor = damageText.color;

        while (elapsed < duration)
        {
            // Move up
            transform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;

            // Fade out
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
        transform.localPosition = startPos; // Reset position
        damageText.color = originalColor; // Reset alpha
    }
}