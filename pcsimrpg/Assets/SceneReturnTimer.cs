using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturnTimer : MonoBehaviour
{
    [SerializeField] private float returnTime = 5f;

    private float timer;

    private void Start()
    {
        timer = returnTime;

        // IMPORTANT FIX
        PauseController.SetPause(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // UNPAUSE BEFORE RETURNING
            PauseController.SetPause(false);

            Time.timeScale = 1f;

            string lastScene =
                PlayerPrefs.GetString("LastScene");

            SceneManager.LoadScene(lastScene);
        }
    }
}