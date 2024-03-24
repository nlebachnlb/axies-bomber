using TMPro;
using UnityEngine;

namespace Module.MapGeneration.Type
{
    public enum RoomType
    {
        NULL,
        NORMAL,
        MINIBOSS,
        BOSS,
        HIDDEN,
        BONUS
    }

    [System.Serializable]
    public class RoomData
    {
        public int roomId = -1;
        public RoomType roomType = RoomType.NULL;
        public Vector2Int index;
        public int distanceToStartRoom;

        public bool IsNotNull()
        {
            return roomType != RoomType.NULL;
        }

        public RoomData SetRoomType(RoomType type)
        {
            roomType = type;
            return this;
        }

        public RoomData SetDistanceToStartRoom(int value)
        {
            distanceToStartRoom = value;
            return this;
        }

        public RoomData SetIndex(Vector2Int value)
        {
            index = value;
            return this;
        }
    }

    [System.Serializable]
    public class RoomPrefabData
    {
        public RoomType roomType;
        public GameObject prefab;
    }

    public static class MapGenerationConstant
    {
        public static readonly Vector2Int[] Direction = {
            new(-1, 0),
            new(+1, 0),
            new(0, -1),
            new(0, +1)
        };
    }
}