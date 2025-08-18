using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int lastQuestID;
}

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }
    public GameData data = new GameData();

    private string filePath;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if (Instance == null)
        {
            var go = new GameObject("PersistenceManager");
            Instance = go.AddComponent<PersistenceManager>();
            DontDestroyOnLoad(go);
        }
    }

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();
    }

    public void SaveGame()
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(json);
        }
    }

    public void OnQuestCompleted(int questId)
    {
        data.lastQuestID = questId;
        SaveGame();
    }

    public void OnLogout()
    {
        SaveGame();
    }

    void OnApplicationQuit()
    {
        OnLogout();
    }
}
