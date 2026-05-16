using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResetSaves : MonoBehaviour
{
    public void ResetSaveData()
    {
        string saveLocation =
            Path.Combine(
                Application.persistentDataPath,
                "saveData.json");

        // Prevent autosave
        SaveController.isResettingSave = true;

        // Delete save file
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);

            Debug.Log("SAVE FILE DELETED");
        }
        else
        {
            Debug.Log("NO SAVE FILE FOUND");
        }

        // Clear prefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("PLAYER PREFS CLEARED");

        // VERIFY DELETE
        if (!File.Exists(saveLocation))
        {
            Debug.Log("SAVE DELETE SUCCESS");
        }
        else
        {
            Debug.LogError("SAVE STILL EXISTS");
        }

#if UNITY_EDITOR

        EditorApplication.isPlaying = false;

#endif
    }
}