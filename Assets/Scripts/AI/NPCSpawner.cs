using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public NPCController npcPrefab;
    public int count = 5;
    public Transform[] spawnPoints;
    public Transform[] patrolPoints;
    public Transform combatTarget;

    void Start()
    {
        if (npcPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            NPCController npc = Instantiate(npcPrefab, point.position, point.rotation);
            npc.patrolPoints = patrolPoints;

            int behaviour = Random.Range(0, 3);
            if (behaviour == 1)
            {
                npc.StartChat(Random.Range(2f, 5f));
            }
            else if (behaviour == 2 && combatTarget != null)
            {
                npc.StartCombat(combatTarget);
            }
        }
    }
}
