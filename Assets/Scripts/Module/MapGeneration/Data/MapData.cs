using System;
using System.Collections.Generic;
using Base.Data;
using Module.MapGeneration.Type;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Module.MapGeneration.Data
{
    public class MapData : MonoBehaviour, IBaseData
    {
        public RoomData[,] RoomGrid => roomGrid;
        public List<RoomData> Rooms => rooms;
        public int RoomCount => roomCount;
        public int MaxRooms => maxRooms;
        public MapGenerationConfig Config => generationConfig;

        public Vector2Int GridSize => gridSize;

        public Vector2Int startIndex;

        [SerializeField] private int maxRooms = 15;
        [SerializeField] Vector2Int gridSize = new(10, 10);
        [SerializeField] private MapGenerationConfig generationConfig;

        private RoomData[,] roomGrid;
        private List<RoomData> rooms;
        private Dictionary<RoomType, int> availableRoomTypes;
        private int roomCount;

        private void Awake()
        {
            roomGrid = new RoomData[gridSize.x, gridSize.y];
            rooms = new();

            availableRoomTypes = new();
            var config = generationConfig.roomTypeConfigs;
            foreach (var type in config)
            {
                int quantity = Random.Range(type.minQuantity, type.maxQuantity + 1);
                availableRoomTypes.Add(type.roomType, quantity);
            }

            string log = "";
            foreach (var x in availableRoomTypes)
                log += "(" + x.Key + "," + x.Value + ")\n";

            Debug.Log("Available:\n" + log);
        }

        public void PlaceRoom(Vector2Int index, bool isStartRoom = false)
        {
            int x = index.x;
            int y = index.y;
            var roomData = GenerateRoomData(index);
            roomGrid[x, y] = roomData;
            rooms.Add(roomData);
            roomData.roomId = roomCount++;
            if (isStartRoom)
            {
                startIndex = index;
                roomData.cleared = true;
            }
        }

        public int CountAdjacentRooms(Vector2Int roomIndex)
        {
            int x = roomIndex.x;
            int y = roomIndex.y;
            int count = 0;
            count += (x > 0 && IsCellNotNull(x - 1, y) && roomGrid[x - 1, y].IsNotNull()) ? 1 : 0;
            count += (x < gridSize.x - 1 && IsCellNotNull(x + 1, y) && roomGrid[x + 1, y].IsNotNull()) ? 1 : 0;
            count += (y > 0 && IsCellNotNull(x, y - 1) && roomGrid[x, y - 1].IsNotNull()) ? 1 : 0;
            count += (y < gridSize.y - 1 && IsCellNotNull(x, y + 1) && roomGrid[x, y + 1].IsNotNull()) ? 1 : 0;

            return count;
        }

        public void DistributeRoomTypes()
        {
            RecalculateDistance();
            foreach (var room in rooms)
                DistributeRoomType(room.index);
        }

        public void PublishChanges()
        {
            DistributeRoomTypes();
            onDataChange?.Invoke();
        }

        public bool IsCellNotNull(Vector2Int index)
        {
            var x = index.x;
            var y = index.y;
            return IsCellNotNull(x, y);
        }

        public bool IsCellNotNull(int x, int y)
        {
            if (x < 0 || x >= gridSize.x) return false;
            if (y < 0 || y >= gridSize.y) return false;

            return roomGrid[x, y] != null;
        }

        private RoomData GenerateRoomData(Vector2Int index)
        {
            return new RoomData()
                .SetIndex(index)
                .SetRoomType(RoomType.NORMAL)
                .SetDistanceToStartRoom(-1);
        }

        private void DistributeRoomType(Vector2Int index)
        {
            if (startIndex.Equals(index))
            {
                roomGrid[index.x, index.y].SetRoomType(RoomType.START);
                return;
            }

            RoomType[] types = (RoomType[])Enum.GetValues(typeof(RoomType));
            foreach (var type in types)
            {
                if (type is RoomType.NULL or RoomType.START) continue;
                if (!availableRoomTypes.ContainsKey(type)) continue;
                if (availableRoomTypes[type] <= 0) continue;
                if (type == roomGrid[index.x, index.y].roomType) continue;

                var config = generationConfig.GetRoomTypeConfig(type);
                if (roomGrid[index.x, index.y].distanceToStartRoom < config.minDistance) continue;

                // Conditions met, successful
                availableRoomTypes[type]--;
                roomGrid[index.x, index.y].SetRoomType(type);
                return;
            }

            // No condition met, normal room by default
            roomGrid[index.x, index.y].SetRoomType(RoomType.NORMAL);
        }

        private void RecalculateDistance()
        {
            Queue<Vector2Int> queue = new();
            for (int x = 0; x < roomGrid.Length; ++x)
            {
                for (int y = 0; y < roomGrid.Length; ++y)
                {
                    if (!IsCellNotNull(x, y)) continue;
                    roomGrid[x, y].distanceToStartRoom = -1;
                }
            }

            queue.Enqueue(startIndex);
            roomGrid[startIndex.x, startIndex.y].distanceToStartRoom = 0;
            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                foreach (var dir in MapGenerationConstant.Direction)
                {
                    var newIndex = index + dir;
                    if (IsCellNotNull(newIndex) && roomGrid[newIndex.x, newIndex.y].distanceToStartRoom == -1)
                    {
                        queue.Enqueue(newIndex);
                        roomGrid[newIndex.x, newIndex.y].distanceToStartRoom = roomGrid[index.x, index.y].distanceToStartRoom + 1;
                    }
                }
            }
        }

        public RoomData GetRoomDataFromId(int roomId)
        {
            return rooms.Find(room => room.roomId == roomId);
        }

        public event IBaseData.OnDataChange onDataChange;
    }
}