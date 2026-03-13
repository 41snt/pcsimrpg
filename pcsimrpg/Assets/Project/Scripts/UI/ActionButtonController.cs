using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButtonController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject attackLogo;
    [SerializeField] private GameObject interactLogo;

    [Header("References")]
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerMeleeAttack playerAttack;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnActionPressed);

        SetAttackMode();
    }

    private void Update()
    {
        if (playerInteraction.Current != null)
            SetInteractMode();
        else
            SetAttackMode();
    }

    private void SetAttackMode()
    {
        attackLogo.SetActive(true);
        interactLogo.SetActive(false);
    }

    private void SetInteractMode()
    {
        attackLogo.SetActive(false);
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
            playerAttack.Attack();
        }
    }
}
