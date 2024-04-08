using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Module.MapGeneration.Data;
using Module.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Module.MapGeneration.View
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private MapData boundModel;
        [SerializeField] private Transform root;
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private CameraFollow camera;
        [SerializeField] private MovementController player;
        [SerializeField] private Transport transportPrefab;
        [SerializeField] private Image loadingMask;

        private readonly Dictionary<int, Room> roomObjects = new();
        private readonly Dictionary<(int, int, int), Transport> transports = new();

        private void Awake()
        {
            boundModel.onDataChange += OnDataChange;
        }

        private void Start()
        {
            loadingMask.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
        }

        public void OnEnterRoom(int roomId)
        {
            Room room = roomObjects[roomId];
            room.CallTransport(GateWayDirection.East);
            room.CallTransport(GateWayDirection.West);
            room.CallTransport(GateWayDirection.North);
            room.CallTransport(GateWayDirection.South);
        }

        public void OnRoomChange(int oldRoomId, int roomId, Vector2Int fromDirection)
        {
            StartCoroutine(DoOnRoomChange(oldRoomId, roomId, fromDirection));
        }

        private IEnumerator DoOnRoomChange(int oldRoomId, int roomId, Vector2Int fromDirection)
        {
            var roomData = boundModel.GetRoomDataFromId(roomId);
            var oldRoomData = boundModel.GetRoomDataFromId(oldRoomId);
            var position = GetPositionFromGridIndex(roomData.index);
            var oldPosition = GetPositionFromGridIndex(oldRoomData.index);
            camera.minPosition = new Vector3(position.x - 100, 0, position.z - 100);
            camera.maxPosition = new Vector3(position.x + 100, 0, position.z + 100);

            Room room = roomObjects[roomId];
            Room oldRoom = roomObjects[oldRoomId];
            var spawnPoint = room.GetSpawnPointFromDirection(fromDirection);
            // var transport = room.GetTransportFromDirection(fromDirection);

            // var oldTransport = oldRoom.GetTransportFromDirection(-fromDirection);
            // oldTransport.TransportPlayerTo(transport.transform.position, player, spawnPoint);

            yield return new WaitForSeconds(3f);
            
            camera.minPosition = new Vector3(position.x - 5, 0, position.z - 5);
            camera.maxPosition = new Vector3(position.x + 5, 0, position.z + 5);
        }

        private void OnDataChange()
        {
            foreach (var roomObject in roomObjects.Values)
                Destroy(roomObject.gameObject);
            roomObjects.Clear();
            
            foreach (var transport in transports.Values)
                Destroy(transport.gameObject);
            transports.Clear();
            
            var rooms = boundModel.Rooms;
            foreach (var room in rooms)
            {
                var roomType = room.roomType;
                var prefab = boundModel.Config.GetRoomPrefabConfig(roomType).prefab;
                var roomObject = Instantiate(RandomUtils.RandomElement(prefab), root);
                var position = GetPositionFromGridIndex(room.index);
                roomObject.gameObject.transform.localPosition = position;
                roomObject.BindWithDataModel(room);
                roomObjects.Add(room.roomId, roomObject);
                OpenDoors(roomObject, room.index.x, room.index.y);
            }
            
            loadingMask.gameObject.SetActive(false);
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
                var transport = CreateTransport(currentRoomId, nextRoomId, 0);
                transport.RegisterRoom(currentRoomId, GateWayDirection.West);
                transport.RegisterRoom(nextRoomId, GateWayDirection.East);
            }

            if (x < gridSize.x - 1 && boundModel.IsCellNotNull(x + 1, y) && right)
            {
                var nextRoomId = boundModel.RoomGrid[x + 1, y].roomId;
                room.OpenDoor(Vector2Int.right, nextRoomId);
                right.OpenDoor(Vector2Int.left, currentRoomId);
                var transport = CreateTransport(currentRoomId, nextRoomId, 1);
                transport.RegisterRoom(currentRoomId, GateWayDirection.East);
                transport.RegisterRoom(nextRoomId, GateWayDirection.West);
            }

            if (y > 0 && boundModel.IsCellNotNull(x, y - 1) && bottom)
            {
                var nextRoomId = boundModel.RoomGrid[x, y - 1].roomId;
                room.OpenDoor(Vector2Int.down, nextRoomId);
                bottom.OpenDoor(Vector2Int.up, currentRoomId);
                var transport = CreateTransport(currentRoomId, nextRoomId, 2);
                transport.RegisterRoom(currentRoomId, GateWayDirection.South);
                transport.RegisterRoom(nextRoomId, GateWayDirection.North);            
            }

            if (y < gridSize.y - 1 && boundModel.IsCellNotNull(x, y + 1) && top)
            {
                var nextRoomId = boundModel.RoomGrid[x, y + 1].roomId;
                room.OpenDoor(Vector2Int.up, nextRoomId);
                top.OpenDoor(Vector2Int.down, currentRoomId);
                var transport = CreateTransport(currentRoomId, nextRoomId, 3);
                transport.RegisterRoom(currentRoomId, GateWayDirection.North);
                transport.RegisterRoom(nextRoomId, GateWayDirection.South);
            }
        }

        private Room GetRoomComponentAt(Vector2Int index)
        {
            var room = roomObjects.Values.ToList().Find(r => r.RoomIndex == index);
            return room;
        }

        private (int, int, int) GetTransportId(int roomIdA, int roomIdB, int axis)
        {
            return (Math.Min(roomIdA, roomIdB), Math.Max(roomIdA, roomIdB), axis);
        }
        
        private Transport CreateTransport(int roomIdA, int roomIdB, int axis)
        {
            Transport transport = Instantiate(transportPrefab, root);
            transport.gameObject.SetActive(false);
            transports.Add((roomIdA, roomIdB, axis), transport);

            Room roomA = roomObjects[roomIdA];
            Room roomB = roomObjects[roomIdB];

            roomA.CallTransportEvent += transport.OnCalledByRoom;
            roomB.CallTransportEvent += transport.OnCalledByRoom;

            transport.name = $"Transport {roomIdA} - {roomIdB}";
            return transport;
        }
    }
}
