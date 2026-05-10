using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [HideInInspector] public IInteractable Current;

    [Header("Fallback Attack")]
    public PlayerMeleeAttack meleeAttack;

    private bool canInteract = true;

    private void Update()
    {
        // KEYBOARD INTERACT
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            Current = interactable;

            Debug.Log("Entered Interactable: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponentInParent<IInteractable>();

        if (interactable != null && Current == interactable)
        {
            Current = null;

            Debug.Log("Left Interactable: " + collision.name);
        }
    }

    public void Interact()
    {
        // Prevent spam while holding mobile button
        if (!canInteract)
            return;

        canInteract = false;

        if (Current != null)
        {
            Current.Interact();
        }
        else
        {
            if (meleeAttack != null)
            {
                meleeAttack.Attack();
            }
        }

        Invoke(nameof(ResetInteract), 0.2f);
    }

    void ResetInteract()
    {
        canInteract = true;
    }
}