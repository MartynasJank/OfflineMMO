using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    Inactive,
    Active,
    Completed
}

[System.Serializable]
public class QuestData
{
    public string id;
    public string title;
    public string description;
    public QuestState state = QuestState.Inactive;
}

public class QuestManager : MonoBehaviour
{
    public List<QuestData> quests = new List<QuestData>();

    public void StartQuest(string id)
    {
        QuestData quest = quests.Find(q => q.id == id);
        if (quest != null && quest.state == QuestState.Inactive)
            quest.state = QuestState.Active;
    }

    public void CompleteQuest(string id)
    {
        QuestData quest = quests.Find(q => q.id == id);
        if (quest != null && quest.state == QuestState.Active)
            quest.state = QuestState.Completed;
    }

    public QuestData GetQuest(string id)
    {
        return quests.Find(q => q.id == id);
    }
}
