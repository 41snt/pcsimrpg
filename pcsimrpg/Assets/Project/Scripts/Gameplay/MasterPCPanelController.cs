using UnityEngine;

public class MasterPCPanelController : MonoBehaviour
{
    public GameObject joystick;           // reference to joystick GameObject
    public GameObject interactiveButton;  // reference to interactive buttons
    public GameObject masterPCPanel;      // the PC panel itself

    // Call this when opening the PC panel
    public void OpenPCPanel()
    {
        masterPCPanel.SetActive(true);
        if (joystick != null) joystick.SetActive(false);
        if (interactiveButton != null) interactiveButton.SetActive(false);
    }

    // Call this when closing the PC panel
    public void ClosePCPanel()
    {
        masterPCPanel.SetActive(false);
        if (joystick != null) joystick.SetActive(true);
        if (interactiveButton != null) interactiveButton.SetActive(true);
    }
}