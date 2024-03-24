using System;
using System.Collections;
using System.Collections.Generic;
using Module.MapGeneration.Data;
using Module.MapGeneration.Type;
using UnityEngine;

public class MinimapHUD : MonoBehaviour
{
    [Header("Minimap")] 
    [SerializeField] private RoomPrefabData[] prefabs;
    [SerializeField] private Transform minimapRoot;
    [SerializeField] private MapData boundModel;
    [SerializeField] private Vector2Int roomSize;
    [SerializeField] private Transform axieIcon;
    [SerializeField] private Transform cameraTransform;

    private readonly List<MinimapRoom> minimapRooms = new();
    private readonly Dictionary<RoomType, MinimapRoom> minimapPrefabMetaData = new();
    private MovementController axieMovement;

    private void Awake()
    {
        boundModel.onDataChange += OnMapChange;

        minimapPrefabMetaData.Clear();
        foreach (var prefab in prefabs)
        {
            minimapPrefabMetaData.Add(prefab.roomType, prefab.prefab.GetComponent<MinimapRoom>());
        }

        axieMovement = FindObjectOfType<MovementController>();
    }

    private void LateUpdate()
    {
        var pos = GetMapPosition(axieMovement.Body.position);
        axieIcon.transform.position = pos;
        cameraTransform.position = new Vector3(pos.x, pos.y, -10);
    }

    private void OnDestroy()
    {
        boundModel.onDataChange -= OnMapChange;
    }

    private void OnMapChange()
    {
        minimapRooms.Clear();

        var rooms = boundModel.Rooms;
        foreach (var room in rooms)
        {
            var roomType = room.roomType;
            var prefab = minimapPrefabMetaData[roomType];
            var roomObject = Instantiate(prefab, minimapRoot);
            var index = room.index;
            roomObject.RoomIndex = index;
            roomObject.gameObject.transform.position = new Vector3(index.x, index.y);
            minimapRooms.Add(roomObject);
            OpenDoors(roomObject, index.x, index.y);
        }
    }
    
    private void OpenDoors(MinimapRoom room, int x, int y)
    {
        var roomGrid = boundModel.RoomGrid;
        var gridSize = boundModel.GridSize;
        
        var left = GetRoomComponentAt(new Vector2Int(x - 1, y));
        var right = GetRoomComponentAt(new Vector2Int(x + 1, y));
        var top = GetRoomComponentAt(new Vector2Int(x, y + 1));
        var bottom = GetRoomComponentAt(new Vector2Int(x, y - 1));

        if (x > 0 && boundModel.IsCellNotNull(x - 1, y) && left)
        {
            room.OpenDoor(Vector2Int.left);
            left.OpenDoor(Vector2Int.right);
        }

        if (x < gridSize.x - 1 && boundModel.IsCellNotNull(x + 1, y) && right)
        {
            room.OpenDoor(Vector2Int.right);
            right.OpenDoor(Vector2Int.left);
        }

        if (y > 0 && boundModel.IsCellNotNull(x, y - 1) && bottom)
        {
            room.OpenDoor(Vector2Int.down);
            bottom.OpenDoor(Vector2Int.up);
        }

        if (y < gridSize.y - 1 && boundModel.IsCellNotNull(x, y + 1) && top)
        {
            room.OpenDoor(Vector2Int.up);
            top.OpenDoor(Vector2Int.down);
        }
    }

    private MinimapRoom GetRoomComponentAt(Vector2Int index)
    {
        var room = minimapRooms.Find(r => r.RoomIndex == index);
        return room;
    }
    
    private Vector2Int GetGridIndexFromPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / roomSize.x + boundModel.GridSize.x * 0.5f);
        int y = Mathf.FloorToInt(position.z / roomSize.y + boundModel.GridSize.y * 0.5f);
        return new Vector2Int(x, y);
    }
    
    private Vector2 GetMapPosition(Vector3 position)
    {
        float x = position.x / roomSize.x + boundModel.GridSize.x * 0.5f;
        float y = position.z / roomSize.y + boundModel.GridSize.y * 0.5f;
        return new Vector2(x, y);
    }
}
