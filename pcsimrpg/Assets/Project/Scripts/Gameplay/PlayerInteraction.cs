using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [HideInInspector] public IInteractable Current;

    [Header("Fallback Attack")]
    public PlayerMeleeAttack meleeAttack;

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
    }
}