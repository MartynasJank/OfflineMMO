using System;
using UnityEngine;

public interface IServerMessage {}
public interface IClientMessage {}

public class LoginRequest : IServerMessage
{
    public string userName;
    public Action<LoginResponse> OnResponse;
}

public class LoginResponse : IClientMessage
{
    public int playerId;
    public Vector3 spawnPosition;
}

public class MoveCommand : IServerMessage
{
    public int playerId;
    public Vector3 targetPosition;
}

public class InteractCommand : IServerMessage
{
    public int playerId;
    public int targetId;
}

public class PositionUpdate : IClientMessage
{
    public int playerId;
    public Vector3 position;
}
