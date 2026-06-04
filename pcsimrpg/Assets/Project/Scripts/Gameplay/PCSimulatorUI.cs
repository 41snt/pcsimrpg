using UnityEngine;

public class PCSimulatorUI : MonoBehaviour, IInteractable
{
    [Header("Main PC View")]
    public GameObject masterPCPanel;

    [Header("Part Sub-Panels")]
    public GameObject cpuPanel;
    public GameObject ramPanel;
    public GameObject gpuPanel;
    public GameObject moboPanel;
    public GameObject psuPanel;
    public GameObject hddPanel;
    public GameObject ssdPanel;
    public GameObject coolerPanel;

    public void Interact() => OpenMainPanel();

    public void OpenMainPanel() => SwitchToPanel(masterPCPanel);

    // Button Functions linked to UI
    public void OpenCPU() => SwitchToPanel(cpuPanel);
    public void OpenRAM() => SwitchToPanel(ramPanel);
    public void OpenGPU() => SwitchToPanel(gpuPanel);
    public void OpenMOBO() => SwitchToPanel(moboPanel);
    public void OpenPSU() => SwitchToPanel(psuPanel);
    public void OpenHDD() => SwitchToPanel(hddPanel);
    public void OpenSSD() => SwitchToPanel(ssdPanel);
    public void OpenCooler() => SwitchToPanel(coolerPanel);

    private void SwitchToPanel(GameObject target)
    {
        // Hide all panels
        if (masterPCPanel) masterPCPanel.SetActive(false);
        if (cpuPanel) cpuPanel.SetActive(false);
        if (ramPanel) ramPanel.SetActive(false);
        if (gpuPanel) gpuPanel.SetActive(false);
        if (moboPanel) moboPanel.SetActive(false);
        if (psuPanel) psuPanel.SetActive(false);
        if (hddPanel) hddPanel.SetActive(false);
        if (ssdPanel) ssdPanel.SetActive(false);
        if (coolerPanel) coolerPanel.SetActive(false);

        // Show selected panel
        if (target != null)
            target.SetActive(true);
    }

    public void CloseAll() => SwitchToPanel(null);
}