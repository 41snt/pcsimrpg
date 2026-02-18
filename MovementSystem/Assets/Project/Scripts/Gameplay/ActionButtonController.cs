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
            SetInteractMode();
        else
            SetAttackMode();
    }

    private void SetAttackMode()
    {
        if (!attackLogo.activeSelf)
        {
            attackLogo.SetActive(true);
            interactLogo.SetActive(false);
        }
    }

    private void SetInteractMode()
    {
        if (!interactLogo.activeSelf)
        {
            interactLogo.SetActive(true);
            attackLogo.SetActive(false);
        }
    }

    private void OnActionPressed()
    {
        if (playerInteraction.Current != null)
        {
            playerInteraction.Current.Interact();
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Player attacks");
        // Hook into combat later
    }
}
