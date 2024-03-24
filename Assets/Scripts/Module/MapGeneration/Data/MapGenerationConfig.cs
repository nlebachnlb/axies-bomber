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
            public GameObject prefab;
        }

        public List<RoomTypeConfig> roomTypeConfigs;

        public RoomTypeConfig GetRoomTypeConfig(RoomType roomType)
        {
            return roomTypeConfigs.Find(r => r.roomType == roomType);
        }
    }
}