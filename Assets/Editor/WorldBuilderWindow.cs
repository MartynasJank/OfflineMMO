using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

public class WorldBuilderWindow : EditorWindow
{
    private GameObject selectedPrefab;

    [MenuItem("Tools/World Builder")]
    public static void ShowWindow()
    {
        GetWindow<WorldBuilderWindow>("World Builder");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Terrain"))
        {
            CreateTerrain();
        }

        selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Environment Prefab", selectedPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Place Prefab") && selectedPrefab != null)
        {
            PrefabUtility.InstantiatePrefab(selectedPrefab);
        }

        if (GUILayout.Button("Bake NavMesh"))
        {
            NavMeshBuilder.BuildNavMesh();
        }
    }

    private void CreateTerrain()
    {
        if (Terrain.activeTerrain == null)
        {
            TerrainData data = new TerrainData();
            GameObject terrainGO = Terrain.CreateTerrainGameObject(data);
            Undo.RegisterCreatedObjectUndo(terrainGO, "Create Terrain");
        }
    }
}
