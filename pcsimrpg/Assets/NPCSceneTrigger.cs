using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private string cutsceneScene =
        "Cutscene";

    [SerializeField]
    private float delay = 1f;

    private bool triggered;

    private float timer;

    // =========================
    // START CUTSCENE
    // =========================

    public void StartCutscene(int index)
    {
        // RESET TIMER SAFETY
        timer = delay;

        // ALLOW REUSE
        triggered = true;

        // =========================
        // SAVE CURRENT SCENE
        // =========================

        PlayerPrefs.SetString(
            "LastScene",
            SceneManager
                .GetActiveScene()
                .name);

        // =========================
        // SAVE PLAYER POSITION
        // =========================

        GameObject player =
            GameObject.FindGameObjectWithTag(
                "Player");

        if (player != null)
        {
            Vector3 pos =
                player.transform.position;

            PlayerPrefs.SetFloat(
                "PlayerX",
                pos.x);

            PlayerPrefs.SetFloat(
                "PlayerY",
                pos.y);

            PlayerPrefs.SetFloat(
                "PlayerZ",
                pos.z);
        }

        // =========================
        // SAVE GAME
        // =========================

        SaveController saveController =
            FindObjectOfType<SaveController>();

        if (saveController != null)
        {
            saveController.SaveGame();
        }

        PlayerPrefs.Save();

        Debug.Log(
            "Loading cutscene...");
    }

    // =========================
    // UPDATE
    // =========================

    private void Update()
    {
        if (!triggered)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            triggered = false;

            Time.timeScale = 1f;

            SceneManager.LoadScene(
                cutsceneScene);
        }
    }

    // =========================
    // RETURN TO GAME
    // =========================

    public static void ReturnToLastScene()
    {
        string lastScene =
            PlayerPrefs.GetString(
                "LastScene",
                "");

        if (string.IsNullOrEmpty(
            lastScene))
        {
            Debug.LogError(
                "No saved scene.");

            return;
        }

        SceneManager.sceneLoaded +=
            OnSceneLoaded;

        SceneManager.LoadScene(
            lastScene);
    }

    // =========================
    // SCENE LOADED
    // =========================

    private static void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -=
            OnSceneLoaded;

        RestoreGame();
    }

    // =========================
    // RESTORE GAME
    // =========================

    private static void RestoreGame()
    {
        // =========================
        // PLAYER POSITION
        // =========================

        GameObject player =
            GameObject.FindGameObjectWithTag(
                "Player");

        if (player != null)
        {
            Vector3 savedPos =
                new Vector3(
                    PlayerPrefs.GetFloat(
                        "PlayerX",
                        0f),

                    PlayerPrefs.GetFloat(
                        "PlayerY",
                        0f),

                    PlayerPrefs.GetFloat(
                        "PlayerZ",
                        0f));

            Rigidbody2D rb =
                player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity =
                    Vector2.zero;

                rb.position =
                    savedPos;
            }
            else
            {
                player.transform.position =
                    savedPos;
            }
        }

        // =========================
        // RELOAD SAVE
        // =========================

        SaveController saveController =
            FindObjectOfType<SaveController>();

        if (saveController != null)
        {
            saveController.LoadGame();
        }

        // =========================
        // FIX DIALOGUE REFERENCES
        // =========================

        DialogueController dialogue =
            FindObjectOfType<DialogueController>();

        if (dialogue != null)
        {
            dialogue.RebindReferences();

            dialogue.ShowDialogueUI(
                false);
        }

        Debug.Log(
            "Returned to game.");
    }
}