using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TransformTriggerData
{
    [Header("Target Object")]
    public Transform targetObject;

    [Header("Dialogue Trigger")]
    public int dialogueIndex;

    [Header("Rotation")]
    public bool rotateObject;

    public Vector3 rotationAmount =
        new Vector3(0f, 0f, 90f);

    [Header("Position")]
    public bool moveObject;

    public Vector3 newPosition;
}

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    private DialogueController dialogueUI;

    private int dialogueIndex;

    private bool isTyping;
    private bool isDialogueActive;

    private WaypointMover waypointMover;
    private NPCSceneTrigger sceneTrigger;

    [Header("Transform Triggers")]
    public TransformTriggerData[] transformTriggers;

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }

    private QuestState questState =
        QuestState.NotStarted;

    private void Start()
    {
        dialogueUI =
            DialogueController.Instance;

        sceneTrigger =
            GetComponent<NPCSceneTrigger>();

        waypointMover =
            GetComponent<WaypointMover>();
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
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

    // =========================
    // START DIALOGUE
    // =========================

    void StartDialogue()
    {
        SyncQuestState();

        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex =
                dialogueData.questInProgressIndex;
        }
        else
        {
            dialogueIndex =
                dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;

        dialogueUI.SetNPCInfo(
            dialogueData.npcName,
            dialogueData.npcPortrait);

        dialogueUI.ShowDialogueUI(true);

        if (waypointMover != null)
        {
            waypointMover.StopMoving();
        }

        DisplayLine();
    }

    // =========================
    // QUEST STATE
    // =========================

    void SyncQuestState()
    {
        if (dialogueData.quest == null)
            return;

        string questID =
            dialogueData.quest.questID;

        if (QuestController.Instance
            .IsQuestHandedIn(questID))
        {
            questState =
                QuestState.Completed;
        }
        else if (QuestController.Instance
            .IsQuestActive(questID))
        {
            questState =
                QuestState.InProgress;
        }
        else
        {
            questState =
                QuestState.NotStarted;
        }
    }

    // =========================
    // NEXT LINE
    // =========================

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();

            dialogueUI.SetDialogueText(
                dialogueData.dialogueLines[
                    dialogueIndex]);

            isTyping = false;

            return;
        }

        dialogueUI.ClearChoices();

        // =========================
        // TRANSFORM TRIGGERS
        // =========================

        foreach (TransformTriggerData trigger
            in transformTriggers)
        {
            if (trigger.targetObject == null)
                continue;

            if (trigger.dialogueIndex !=
                dialogueIndex)
                continue;

            // ROTATE OBJECT
            if (trigger.rotateObject)
            {
                trigger.targetObject.Rotate(
                    trigger.rotationAmount);
            }

            // MOVE OBJECT
            if (trigger.moveObject)
            {
                trigger.targetObject.position =
                    trigger.newPosition;
            }
        }

        // =========================
        // END DIALOGUE
        // =========================

        if (dialogueData.endDialogueLines.Length >
            dialogueIndex &&
            dialogueData.endDialogueLines[
                dialogueIndex])
        {
            EndDialogue();
            return;
        }

        // =========================
        // CHOICES
        // =========================

        for (int i = 0;
            i < dialogueData.choices.Length;
            i++)
        {
            if (dialogueData.choices[i]
                .dialogueIndex == dialogueIndex)
            {
                DisplayChoices(
                    dialogueData.choices[i]);

                return;
            }
        }

        dialogueIndex++;

        if (dialogueIndex <
            dialogueData.dialogueLines.Length)
        {
            DisplayLine();
        }
        else
        {
            EndDialogue();
        }
    }

    // =========================
    // DISPLAY LINE
    // =========================

    void DisplayLine()
    {
        StopAllCoroutines();

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        dialogueUI.SetDialogueText("");

        foreach (char letter in
            dialogueData.dialogueLines[
                dialogueIndex])
        {
            dialogueUI.SetDialogueText(
                dialogueUI.dialogueText.text
                + letter);

            yield return new WaitForSeconds(
                dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    // =========================
    // DISPLAY CHOICES
    // =========================

    void DisplayChoices(DialogueChoice choice)
    {
        dialogueUI.ClearChoices();

        for (int i = 0;
            i < choice.choices.Length;
            i++)
        {
            int nextIndex =
                choice.nextDialogueIndexes[i];

            bool givesQuest =
                choice.givesQuest != null &&
                i < choice.givesQuest.Length &&
                choice.givesQuest[i];

            bool triggersCutscene =
                choice.triggersCutscene != null &&
                i < choice.triggersCutscene.Length &&
                choice.triggersCutscene[i];

            GameObject button =
                dialogueUI.CreateChoiceButton(
                    choice.choices[i],
                    null);

            Button btn =
                button.GetComponent<Button>();

            btn.onClick.RemoveAllListeners();

            int capturedIndex =
                nextIndex;

            bool capturedQuest =
                givesQuest;

            bool capturedCutscene =
                triggersCutscene;

            btn.onClick.AddListener(() =>
            {
                ChooseOption(
                    capturedIndex,
                    capturedCutscene,
                    capturedQuest);
            });
        }
    }

    // =========================
    // CHOOSE OPTION
    // =========================

    void ChooseOption(
        int nextIndex,
        bool triggersCutscene,
        bool givesQuest)
    {
        if (givesQuest &&
            dialogueData.quest != null)
        {
            QuestController.Instance
                .AcceptQuest(
                    dialogueData.quest);

            questState =
                QuestState.InProgress;
        }

        dialogueIndex =
            nextIndex;

        dialogueUI.ClearChoices();

        if (triggersCutscene)
        {
            sceneTrigger?.StartCutscene(
                dialogueIndex);
        }

        DisplayLine();
    }

    // =========================
    // END DIALOGUE
    // =========================

    public void EndDialogue()
    {
        if (dialogueData.quest != null)
        {
            string questID =
                dialogueData.quest.questID;

            if (QuestController.Instance
                .IsQuestCompleted(questID) &&
                !QuestController.Instance
                .IsQuestHandedIn(questID))
            {
                QuestController.Instance
                    .HandInQuest(questID);
            }
        }

        StopAllCoroutines();

        isDialogueActive = false;

        dialogueUI.SetDialogueText("");

        dialogueUI.ShowDialogueUI(false);

        if (waypointMover != null)
        {
            waypointMover.ResumeMoving();
        }
    }
}