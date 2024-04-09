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
            if (boundModel.GetRoomDataFromId(roomId).cleared)
                CallTransports(roomId);
        }

        public void OnRoomChange(int oldRoomId, int roomId)
        {
            StartCoroutine(DoOnRoomChange(oldRoomId, roomId));
        }

        private void CallTransports(int roomId)
        {
            Room room = roomObjects[roomId];
            var (x, y) = (room.RoomIndex.x, room.RoomIndex.y);
            var left = GetRoomComponentAt(new Vector2Int(x - 1, y));
            var right = GetRoomComponentAt(new Vector2Int(x + 1, y));
            var top = GetRoomComponentAt(new Vector2Int(x, y + 1));
            var bottom = GetRoomComponentAt(new Vector2Int(x, y - 1));

            if (left)
            {
                var transport = CreateTransport();
                var destGateWay = left.GetGateWay(GateWayDirection.East);
                transport.Destination = destGateWay.transform.position;
                transport.DestinationSpawnPoint = destGateWay.spawnPoint.position;
                transport.TargetRoomId = left.RoomId;
                transport.DoEntrance(room.GetGateWay(GateWayDirection.West).transform.position);
            }
            
            if (right)
            {
                var transport = CreateTransport();
                var destGateWay = right.GetGateWay(GateWayDirection.West);
                transport.Destination = destGateWay.transform.position;
                transport.DestinationSpawnPoint = destGateWay.spawnPoint.position;
                transport.TargetRoomId = right.RoomId;
                transport.DoEntrance(room.GetGateWay(GateWayDirection.East).transform.position);
            }
            
            if (top)
            {
                var transport = CreateTransport();
                var destGateWay = top.GetGateWay(GateWayDirection.South);
                transport.Destination = destGateWay.transform.position;
                transport.DestinationSpawnPoint = destGateWay.spawnPoint.position;
                transport.TargetRoomId = top.RoomId;
                transport.DoEntrance(room.GetGateWay(GateWayDirection.North).transform.position);
            }
            
            if (bottom)
            {
                var transport = CreateTransport();
                var destGateWay = bottom.GetGateWay(GateWayDirection.North);
                transport.Destination = destGateWay.transform.position;
                transport.DestinationSpawnPoint = destGateWay.spawnPoint.position;
                transport.TargetRoomId = bottom.RoomId;
                transport.DoEntrance(room.GetGateWay(GateWayDirection.South).transform.position);
            }
        }

        private IEnumerator DoOnRoomChange(int oldRoomId, int roomId)
        {
            camera.OnLeaveToRoom();
            var roomData = boundModel.GetRoomDataFromId(roomId);
            var position = GetPositionFromGridIndex(roomData.index);
            camera.minPosition = new Vector3(position.x - 100, 0, position.z - 100);
            camera.maxPosition = new Vector3(position.x + 100, 0, position.z + 100);

            yield return new WaitForSeconds(2f);

            camera.smoothSpeed = 0.05f;
            camera.minPosition = new Vector3(position.x - 2, 0, position.z - 2);
            camera.maxPosition = new Vector3(position.x + 2, 0, position.z + 2);

            yield return new WaitForSeconds(1f);
            EventBus.Instance.OnEnterRoomEvent(roomId);
            camera.smoothSpeed = 0.2f;
            
            camera.OnEnterRoom();
        }

        private void OnDataChange()
        {
            foreach (var roomObject in roomObjects.Values)
                Destroy(roomObject.gameObject);
            roomObjects.Clear();
            
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

            transport.name = $"Transport {roomIdA} - {roomIdB}";
            return transport;
        }

        private Transport CreateTransport()
        {
            Transport transport = Instantiate(transportPrefab, root);
            return transport;
        }
    }
}
