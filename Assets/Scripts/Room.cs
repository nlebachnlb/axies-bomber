using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor, bottomDoor, leftDoor, rightDoor;
    [SerializeField] GameObject topBlock, bottomBlock, leftBlock, rightBlock;
    [SerializeField] private int roomWidth, roomHeight;
    [SerializeField] private MapChanger topTerminal, bottomTerminal, leftTerminal, rightTerminal;
    public Vector2Int RoomIndex { get; set; }

    public Vector3 MinPosition => new(
        transform.position.x - roomWidth * 0.5f, 0,
        transform.position.y - roomHeight * 0.5f);
    
    public Vector3 MaxPosition => new(
        transform.position.x + roomWidth * 0.5f, 0,
        transform.position.y + roomHeight * 0.5f);
    
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

    public Vector3 GetSpawnPointFromDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return bottomTerminal.spawnPoint.position;
        
        if (direction == Vector2Int.down)
            return topTerminal.spawnPoint.position;
        
        if (direction == Vector2Int.left)
            return rightTerminal.spawnPoint.position;
        
        if (direction == Vector2Int.right)
            return leftTerminal.spawnPoint.position;

        return new Vector3(-1, -1, -1);
    }
}
