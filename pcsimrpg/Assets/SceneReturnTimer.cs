using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturnTimer : MonoBehaviour
{
    [SerializeField] private float returnTime = 5f;

    private float timer;

    private void Start()
    {
        timer = returnTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            string lastScene = PlayerPrefs.GetString("LastScene");
            SceneManager.LoadScene(lastScene);
        }
    }
}