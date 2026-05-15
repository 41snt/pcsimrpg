using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;

    public float attackRange = 1f;

    public LayerMask enemyLayers;

    public int attackDamage = 25;

    [Header("Weapon Swing")]
    public WeaponDirection weaponSwing;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        // PLAY WEAPON SWING
        if (weaponSwing != null)
        {
            weaponSwing.Swing();
        }

        if (attackPoint == null)
        {
            Debug.LogError(
                "AttackPoint is NOT assigned!"
            );

            return;
        }

        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth =
                enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(
                    attackDamage
                );
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}