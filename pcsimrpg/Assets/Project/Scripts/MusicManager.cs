using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Scene Music Lists")]
    public AudioClip[] mainMenuTracks;
    public AudioClip[] loadingTracks;
    public AudioClip[] gameTracks;

    private Queue<AudioClip> currentQueue = new Queue<AudioClip>();
    private Coroutine playRoutine;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ApplySceneMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneMusic(scene.name);
    }

    void ApplySceneMusic(string sceneName)
    {
        currentQueue.Clear();

        AudioClip[] selected = null;

        if (sceneName == "MainMenu")
            selected = mainMenuTracks;

        else if (sceneName == "LoadingScene")
            selected = loadingTracks;

        else if (sceneName == "SampleScene")
            selected = gameTracks;

        // ❗ NO MUSIC CASE → STOP EVERYTHING
        if (selected == null || selected.Length == 0)
        {
            if (playRoutine != null)
                StopCoroutine(playRoutine);

            playRoutine = StartCoroutine(FadeOutAndStop());
            return;
        }

        foreach (var clip in selected)
        {
            if (clip != null)
                currentQueue.Enqueue(clip);
        }

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayQueue());
    }

    IEnumerator PlayQueue()
    {
        yield return StartCoroutine(FadeOut());

        while (currentQueue.Count > 0)
        {
            AudioClip clip = currentQueue.Dequeue();

            audioSource.clip = clip;
            audioSource.Play();

            yield return StartCoroutine(FadeIn());

            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        // If only one track → loop it
        if (audioSource.clip != null)
        {
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        audioSource.volume = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 0.5f, t);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float startVol = audioSource.volume;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, t);
            yield return null;
        }
    }

    IEnumerator FadeOutAndStop()
    {
        yield return StartCoroutine(FadeOut());
        audioSource.Stop();
        audioSource.clip = null;
    }
}