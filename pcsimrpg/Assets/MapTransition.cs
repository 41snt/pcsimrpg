using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MapTransition : MonoBehaviour
{
    [Header("Map Boundary")]
    public PolygonCollider2D mapBoundry;

    [Header("Direction")]
    public Direction direction;

    [Header("Teleport Target")]
    public Transform teleportTargetPosition;

    [Header("Area Message")]
    [TextArea(2, 4)]
    public string areaMessage;

    public float messageDuration = 3f;

    [Header("UI")]
    public TextMeshProUGUI areaTextUI;
    public Image backgroundImage;

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

        // Hide UI at start
        if (areaTextUI != null)
            areaTextUI.gameObject.SetActive(false);

        if (backgroundImage != null)
            backgroundImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change camera boundary
            confiner.m_BoundingShape2D = mapBoundry;

            // Move player
            UpdatePlayerPosition(collision.gameObject);

            // Show message
            if (!string.IsNullOrEmpty(areaMessage))
            {
                StopAllCoroutines();
                StartCoroutine(ShowAreaMessage());
            }
        }
    }

    IEnumerator ShowAreaMessage()
    {
        // Enable UI
        areaTextUI.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);

        areaTextUI.text = areaMessage;

        Color textColor = areaTextUI.color;
        Color bgColor = backgroundImage.color;

        // Start invisible
        textColor.a = 0f;
        bgColor.a = 0f;

        areaTextUI.color = textColor;
        backgroundImage.color = bgColor;

        // FADE IN
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 2f;

            float alpha = Mathf.Lerp(0f, 1f, timer);

            textColor.a = alpha;
            bgColor.a = alpha;

            areaTextUI.color = textColor;
            backgroundImage.color = bgColor;

            yield return null;
        }

        // WAIT
        yield return new WaitForSeconds(messageDuration);

        // FADE OUT
        timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 2f;

            float alpha = Mathf.Lerp(1f, 0f, timer);

            textColor.a = alpha;
            bgColor.a = alpha;

            areaTextUI.color = textColor;
            backgroundImage.color = bgColor;

            yield return null;
        }

        // Hide UI
        areaTextUI.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }

    void UpdatePlayerPosition(GameObject player)
    {
        // TELEPORT MODE
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
                additivePos.y -= 2;
                break;

            case Direction.Left:
                additivePos.x -= 2;
                break;

            case Direction.Right:
                additivePos.x += 2;
                break;
        }

        player.transform.position = additivePos;
    }
}