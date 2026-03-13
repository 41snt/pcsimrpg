using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Drop Prefabs")]
    public GameObject manaDrop;

    public GameObject[] itemDrops;

    [Header("Drop Chances")]
    [Range(0f, 1f)]
    public float manaDropChance = 0.6f;

    [Range(0f, 1f)]
    public float itemDropChance = 0.3f;

    public void DropLoot()
    {
        // Mana drop
        if (manaDrop != null && Random.value <= manaDropChance)
        {
            Instantiate(manaDrop, transform.position, Quaternion.identity);
        }

        // Item drop
        if (itemDrops.Length > 0 && Random.value <= itemDropChance)
        {
            int index = Random.Range(0, itemDrops.Length);
            Instantiate(itemDrops[index], transform.position, Quaternion.identity);
        }
    }
}