using System.Collections.Generic;
using System.Text.Json;
#if UNITY_2020_1_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace OfflineMMO.Server
{
    public class Server
    {
        readonly List<Npc> npcs = new();

        public IReadOnlyList<Npc> Npcs => npcs;

        public void AddNpc(Npc npc) => npcs.Add(npc);

        public void Update()
        {
#if UNITY_2020_1_OR_NEWER
            Profiler.BeginSample("AI Updates");
#endif
            foreach (var npc in npcs)
            {
                npc.UpdateAI();
            }
#if UNITY_2020_1_OR_NEWER
            Profiler.EndSample();
#endif
        }

        public string SaveState()
        {
            return JsonSerializer.Serialize(npcs);
        }

        public void LoadState(string json)
        {
            npcs.Clear();
            var loaded = JsonSerializer.Deserialize<List<Npc>>(json);
            if (loaded != null)
            {
                npcs.AddRange(loaded);
            }
        }
    }
}
