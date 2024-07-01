using System.Collections;
using System.Collections.Generic;
using Cathei.BakingSheet;
using UnityEngine;

namespace ExcelConfig
{
    public class MainConfigSheetContainer : SheetContainerBase
    {
        public AxieUpgradeSheet AxieUpgrade { get; private set; }

        public MainConfigSheetContainer(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
        }
    }
}
