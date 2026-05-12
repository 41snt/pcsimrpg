using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("Chest State")]
    public bool isOpened;

    [Header("Chest ID")]
    public string chestID;

    [Header("Loot")]
    public GameObject itemPrefab;

    [Header("Visual")]
    public Sprite openedSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        // GENERATE ID
        if (string.IsNullOrEmpty(chestID))
        {
            chestID =
                gameObject.name + "_" +
                transform.position.x + "_" +
                transform.position.y;
        }
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        OpenChest();
    }

    public void OpenChest()
    {
        SetOpened(true);

        if (itemPrefab != null)
        {
            Instantiate(
                itemPrefab,
                transform.position +
                Vector3.down,
                Quaternion.identity);
        }
    }

    public void SetOpened(bool opened)
    {
        isOpened = opened;

        if (isOpened &&
            openedSprite != null &&
            spriteRenderer != null)
        {
            spriteRenderer.sprite =
                openedSprite;
        }
    }
}