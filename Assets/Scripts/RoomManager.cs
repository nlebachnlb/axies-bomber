using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    public int numberOfFunctionRooms;
    public int numberOfBossRooms;
    public int numberOfMinibossRooms;
    public int numberOfHiddenRooms;

    int roomWidth = 20;
    int roomHeight = 20;

    Vector2Int gridSize = new Vector2Int(10, 10);

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;
    private int roomCount;

    private bool generationComplete = false;

    private void Start()
    {
        roomGrid = new int[gridSize.x, gridSize.y];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int x = roomIndex.x;
            int y = roomIndex.y;

            int successCount = 0;
            successCount += TryGenerateRoom(new Vector2Int(x - 1, y)) ? 1 : 0;
            successCount += TryGenerateRoom(new Vector2Int(x + 1, y)) ? 1 : 0;
            successCount += TryGenerateRoom(new Vector2Int(x, y + 1)) ? 1 : 0;
            successCount += TryGenerateRoom(new Vector2Int(x, y - 1)) ? 1 : 0;

            if (successCount == 0)
                roomQueue.Enqueue(roomIndex);
        }
        else if (!generationComplete)
        {
            generationComplete = true;
            Debug.Log("Generation complete");
        }
    }


    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        count += (x > 0 && roomGrid[x - 1, y] != 0) ? 1 : 0;
        count += (x < gridSize.x - 1 && roomGrid[x + 1, y] != 0) ? 1 : 0;
        count += (y > 0 && roomGrid[x, y - 1] != 0) ? 1 : 0;
        count += (y < gridSize.y - 1 && roomGrid[x, y + 1] != 0) ? 1 : 0;

        return count;
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;

        //var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        //initialRoom.name = $"Room-{roomCount}";
        //initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        //roomObjects.Add(initialRoom);

        // var minimapRoom = Instantiate(minimapRoomPrefab, minimapRoot);
        // roomObjects.Add(minimapRoom);
        // minimapRoom.transform.position = new Vector3(roomIndex.x, roomIndex.y);
        // minimapRoom.GetComponent<MinimapRoom>().RoomIndex = roomIndex;
        // OpenDoors(minimapRoom, x, y);
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (x < 0 || y < 0 || x >= gridSize.x || y >= gridSize.y)
        {
            Debug.LogWarning("Generate failed: out of bound");
            return false;
        }

        if (roomCount >= maxRooms)
        {
            Debug.LogWarning("Generate failed: max room reached");
            return false;
        }

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            Debug.LogWarning("Generate failed: not random success");
            return false;
        }

        if (CountAdjacentRooms(roomIndex) > 1)
        {
            Debug.LogWarning("Generate failed: more than 1 adjacent room");
            return false;
        }

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        //var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        //initialRoom.name = $"Room-{roomCount}";
        //initialRoom.GetComponent<Room>().RoomIndex = roomIndex;

        // var minimapRoom = Instantiate(minimapRoomPrefab, minimapRoot);
        // minimapRoom.transform.position = new Vector3(roomIndex.x, roomIndex.y);
        // minimapRoom.GetComponent<MinimapRoom>().RoomIndex = roomIndex;
        // roomObjects.Add(minimapRoom);
        // OpenDoors(minimapRoom, x, y);

        return true;
    }

    private void OpenDoors(GameObject room, int x, int y)
    {
        var roomComp = room.GetComponent<MinimapRoom>();
        
        var left = GetRoomComponentAt(new Vector2Int(x - 1, y));
        var right = GetRoomComponentAt(new Vector2Int(x + 1, y));
        var top = GetRoomComponentAt(new Vector2Int(x, y + 1));
        var bottom = GetRoomComponentAt(new Vector2Int(x, y - 1));

        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            roomComp.OpenDoor(Vector2Int.left);
            left.OpenDoor(Vector2Int.right);
        }

        if (x < gridSize.x - 1 && roomGrid[x + 1, y] != 0)
        {
            roomComp.OpenDoor(Vector2Int.right);
            right.OpenDoor(Vector2Int.left);
        }

        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            roomComp.OpenDoor(Vector2Int.down);
            bottom.OpenDoor(Vector2Int.up);
        }

        if (y < gridSize.y - 1 && roomGrid[x, y + 1] != 0)
        {
            roomComp.OpenDoor(Vector2Int.up);
            top.OpenDoor(Vector2Int.down);
        }
    }

    private MinimapRoom GetRoomComponentAt(Vector2Int index)
    {
        GameObject gameObject = roomObjects.Find(r => r.GetComponent<MinimapRoom>().RoomIndex == index);
        return gameObject?.GetComponent<MinimapRoom>();
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int grid)
    {
        int x = grid.x;
        int z = grid.y;
        return new Vector3(roomWidth * (x - gridSize.x / 2), 0, roomHeight * (z - gridSize.y / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSize.x; ++x)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, 1, roomHeight));
            }
        }
    }
}
