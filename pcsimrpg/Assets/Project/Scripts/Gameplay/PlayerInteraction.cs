using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public IInteractable Current { get; private set; }
    public EnemyHealth CurrentEnemy { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            CurrentEnemy = enemy;
            return;
        }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Current = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null && enemy == CurrentEnemy)
        {
            CurrentEnemy = null;
        }

        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == Current)
        {
            Current = null;
        }
    }
}