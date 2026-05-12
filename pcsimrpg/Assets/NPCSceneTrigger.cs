using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSceneTrigger : MonoBehaviour
{
    [SerializeField] private string cutsceneScene = "Cutscene";
    [SerializeField] private float delay = 2f;

    private bool triggered;
    private float timer;

    public void StartCutscene(int index)
    {
        if (triggered) return;

        triggered = true;
        timer = delay;

        // SAVE CURRENT SCENE
        PlayerPrefs.SetString(
            "LastScene",
            SceneManager.GetActiveScene().name);

        // SAVE PLAYER POSITION
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerPrefs.SetFloat(
                "PlayerX",
                player.transform.position.x);

            PlayerPrefs.SetFloat(
                "PlayerY",
                player.transform.position.y);
        }

        // CREATE REAL SAVE
        SaveController saveController =
            FindObjectOfType<SaveController>();

        if (saveController != null)
        {
            saveController.SaveGame();
        }

        Debug.Log(
            "Cutscene triggered from choice at index: "
            + index);
    }

    private void Update()
    {
        if (!triggered) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PauseController.SetPause(false);

            Time.timeScale = 1f;

            SceneManager.LoadScene(cutsceneScene);
        }
    }
}