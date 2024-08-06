using System;
using UnityEngine;

[Serializable]
public class DungeonData
{
    public int oil;

    public int Oil
    {
        get => oil;
        set
        {
            oil = value;
            EventBus.RaiseOnOilChanged(oil);
        }
    }
}

public class DungeonDataModel : MonoBehaviour
{
    public DungeonData DungeonData { get; private set; }

    private void Awake()
    {
        EventBus.onPickCollectible += OnPickCollectible;
    }

    private void OnDestroy()
    {
        EventBus.onPickCollectible -= OnPickCollectible;
    }

    public void Generate()
    {
        DungeonData = new();
    }

    private void OnPickCollectible(Collectible collectible)
    {
        if (collectible.Type == CollectibleType.Oil)
        {
            DungeonData.Oil += collectible.Amount;
        }
    }
}
