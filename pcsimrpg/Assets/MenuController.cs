using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject gameplayUI;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    public void ToggleMenu()
    {
        // Prevent opening another menu if already paused
        if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
        {
            return;
        }

        bool isOpen = !menuCanvas.activeSelf;

        menuCanvas.SetActive(isOpen);

        // Hide gameplay UI when menu is open
        gameplayUI.SetActive(!isOpen);

        // Pause system
        PauseController.SetPause(isOpen);
    }
}