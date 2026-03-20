using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [HideInInspector] public IInteractable Current;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var interactable))
        {
            Current = interactable; // player is near an NPC
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var interactable))
        {
            if (Current == interactable)
                Current = null; // player left NPC
        }
    }
}
