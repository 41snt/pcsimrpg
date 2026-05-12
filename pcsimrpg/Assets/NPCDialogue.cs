using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;

    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;

    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public DialogueChoice[] choices;

    public int questInProgressIndex;
    public int questCompletedIndex;

    public Quest quest;

    // =========================
    // ROTATE OBJECT SUPPORT
    // =========================

    public DialogueRotateTrigger[] rotateTriggers;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;

    public string[] choices;

    public int[] nextDialogueIndexes;

    public bool[] givesQuest;

    public bool[] triggersCutscene;

    public string[] cutsceneNames;
}

[System.Serializable]
public class DialogueRotateTrigger
{
    public int dialogueIndex;

    public Vector3 rotationAmount =
        new Vector3(0f, 0f, 90f);
}