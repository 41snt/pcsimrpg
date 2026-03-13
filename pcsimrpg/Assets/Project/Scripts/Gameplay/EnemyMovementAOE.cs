using UnityEngine;

public class EnemyMovementAOE : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float attackRange = 2f;

    EnemyAOEAttack aoeAttack;

    void Start()
    {
        // Automatically find the AOE script on this enemy
        aoeAttack = GetComponent<EnemyAOEAttack>();

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        if(player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            StopAndAttack();
        }
    }

    void StopAndAttack()
    {
        if(aoeAttack != null)
        {
            aoeAttack.StartAOE();
        }
    }
}