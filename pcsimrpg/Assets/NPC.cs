using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private WaypointMover waypointMover;
    private NPCSceneTrigger sceneTrigger;

    private void Start()
    {
        dialogueUI = DialogueController.Instance;
        sceneTrigger = GetComponent<NPCSceneTrigger>();
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
            NextLine();
        else
            StartDialogue();
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);

        PauseController.SetPause(true);

        waypointMover = GetComponent<WaypointMover>();
        if (waypointMover != null)
            waypointMover.StopMoving();

        DisplayLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        dialogueUI.ClearChoices();

        // END DIALOGUE CHECK
        if (dialogueData.endDialogueLines.Length > dialogueIndex &&
            dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        // CHOICES CHECK
        for (int i = 0; i < dialogueData.choices.Length; i++)
        {
            if (dialogueData.choices[i].dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueData.choices[i]);
                return;
            }
        }

        dialogueIndex++;

        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
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

    void DisplayChoices(DialogueChoice choice)
    {
        dialogueUI.ClearChoices();

        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool triggersCutscene =
                choice.triggersCutscene != null &&
                i < choice.triggersCutscene.Length &&
                choice.triggersCutscene[i];

            GameObject button = dialogueUI.CreateChoiceButton(choice.choices[i], null);

            Button btn = button.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            int capturedIndex = nextIndex;
            bool capturedCutscene = triggersCutscene;

            btn.onClick.AddListener(() =>
            {
                ChooseOption(capturedIndex, capturedCutscene);
            });
        }
    }

    void ChooseOption(int nextIndex, bool triggersCutscene)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();

        // 🔥 ONLY trigger if THIS choice says so
        if (triggersCutscene)
        {
            sceneTrigger?.StartCutscene(dialogueIndex);
        }

        DisplayLine();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;

        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

        PauseController.SetPause(false);

        if (waypointMover != null)
            waypointMover.ResumeMoving();
    }
}