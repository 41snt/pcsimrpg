using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLastScene : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        string lastScene = PlayerPrefs.GetString("LastScene");

        SceneManager.LoadScene(lastScene);
    }

    public bool CanInteract()
    {
        return true;
    }
}