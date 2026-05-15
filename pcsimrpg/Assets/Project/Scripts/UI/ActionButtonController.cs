using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButtonController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject attackLogo;

    [SerializeField]
    private GameObject interactLogo;

    [Header("References")]
    [SerializeField]
    private PlayerInteraction playerInteraction;

    [SerializeField]
    private PlayerMeleeAttack playerAttack;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(
            OnActionPressed
        );

        UpdateButtonMode();
    }

    private void Update()
    {
        UpdateButtonMode();
    }

    private void UpdateButtonMode()
    {
        if (playerInteraction.Current != null)
            SetInteractMode();
        else
            SetAttackMode();
    }

    private void SetAttackMode()
    {
        if (!attackLogo.activeSelf)
            attackLogo.SetActive(true);

        if (interactLogo.activeSelf)
            interactLogo.SetActive(false);
    }

    private void SetInteractMode()
    {
        if (attackLogo.activeSelf)
            attackLogo.SetActive(false);

        if (!interactLogo.activeSelf)
            interactLogo.SetActive(true);
    }

    private void OnActionPressed()
    {
        if (playerInteraction.Current != null)
        {
            playerInteraction.Current.Interact();
        }
        else
        {
            if (playerAttack != null)
            {
                playerAttack.Attack();
            }
        }
    }
}