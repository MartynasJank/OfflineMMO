using System;

namespace OfflineMMO.Server
{
    [Serializable]
    public class Npc
    {
        public string Name { get; set; }
        public int Ticks { get; set; }

        public Npc() : this("npc") { }

        public Npc(string name)
        {
            Name = name;
        }

        public void UpdateAI()
        {
            Ticks++;
        }
    }
}
