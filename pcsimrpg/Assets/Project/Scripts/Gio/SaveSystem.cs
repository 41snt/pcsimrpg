using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetPath(int slot)
    {
        return Application.persistentDataPath + "/save" + slot + ".json";
    }

    public static void SaveGame(int slot, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = GetPath(slot);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        return null;
    }

    public static void DeleteSave(int slot)
    {
        string path = GetPath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }
}