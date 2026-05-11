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

        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);

        Debug.Log("Cutscene triggered from choice at index: " + index);
    }

    private void Update()
    {
        if (!triggered) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SceneManager.LoadScene(cutsceneScene);
        }
    }
}