using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int attackDamage = 25;
    public float attackRange = 1.5f;

    [Header("References")]
    public WeaponDirection weapon; // assign WeaponHolder here

    public void Attack()
    {
        // Trigger weapon swing animation
        if (weapon != null)
        {
            weapon.Swing();
        }

        // Detect enemies within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        bool hitEnemy = false;

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Enemy Hit!");
                hitEnemy = true;
            }
        }

        if (!hitEnemy)
        {
            Debug.Log("No enemy in range.");
        }
    }

    // For button press
    public void Interact()
    {
        Attack();
    }

    // Visualize attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
