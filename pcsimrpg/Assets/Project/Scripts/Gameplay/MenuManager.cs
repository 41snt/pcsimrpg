using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Transition Speed")]
    [Tooltip("1.5 to 2.0 is usually the 'sweet spot' for smooth but not slow.")]
    public float transitionSpeed = 1.8f;

    public void OpenPanel(GameObject targetPanel)
    {
        StopAllCoroutines();

        CanvasGroup group = targetPanel.GetComponent<CanvasGroup>();
        if (group == null) group = targetPanel.AddComponent<CanvasGroup>();

        StartCoroutine(SmoothFadeRoutine(targetPanel, group));
    }

    IEnumerator SmoothFadeRoutine(GameObject panel, CanvasGroup group)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.one * 0.9f;
        group.alpha = 0;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;

            // SmoothStep makes the transition feel 'weighted' and professional
            float smoothT = Mathf.SmoothStep(0, 1, t);

            panel.transform.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one, smoothT);
            group.alpha = smoothT;
            yield return null;
        }

        panel.transform.localScale = Vector3.one;
        group.alpha = 1;
    }
}