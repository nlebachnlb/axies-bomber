using System;
using System.Collections.Generic;
using System.Linq;
using Module.MapGeneration.Data;
using Module.Utils;
using UnityEngine;

namespace Module.MapGeneration.View
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private MapData boundModel;
        [SerializeField] private Transform root;
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private CameraFollow camera;
        [SerializeField] private Transform player;

        private readonly Dictionary<int, Room> roomObjects = new();

        private void Awake()
        {
            boundModel.onDataChange += OnDataChange;

            EventBus.onRoomChange += OnRoomChange;
        }

        private void OnDestroy()
        {
            EventBus.onRoomChange -= OnRoomChange;
        }

        private void OnRoomChange(int roomId, Vector2Int fromDirection)
        {
            var roomData = boundModel.GetRoomDataFromId(roomId);
            var position = GetPositionFromGridIndex(roomData.index);
            Debug.Log($"Change camera bound to: {position.x}, {position.z}");
            camera.minPosition = new Vector3(position.x - 5, 0, position.z - 5);
            camera.maxPosition = new Vector3(position.x + 5, 0, position.z + 5);

            Room room = roomObjects[roomId];
            var spawnPoint = room.GetSpawnPointFromDirection(fromDirection);
            player.position = spawnPoint;
        }

        private void OnDataChange()
        {
            roomObjects.Clear();
            var rooms = boundModel.Rooms;
            foreach (var room in rooms)
            {
                var roomType = room.roomType;
                var prefab = boundModel.Config.GetRoomPrefabConfig(roomType).prefab;
                var roomObject = Instantiate(RandomUtils.RandomElement(prefab), root);
                var position = GetPositionFromGridIndex(room.index);
                roomObject.gameObject.transform.localPosition = position;
                roomObject.RoomIndex = room.index;
                roomObjects.Add(room.roomId, roomObject);
                OpenDoors(roomObject, room.index.x, room.index.y);
            }
        }
        
        private Vector3 GetPositionFromGridIndex(Vector2Int grid)
        {
            int x = grid.x;
            int z = grid.y;
            var gridSize = boundModel.GridSize;
            return new Vector3(roomSize.x * (x - gridSize.x * 0.5f), 0, roomSize.y * (z - gridSize.y * 0.5f));
        }
        
        private void OpenDoors(Room room, int x, int y)
        {
            var gridSize = boundModel.GridSize;
            var left = GetRoomComponentAt(new Vector2Int(x - 1, y));
            var right = GetRoomComponentAt(new Vector2Int(x + 1, y));
            var top = GetRoomComponentAt(new Vector2Int(x, y + 1));
            var bottom = GetRoomComponentAt(new Vector2Int(x, y - 1));
            
            var currentRoomId = boundModel.RoomGrid[x, y].roomId;
            if (x > 0 && boundModel.IsCellNotNull(x - 1, y) && left)
            {
                var nextRoomId = boundModel.RoomGrid[x - 1, y].roomId;
                room.OpenDoor(Vector2Int.left, nextRoomId);
                left.OpenDoor(Vector2Int.right, currentRoomId);
            }

            if (x < gridSize.x - 1 && boundModel.IsCellNotNull(x + 1, y) && right)
            {
                var nextRoomId = boundModel.RoomGrid[x + 1, y].roomId;
                room.OpenDoor(Vector2Int.right, nextRoomId);
                right.OpenDoor(Vector2Int.left, currentRoomId);
            }

            if (y > 0 && boundModel.IsCellNotNull(x, y - 1) && bottom)
            {
                var nextRoomId = boundModel.RoomGrid[x, y - 1].roomId;
                room.OpenDoor(Vector2Int.down, nextRoomId);
                bottom.OpenDoor(Vector2Int.up, currentRoomId);
            }

            if (y < gridSize.y - 1 && boundModel.IsCellNotNull(x, y + 1) && top)
            {
                var nextRoomId = boundModel.RoomGrid[x, y + 1].roomId;
                room.OpenDoor(Vector2Int.up, nextRoomId);
                top.OpenDoor(Vector2Int.down, currentRoomId);
            }
        }

        private Room GetRoomComponentAt(Vector2Int index)
        {
            var room = roomObjects.Values.ToList().Find(r => r.RoomIndex == index);
            return room;
        }
    }
}
