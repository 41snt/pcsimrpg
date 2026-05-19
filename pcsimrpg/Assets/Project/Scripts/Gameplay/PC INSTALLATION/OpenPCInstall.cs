using UnityEngine;

public class OpenPCInstall : MonoBehaviour
{
    public GameObject pcInstallPanel;

    public void OpenPanel()
    {
        pcInstallPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        pcInstallPanel.SetActive(false);
    }
}