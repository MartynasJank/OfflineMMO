using NUnit.Framework;
using OfflineMMO.Server;

namespace ServerLogic.Tests
{
    public class ServerTests
    {
        [Test]
        public void NpcUpdateIncrementsTicks()
        {
            var server = new Server();
            var npc = new Npc("Test");
            server.AddNpc(npc);

            server.Update();

            Assert.That(npc.Ticks, Is.EqualTo(1));
        }

        [Test]
        public void SaveAndLoadRestoresNpcTicks()
        {
            var server = new Server();
            var npc = new Npc("Persisted") { Ticks = 5 };
            server.AddNpc(npc);

            var json = server.SaveState();

            var loaded = new Server();
            loaded.LoadState(json);

            Assert.That(loaded.Npcs.Count, Is.EqualTo(1));
            Assert.That(loaded.Npcs[0].Ticks, Is.EqualTo(5));
        }
    }
}
