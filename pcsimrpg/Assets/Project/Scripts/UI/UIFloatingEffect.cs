using UnityEngine;

public class UIFloatingEffect : MonoBehaviour
{
    [Header("Main Image Settings")]
    public RectTransform mainIcon;
    public float floatAmplitude = 20f; // How high it goes
    public float floatSpeed = 2f;      // How fast it moves

    [Header("Shadow Settings")]
    public RectTransform shadow;
    public float minShadowScale = 0.7f;
    public float maxShadowScale = 1.0f;

    private Vector3 startPos;

    void Start()
    {
        if (mainIcon != null)
            startPos = mainIcon.anchoredPosition;
    }

    void Update()
    {
        // 1. Calculate the Sine Wave (The Up and Down movement)
        float movement = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 2. Move the Icon up and down
        if (mainIcon != null)
        {
            mainIcon.anchoredPosition = startPos + new Vector3(0, movement, 0);
        }

        // 3. Scale the shadow (smaller when icon is high, larger when low)
        if (shadow != null)
        {
            // Normalize movement to a 0 to 1 range
            float lerpVal = (movement + floatAmplitude) / (floatAmplitude * 2);
            float currentScale = Mathf.Lerp(maxShadowScale, minShadowScale, lerpVal);
            shadow.localScale = new Vector3(currentScale, currentScale, 1f);
        }
    }
}