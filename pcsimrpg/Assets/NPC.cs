using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    // Bigger interaction distance
    public float interactionDistance = 5f;

    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private WaypointMover mover;

    private void Start()
    {
        mover = GetComponent<WaypointMover>();

        dialogueUI = DialogueController.Instance;
    }

    public void Interact()
    {
        // Check distance
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > interactionDistance)
                return;
        }

        if (dialogueData == null)
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        // stop movement
        if (mover != null)
            mover.isStopped = true;

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();

            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text + letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

        // resume movement
        if (mover != null)
            mover.isStopped = false;

        isDialogueActive = false;
    }
}