using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSceneChanger : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float delayBeforeLoad = 2f;

    private bool triggered;
    private float timer;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (triggered) return;

        triggered = true;
        timer = delayBeforeLoad;

        // SAVE ORIGINAL SCENE (ONLY ON INTERACT)
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
    }

    private void Update()
    {
        if (!triggered) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}