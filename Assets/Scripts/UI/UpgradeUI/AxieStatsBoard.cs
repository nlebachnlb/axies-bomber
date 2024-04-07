using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxieStatsBoard : MonoBehaviour
{
    [SerializeField] private List<StatItem> statItems;

    private int currentAxieId = -1;

    public void Load(int axieId, Dictionary<Stat, int> statsLevel)
    {
        currentAxieId = axieId;
        foreach (var statItem in statItems)
        {
            statItem.Init(currentAxieId, AppRoot.Instance.UserDataModel.GetAxieStatLevel(axieId, statItem.Stat));
            statItem.LoadUI();
        }
    }
}
