using System.Collections;
using System.Collections.Generic;
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

        if (attackLogo == null || interactLogo == null || playerInteraction == null)
        {
            Debug.LogError("ActionButtonController is missing references.");
            enabled = false;
            return;
        }

        button.onClick.AddListener(OnActionPressed);
        SetAttackMode();
    }

    private void Update()
    {
        if (playerInteraction.Current != null)
        {
            SetInteractMode();
        }
        else
        {
            SetAttackMode();
        }
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
        else if (playerInteraction.CurrentEnemy != null)
        {
            playerAttack.Attack();
        }
        else
        {
            playerAttack.Attack();
        }
    }
}