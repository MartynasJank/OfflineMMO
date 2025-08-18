using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LocalServer : MonoBehaviour
{
    public static LocalServer Instance { get; private set; }

    private Thread _thread;
    private bool _running;

    private readonly ConcurrentQueue<IServerMessage> _messages = new();
    private readonly Dictionary<int, PlayerData> _players = new();
    private int _nextPlayerId = 1;

    public event Action<PositionUpdate> OnPositionUpdate;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        StartServer();
    }

    void OnDisable()
    {
        StopServer();
    }

    public void Send(IServerMessage msg)
    {
        _messages.Enqueue(msg);
    }

    public void StartServer()
    {
        if (_running) return;
        _running = true;
        _thread = new Thread(ServerLoop) { IsBackground = true };
        _thread.Start();
    }

    public void StopServer()
    {
        _running = false;
        _thread?.Join();
        _thread = null;
    }

    private void ServerLoop()
    {
        while (_running)
        {
            while (_messages.TryDequeue(out IServerMessage msg))
            {
                HandleMessage(msg);
            }

            Thread.Sleep(50);
        }
    }

    private void HandleMessage(IServerMessage msg)
    {
        switch (msg)
        {
            case LoginRequest login:
                HandleLogin(login);
                break;
            case MoveCommand move:
                HandleMove(move);
                break;
            case InteractCommand interact:
                HandleInteract(interact);
                break;
        }
    }

    private void HandleLogin(LoginRequest login)
    {
        int id = _nextPlayerId++;
        Vector3 spawn = Vector3.zero;
        _players[id] = new PlayerData { id = id, name = login.userName, position = spawn };
        login.OnResponse?.Invoke(new LoginResponse { playerId = id, spawnPosition = spawn });
    }

    private void HandleMove(MoveCommand move)
    {
        if (_players.TryGetValue(move.playerId, out PlayerData player))
        {
            player.position = move.targetPosition;
            OnPositionUpdate?.Invoke(new PositionUpdate { playerId = player.id, position = player.position });
        }
    }

    private void HandleInteract(InteractCommand interact)
    {
        // Placeholder for interaction logic (e.g., open chest, talk to NPC)
    }

    private class PlayerData
    {
        public int id;
        public string name;
        public Vector3 position;
    }
}
