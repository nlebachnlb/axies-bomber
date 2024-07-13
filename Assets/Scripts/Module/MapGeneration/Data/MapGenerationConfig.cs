using System;
using System.Collections.Generic;
using Module.MapGeneration.Type;
using UnityEngine;

namespace Module.MapGeneration.Data
{
    [Serializable]
    public class MapGenerationConfig
    {
        [Serializable]
        public class RoomTypeConfig
        {
            public RoomType roomType;
            public int minQuantity, maxQuantity;
            public int minDistance = 1;
        }

        [Serializable]
        public class RoomPrefabConfig
        {
            public RoomType roomType;
            public List<Room> prefab;
        }

        public List<RoomTypeConfig> roomTypeConfigs;
        public List<RoomPrefabConfig> roomPrefabConfigs;

        public RoomTypeConfig GetRoomTypeConfig(RoomType roomType)
        {
            return roomTypeConfigs.Find(r => r.roomType == roomType);
        }
        
        public RoomPrefabConfig GetRoomPrefabConfig(RoomType roomType)
        {
            return roomPrefabConfigs.Find(r => r.roomType == roomType);
        }
    }
}