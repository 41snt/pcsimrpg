using UnityEngine;

public class CleanButtonUI : MonoBehaviour
{
    public GameObject brush;

    public void OnCleanPressed()
    {
        brush.SetActive(true);
    }
}