using System;
using UnityEngine;

public enum GateWayDirection
{
    North,
    South,
    East,
    West
}

public class GateWay : MonoBehaviour
{
    public int targetRoomId;
    public Transform spawnPoint;
    public GateWayDirection gateWayDirection;
    
    public Vector2Int Direction
    {
        get
        {
            return gateWayDirection switch
            {
                GateWayDirection.East => Vector2Int.right,
                GateWayDirection.West => Vector2Int.left,
                GateWayDirection.North => Vector2Int.up,
                GateWayDirection.South => Vector2Int.down,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
