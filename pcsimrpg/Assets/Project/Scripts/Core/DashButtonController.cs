using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DashButtonController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("DashButtonController: No PlayerMovement found in scene!");
            return;
        }

        button.onClick.AddListener(OnDashPressed);
    }

    void OnDashPressed()
    {
        playerMovement.DashButton();
    }
}