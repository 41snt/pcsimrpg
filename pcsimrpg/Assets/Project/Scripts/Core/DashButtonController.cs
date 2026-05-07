using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class DashButtonController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Button button;
    private AudioSource audioSource;

    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("DashButtonController: No PlayerMovement found in scene!");
        }

        button.onClick.AddListener(TriggerDashAction);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            TriggerDashAction();
        }
    }

    void TriggerDashAction()
    {
        if (playerMovement != null)
        {
            // The sound only plays if DashButton() returns true
            if (playerMovement.DashButton())
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
        }
    }
}
