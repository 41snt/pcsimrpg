using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [Header("Map Boundary")]
    public PolygonCollider2D mapBoundry;

    [Header("Direction")]
    public Direction direction;

    [Header("Teleport Target")]
    public Transform teleportTargetPosition;

    private CinemachineConfiner confiner;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Teleport
    }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change camera boundary
            confiner.m_BoundingShape2D = mapBoundry;

            // Move player
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    void UpdatePlayerPosition(GameObject player)
    {
        // Teleport mode
        if (direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 additivePos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                additivePos.y += 2;
                break;

            case Direction.Down:
                additivePos.y += -2;
                break;

            case Direction.Left:
                additivePos.x += -2;
                break;

            case Direction.Right:
                additivePos.x += 2;
                break;
        }

        player.transform.position = additivePos;
    }
}