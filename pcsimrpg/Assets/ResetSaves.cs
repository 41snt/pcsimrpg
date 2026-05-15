using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResetSaves : MonoBehaviour
{
    private string saveLocation;

    private void Start()
    {
        saveLocation =
            Path.Combine(
                Application.persistentDataPath,
                "saveData.json");
    }

    // BUTTON FUNCTION
    public void ResetSaveData()
    {
        // Delete save file
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);

            Debug.Log("Save file deleted.");
        }

        // Clear PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("All progress reset.");

        // STOP PLAY MODE IN UNITY EDITOR
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        // If built game, restart current scene
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
#endif
    }
}