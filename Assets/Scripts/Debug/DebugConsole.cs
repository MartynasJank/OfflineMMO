using System.Collections.Generic;
using UnityEngine;
using OfflineMMO.Server;

namespace OfflineMMO.Debugging
{
    public class DebugConsole : MonoBehaviour
    {
        string input = string.Empty;
        readonly List<string> log = new();
        Dictionary<string, bool> features = new();
        ServerLoopBehaviour serverLoop;

        void Awake()
        {
            serverLoop = FindObjectOfType<ServerLoopBehaviour>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 400, 300));
            foreach (var line in log)
            {
                GUILayout.Label(line);
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            input = GUILayout.TextField(input);
            if (GUILayout.Button("Enter") && !string.IsNullOrEmpty(input))
            {
                ExecuteCommand(input);
                input = string.Empty;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void ExecuteCommand(string cmd)
        {
            var parts = cmd.Split(' ');
            if (parts.Length == 0) return;
            switch (parts[0].ToLower())
            {
                case "spawn":
                    if (parts.Length < 3)
                    {
                        log.Add("Usage: spawn [item|npc] id");
                        break;
                    }
                    if (parts[1].ToLower() == "npc")
                    {
                        var npc = new Npc(parts[2]);
                        serverLoop?.Server.AddNpc(npc);
                        log.Add($"Spawned NPC {parts[2]}");
                    }
                    else
                    {
                        log.Add($"Spawned item {parts[2]}");
                    }
                    break;
                case "toggle":
                    if (parts.Length < 2) break;
                    var feature = parts[1];
                    features[feature] = !features.GetValueOrDefault(feature);
                    log.Add($"{feature} {(features[feature] ? "on" : "off")}");
                    break;
                default:
                    log.Add($"Unknown command {cmd}");
                    break;
            }
        }
    }
}
