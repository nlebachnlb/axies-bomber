using System;
using System.Collections;
using System.Collections.Generic;
using Cathei.BakingSheet;
using UnityEngine;

namespace ExcelConfig
{
    // Private properties are data to be filled in by Cathei.BakingSheet
    // Public fields are fields to be serialized into scriptable objectss
    [Serializable]
    public class AxieUpgradeConfigData : SheetRowElem
    {
        public int Damage;
        private int cDamage { get; set; }

        public float Speed;
        private float cSpeed { get; set; }

        public int BombRange;
        private int cBombRange { get; set; }

        public int BombCount;
        private int cBombCount { get; set; }

        public int Health;
        private int cHealth { get; set; }

        public int DamageCost;
        private int cDamageCost { get; set; }

        public int SpeedCost;
        private int cSpeedCost { get; set; }

        public int BombRangeCost;
        private int cBombRangeCost { get; set; }

        public int BombCountCost;
        private int cBombCountCost { get; set; }

        public int HealthCost;
        private int cHealthCost { get; set; }

        public override void PostLoad(SheetConvertingContext context)
        {
            base.PostLoad(context);

            Damage = cDamage;
            Speed = cSpeed;
            BombRange = cBombRange;
            BombCount = cBombCount;
            Health = cHealth;

            DamageCost = cDamageCost;
            SpeedCost = cSpeedCost;
            BombRangeCost = cBombRangeCost;
            BombCountCost = cBombCountCost;
            HealthCost = cHealthCost;
        }
    }

    public class AxieUpgradeSheet : Sheet<AxieIdentity, AxieUpgradeSheet.AxieUpgradeSchema>
    {
        public class AxieUpgradeSchema : SheetRowArray<AxieIdentity, AxieUpgradeConfigData>
        {
        }
    }
}
