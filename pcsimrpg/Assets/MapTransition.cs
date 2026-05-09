using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private Collider2D mapBoundary;
    [SerializeField] private Direction direction;

    private CinemachineConfiner confiner;

    enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += 2;
                break;

            case Direction.Down:
                newPos.y -= 2;
                break;

            case Direction.Left:
                newPos.x -= 2;
                break;

            case Direction.Right:
                newPos.x += 2;
                break;
        }

        player.transform.position = newPos;
    }
}