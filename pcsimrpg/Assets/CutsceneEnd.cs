using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public void EndCutscene()
    {
        NPCSceneTrigger.ReturnToLastScene();
    }
}