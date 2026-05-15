using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text loadingText;
    public CanvasGroup canvasGroup;

    private string nextScene = "SampleScene";

    private float targetProgress = 0f;

    void Start()
    {
        canvasGroup.alpha = 0f;

        StartCoroutine(FadeIn());
        StartCoroutine(LoadAsync());
        StartCoroutine(AnimateDots());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            targetProgress = operation.progress / 0.9f;
            yield return null;
        }

        targetProgress = 1f;

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeOut());

        operation.allowSceneActivation = true;
    }

    void Update()
    {
        // Smooth easing (critical upgrade)
        progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, Time.deltaTime * 5f);
    }

    IEnumerator AnimateDots()
    {
        string baseText = "Loading";
        int dots = 0;

        while (true)
        {
            dots = (dots + 1) % 4;
            loadingText.text = baseText + new string('.', dots);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
    }
}