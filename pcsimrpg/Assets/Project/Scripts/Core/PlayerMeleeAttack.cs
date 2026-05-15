using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int attackDamage = 5;

    public float attackRange = 1.5f;

    [Header("References")]
    public WeaponDirection weapon;

    [Header("Hit Flash")]
    public Color flashColor = Color.red;

    public float flashDuration = 0.1f;

    // =========================
    // ATTACK
    // =========================

    public void Attack()
    {
        // Swing weapon animation
        if (weapon != null)
        {
            weapon.Swing();
        }

        // Detect enemies in range
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                attackRange);

        bool hitSomething = false;

        // Prevent multi-hit per swing
        HashSet<GameObject> hitTargets =
            new HashSet<GameObject>();

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            // IGNORE PLAYER
            if (hit.CompareTag("Player"))
                continue;

            // Try get enemy or boss from collider or parent
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            BossHealth boss =
                hit.GetComponentInParent<BossHealth>();

            GameObject target = null;

            if (enemy != null)
                target = enemy.gameObject;
            else if (boss != null)
                target = boss.gameObject;

            if (target == null)
                continue;

            // Prevent duplicate hits
            if (hitTargets.Contains(target))
                continue;

            hitTargets.Add(target);

            // =========================
            // DAMAGE ENEMY
            // =========================

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                StartCoroutine(FlashEnemy(target));
                Debug.Log("Enemy Hit!");
                hitSomething = true;
            }

            // =========================
            // DAMAGE BOSS
            // =========================

            if (boss != null)
            {
                boss.TakeDamage(attackDamage);
                StartCoroutine(FlashEnemy(target));
                Debug.Log("Boss Hit!");
                hitSomething = true;
            }
        }

        if (!hitSomething)
        {
            Debug.Log("No enemy in range.");
        }
    }

    // =========================
    // FLASH EFFECT (NO PLAYER)
    // =========================

    IEnumerator FlashEnemy(GameObject enemyObject)
    {
        if (enemyObject.CompareTag("Player"))
            yield break;

        SpriteRenderer[] renderers =
            enemyObject.GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length == 0)
            yield break;

        Color[] originalColors =
            new Color[renderers.Length];

        // Apply flash
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;

            originalColors[i] =
                renderers[i].color;

            renderers[i].color =
                flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore colors
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].color =
                    originalColors[i];
            }
        }
    }

    // =========================
    // INTERACT
    // =========================

    public void Interact()
    {
        Attack();
    }

    // =========================
    // DEBUG RANGE
    // =========================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange);
    }
}