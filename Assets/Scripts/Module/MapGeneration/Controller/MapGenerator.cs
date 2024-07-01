using System.Collections.Generic;
using Module.MapGeneration.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Module.MapGeneration.Controller
{
    public class MapGenerator : MonoBehaviour
    {
        public MapData DataModel => dataModel;

        [SerializeField] private MapData dataModel;

        private readonly Queue<Vector2Int> roomQueue = new();
        private bool generationComplete = false;

        public void StartRoomGeneration()
        {
            StartRoomGenerationFromRoom(DataModel.GridSize / 2);
        }

        private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
        {
            Debug.Log("Start index:" + roomIndex);
            generationComplete = false;
            roomQueue.Enqueue(roomIndex);
            DataModel.PlaceRoom(roomIndex, true);
        }

        private void Update()
        {
            int roomCount = DataModel.RoomCount;
            int maxRooms = DataModel.MaxRooms;
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
                DataModel.PublishChanges();
                EventBus.Instance.OnEnterRoomEvent(0);
                Debug.Log("Generation complete");
            }
        }

        private bool TryGenerateRoom(Vector2Int roomIndex)
        {
            var gridSize = DataModel.GridSize;
            var roomCount = DataModel.RoomCount;
            var maxRooms = DataModel.MaxRooms;

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

            if (DataModel.CountAdjacentRooms(roomIndex) > 1)
            {
                Debug.LogWarning("Generate failed: more than 1 adjacent room");
                return false;
            }

            if (DataModel.IsCellNotNull(roomIndex) && DataModel.RoomGrid[x, y].IsNotNull())
                return false;

            roomQueue.Enqueue(roomIndex);
            DataModel.PlaceRoom(new Vector2Int(x, y));

            return true;
        }
    }
}
