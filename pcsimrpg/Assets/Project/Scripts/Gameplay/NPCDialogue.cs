using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    [TextArea(3, 5)]
    public string[] dialogueLines;

    public float typingSpeed = 0.05f;
}