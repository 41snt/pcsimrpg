using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public int attackDamage = 25;
    public float attackRange = 1.5f;

    public void Attack()
    {
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

    public void Interact()
    {
        Attack();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
