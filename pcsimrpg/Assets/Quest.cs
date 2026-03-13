using System;

[Serializable]
public class Quest
{
    public string questName;
    public string objective;
    public bool isCompleted;

    public Quest(string name, string obj)
    {
        questName = name;
        objective = obj;
        isCompleted = false;
    }
}
