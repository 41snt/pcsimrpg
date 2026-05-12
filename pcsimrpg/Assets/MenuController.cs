using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject gameplayUI;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // This function will be called by the UI Button
    public void ToggleMenu()
    {
        // Prevent opening another menu if game is already paused
        if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
        {
            return;
        }

        bool isOpen = !menuCanvas.activeSelf;

        menuCanvas.SetActive(isOpen);
        gameplayUI.SetActive(!isOpen);

        PauseController.SetPause(isOpen);
    }
}