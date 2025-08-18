using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamer : MonoBehaviour
{
    [System.Serializable]
    public class SceneArea
    {
        public string sceneName;
        public Vector2 center;
        public float loadRadius = 100f;
    }

    public Transform player;
    public List<SceneArea> scenes = new();

    private readonly Dictionary<string, AsyncOperation> loadedScenes = new();

    void Update()
    {
        if (player == null)
        {
            return;
        }

        Vector2 playerPos = new(player.position.x, player.position.z);
        foreach (SceneArea area in scenes)
        {
            float dist = Vector2.Distance(playerPos, area.center);
            bool shouldLoad = dist <= area.loadRadius;
            bool isLoaded = loadedScenes.ContainsKey(area.sceneName);

            if (shouldLoad && !isLoaded)
            {
                AsyncOperation op = SceneManager.LoadSceneAsync(area.sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(area.sceneName, op);
            }
            else if (!shouldLoad && isLoaded)
            {
                SceneManager.UnloadSceneAsync(area.sceneName);
                loadedScenes.Remove(area.sceneName);
            }
        }
    }
}
