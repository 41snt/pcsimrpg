using UnityEngine;
using UnityEngine.UI;

public class ThermalPasteButton : MonoBehaviour
{
    public CPUThermalPaste thermalPasteSystem; // Reference to your Thermal Paste script
    public Button button;                       // The button itself
    public GameObject brushUI;                  // The brush image for paste

    // Called when you click "Add Thermal Paste"
    public void OnClickApplyPaste()
    {
        if (thermalPasteSystem == null) return;

        // Activate the brush UI
        if (brushUI != null)
            brushUI.SetActive(true);

        // Activate paste system
        thermalPasteSystem.EnablePaste();

        // Disable the button while swiping
        if (button != null)
            button.interactable = false;
    }
}