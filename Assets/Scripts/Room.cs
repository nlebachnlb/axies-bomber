using System;
using System.Collections;
using System.Collections.Generic;
using Module.MapGeneration.Type;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int RoomIndex => dataModel.index;
    public int RoomId => dataModel.roomId;

    [SerializeField] private GameObject topDoor, bottomDoor, leftDoor, rightDoor;
    [SerializeField] private GameObject topBlock, bottomBlock, leftBlock, rightBlock;
    [SerializeField] private int roomWidth, roomHeight;
    [SerializeField] private GateWay topTerminal, bottomTerminal, leftTerminal, rightTerminal;

    private RoomData dataModel;

    public Vector3 MinPosition => new(
        transform.position.x - roomWidth * 0.5f, 0,
        transform.position.y - roomHeight * 0.5f);

    public Vector3 MaxPosition => new(
        transform.position.x + roomWidth * 0.5f, 0,
        transform.position.y + roomHeight * 0.5f);

    public void BindWithDataModel(RoomData data)
    {
        dataModel = data;
    }

    public void OpenDoor(Vector2Int direction, int targetRoomId)
    {
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(true);
            topTerminal.targetRoomId = targetRoomId;
        }

        if (direction == Vector2Int.down)
        {
            bottomDoor.SetActive(true);
            bottomTerminal.targetRoomId = targetRoomId;
        }

        if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(true);
            leftTerminal.targetRoomId = targetRoomId;
        }

        if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(true);
            rightTerminal.targetRoomId = targetRoomId;
        }

        topBlock.SetActive(!topDoor.activeInHierarchy);
        leftBlock.SetActive(!leftDoor.activeInHierarchy);
        rightBlock.SetActive(!rightDoor.activeInHierarchy);
        bottomBlock.SetActive(!bottomDoor.activeInHierarchy);
    }

    public GateWay GetGateWay(GateWayDirection direction)
    {
        return direction switch
        {
            GateWayDirection.West => leftTerminal,
            GateWayDirection.East => rightTerminal,
            GateWayDirection.North => topTerminal,
            GateWayDirection.South => bottomTerminal,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
