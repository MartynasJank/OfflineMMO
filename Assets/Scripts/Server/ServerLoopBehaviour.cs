using UnityEngine;
using UnityEngine.Profiling;

namespace OfflineMMO.Server
{
    public class ServerLoopBehaviour : MonoBehaviour
    {
        readonly Server server = new();

        public Server Server => server;

        void Update()
        {
            Profiler.BeginSample("Server Loop");
            server.Update();
            Profiler.EndSample();
        }
    }
}
