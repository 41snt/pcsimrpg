using UnityEngine;

public class LoadMenuController : MonoBehaviour
{
    public GameObject loadMenuPanel;

    private void Start()
    {
        loadMenuPanel.SetActive(false);
    }

    public void OpenLoadMenu()
    {
        loadMenuPanel.SetActive(true);
    }

    public void CloseLoadMenu()
    {
        loadMenuPanel.SetActive(false);
    }
}