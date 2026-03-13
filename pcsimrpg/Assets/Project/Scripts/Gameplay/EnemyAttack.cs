using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10; // You can change this for each enemy type!

    // We don't need code here because the PlayerHealth script 
    // reaches into THIS script to grab the damageAmount.
}