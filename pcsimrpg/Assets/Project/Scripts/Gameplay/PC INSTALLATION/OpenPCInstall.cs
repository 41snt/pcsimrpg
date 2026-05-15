using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPCInstall : MonoBehaviour
{
    public void OpenScene()
    {
        SceneManager.LoadScene("PCInstallation");
    }
}