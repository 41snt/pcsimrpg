using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject uiPanel; // assign your UI panel here in Inspector

    private bool playerNearby = false;

    void Update()
    {
        // Check if player presses the interaction key and is near
        if (playerNearby && Input.GetKeyDown(KeyCode.E)) // E is your interaction key
        {
            uiPanel.SetActive(!uiPanel.activeSelf); // toggle the UI panel
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to interact!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}